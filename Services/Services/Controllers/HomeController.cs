using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;

using Services.DAL;

namespace Services.Controllers
{
    public class HomeController : ApiController
    {
        public IHttpActionResult Index()
        {
            using (var context = new UserDBContext())
            {
                int count = context.Users.ToArray().Length;
            }

            return NotFound();
        }
    }
}
