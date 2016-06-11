using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
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
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }
        [JsonIgnore]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DeathDate { get; set; }

        public int? MotherId { get; set; }
        public int? FatherId { get; set; }
        public int? PartnerId { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public int UserId { get; set; }

        public bool ShouldSerializeBirthDate()
        {
            return BirthDate != null;
        }
        public bool ShouldSerializeDeathDate()
        {
            return DeathDate != null;
        }
        public bool ShouldSerializeMotherId()
        {
            return MotherId != null;
        }
        public bool ShouldSerializeFatherId()
        {
            return FatherId != null;
        }
        public bool ShouldSerializePartnerId()
        {
            return PartnerId != null;
        }
    }
}