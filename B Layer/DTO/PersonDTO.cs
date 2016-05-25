using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B_Layer.DTO
{
    public class PersonDTO
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public bool IsMale { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }

        public int? MotherId { get; set; }
        public int? FatherId { get; set; }

        public override string ToString()
        {
            return Name + " " + Surname;
        }
    }
}
