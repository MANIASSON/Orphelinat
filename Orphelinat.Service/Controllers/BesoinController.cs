using Orphelinat.Service.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Orphelinat.Service.Controllers
{
    public class BesoinController : BaseController
    {
        [HttpPost]
        public IHttpActionResult AjouterBesoin([FromBody] Besoin besoin)
        {
            try
            {
                var p = db.Besoins.FirstOrDefault(x => x.Libele.ToLower() == besoin.Libele.ToLower());
                if (p != null)
                    return Content(HttpStatusCode.Conflict, "Cette référence de besoin existe déjà.");
                besoin.IdBesoin = 0;
                db.Besoins.Add(besoin);
                db.SaveChanges();
                return Ok(besoin);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> ListerBesoin(int IdOrphelinat)
        {
            try
            {
                var models = await db.Besoins.Where(x => x.IdOrphelinat == IdOrphelinat)
                    .ToArrayAsync();
                return Ok(models);
            }
            catch (DbUpdateException ex)
            {
                var exception = ex.InnerException?.InnerException as SqlException;
                return BadRequest(exception?.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult DetailsBesoin(int IdBesoin)
        {
            try
            {
                var besoin = db.Besoins.Find(IdBesoin);
                return Ok(besoin);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public IHttpActionResult SupprimerBesoin(int Id)
        {
            try
            {
                var besoin = db.Besoins.Find(Id);
                if (besoin == null)
                    return Content(HttpStatusCode.NotFound, "Ce besoin n'existe pas.");
                db.Besoins.Remove(besoin);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IHttpActionResult EditerBesoin([FromBody] Besoin newBesoin)
        {
            try
            {
                var oldBesoin = db.Besoins.AsNoTracking().FirstOrDefault(x => x.IdBesoin == newBesoin.IdBesoin);
                if (oldBesoin == null)
                    return Content(HttpStatusCode.NotFound, "Le besoin " + newBesoin.IdBesoin + " n'existe pas.");
                db.Entry(newBesoin).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Ok(newBesoin);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
