using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.V1.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.V1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //[HttpGet("api/v1/user")]
        //public IActionResult GetUserData()
        //{
        //    return Ok(new
        //    {
        //        name = "Joffrey",
        //        age = 26,
        //        City = "Boston",
        //        Country = "US"
        //    });
        //}

        //[HttpGet(ApiRoutes.Users.GetUser)]
        [HttpGet("User")]
        public IActionResult GetUser()
        {
            User user = new User();

            Random random = new Random();
            List<string> names = new List<string>
            {
                "Jeniffer", "Mia", "Angelina", "Sarah",
                "Elizabeth", "Eva", "Audrey", "Carmen",
                "Catherine", "Francesca", "Vanessa", "Melissa", "Jack"
            };

            List<string> cities = new List<string>
            {
                "Washington", "New York", "Chicago", "Los Angeles",
                "Seattle", "San Francisco", "San Diego", "Boston",
                "Las Vegas", "Detroit", "Dallas", "Austin"
            };

            int nameIndex = random.Next(names.Count);
            int cityIndex = random.Next(cities.Count);

            user.Id = Guid.NewGuid().ToString();
            user.Age = random.Next(18, 40);
            user.Name = names[nameIndex];
            user.City = cities[cityIndex];

            return Ok(user);
        }
    }
}