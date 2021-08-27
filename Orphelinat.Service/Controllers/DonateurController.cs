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
    public class DonateurController : BaseController
    {
        [HttpPost]
        public IHttpActionResult AjouterDonateur([FromBody] Donateur donateur)
        {
            try
            {
                donateur.IdDonateur = 0;
                db.Donateurs.Add(donateur);
                db.SaveChanges();
                return Ok(donateur);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> ListerDonateur(int IdOrphelinat)
        {
            try
            {
                var models = await db.Donateurs.Where(x => x.IdOrphelinat == IdOrphelinat)
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

        [HttpDelete]
        public IHttpActionResult SupprimerDonateur(int Id)
        {
            try
            {
                var donateur = db.Donateurs.Find(Id);
                if (donateur == null)
                    return Content(HttpStatusCode.NotFound, "Ce donateur n'existe pas.");
                db.Donateurs.Remove(donateur);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
