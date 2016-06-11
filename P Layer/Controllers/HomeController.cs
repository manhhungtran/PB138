using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
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
            ViewBag.Partner = people
                .Where(partner => (person.IsMale ? !partner.IsMale : partner.IsMale) &&
                    partner.Id != person.FatherId && partner.Id != person.MotherId)
                .ToList();
            return View(ModelMapping.Mapper.Map<PersonModel>(person));
        }
        
        [HttpPost]
        public ActionResult Edit(int id, PersonModel person)
        {
            var originalPerson = _personFacade.GetPerson(id);

            originalPerson.Name = person.Name;
            originalPerson.Surname = person.Surname;
            originalPerson.IsMale = person.IsMale;
            originalPerson.BirthDate = person.BirthDate;
            originalPerson.DeathDate = person.DeathDate;
            if (person.FatherId != null)
            {
                originalPerson.FatherId = person.FatherId;
            }
            if (person.MotherId != null)
            {
                originalPerson.MotherId = person.MotherId;
            }
            if (person.PartnerId != null)
            {
                originalPerson.PartnerId = person.PartnerId;
            }
            
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

        public ActionResult RemoveMother(int id, string action)
        {
            if (_personFacade.GetPerson(id).MotherId == null)
            {
                return RedirectToAction(action, new { id });
            }

            _personFacade.RemoveMother(id);
            return RedirectToAction(action, new { id });
        }
        public ActionResult RemoveFather(int id, string action)
        {
            if (_personFacade.GetPerson(id).FatherId == null)
            {
                return RedirectToAction(action, new { id });
            }

            _personFacade.RemoveFather(id);
            return RedirectToAction(action, new {id});
        }
        public ActionResult RemovePartner(int id, string action)
        {
            if (_personFacade.GetPerson(id).PartnerId == null)
            {
                return RedirectToAction(action, new { id });
            }

            _personFacade.RemovePartner(id);
            return RedirectToAction(action, new { id });
        }

        public ActionResult Graph()
        {
            var people = _personFacade.GetAllPeople(int.Parse(User.Identity.GetUserId()))
                .Select(element => ModelMapping.Mapper.Map<PersonModel>(element))
                .ToList();

            JsonSerializerSettings settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

            string neco = JsonConvert.SerializeObject(people, settings);
            ViewBag.neco = neco;
            return View();
        }

        public ActionResult XML()
        {
            var people = _personFacade.GetAllPeople(User.Identity.GetUserId<int>())
                .Select(element => ModelMapping.Mapper.Map<PersonModel>(element))
                .ToList();

            return View(XMLSerialize(people));
        }

        private StringWriter XMLSerialize(object o)
        {
            var xmlSerializer = new XmlSerializer(o.GetType());
            var xml = new StringWriter();
            xmlSerializer.Serialize(xml, o);

            return xml;
        }
    }

    public class BoolConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue((bool)value ? "true" : "false");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value.ToString() == "true";
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }
    }
}