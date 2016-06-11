using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using P_Layer.Controllers;

namespace P_Layer.Models
{
    public class PersonModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [JsonConverter(typeof(BoolConverter))]
        public bool IsMale { get; set; }
        [JsonIgnore]
        public DateTime? BirthDate { get; set; }
        [JsonIgnore]
        public DateTime? DeathDate { get; set; }

        public int? MotherId { get; set; }
        public int? FatherId { get; set; }
        public int? PartnerId { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}