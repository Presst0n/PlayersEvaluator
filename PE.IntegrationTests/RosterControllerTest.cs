﻿using FluentAssertions;
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
    public class RosterControllerTest : IntegrationTest
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
        public async Task GetAll_WithRosters_ReturnsPopulatedCollectionOfRosters()
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
            var response = await TestClient.GetAsync(ApiRoutes.Rosters.Get.Replace("{rosterId}", createdRoster.Id.ToString()));

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
                CreatorName = "Haha test post go brrrrrr!", 
                Description = "Test description" 
            });

                                                                                                                                        // Extract the code below and move it to IntegrationTest
            var response = await TestClient.GetAsync(ApiRoutes.Rosters.Get.Replace("{rosterId}", createRosterResponse.Id.ToString())); // Remove later this ToString() extension method cuz it is not needed.
            var rosterToUpdate = await response.Content.ReadAsAsync<RosterResponse>();
            rosterToUpdate.Name = "Unfortunately test do not go brrrrr :/";
            rosterToUpdate.Description = "Life hurts";
            HttpContent content = new StringContent(JsonConvert.SerializeObject(rosterToUpdate), Encoding.UTF8, "application/json"); 

            // Act
            var response_ = await TestClient.PutAsync(ApiRoutes.Rosters.Update.Replace("{rosterId}", createRosterResponse.Id.ToString()), content);

            // Assert
            response_.StatusCode.Should().Be(HttpStatusCode.OK);
            var updatedRoster = await response_.Content.ReadAsAsync<Response<RosterResponse>>();
            updatedRoster.Data.Name.Should().Be("Unfortunately test do not go brrrrr :/");
            updatedRoster.Data.Description.Should().Be("Life hurts");
        }

        [Fact]
        public async Task Delete_RemovesRoster_IfRostertExistsInTheDatabase()
        {
            // Arrange
            await AuthenticateAsync();
            var createRosterResponse = await CreateRosterAsync(new CreateRosterRequest { CreatorName = "Haha test post go brrrrrr!", Description = "Test description" });

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
    }
}
