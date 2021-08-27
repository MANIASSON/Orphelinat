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
    public class AccueilController : Controller
    {
        string LienOrphelinat = "http://localhost:81/Orphelinat.Service/api/Orphelinat";
        string LienDonateurApi = "http://localhost:81/Orphelinat.Service/api/Donateur";
        // GET: Accueil
        public async Task<ActionResult> Index()
        {
            IEnumerable<OrphelinatModel> models = new List<OrphelinatModel>();

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(LienOrphelinat);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    models = JsonConvert.DeserializeObject<IEnumerable<OrphelinatModel>>(json);
                }
            }
            return View(models);
        }

        public async Task<ActionResult> DetailsOrphelinat(int IdOrphelinat)
        {
            OrphelinatModel model = new OrphelinatModel();

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(LienOrphelinat + "?Id=" + IdOrphelinat);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<OrphelinatModel>(json);
                }
            }
            return View(model);
        }

        public ActionResult Ajouter(int IdOrphelinat)
        {
            DonateurModel model = new DonateurModel();
            model.IdOrphelinat = IdOrphelinat;
            Session["IdOrphelinat"] = IdOrphelinat;
            return View("Don", model);
        }

        [HttpPost]
        public async Task<ActionResult> Editer(DonateurModel model)
        {
            try
            {
                model.IdOrphelinat = (int)Session["IdOrphelinat"];

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
                            return RedirectToAction("DetailsOrphelinat", new { IdOrphelinat = model.IdOrphelinat });
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
    }
}