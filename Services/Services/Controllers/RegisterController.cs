﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using Services.Models;
using Services.DAL;

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
            using (var context = new UserDBContext())
            {
            //    int count = context.Users.ToArray().Length;
                var query = from usr in context.users where usr.email == request.EmailAddress.ToLowerInvariant() select usr;

                var user = query.FirstOrDefault<User>();
                RegistrationResponce responce = new RegistrationResponce();
                if (user == null && request.Valid())
                {
                //    Hasher.GenHash(request.Credentials)
                    responce.Ok = true;
                    responce.Result = request.Callsign;
                    responce.Token = new Random().Next().ToString();
                }
                else
                {
                    responce.Ok = false;
                    responce.Result = "Unavailable";
                }

                return Ok(responce);
            }
             
        }

    }
}
