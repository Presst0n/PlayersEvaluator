using FluentAssertions;
using Newtonsoft.Json;
using PE.Contracts.V1;
using PE.Contracts.V1.Requests;
using PE.Contracts.V1.Responses;
using PE.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PE.IntegrationTests
{
    public class RosterControllerTests : RosterIntegrationTest
    {
        [Fact]
        public async Task GetAll_WithoutAnyRosters_ReturnsEmptyResponse()
        {
            // Arrange 
            await AuthenticateAsync();

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Rosters.GetAll);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var rosterResponse = await response.Content.ReadAsAsync<PagedResponse<RosterResponse>>();
            rosterResponse.Data.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAll_IfUserHasAccessToRosters_ReturnsPopulatedCollectionOfRosters()
        {
            // Arrange 
            await AuthenticateAsync();

            var createRosterRequests = new List<CreateRosterRequest> 
            { 
                new CreateRosterRequest { CreatorName = "Test roster1", Description = "Test description1" },
                new CreateRosterRequest { CreatorName = "Test roster2", Description = "Test description2" },
                new CreateRosterRequest { CreatorName = "Test roster3", Description = "Test description3" }
            };

            await CreateRostersAsync(createRosterRequests);

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Rosters.GetAll);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var rosterResponse = await response.Content.ReadAsAsync<PagedResponse<RosterResponse>>();
            rosterResponse.Data.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Get_DisplayNotFoundStatusCode_IfPostDoesNotExist()
        {
            // Arrange 
            await AuthenticateAsync();
            var notExistingRoster = Guid.NewGuid().ToString();

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Rosters.Get.Replace("{rosterId}", notExistingRoster));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var returnedRoster = await response.Content.ReadAsAsync<Response<RosterResponse>>();
            returnedRoster.Should().BeNull();
        }

        [Fact]
        public async Task Get_ReturnsRoster_WhenPostExistsInTheDatabase()
        {
            // Arrange 
            await AuthenticateAsync();
            var createdRoster = await CreateRosterAsync(new CreateRosterRequest { CreatorName = "Test roster", Description = "Test description" });

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Rosters.Get.Replace("{rosterId}", createdRoster.Id));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedRoster = await response.Content.ReadAsAsync<Response<RosterResponse>>();
            returnedRoster.Data.CreatorName.Should().Be("Test roster");
            returnedRoster.Data.Id.Should().Be(createdRoster.Id);
        }

        [Fact]
        public async Task Update_ReturnsUpdatedRoster_IfRosterExist()
        {
            // Arrange
            await AuthenticateAsync();
            var createRosterResponse = await CreateRosterAsync(new CreateRosterRequest 
            { 
                CreatorName = "Haha test post go brrrrrr", 
                Description = "Test description",
                Name = "YupiLoopi"
            });
            var response = await TestClient.GetAsync(ApiRoutes.Rosters.Get.Replace("{rosterId}", createRosterResponse.Id)); 
            var rosterToUpdate = await response.Content.ReadAsAsync<RosterResponse>();
            rosterToUpdate.Name = "Unfortunately test do not go brrrrr";
            rosterToUpdate.Description = "Life hurts";
            var content = CreateHttpContent(rosterToUpdate);

            // Act
            var response_ = await TestClient.PutAsync(ApiRoutes.Rosters.Update.Replace("{rosterId}", createRosterResponse.Id), content);

            // Assert
            response_.StatusCode.Should().Be(HttpStatusCode.OK);
            var updatedRoster = await response_.Content.ReadAsAsync<Response<RosterResponse>>();
            updatedRoster.Data.Name.Should().Be("Unfortunately test do not go brrrrr");
            updatedRoster.Data.Description.Should().Be("Life hurts");
            updatedRoster.Data.Id.Should().Be(createRosterResponse.Id);
        }

        [Fact]
        public async Task Update_DisplayNotFoundStatusCode_IfRosterDoesNotExist()
        {
            // Arrange
            await AuthenticateAsync();
            var id = Guid.NewGuid().ToString();
            var fakeRoster = new RosterResponse { Id = id, Name = "TestName", CreatorName = "Luci", CreatorId = id, Description = ":/" };
            fakeRoster.Name = "Unfortunately test do not go brrrrr :/";
            fakeRoster.Description = "Life hurts";
            var content = CreateHttpContent(fakeRoster);

            // Act
            var response_ = await TestClient.PutAsync(ApiRoutes.Rosters.Update.Replace("{rosterId}", fakeRoster.Id), content);

            // Assert
            response_.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Update_DisplayForbbidenStatusCode_IfUserHaveNoPermissionsToUpdate()
        {
            // Arrange
            await AuthenticateAsync();
            var createRosterResponse = await CreateRosterAsync(new CreateRosterRequest
            {
                CreatorName = "Haha test post go brrrrrr",
                Description = "Test description",
                Name = "YupiLoopi"
            });
            var response = await TestClient.GetAsync(ApiRoutes.Rosters.Get.Replace("{rosterId}", createRosterResponse.Id));
            var rosterToUpdate = await response.Content.ReadAsAsync<RosterResponse>();
            rosterToUpdate.Name = "Unfortunately test do not go brrrrr";
            rosterToUpdate.Description = "Life hurts";
            var content = CreateHttpContent(rosterToUpdate);
            await AuthenticateAsync("test@test", "Byczek");

            // Act
            var response_ = await TestClient.PutAsync(ApiRoutes.Rosters.Update.Replace("{rosterId}", createRosterResponse.Id), content);

            // Assert
            response_.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_RemovesRoster_IfRostertExistsInTheDatabase()
        {
            // Arrange
            await AuthenticateAsync();
            var createRosterResponse = await CreateRosterAsync(new CreateRosterRequest 
            { 
                CreatorName = "Haha test post go brrrrrr!", 
                Description = "Test description" 
            });

            // Act
            var response = await TestClient.DeleteAsync(ApiRoutes.Rosters.Delete.Replace("{rosterId}", createRosterResponse.Id.ToString()));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_DisplayNotFoundStatusCode_IfRostertDoesNotExist()
        {
            // Arrange
            await AuthenticateAsync();
            string id = Guid.NewGuid().ToString();

            // Act
            var response = await TestClient.DeleteAsync(ApiRoutes.Rosters.Delete.Replace("{rosterId}", id));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_DisplayForbbidenStatusCode_IfUserHasInsufficientPermissions()
        {
            // Arrange
            await AuthenticateAsync();
            var createdRoster = await CreateRosterAsync(new CreateRosterRequest 
            { 
                Name = "Awesome Roster", 
                Description = "That's my best roster I've ever done in my whole fuckin' life." 
            });
            await AuthenticateAsync("test@test", "Byczek");

            // Act
            var response = await TestClient.DeleteAsync(ApiRoutes.Rosters.Delete.Replace("{rosterId}", createdRoster.Id));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Create_CreatesRosterAndAddsToDatabase()
        {    
            // Arrange
            await AuthenticateAsync();

            // Act
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Rosters.Create, new CreateRosterRequest
            {
                Name = "Even more awesome Roster",
                Description = "That's my best roster I've ever done in my whole fuckin' life."
            });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var roster = await response.Content.ReadAsAsync<RosterResponse>();
            roster.Should().NotBeNull();
        }
    }
}
