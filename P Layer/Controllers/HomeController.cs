using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using B_Layer.DTO;
using B_Layer.Facades;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using P_Layer.Models;

namespace P_Layer.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        readonly PersonFacade _personFacade = new PersonFacade();

        public ActionResult Index()
        {
            return View();
        }


        // GET: Family
        public ActionResult Table()
        {
            var model = _personFacade.GetAllPeople(User.Identity.GetUserId<int>());

            return View(model.Select(element => ModelMapping.Mapper.Map<PersonModel>(element)));
        }
        
        public ActionResult Create()
        {
            var people = _personFacade.GetAllPeople(User.Identity.GetUserId<int>())
                .Select(element => ModelMapping.Mapper.Map<PersonModel>(element));

            ViewBag.Women = people
                .Where(person => !person.IsMale)
                .ToList();
            ViewBag.Men = people
                .Where(person => person.IsMale)
                .ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(PersonModel person)
        {
            NameValueCollection nvc = Request.Form;

            int motherId;
            int fatherId;

            bool result = int.TryParse(nvc["woman"], out motherId);
            if (result)
            {
                person.MotherId = motherId;
            }

            result = int.TryParse(nvc["man"], out fatherId);
            if (result)
            {
                person.FatherId = fatherId;
            }

            person.UserId = User.Identity.GetUserId<int>();

            _personFacade.CreatePerson(ModelMapping.Mapper.Map<PersonDTO>(person));

            return RedirectToAction("Table");
        }
        
        public ActionResult Edit(int id)
        {
            var person = _personFacade.GetPerson(id);

            var people = _personFacade.GetAllPeople(User.Identity.GetUserId<int>())
                .Where(element => element.Id != id && element.MotherId != id && element.FatherId != id)
                .Select(element => ModelMapping.Mapper.Map<PersonModel>(element));

            ViewBag.Women = people
                .Where(woman => !woman.IsMale)
                .ToList();
            ViewBag.Men = people
                .Where(man => man.IsMale)
                .ToList();

            return View(ModelMapping.Mapper.Map<PersonModel>(person));
        }
        
        [HttpPost]
        public ActionResult Edit(int id, PersonModel person)
        {
            var originalPerson = _personFacade.GetPerson(id);

            NameValueCollection nvc = Request.Form;

            int motherId;
            int fatherId;

            bool result = int.TryParse(nvc["woman"], out motherId);
            if (result)
            {
                originalPerson.MotherId = motherId;
            }
            else
            {
                originalPerson.MotherId = null;
            }

            result = int.TryParse(nvc["man"], out fatherId);
            if (result)
            {
                originalPerson.FatherId = fatherId;
            }
            else
            {
                originalPerson.FatherId = null;
            }

            originalPerson.Name = person.Name;
            originalPerson.Surname = person.Surname;
            originalPerson.IsMale = person.IsMale;
            originalPerson.BirthDate = person.BirthDate;
            originalPerson.DeathDate = person.DeathDate;

            _personFacade.UpdatePerson(originalPerson);

            return RedirectToAction("Table");
        }
        
        public ActionResult Delete(int id)
        {
            _personFacade.DeletePerson(id);
            return RedirectToAction("Table");
        }
        
        public ActionResult Details(int id)
        {
            var person = _personFacade.GetPerson(id);
            return View(ModelMapping.Mapper.Map<PersonModel>(person));
        }
        
        public ActionResult SetPartner(int id)
        {
            var person = _personFacade.GetPerson(id);

            var people = _personFacade.GetAllPeople(int.Parse(User.Identity.GetUserId()))
                .Select(element => ModelMapping.Mapper.Map<PersonModel>(element));

            if (person.IsMale)
            {
                ViewBag.Partners = people
                    .Where(partner => partner.IsMale)
                    .ToList();
            }
            else
            {
                ViewBag.Partners = people
                    .Where(partner => !partner.IsMale)
                    .ToList();
            }

            return View();
        }
        
        [HttpPost]
        public ActionResult SetPartner(int id, PersonModel person)
        {
            NameValueCollection nvc = Request.Form;

            int partnerId;

            bool result = int.TryParse(nvc["partner"], out partnerId);
            if (result)
            {
                _personFacade.SetPartner(id, partnerId);
            }
            else
            {
                if (_personFacade.GetPerson(id).PartnerId != null)
                {
                    _personFacade.RemovePartner(ModelMapping.Mapper.Map<PersonDTO>(person));
                }
            }

            return RedirectToAction("Table");
        }

        public ActionResult Graph()
        {
            var people = _personFacade.GetAllPeople(int.Parse(User.Identity.GetUserId()))
                .Select(element => ModelMapping.Mapper.Map<PersonModel>(element))
                .ToList();
                
            return View(JsonSerialize(people));
        }

        private static string JsonSerialize(List<PersonModel> people)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore};

            return JsonConvert.SerializeObject(people, settings);
        }

        public ActionResult XML()
        {
            var people = _personFacade.GetAllPeople(int.Parse(User.Identity.GetUserId()))
                .Select(element => ModelMapping.Mapper.Map<PersonModel>(element));

            return View(XmlSerialize(people));
        }

        private static StringWriter XmlSerialize(object o)
        {
            var xmlSerializer = new XmlSerializer(o.GetType());
            var xml = new StringWriter();
            xmlSerializer.Serialize(xml, o);

            return xml;
        }
    }
}