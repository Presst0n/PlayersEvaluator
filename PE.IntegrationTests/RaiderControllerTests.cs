using FluentAssertions;
using PE.Contracts.V1;
using PE.Contracts.V1.Requests;
using PE.Contracts.V1.Responses;
using PE.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PE.IntegrationTests
{
    public class RaiderControllerTests : RaiderIntegrationTest
    {
        [Fact]
        public async Task Get_IfRaiderExists_ReturnsRaider()
        {
            // Arrange
            await AuthenticateAsync();

            var roster = await CreateRosterAsync(new CreateRosterRequest { Name = "Rosterio", CreatorName = "Timothy", Description = "randomDescription" });

            var createdRaiderResponse = await CreateRaiderAsync(new CreateRaiderRequest
            {
                Name = "Zarathustra",
                Class = "Mage",
                MainSpecialization = "Fire",
                OffSpecialization = "Frost",
                Role = "DamageDealer",
                RosterId = roster.Id
            });

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Raiders.Get.Replace("{raiderId}", createdRaiderResponse.RaiderId));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var raiderResponse = await response.Content.ReadAsAsync<Response<RaiderResponse>>();
            raiderResponse.Data.Name.Should().Be("Zarathustra");
            raiderResponse.Data.Class.Should().Be("Mage");
            raiderResponse.Data.MainSpecialization.Should().Be("Fire");
            raiderResponse.Data.OffSpecialization.Should().Be("Frost");
            raiderResponse.Data.Role.Should().Be("DamageDealer");
            raiderResponse.Data.RosterId.Should().Be(roster.Id);
        }

        [Fact]
        public async Task Get_IfRaiderDoesNotExist_ReturnsNotFoundStatusCode()
        {
            // Arrange
            await AuthenticateAsync();

            var roster = await CreateRosterAsync(new CreateRosterRequest { Name = "Roster", CreatorName = "Stachu", Description = "random description" });

            var fakeRaiderId = Guid.NewGuid().ToString();

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Raiders.Get.Replace("{raiderId}", fakeRaiderId));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_WithoutAnyPermissions_ReturnsForbbidenStatusCode()
        {
            // Arrange
            await AuthenticateAsync();

            var roster = await CreateRosterAsync(new CreateRosterRequest { Name = "Roster", CreatorName = "Stachu", Description = "random description" });

            var createdRaiderResponse = await CreateRaiderAsync(new CreateRaiderRequest
            {
                Name = "Zarathustra",
                Class = "Mage",
                MainSpecialization = "Fire",
                OffSpecialization = "Frost",
                Role = "DamageDealer",
                RosterId = roster.Id
            });

            await AuthenticateAsync("anotherbrick@inthewall.com", "GeorgSlaveinsky");

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Raiders.Get.Replace("{raiderId}", createdRaiderResponse.RaiderId));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetAll_WithoutAnyPermissions_ReturnsForbbidenStatusCode()
        {
            // Arrange
            await AuthenticateAsync();

            var roster = await CreateRosterAsync(new CreateRosterRequest { Name = "Roster", CreatorName = "Stachu", Description = "random description" });

            List<CreateRaiderRequest> createRaiderRequests = new List<CreateRaiderRequest>()
            {
                new CreateRaiderRequest
                {
                    Name = "Zarathustra",
                    Class = "Mage",
                    MainSpecialization = "Fire",
                    OffSpecialization = "Frost",
                    Role = "DamageDealer",
                    RosterId = roster.Id
                },
                new CreateRaiderRequest
                {
                    Name = "Bobek",
                    Class = "Warrior",
                    MainSpecialization = "Fury",
                    OffSpecialization = "Prot",
                    Role = "DamageDealer",
                    RosterId = roster.Id
                }
            };

            await CreateRaidersAsync(createRaiderRequests);


            await AuthenticateAsync("anotherbrick@inthewall.com", "DavidSlaveinsky");

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Raiders.GetAll + $"?rosterId={roster.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetAll_IfRostersExistAndUserHasAccessToThose_ReturnsRostersWithOkStatusCode()
        {
            // Arrange
            await AuthenticateAsync();

            var roster = await CreateRosterAsync(new CreateRosterRequest { Name = "Roster", CreatorName = "Stachu", Description = "random description" });

            List<CreateRaiderRequest> createRaiderRequests = new List<CreateRaiderRequest>()
            {
                new CreateRaiderRequest
                {
                    Name = "FLiszt",
                    Class = "Warlock",
                    MainSpecialization = "Destruction",
                    OffSpecialization = "Affliction",
                    Role = "DamageDealer",
                    RosterId = roster.Id
                },
                new CreateRaiderRequest
                {
                    Name = "Bobek",
                    Class = "Warrior",
                    MainSpecialization = "Fury",
                    OffSpecialization = "Prot",
                    Role = "DamageDealer",
                    RosterId = roster.Id
                }
            };

            await CreateRaidersAsync(createRaiderRequests);

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Raiders.GetAll + $"?rosterId={roster.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var pagedResponse = await response.Content.ReadAsAsync<PagedResponse<RaiderResponse>>();
            pagedResponse.Data.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Update_IfRaiderExists_ReturnsUpdatedRaider()
        {
            // Arrange
            await AuthenticateAsync();
            var roster = await CreateRosterAsync(new CreateRosterRequest { Name = "Roster", CreatorName = "Stachu", Description = "random description" });

            var createdRaiderResponse = await CreateRaiderAsync(new CreateRaiderRequest
            {
                Name = "Zarathustra",
                Class = "Mage",
                MainSpecialization = "Fire",
                OffSpecialization = "Frost",
                Role = "DamageDealer",
                RosterId = roster.Id
            });

            var response = await TestClient.GetAsync(ApiRoutes.Raiders.Get.Replace("{raiderId}", createdRaiderResponse.RaiderId));
            var raiderToUpdate = await response.Content.ReadAsAsync<RaiderResponse>();
            raiderToUpdate.Name = "Ronald";
            raiderToUpdate.Class = "Hunter";
            raiderToUpdate.MainSpecialization = "Beast Mastery";
            raiderToUpdate.OffSpecialization = "Marksmanship";
            raiderToUpdate.Role = "DamageDealer";
            var content = CreateHttpContent(raiderToUpdate);

            // Act
            var raiderResponse = await TestClient.PutAsync(ApiRoutes.Raiders.Update.Replace("{raiderId}", createdRaiderResponse.RaiderId), content);

            // Assert
            raiderResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var raider = await raiderResponse.Content.ReadAsAsync<Response<RaiderResponse>>();
            raider.Data.Name.Should().NotBeSameAs(createdRaiderResponse.Name);
            raider.Data.MainSpecialization.Should().NotBeSameAs(createdRaiderResponse.MainSpecialization);
            raider.Data.OffSpecialization.Should().NotBeSameAs(createdRaiderResponse.OffSpecialization);
            raider.Data.Class.Should().NotBeSameAs(createdRaiderResponse.Class);
            raider.Data.Role.Should().NotBeSameAs(createdRaiderResponse.Role);
        }

        [Fact]
        public async Task Update_IfRaiderDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            await AuthenticateAsync();
            var roster = await CreateRosterAsync(new CreateRosterRequest { Name = "Roster", CreatorName = "Stachu", Description = "Random Description" });

            var fakeRaider = new Raider() 
            { 
                Class = "Warrior",
                Name = "Steven",
                MainSpecialization = "Fury",
                OffSpecialization = "Protection",
                Points = 250,
                RaiderId = Guid.NewGuid().ToString(),
                Role = "DD"
            };

            var content = CreateHttpContent(fakeRaider);

            // Act
            var raiderResponse = await TestClient.PutAsync(ApiRoutes.Raiders.Update.Replace("{raiderId}", fakeRaider.RaiderId), content);

            // Assert
            raiderResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_IfRaiderExistsAndUserIsOwner_ReturnsNoContentStatusCode()
        {
            // Arrange
            await AuthenticateAsync();
            var roster = await CreateRosterAsync(new CreateRosterRequest { Name = "Roster", CreatorName = "Stachu", Description = "Random Description" });

            var createdRaiderResponse = await CreateRaiderAsync(new CreateRaiderRequest
            {
                Name = "Zarathustra",
                Class = "Mage",
                MainSpecialization = "Fire",
                OffSpecialization = "Frost",
                Role = "DamageDealer",
                RosterId = roster.Id
            });

            // Act
            var raiderResponse = await TestClient.DeleteAsync(ApiRoutes.Raiders.Update.Replace("{raiderId}", createdRaiderResponse.RaiderId));

            // Assert
            raiderResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_IfRaiderExistsButUserIsNotOwner_ReturnsForbiddenStatusCode()
        {
            // Arrange
            await AuthenticateAsync();
            var roster = await CreateRosterAsync(new CreateRosterRequest { Name = "Roster", CreatorName = "Stachu", Description = "Random Description" });

            var createdRaiderResponse = await CreateRaiderAsync(new CreateRaiderRequest
            {
                Name = "Zarathustra",
                Class = "Mage",
                MainSpecialization = "Fire",
                OffSpecialization = "Frost",
                Role = "DamageDealer",
                RosterId = roster.Id
            });

            await AuthenticateAsync("testnik@test.com", "Testnik");

            // Act
            var raiderResponse = await TestClient.DeleteAsync(ApiRoutes.Raiders.Update.Replace("{raiderId}", createdRaiderResponse.RaiderId));

            // Assert
            raiderResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_IfRaiderDoesNotExist_ReturnsNotFoundStatusCode()
        {
            // Arrange
            await AuthenticateAsync();

            var fakeRaider = new Raider()
            {
                RaiderId = Guid.NewGuid().ToString(),
                Class = "Warrior",
                Name = "Steven",
                MainSpecialization = "Fury",
                OffSpecialization = "Protection",
                Points = 250,
                Role = "DD"
            };

            // Act
            var raiderResponse = await TestClient.DeleteAsync(ApiRoutes.Raiders.Update.Replace("{raiderId}", fakeRaider.RaiderId));

            // Assert
            raiderResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
