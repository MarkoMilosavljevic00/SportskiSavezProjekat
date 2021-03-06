using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Organizator
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Naziv { get; set; }

        [Required]
        public double Sredstva { get; set; }


        [MaxLength(100)]
        public string Sportski_objekat { get; set; }

        [JsonIgnore]
        public virtual List<Takmicenje> Takmicenja { get; set; }

    }
}