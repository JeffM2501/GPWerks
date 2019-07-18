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
                return results.ToArray();
            }
                
        }


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
                    if (context.FindByEmail(value.EmailAddress) != null)
                    {
                        var usr = new User();
                        var callsign = new Callsign();

                        if (!string.IsNullOrEmpty(value.AnonKey))
                        {
                            // they want to upgrade an anon callsign so don't make a new user or callsign
                            var oldUser = context.FindByEmail(value.Callsign);
                            if (oldUser != null && oldUser.IsAnon() && oldUser.hash == value.AnonKey)
                            {
                                usr = oldUser;
                                callsign = context.CallsignsForUser(usr).FirstOrDefault();

                                usr.email = value.EmailAddress.ToLowerInvariant();
                                usr.verification_token = context.GenEmailToken();
                                usr.hash = new PasswordHasher<User>().HashPassword(usr, value.Credentials);
                                usr.verified = 0;

                                newUserID = usr.user_id;
                                context.SaveChanges();
                            }
                            else // bad upgrade, make them resubmit as a new registration
                            {
                                return Forbid();
                            }
                        }
                        else
                        {
                            usr.email = value.EmailAddress.ToLowerInvariant();
                            usr.hash = new PasswordHasher<User>().HashPassword(usr, value.Credentials);
                            usr.enabled = 1;
                            usr.created = DateTime.Now;
                            usr.verified = 0;
                            usr.verification_token = context.GenEmailToken();
                            context.users.Add(usr);

                            context.SaveChanges();

                            newUserID = usr.user_id;
                            
                            callsign.enabled = 1;
                            callsign.name = value.Callsign;
                            callsign.created = DateTime.Now;
                            callsign.user = usr;

                            context.callsigns.Add(callsign);
                            context.SaveChanges();
                        }

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
        }
    }
}
