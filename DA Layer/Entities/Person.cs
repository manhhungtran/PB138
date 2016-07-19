using System;
using System.ComponentModel.DataAnnotations;

namespace DA_Layer.Entities
{
    public class Person
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public bool IsMale { get; set; }

        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }

        public int? MotherId { get; set; }
        public int? FatherId { get; set; }
        public int? PartnerId { get; set; }

        public int UserId { get; set; }

        public string Email { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string Facebook { get; set; }
    }
}
