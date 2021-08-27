using Newtonsoft.Json;
using Orphelinat.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Orphelinat.Web.Controllers
{
    public class EvenementController : Controller
    {
        string LienEvenementApi = "http://localhost:81/Orphelinat.Service/api/Evenement";
        static EvenementModel evenementModel = new EvenementModel();
        // GET: Evenement
        public async Task<ActionResult> Lister()
        {
            IEnumerable<EvenementModel> model = new List<EvenementModel>();
            OrphelinatModel orphelinat = (OrphelinatModel)Session["Orphelinat"];

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(LienEvenementApi + "?IdOrphelinat=" + orphelinat.IdOrphelinat);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<IEnumerable<EvenementModel>>(json);
                }
            }
            return View(model);
        }

        public async Task<ActionResult> ListeEditer()
        {
            IEnumerable<EvenementModel> model = new List<EvenementModel>();
            OrphelinatModel orphelinat = (OrphelinatModel)Session["Orphelinat"];

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(LienEvenementApi + "?IdOrphelinat=" + orphelinat.IdOrphelinat);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<IEnumerable<EvenementModel>>(json);
                }
            }
            return View(model);
        }

        public async Task<ActionResult> ListeSupprimer()
        {
            IEnumerable<EvenementModel> model = new List<EvenementModel>();
            OrphelinatModel orphelinat = (OrphelinatModel)Session["Orphelinat"];

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(LienEvenementApi + "?IdOrphelinat=" + orphelinat.IdOrphelinat);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<IEnumerable<EvenementModel>>(json);
                }
            }
            return View(model);
        }

        public ActionResult Ajouter()
        {
            EvenementModel model = new EvenementModel();
            OrphelinatModel orphelinat = (OrphelinatModel)Session["Orphelinat"];
            model.IdOrphelinat = orphelinat.IdOrphelinat;
            model.Orphelinat = model.Orphelinat = orphelinat;

            return View("Editer", model);
        }

        public async Task<ActionResult> Editer(int Id)
        {
            EvenementModel model = new EvenementModel();
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(LienEvenementApi + "?IdEvenement=" + Id);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<EvenementModel>(json);
                }
            }
            evenementModel = model;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Editer(EvenementModel model)
        {
            try
            {
                OrphelinatModel orphelinat = (OrphelinatModel)Session["Orphelinat"];
                model.IdOrphelinat = orphelinat.IdOrphelinat;
                model.IdEvenement = evenementModel.IdEvenement;

                if (ModelState.IsValid)
                {
                    var json = JsonConvert.SerializeObject(model);
                    StringContent content = new StringContent
                    (
                        json,
                        Encoding.UTF8,
                        "application/json"
                    );

                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response;
                        if (model.IdEvenement == 0)
                        {
                            response = await client.PostAsync(LienEvenementApi, content);
                        }
                        else
                        {
                            response = await client.PutAsync(LienEvenementApi, content);
                        }

                        if (response.IsSuccessStatusCode)
                            return RedirectToAction("Lister");
                        else
                            ModelState.AddModelError("", await response.Content.ReadAsStringAsync());
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(model);
        }

        public async Task<ActionResult> Supprimer(int Id)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.DeleteAsync(LienEvenementApi + "?Id=" + Id);
            }
            return RedirectToAction("ListeSupprimer");
        }
    }
}