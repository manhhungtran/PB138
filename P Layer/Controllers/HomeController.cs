using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using B_Layer.DTO;
using B_Layer.Facades;
using Microsoft.AspNet.Identity;
using P_Layer.Models;

namespace P_Layer.Controllers
{
    public class HomeController : Controller
    {
        PersonFacade personFacade = new PersonFacade();

        public ActionResult Index()
        {
            return View();
        }


        // GET: Family
        [Authorize]
        public ActionResult FamilyList()
        {
            var model = personFacade.GetAllPeople(int.Parse(User.Identity.GetUserId()));

            return View(model.Select(element => ModelMapping.Mapper.Map<PersonModel>(element)));
        }

        [Authorize]
        public ActionResult Create()
        {
            var people = personFacade.GetAllPeople(int.Parse(User.Identity.GetUserId()))
                .Select(element => ModelMapping.Mapper.Map<PersonModel>(element));

            ViewBag.Women = people
                .Where(person => !person.IsMale)
                .ToList();
            ViewBag.Men = people
                .Where(person => person.IsMale)
                .ToList();
            return View();
        }

        [Authorize]
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

            personFacade.CreatePerson(ModelMapping.Mapper.Map<PersonDTO>(person));

            return RedirectToAction("FamilyList");
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var person = personFacade.GetPerson(id);
            return View(ModelMapping.Mapper.Map<PersonModel>(person));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(int id, PersonModel person)
        {
            if (!ModelState.IsValid)
            {
                return View(person);
            }

            var originalPerson = personFacade.GetPerson(id);
            originalPerson.Name = person.Name;
            originalPerson.Surname = person.Surname;
            originalPerson.IsMale = person.IsMale;
            originalPerson.BirthDate = person.BirthDate;
            originalPerson.DeathDate = person.DeathDate;

            personFacade.UpdatePerson(originalPerson);

            return RedirectToAction("FamilyList");
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            personFacade.DeletePerson(id);
            return RedirectToAction("FamilyList");
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            var person = personFacade.GetPerson(id);
            return View(ModelMapping.Mapper.Map<PersonModel>(person));
        }

        [Authorize]
        public ActionResult SetPartner(int id)
        {
            var person = personFacade.GetPerson(id);

            var people = personFacade.GetAllPeople(int.Parse(User.Identity.GetUserId()))
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

        [Authorize]
        [HttpPost]
        public ActionResult SetPartner(int id, PersonModel person)
        {
            NameValueCollection nvc = Request.Form;

            int partnerId;

            bool result = int.TryParse(nvc["partner"], out partnerId);
            if (result)
            {
                personFacade.SetPartner(id, partnerId);
            }
            else
            {
                if (personFacade.GetPerson(id).PartnerId != null)
                {
                    personFacade.RemovePartner(ModelMapping.Mapper.Map<PersonDTO>(person));
                }
            }

            return RedirectToAction("FamilyList");
        }

        [Authorize]
        public ActionResult SetMother(int id)
        {
            ViewBag.Women = personFacade.GetAllPeople(int.Parse(User.Identity.GetUserId()))
                .Where(element => element.Id != id && !element.IsMale)
                .Select(element => ModelMapping.Mapper.Map<PersonModel>(element))
                .ToList();

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult SetMother(int id, PersonModel person)
        {
            NameValueCollection nvc = Request.Form;

            int motherId;

            bool result = int.TryParse(nvc["woman"], out motherId);
            if (result)
            {
                personFacade.SetMother(id, motherId);
            }
            else
            {
                if (personFacade.GetPerson(id).MotherId != null)
                {
                    personFacade.RemoveMother(id);
                }
            }

            return RedirectToAction("FamilyList");
        }

        [Authorize]
        public ActionResult SetFather(int id)
        {
            ViewBag.Women = personFacade.GetAllPeople(int.Parse(User.Identity.GetUserId()))
                .Where(element => element.Id != id && element.IsMale)
                .Select(element => ModelMapping.Mapper.Map<PersonModel>(element))
                .ToList();

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult SetFather(int id, PersonModel person)
        {
            NameValueCollection nvc = Request.Form;

            int fatherId;

            bool result = int.TryParse(nvc["woman"], out fatherId);
            if (result)
            {
                personFacade.SetMother(id, fatherId);
            }
            else
            {
                if (personFacade.GetPerson(id).FatherId != null)
                {
                    personFacade.RemoveFather(id);
                }
            }

            return RedirectToAction("FamilyList");
        }


        //public static string JsonSerialize(object o)
        //{
        //    JsonSerializerSettings settings = new JsonSerializerSettings();
        //    settings.NullValueHandling = NullValueHandling.Ignore;
        //    settings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

        //    return JsonConvert.SerializeObject(o, Formatting.Indented, settings);
        //}

        //public static StringWriter XmlSerialize(object o)
        //{
        //    var xmlSerializer = new XmlSerializer(o.GetType());
        //    var xml = new StringWriter();
        //    xmlSerializer.Serialize(xml, o);

        //    return xml;
        //}
    }
}