using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using B_Layer.DTO;
using DA_Layer;
using DA_Layer.Entities;
 
namespace B_Layer.Facades
{
    public class PersonFacade
    {
        /// <summary>
        /// Method CrreatePerson creates Person and inserts it to database
        /// </summary>
        /// <param name="person">Person to be created</param>
        public void CreatePerson(PersonDTO person)
        {
            if (person == null) { throw new ArgumentNullException(nameof(person)); }

            if (person.BirthDate >= person.DeathDate)
            {
                throw new InvalidOperationException("Birth Date must be before Death Date");
            }

            Person newPerson = Mapping.Mapper.Map<Person>(person);

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                context.People.Add(newPerson);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Method GetPerson retrieves Person from database based on id
        /// </summary>
        /// <param name="id">ID of wanted Person</param>
        /// <returns>PersonDTO of found Person</returns>
        public PersonDTO GetPerson(int id)
        {
            if (id <= 0) { throw new ArgumentException("Id must be greater than 0, was " + id); }

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var person = context.People
                    .FirstOrDefault(c => c.Id == id);

                if (person == null)
                {
                    throw new InvalidOperationException("Person with id " + id
                        + " was not found in database");
                }

                return Mapping.Mapper.Map<PersonDTO>(person);
            }
        }

        /// <summary>
        /// Retrieves all people from database, depending on currently logged user.
        /// </summary>
        /// <param name="id">Identification of currently logged user</param>
        /// <returns>List of people</returns>
        public List<PersonDTO> GetAllPeople(int id)
        {
            using (var context = new AppDbContext())
            {
                var students = context.People
                    .Where(person => person.UserId == id)
                    .ToList();

                return students
                    .Select(person => Mapping.Mapper.Map<PersonDTO>(person))
                    .ToList();
            }
        }

        /// <summary>
        /// Updates person in database.
        /// </summary>
        /// <param name="person">Person to be updated</param>
        public void UpdatePerson(PersonDTO person)
        {
            if (person == null) { throw new ArgumentNullException(nameof(person)); }

            Person personToBeUpdated = Mapping.Mapper.Map<Person>(person);
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                if (personToBeUpdated.PartnerId != null)
                {
                    Person partner = context.People.Find(personToBeUpdated.PartnerId);
                    partner.PartnerId = person.Id;
                }
                context.Entry(personToBeUpdated).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Method DeletePerson deletes person from database
        /// </summary>
        /// <param name="id">ID of Person to be deleted</param>
        public void DeletePerson(int id)
        {
            if (id <= 0) { throw new ArgumentException("ID must be greater than 0, was " + id); }

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;

                var person = context.People.Find(id);
                
                if (person == null)
                {
                    throw new InvalidOperationException("Person with id " + id
                        + " was not found in database");
                }

                var partner = context.People.Find(person.PartnerId);

                if (partner != null)
                {
                    partner.PartnerId = null;
                }

                foreach (var child in context.People)
                {
                    if (person.IsMale)
                    {
                        if (child.FatherId == id)
                        {
                            child.FatherId = null;
                        }
                    }
                    else
                    {
                        if (child.MotherId == id)
                        {
                            child.MotherId = null;
                        }
                    }
                }


                context.People.Remove(person);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Method RemovePartner removes person's mother
        /// </summary>
        /// <param name="personId">ID of person which mother is going to be removed</param>
        public void RemoveMother(int personId)
        {
            if (personId <= 0)
            {
                throw new ArgumentException("ID must be greater than 0, was " + personId);
            }

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var person = context.People
                    .FirstOrDefault(c => c.Id == personId);

                if (person == null)
                {
                    throw new InvalidOperationException("Person " + personId
                        + " was not found in database");
                }
                
                person.MotherId = null;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Method RemovePartner removes person's father
        /// </summary>
        /// <param name="personId">ID of person which father is going to be removed</param>
        public void RemoveFather(int personId)
        {
            if (personId <= 0)
            {
                throw new ArgumentException("ID must be greater than 0, was " + personId);
            }

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var person = context.People
                    .FirstOrDefault(c => c.Id == personId);

                if (person == null)
                {
                    throw new InvalidOperationException("Person " + personId
                                                        + " was not found in database");
                }

                person.FatherId = null;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Method RemovePartner removes person's partner and partner's partner, which is person
        /// </summary>
        /// <param name="personId">ID of person which partner is going to be removed</param>
        public void RemovePartner(int personId)
        {
            if (personId <= 0)
            {
                throw new ArgumentException("ID must be greater than 0, was " + personId);
            }

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var person = context.People
                    .Find(personId);

                if (person == null)
                {
                    throw new InvalidOperationException("Person " + personId
                        + " was not found in database");
                }

                var partner = context.People
                    .Find(person.PartnerId);

                if (partner == null)
                {
                    throw new InvalidOperationException("Partner of person " + person.PartnerId
                        + " was not found in database");
                }

                person.PartnerId = null;
                partner.PartnerId = null;
                context.SaveChanges();
            }
        }
    }
}