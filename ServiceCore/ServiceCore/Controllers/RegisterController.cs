using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using ServiceCore.Models;

namespace ServiceCore.Controllers
{
    [Route("api/[controller]")]
    public class RegisterController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            using (var context = new DataAccess.UsersContext())
            {
                int count = context.users.ToArray().Length;

                List<string> results = new List<string>();
                results.Add(count.ToString());

//                 foreach (var user in context.users)
//                 {
//                     results.Add(user.id.ToString());
//                 }

                return results.ToArray();
            }
                
        }

//         // GET api/values/5
//         [HttpGet("{id}")]
//         public string Get(int id)
//         {
//             using (var context = new DataAccess.UsersContext())
//             {
//                 var userswithEmail = context.users
//                                       .Where(s => s.id == id)
//                                       .ToList();
// 
//                 return userswithEmail.FirstOrDefault<User>()?.email;
//             }
//         }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]RegistrationRequest value)
        {
            using (var context = new DataAccess.UsersContext())
            {
                RegistrationResponce responce = new RegistrationResponce();

                if (value.Valid())
                {
                    var usr = new User();
                    usr.email = value.EmailAddress.ToLowerInvariant();
                    usr.hash = new PasswordHasher<User>().HashPassword(usr, value.Credentials);
                    context.users.Add(usr);

                    int result = context.SaveChanges();

                   

                    
                }
                else
                {
                    responce.OK = false;
                    responce.Result = "INVALID";
                }

                return Ok(responce);
            }

            return NotFound();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
