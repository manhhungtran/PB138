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
        /// Method GetAllPeople retrieves all people from database, depending on currently logged user
        /// </summary>
        /// <param name="id">ID of currently logged user</param>
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
        /// Method UpdatePerson updates person in database
        /// </summary>
        /// <param name="person">Person to be updated</param>
        public void UpdatePerson(PersonDTO person)
        {
            if (person == null) { throw new ArgumentNullException(nameof(person)); }

            Person personToBeUpdated = Mapping.Mapper.Map<Person>(person);
            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
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
        /// 
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="motherId"></param>
        public void SetMother(int personId, int motherId)
        {
            if (personId <= 0) { throw new ArgumentException("ID must be greater than 0, was " + personId); }
            if (motherId <= 0) { throw new ArgumentException("ID must be greater than 0, was " + motherId); }

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var person = context.People
                    .FirstOrDefault(c => c.Id == personId);
                var mother = context.People
                    .FirstOrDefault(c => c.Id == motherId);

                if (person == null)
                {
                    throw new InvalidOperationException("Person " + personId
                        + " was not found in database");
                }
                if (mother == null)
                {
                    throw new InvalidOperationException("Person " + motherId
                        + " was not found in database");
                }

                person.MotherId = mother.Id;
                context.SaveChanges();
            }
        }

        public PersonDTO GetMother(int personId)
        {
            if (personId <= 0) { throw new ArgumentException("ID must be greater than 0, was " + personId); }

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var person = context.People
                    .FirstOrDefault(c => c.Id == personId);
                var mother = context.People
                    .FirstOrDefault(c => c.Id == person.MotherId);

                return Mapping.Mapper.Map<PersonDTO>(mother);
            }
        }

        public void RemoveMother(int personId)
        {
            if (personId <= 0) { throw new ArgumentException("ID must be greater than 0, was " + personId); }

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

        public void SetFather(int personId, int fatherId)
        {
            if (personId <= 0) { throw new ArgumentException("ID must be greater than 0, was " + personId); }
            if (fatherId <= 0) { throw new ArgumentException("ID must be greater than 0, was " + fatherId); }

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var person = context.People
                    .FirstOrDefault(c => c.Id == personId);
                var father = context.People
                    .FirstOrDefault(c => c.Id == fatherId);

                if (person == null)
                {
                    throw new InvalidOperationException("Person " + personId
                        + " was not found in database");
                }
                if (father == null)
                {
                    throw new InvalidOperationException("Person " + fatherId
                        + " was not found in database");
                }
               
                person.FatherId = father.Id;
                context.SaveChanges();
            }
        }

        public PersonDTO GetFather(int personId)
        {
            if (personId <= 0) { throw new ArgumentException("ID must be greater than 0, was " + personId); }

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var person = context.People
                    .FirstOrDefault(c => c.Id == personId);
                var father = context.People
                    .FirstOrDefault(c => c.Id == person.FatherId);

                return Mapping.Mapper.Map<PersonDTO>(father);
            }
        }

        public void RemoveFather(int personId)
        {
            if(personId <= 0) { throw new ArgumentException("ID must be greater than 0, was " + personId); }

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

        public void SetChildren(PersonDTO personDTO, List<PersonDTO> childrenDTOs)
        {
            if (personDTO == null) { throw new ArgumentNullException(nameof(personDTO)); }
            if (childrenDTOs == null) { throw new ArgumentNullException(nameof(childrenDTOs)); }

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var person = context.People
                    .FirstOrDefault(c => c.Id == personDTO.Id);
                var children = childrenDTOs
                    .Select(childDTO => context.People.FirstOrDefault(c => c.Id == childDTO.Id))
                    .ToList();

                if (person == null)
                {
                    throw new InvalidOperationException("Person " + personDTO
                        + " was not found in database");
                }

                foreach (var child in children)
                {
                    if (person.IsMale)
                    {
                        if (child.FatherId == null)
                        {
                            child.FatherId = person.Id;
                        }
                        else
                        {
                            throw new InvalidOperationException("Person " + child 
                                + " already has Father");
                        }
                    }
                    else
                    {
                        if (child.MotherId == null)
                        {
                            child.MotherId = person.Id;
                        }
                        else
                        {
                            throw new InvalidOperationException("Person " + child
                                + " already has Mother");
                        }
                    }
                }
                context.SaveChanges();
            }
        }

        public List<PersonDTO> GetChildren(PersonDTO personDTO)
        {
            if (personDTO == null) { throw new ArgumentNullException(nameof(personDTO)); }

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var person = context.People
                    .FirstOrDefault(c => c.Id == personDTO.Id);

                if (person == null)
                {
                    throw new InvalidOperationException("Person " + personDTO
                        + " was not found in database");
                }

                var children = context.People
                    .Where(c => (c.FatherId == person.Id) || (c.MotherId == person.Id))
                    .ToList();

                return children
                    .Select(elem => Mapping.Mapper.Map<PersonDTO>(elem))
                    .ToList();
            }
        }

        public void SetPartner(PersonDTO personDTO, PersonDTO partnerDTO)
        {
            if (personDTO == null) { throw new ArgumentNullException(nameof(personDTO)); }
            if (personDTO.PartnerId != null)
            {
                throw new InvalidOperationException("Person " + personDTO
                + " already has Partner");
            }

            if (partnerDTO == null) { throw new ArgumentNullException(nameof(partnerDTO)); }
            if (partnerDTO.PartnerId != null)
            {
                throw new InvalidOperationException("Person " + partnerDTO
                + " already has Partner");
            }

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var person = context.People
                    .FirstOrDefault(c => c.Id == personDTO.Id);
                var partner = context.People
                    .FirstOrDefault(c => c.Id == partnerDTO.Id);

                if (person == null)
                {
                    throw new InvalidOperationException("Person " + personDTO
                        + " was not found in database");
                }
                if (partner == null)
                {
                    throw new InvalidOperationException("Person " + partnerDTO
                        + " was not found in database");
                }

                person.PartnerId = partner.Id;
                partner.PartnerId = person.Id;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Method SetPartner sets partner to person based on personId and vice versa
        /// </summary>
        /// <param name="personId">ID of the first person</param>
        /// <param name="partnerId">ID of the second person</param>
        public void SetPartner(int personId, int partnerId)
        {
            if (personId <= 0) { throw new ArgumentException("Id must be greater than 0, was " + personId); }
            if (partnerId <= 0) { throw new ArgumentException("Id must be greater than 0, was " + partnerId); }

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var person = context.People
                    .FirstOrDefault(c => c.Id == personId);
                var partner = context.People
                    .FirstOrDefault(c => c.Id == partnerId);

                if (person == null)
                {
                    throw new InvalidOperationException("Person with ID " + personId
                        + " was not found in database");
                }
                if (partner == null)
                {
                    throw new InvalidOperationException("Person with ID" + partnerId
                        + " was not found in database");
                }

                person.PartnerId = partner.Id;
                partner.PartnerId = person.Id;
                context.SaveChanges();
            }
        }

        public PersonDTO GetPartner(PersonDTO personDTO)
        {
            if (personDTO == null) { throw new ArgumentNullException(nameof(personDTO)); }

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var partner = context.People
                    .FirstOrDefault(c => c.Id == personDTO.PartnerId);
                return Mapping.Mapper.Map<PersonDTO>(partner);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="personDTO">Person whi</param>
        public void RemovePartner(PersonDTO personDTO)
        {
            if (personDTO == null) { throw new ArgumentNullException(nameof(personDTO)); }
            if (personDTO.PartnerId == null)
            {
                throw new InvalidOperationException("Person " + personDTO
                    + " does not have partner");
            }

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var person = context.People
                    .FirstOrDefault(c => c.Id == personDTO.Id);
                var partner = context.People
                    .FirstOrDefault(c => c.Id == personDTO.PartnerId);

                if (person == null)
                {
                    throw new InvalidOperationException("Person " + personDTO
                        + " was not found in database");
                }
                if (partner == null)
                {
                    throw new InvalidOperationException("Partner of person " + personDTO
                        + " was not found in database");
                }

                person.PartnerId = null;
                partner.PartnerId = null;
                context.SaveChanges();
            }
        }
    }
}