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
    public class DonateurController : Controller
    {
        string LienDonateurApi = "http://localhost:81/Orphelinat.Service/api/Donateur";
        static DonateurModel donateurModel = new DonateurModel();
        // GET: Donateur
        public async Task<ActionResult> Lister()
        {
            IEnumerable<DonateurModel> model = new List<DonateurModel>();
            OrphelinatModel orphelinat = (OrphelinatModel)Session["Orphelinat"];

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(LienDonateurApi + "?IdOrphelinat=" + orphelinat.IdOrphelinat);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<IEnumerable<DonateurModel>>(json);
                }
            }
            return View(model);
        }

        public async Task<ActionResult> ListeSupprimer()
        {
            IEnumerable<DonateurModel> model = new List<DonateurModel>();
            OrphelinatModel orphelinat = (OrphelinatModel)Session["Orphelinat"];

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(LienDonateurApi + "?IdOrphelinat=" + orphelinat.IdOrphelinat);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<IEnumerable<DonateurModel>>(json);
                }
            }
            return View(model);
        }

        public ActionResult Ajouter()
        {
            DonateurModel model = new DonateurModel();
            OrphelinatModel orphelinat = (OrphelinatModel)Session["Orphelinat"];
            model.IdOrphelinat = orphelinat.IdOrphelinat;
            model.Orphelinat = model.Orphelinat = orphelinat;

            return View("Editer", model);
        }

        [HttpPost]
        public async Task<ActionResult> Editer(DonateurModel model)
        {
            try
            {
                OrphelinatModel orphelinat = (OrphelinatModel)Session["Orphelinat"];
                model.IdOrphelinat = orphelinat.IdOrphelinat;
                model.IdDonateur = donateurModel.IdDonateur;

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
                        if (model.IdDonateur == 0)
                        {
                            response = await client.PostAsync(LienDonateurApi, content);
                        }
                        else
                        {
                            response = await client.PutAsync(LienDonateurApi, content);
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
                var response = await client.DeleteAsync(LienDonateurApi + "?Id=" + Id);
            }
            return RedirectToAction("ListeSupprimer");
        }
    }
}