using Orphelinat.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Orphelinat.Service.Controllers
{
    public class BaseController : ApiController
    {
        protected OrphelinatEntities db = new OrphelinatEntities();
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
