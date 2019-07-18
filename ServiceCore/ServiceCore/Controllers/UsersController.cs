using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


using ServiceCore.Models;

namespace ServiceCore.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : Controller
    {
        public IActionResult Get(string token)
        {
            using (var context = new DataAccess.UserDBContext())
            {
                var user = context.FindUserByToken(token);
                if (user == null)
                    return NotFound();

                List<string> responce = new List<string>();

                responce.Add(user.email);
                foreach (var c in context.CallsignsForUser(user))
                    responce.Add(c.name);

                return Ok(responce);
            }
        }

        [HttpPut]
        public IActionResult Put(UserUpdateAction request)
        {
            using (var context = new DataAccess.UserDBContext())
            {
                var user = context.FindUserByToken(request.Token);
                if (user == null)
                    return Forbid();

                switch (request.EditAction)
                {
                    case UserUpdateAction.Actions.ChangeCallsign:
                        {
                            var callsign = context.CallsignsForUser(user).First(x => x.name.ToLowerInvariant() == request.OldValue.ToLowerInvariant());
                            if (callsign == null || request.NewValue.Length < 4)
                                return NotFound();

                            if (!context.CallsignAvailable(request.NewValue))
                                return NoContent();

                            callsign.name = request.NewValue;
                            context.SaveChanges();

                            return Ok(callsign.callsign_id);
                        }

                    case UserUpdateAction.Actions.AddCallsign:
                        {
                            var callsigns = context.CallsignsForUser(user);
                            if (callsigns.First(x => x.name.ToLowerInvariant() == request.NewValue.ToLowerInvariant()) != null)
                                return Forbid();

                            if (callsigns.Count() >= 3 || request.NewValue.Length < 4)
                                return NoContent();

                            if (!context.CallsignAvailable(request.NewValue))
                                return NoContent();

                            Callsign newCallsign = new Callsign();
                            newCallsign.user = user;
                            newCallsign.name = request.NewValue;
                            newCallsign.enabled = 1;
                            newCallsign.created = DateTime.Now;

                            context.callsigns.Add(newCallsign);
                            context.SaveChanges();

                            return Ok(newCallsign.callsign_id);
                        }


                    case UserUpdateAction.Actions.RemoveCallsign:
                        {
                            var callsign = context.CallsignsForUser(user).First(x => x.name.ToLowerInvariant() == request.OldValue.ToLowerInvariant());
                            if (callsign == null)
                                return NotFound();

                            callsign.enabled = 0;
                            context.SaveChanges();

                            return Ok(callsign.callsign_id);
                        }

                    case UserUpdateAction.Actions.VerifyEmail:
                        {
                            if (user.verified != 0 || user.verification_token != request.NewValue)
                                return NotFound();

                            user.verified = 1;
                            user.verification_token = string.Empty;
                            context.SaveChanges();

                            return Ok(user.email);
                        }

                    case UserUpdateAction.Actions.ChangePassword:
                        {
                            var hasher = new PasswordHasher<User>();
                            var result = hasher.VerifyHashedPassword(user, user.hash, request.OldValue);
                            if (result == PasswordVerificationResult.Failed)
                                return Forbid();

                            user.hash = hasher.HashPassword(user, request.NewValue);
                            context.SaveChanges();
                            return Ok(user.user_id);
                        }


                    case UserUpdateAction.Actions.ChangeEmail:
                        {
                            var existingUser = context.FindByEmail(request.NewValue);
                            if (existingUser != null || user.email != request.OldValue.ToLowerInvariant())
                                return Forbid();

                            if (!request.NewValue.Contains("@") || !request.NewValue.Contains("."))
                                return NotFound();

                            user.email = request.NewValue;
                            user.verified = 0;
                            user.verification_token = context.GenEmailToken();
                            context.SaveChanges();

                            Emailer.SendVerificationEmail(user);

                            return Ok(user.email);
                        }

                    case UserUpdateAction.Actions.RecoverPassword:
                        {
                            if (user.verification_token != request.OldValue)
                                return Forbid();

                            var hasher = new PasswordHasher<User>();
 
                            user.hash = hasher.HashPassword(user, request.NewValue);
                            user.verified = 1;
                            user.verification_token = string.Empty;
                            context.SaveChanges();
                            return Ok(user.user_id);
                        }

                }

                return NoContent();
            }
        }
    }
}