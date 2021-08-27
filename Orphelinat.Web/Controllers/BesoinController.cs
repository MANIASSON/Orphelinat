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
    public class BesoinController : Controller
    {
        string LienBesoinApi = "http://localhost:81/Orphelinat.Service/api/Besoin";
        static BesoinModel besoinModel = new BesoinModel();
        // GET: Besoin
        public async Task<ActionResult> Lister()
        {
            IEnumerable<BesoinModel> model = new List<BesoinModel>();
            OrphelinatModel orphelinat = (OrphelinatModel)Session["Orphelinat"];

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(LienBesoinApi + "?IdOrphelinat=" + orphelinat.IdOrphelinat);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<IEnumerable<BesoinModel>>(json);
                }
            }
            return View(model);
        }

        public async Task<ActionResult> ListeEditer()
        {
            IEnumerable<BesoinModel> model = new List<BesoinModel>();
            OrphelinatModel orphelinat = (OrphelinatModel)Session["Orphelinat"];

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(LienBesoinApi + "?IdOrphelinat=" + orphelinat.IdOrphelinat);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<IEnumerable<BesoinModel>>(json);
                }
            }
            return View(model);
        }

        public async Task<ActionResult> ListeSupprimer()
        {
            IEnumerable<BesoinModel> model = new List<BesoinModel>();
            OrphelinatModel orphelinat = (OrphelinatModel)Session["Orphelinat"];

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(LienBesoinApi + "?IdOrphelinat=" + orphelinat.IdOrphelinat);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<IEnumerable<BesoinModel>>(json);
                }
            }
            return View(model);
        }

        public ActionResult Ajouter()
        {
            BesoinModel model = new BesoinModel();
            OrphelinatModel orphelinat = (OrphelinatModel)Session["Orphelinat"];
            model.IdOrphelinat = orphelinat.IdOrphelinat;
            model.Orphelinat = model.Orphelinat = orphelinat;

            return View("Editer", model);
        }

        public async Task<ActionResult> Editer(int Id)
        {
            BesoinModel model = new BesoinModel();
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(LienBesoinApi + "?IdBesoin=" + Id);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<BesoinModel>(json);
                }
            }
            besoinModel = model;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Editer(BesoinModel model)
        {
            try
            {
                OrphelinatModel orphelinat = (OrphelinatModel)Session["Orphelinat"];
                model.IdOrphelinat = orphelinat.IdOrphelinat;
                model.IdBesoin = besoinModel.IdBesoin;

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
                        if (model.IdBesoin == 0)
                        {
                            response = await client.PostAsync(LienBesoinApi, content);
                        }
                        else
                        {
                            response = await client.PutAsync(LienBesoinApi, content);
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
                var response = await client.DeleteAsync(LienBesoinApi + "?Id=" + Id);
            }
            return RedirectToAction("ListeSupprimer");
        }
    }
}