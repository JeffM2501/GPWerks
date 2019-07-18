using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using ServiceCore.Models;

namespace ServiceCore.Controllers
{
    [Produces("application/json")]
    [Route("api/Authenticate")]
    public class AuthenticateController : Controller
    {
        [HttpPost]
        public IActionResult Post([FromBody]AuthenticationRequest value)
        {
            using (var context = new DataAccess.UserDBContext())
            {
                var user = context.FindByEmail(value.EmailAddress);
                if (user == null)
                    return Forbid();

                var hasher = new PasswordHasher<User>();
                var result = hasher.VerifyHashedPassword(user, user.hash, value.Credentials);
                if (result == PasswordVerificationResult.Failed)
                    return Forbid();

                if (result == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    user.hash = hasher.HashPassword(user, value.Credentials);
                    context.SaveChangesAsync();
                }

                foreach (var token in context.AccessTokensForUser(user))
                {
                    token.expires = DateTime.MinValue;
                    context.SaveChangesAsync();
                }

                AccessToken tok = new AccessToken();
                tok.user = user;
                tok.expires = DateTime.Now + new TimeSpan(0, 5, 0);
                tok.token = context.GetAccessToken();
                context.access_tokens.Add(tok);
                context.SaveChanges();

                return Ok( new string[] { tok.token, tok.callsign_index.ToString() });
            }
        }
    }
}