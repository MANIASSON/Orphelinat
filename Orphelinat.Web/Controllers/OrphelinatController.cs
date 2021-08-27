using Newtonsoft.Json;
using Orphelinat.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Orphelinat.Web.Controllers
{
    public class OrphelinatController : Controller
    {
        string LienOrphelinat = "http://localhost:81/Orphelinat.Service/api/Orphelinat";

        // GET: Orphelinat
        public ActionResult Connexion(string returnUrl)
        {
            OrphelinatModel o = (OrphelinatModel)Session["Orphelinat"];
            if (o != null)
            {
                return RedirectToAction("Index", "Gestion");
            }
            return View(new ConnexionModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<ActionResult> Connexion(ConnexionModel model)
        {
            try
            {
                if (ModelState.IsValid)
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

                    foreach (var o in models)
                    {
                        if (o.Email.ToLower().Equals(model.Email, StringComparison.OrdinalIgnoreCase))
                        {
                            if (o.MotDePasse.Equals(model.MotDePasse, StringComparison.OrdinalIgnoreCase))
                            {
                                Session["Orphelinat"] = o;
                                Session["Image"] = o.Image;
                                Session["Nom"] = o.Nom;
                                return RedirectToAction("Index", "Gestion");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            model.IsError = true;
            model.Message = "Email ou mot de passe invalide !";
            return View(model);
        }

        public ActionResult Enregistrer(string returnUrl)
        {
            return View(new OrphelinatModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<ActionResult> Enregistrer(OrphelinatModel model)
        {
            try
            {
                MultipartFormDataContent multipart = new MultipartFormDataContent();

                if (ModelState.IsValid)
                {
                    var json = JsonConvert.SerializeObject(model);
                    StringContent content = new StringContent
                    (
                        json,
                        Encoding.UTF8,
                        "application/json"
                    );

                    multipart.Add(content, "data");

                    if (model.File.ContentLength > 0)
                    {
                        byte[] picture = new byte[model.File.ContentLength];
                        model.File.InputStream.Read(picture, 0, picture.Length);
                        ByteArrayContent byteContent = new ByteArrayContent(picture);
                        multipart.Add(byteContent, "picture", model.File.FileName);
                    }

                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.PostAsync(LienOrphelinat, multipart);
                        if (response.IsSuccessStatusCode)
                            return RedirectToAction("Connexion");
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

        public ActionResult Deconnexion()
        {
            Session.Clear();
            return RedirectToAction("Index", "Accueil");
        }

    }
}