using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B_Layer.DTO;
using B_Layer.Facades;

namespace A_Layer
{
    class Program
    {
        static void Main(string[] args)
        {
            PersonFacade personFacade = new PersonFacade();

            PersonDTO person1 = new PersonDTO
            {
                Name = "Karel",
                Surname = "Novak",
                BirthDate = new DateTime(1968, 7, 15),
                IsMale = true
            };

            PersonDTO person2 = new PersonDTO
            {
                Name = "Marie",
                Surname = "Novakova",
                BirthDate = new DateTime(1970, 4, 8),
                IsMale = false
            };

            PersonDTO person3 = new PersonDTO
            {
                Name = "Petr",
                Surname = "Novak",
                BirthDate = new DateTime(1995, 2, 2),
                IsMale = true
            };

            PersonDTO person4 = new PersonDTO
            {
                Name = "Jana",
                Surname = "Novakova",
                BirthDate = new DateTime(1999, 2, 4),
                IsMale = false
            };

            //personFacade.CreatePerson(person1);
            //personFacade.CreatePerson(person2);
            //personFacade.CreatePerson(person3);
            //personFacade.CreatePerson(person4);
            person1.Id = 1;
            person2.Id = 2;
            person3.Id = 3;
            person4.Id = 4;

            //personFacade.SetFather(person4, person1);
            //personFacade.SetMother(person3, person2);
            person4.FatherId = 1;
            person4.MotherId = 2;

            personFacade.DeletePerson(3);

            //personFacade.SetPartner(person3, person4);

            //StringBuilder str = new StringBuilder();
            //foreach (var person in personFacade.GetAllPeople())
            //{
            //    str.Append(person)
            //        .Append("| Mother: ")
            //        .Append(person.MotherId?.ToString() ?? "Unknown")
            //        .Append("| Father: ")
            //        .Append(person.FatherId?.ToString() ?? "Unknown")
            //        .Append("| Partner: ")
            //        .Append(person.PartnerId?.ToString() ?? "Unknown")
            //        .Append(Environment.NewLine);
            //}
            //Console.WriteLine(str.ToString());
        }
    }
}
