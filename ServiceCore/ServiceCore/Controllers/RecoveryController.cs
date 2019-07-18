using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServiceCore.Controllers
{
    [Produces("application/json")]
    [Route("api/Recovery")]
    public class RecoveryController : Controller
    {
        [HttpPut]
        public IActionResult Put([FromBody]string email)
        {
            using (var context = new DataAccess.UserDBContext())
            {
                var user = context.FindByEmail(email);
                if (user == null || user.IsAnon())
                    return Forbid();

                user.verification_token = context.GenEmailToken();
                context.SaveChanges();

                return Ok(Emailer.SendPasswordResetEmail(user));
            }
        }
    }
}