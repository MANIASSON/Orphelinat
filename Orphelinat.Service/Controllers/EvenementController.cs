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
    public class EvenementController : BaseController
    {
        [HttpPost]
        public IHttpActionResult AjouterEvenement([FromBody] Evenement evenement)
        {
            try
            {
                var p = db.Evenements.FirstOrDefault(x => x.Libele.ToLower() == evenement.Libele.ToLower());
                if (p != null)
                    return Content(HttpStatusCode.Conflict, "Cet evenement existe déjà.");
                evenement.IdEvenement = 0;
                db.Evenements.Add(evenement);
                db.SaveChanges();
                return Ok(evenement);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> ListerEvenement(int IdOrphelinat)
        {
            try
            {
                var models = await db.Evenements.Where(x => x.IdOrphelinat == IdOrphelinat)
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
        public IHttpActionResult DetailsEvenement(int IdEvenement)
        {
            try
            {
                var evenement = db.Evenements.Find(IdEvenement);
                return Ok(evenement);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public IHttpActionResult SupprimerEvenement(int Id)
        {
            try
            {
                var evenement = db.Evenements.Find(Id);
                if (evenement == null)
                    return Content(HttpStatusCode.NotFound, "Cet evenement n'existe pas.");
                db.Evenements.Remove(evenement);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IHttpActionResult EditerEvenement([FromBody] Evenement newEvenement)
        {
            try
            {
                var oldEvenement = db.Evenements.AsNoTracking().FirstOrDefault(x => x.IdEvenement == newEvenement.IdEvenement);
                if (oldEvenement == null)
                    return Content(HttpStatusCode.NotFound, "L'evenement " + newEvenement.IdEvenement + " n'existe pas.");
                db.Entry(newEvenement).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Ok(newEvenement);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
