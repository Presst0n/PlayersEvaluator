﻿using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using PE.API;
using PE.API.Data;
using PE.Contracts.V1;
using PE.Contracts.V1.Requests;
using PE.Contracts.V1.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PE.IntegrationTests
{
    public class RosterIntegrationTest : IDisposable
    {
        protected readonly HttpClient TestClient;
        private readonly IServiceProvider _serviceProvider;

        protected RosterIntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DataContext));

                        var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<DataContext>));
                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        services.AddDbContext<DataContext>(options =>
                        {
                            options.UseInMemoryDatabase("MyTestDB");
                        });
                    });
                });
            _serviceProvider = appFactory.Services;
            TestClient = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync(string email = null, string name = null)
        {
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync(email, name));
        }

        protected async Task<RosterResponse> CreateRosterAsync(CreateRosterRequest request)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Rosters.Create, request);
            return await response.Content.ReadAsAsync<RosterResponse>();
        }

        protected async Task CreateRostersAsync(List<CreateRosterRequest> request)
        {
            foreach (var item in request)
            {
                await TestClient.PostAsJsonAsync(ApiRoutes.Rosters.Create, item);
            }
        }

        protected async Task<RaiderResponse> CreateRaiderAsync(CreateRaiderRequest request)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Raiders.Create, request);
            return await response.Content.ReadAsAsync<RaiderResponse>();
        }

        protected HttpContent CreateHttpContent<T>(T data) where T : class
        {
            return new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        } 

        private async Task<string> GetJwtAsync(string email = null, string name = null)
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Identity.Register, new UserRegistrationRequest
            {
                Email = !string.IsNullOrEmpty(email) ? email : "ttt@ttt.com",
                Password = "TestPassword255.",
                UserName = !string.IsNullOrEmpty(name) ? name : "JohnnyTest"
            });

            var registrationResponse = await response.Content.ReadAsAsync<AuthSuccessResponse>();
            return registrationResponse.Token;
        }

        public void Dispose()
        {
            using var serviceScope = _serviceProvider.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<DataContext>();
            context.Database.EnsureDeleted();
        }
    }
}