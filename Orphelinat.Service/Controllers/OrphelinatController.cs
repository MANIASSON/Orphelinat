using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Orphelinat.Service.Controllers
{
    public class OrphelinatController : BaseController
    {
        [HttpPost]
        public async Task<IHttpActionResult> AjouterOrphelinat()
        {
            try
            {
                var json = HttpContext.Current.Request.Form["data"];
                Entities.Orphelinat orphelinat = JsonConvert.DeserializeObject<Entities.Orphelinat>(json);

                string uploadFolder = HttpContext.Current.Server.MapPath("~/Uploads");
                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    var file = HttpContext.Current.Request.Files[0];
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    file.SaveAs(Path.Combine(uploadFolder, filename));
                    orphelinat.Image = filename;
                }

                orphelinat.IdOrphelinat = 0;
                db.Orphelinats.Add(orphelinat);
                await db.SaveChangesAsync();
                return Ok(orphelinat);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> ListerOrphelinat()
        {
            try
            {
                var models = await db.Orphelinats
                    .OrderByDescending(x => x.DateCreation)
                    .ToArrayAsync();
                foreach (var p in models)
                {
                    p.Image = Url.Content("~/Uploads/" + p.Image).ToString();
                }
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
        public async Task<IHttpActionResult> DetailsOrphelinat(int Id)
        {
            try
            {
                var models = await db.Orphelinats.FindAsync(Id);
                models.Image = Request.RequestUri.GetLeftPart(UriPartial.Authority) +
                    "/Uploads/" + models.Image;
                return Ok(models);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IHttpActionResult> SupprimerOrphelinat(int Id)
        {
            try
            {
                var model = db.Orphelinats.Find(Id);
                if (model == null)
                    return Content(HttpStatusCode.NotFound, "Cet orphelinat n'existe pas.");
                db.Orphelinats.Remove(model);
                await db.SaveChangesAsync();

                string uploadFolder = HttpContext.Current.Server.MapPath("~/Uploads");
                if (!string.IsNullOrEmpty(model.Image) &&
                        File.Exists(Path.Combine(uploadFolder, model.Image)))
                {
                    File.Delete(Path.Combine(uploadFolder, model.Image));
                }


                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IHttpActionResult EditerOrphelinat()
        {
            try
            {
                var json = HttpContext.Current.Request.Form["data"];
                Entities.Orphelinat newOrphelinat = JsonConvert.DeserializeObject<Entities.Orphelinat>(json);

                var oldOrphelinat = db.Orphelinats.AsNoTracking().FirstOrDefault(x => x.IdOrphelinat == newOrphelinat.IdOrphelinat);
                if (oldOrphelinat == null)
                    return Content(HttpStatusCode.NotFound, "Cet orphelinat n'existe pas.");

                string uploadFolder = HttpContext.Current.Server.MapPath("~/Uploads");
                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    if (!string.IsNullOrEmpty(newOrphelinat.Image) &&
                        File.Exists(Path.Combine(uploadFolder, oldOrphelinat.Image)))
                    {
                        File.Delete(Path.Combine(uploadFolder, oldOrphelinat.Image));
                    }
                    var file = HttpContext.Current.Request.Files[0];
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    file.SaveAs(Path.Combine(uploadFolder, filename));
                    newOrphelinat.Image = filename;
                }
                else
                {
                    newOrphelinat.Image = oldOrphelinat.Image;
                }
                newOrphelinat.DateCreation = oldOrphelinat.DateCreation;
                db.Entry(newOrphelinat).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Ok(newOrphelinat);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}