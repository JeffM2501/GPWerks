using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;

namespace Services.Controllers
{
    public class HomeController : ApiController
    {
        public IHttpActionResult Index()
        {
            return NotFound();
        }
    }
}
