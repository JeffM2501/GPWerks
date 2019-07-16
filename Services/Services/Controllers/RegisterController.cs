using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Services.Models;

namespace Services.Controllers
{
    public class RegisterController : ApiController
    {
        public IHttpActionResult Get()
        {
            return NotFound();
        }

        // POST api/values
        public IHttpActionResult Post([FromBody]RegistrationRequest request)
        {
            RegistrationResponce responce = new RegistrationResponce();

            responce.Ok = true;
            responce.Result = request.Callsign;
            responce.Token = new Random().Next().ToString();

            return Ok(responce);
        }

    }
}
