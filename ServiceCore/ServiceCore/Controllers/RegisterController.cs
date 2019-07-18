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
            using (var context = new DataAccess.UserDBContext())
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
            int newUserID = -1;
            using (var context = new DataAccess.UserDBContext())
            {
                RegistrationResponce responce = new RegistrationResponce();

                if (value.Valid())
                {
                    value.EmailAddress = value.EmailAddress.ToLowerInvariant();
                    if (context.FindByEmail(value.EmailAddress) == null)
                    {
                        var usr = new User();
                        usr.email = value.EmailAddress.ToLowerInvariant();
                        usr.hash = new PasswordHasher<User>().HashPassword(usr, value.Credentials);
                        usr.enabled = 1;
                        usr.verified = 0;
                        usr.verification_token = context.GenEmailToken();
                        context.users.Add(usr);

                        int result = context.SaveChanges();

                        newUserID = usr.user_id;

                        var callsign = new Callsign();
                        callsign.enabled = 1;
                        callsign.name = value.Callsign;
                        callsign.created = DateTime.Now;
                        callsign.user = usr;

                        context.callsigns.Add(callsign);
                        context.SaveChanges();

                        responce.CallsignID = callsign.callsign_id;

                        AccessToken tok = new AccessToken();
                        tok.user = usr;
                        tok.expires = DateTime.Now + new TimeSpan(0, 5, 0);
                        tok.token = context.GetAccessToken();
                        tok.callsign_index = callsign.callsign_id;
                        context.access_tokens.Add(tok);
                        context.SaveChanges();

                        responce.OK = true;
                        responce.Token = tok.token;
                        responce.Result = callsign.name;

                        Emailer.SendVerificationEmail(usr);
                    }
                    else
                    {
                        responce.OK = false;
                        responce.Result = "UNAVAILBLE";
                    }
                }
                else
                {
                    responce.OK = false;
                    responce.Result = "INVALID";
                }

                if (responce.OK)
                    return Ok(responce);
                else
                    return NotFound(responce);
            }

            return Forbid();
        }
// 
//         // PUT api/values/5
//         [HttpPut("{id}")]
//         public void Put(int id, [FromBody]string value)
//         {
//         }
// 
//         // DELETE api/values/5
//         [HttpDelete("{id}")]
//         public void Delete(int id)
//         {
//         }
    }
}
