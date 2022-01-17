using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Takmicar
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(25)]
        public string Ime { get; set; }

        [Required]
        [MaxLength(50)]
        public string Prezime { get; set; }

        [Required]
        [MaxLength(1)]
        public string Pol { get; set; }

        [Required]
        [MaxLength(50)]
        public string Sport{get; set;}


        [Required]
        [MaxLength(50)]
        public string Kategorija{get; set;}

        [JsonIgnore]
        public virtual List<Registruje> Registracije { get; set; }
    }
}