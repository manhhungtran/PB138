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
        public void CreatePerson(PersonDTO person)
        {
            if (person == null) { throw new ArgumentNullException(nameof(person)); }

            Person newPerson = Mapping.Mapper.Map<Person>(person);

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                context.People.Add(newPerson);
                context.SaveChanges();
            }
        }

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

        public List<PersonDTO> GetAllPeople()
        {
            using (var context = new AppDbContext())
            {
                var students = context.People.ToList();
                return students
                    .Select(element => Mapping.Mapper.Map<PersonDTO>(element))
                    .ToList();
            }
        }

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

        public void DeletePerson(int id)
        {
            if (id <= 0) { throw new ArgumentException("Id must be greater than 0, was " + id); }

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var person = context.People.Find(id);

                if (person == null)
                {
                    throw new InvalidOperationException("Person with id " + id
                        + " was not found in database");
                }

                context.People.Remove(person);
                context.SaveChanges();
            }
        }

        public void SetMother(PersonDTO personDTO, PersonDTO motherDTO)
        {
            if (personDTO == null) { throw new ArgumentNullException(nameof(personDTO)); }
            if (personDTO.MotherId != null) {throw new InvalidOperationException("Person " + personDTO
                + " already has Mother"); }

            if (motherDTO == null) { throw new ArgumentNullException(nameof(motherDTO)); }
            if (motherDTO.IsMale) { throw new InvalidOperationException("Mother must be female"); }

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var person = context.People
                    .FirstOrDefault(c => c.Id == personDTO.Id);
                var mother = context.People
                    .FirstOrDefault(c => c.Id == motherDTO.Id);

                if (person == null)
                {
                    throw new InvalidOperationException("Person " + personDTO
                        + " was not found in database");
                }
                if (mother == null)
                {
                    throw new InvalidOperationException("Person " + motherDTO
                        + " was not found in database");
                }

                person.MotherId = mother.Id;
                context.SaveChanges();
            }
        }

        public PersonDTO GetMother(PersonDTO personDTO)
        {
            if (personDTO == null) { throw new ArgumentNullException(nameof(personDTO)); }

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var father = context.People
                    .FirstOrDefault(c => c.Id == personDTO.MotherId);

                return Mapping.Mapper.Map<PersonDTO>(father);
            }
        }

        public void RemoveMother(PersonDTO personDTO)
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
                
                person.MotherId = null;
                context.SaveChanges();
            }
        }

        public void SetFather(PersonDTO personDTO, PersonDTO fatherDTO)
        {
            if (personDTO == null) { throw new ArgumentNullException(nameof(personDTO)); }
            if (personDTO.FatherId != null) { throw new InvalidOperationException("Person " + personDTO 
                + " already has Father"); }

            if (fatherDTO == null) { throw new ArgumentNullException(nameof(fatherDTO)); }
            if (!fatherDTO.IsMale) { throw new InvalidOperationException("Father must be male"); }

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var person = context.People
                    .FirstOrDefault(c => c.Id == personDTO.Id);
                var father = context.People
                    .FirstOrDefault(c => c.Id == fatherDTO.Id);

                if (person == null)
                {
                    throw new InvalidOperationException("Person " + personDTO
                        + " was not found in database");
                }
                if (father == null)
                {
                    throw new InvalidOperationException("Person " + fatherDTO
                        + " was not found in database");
                }
               
                person.FatherId = father.Id;
                context.SaveChanges();
            }
        }

        public PersonDTO GetFather(PersonDTO personDTO)
        {
            if (personDTO == null) { throw new ArgumentNullException(nameof(personDTO)); }

            using (var context = new AppDbContext())
            {
                context.Database.Log = Console.WriteLine;
                var father = context.People
                    .FirstOrDefault(c => c.Id == personDTO.FatherId);
                return Mapping.Mapper.Map<PersonDTO>(father);
            }
        }

        public void RemoveFather(PersonDTO personDTO)
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
    }
}