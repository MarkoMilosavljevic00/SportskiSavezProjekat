using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Takmicenje
    {
        [Key]
        public int ID { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Naziv { get; set; }

        [Required]
        [MaxLength(50)]
        public string Sport { get; set; }

        [Required]
        [MaxLength(50)]
        public string Kategorija { get; set; }
        
        public DateTime Datum_odrzavanja { get; set; }

        public virtual Organizator Organizator { get; set; }

        [JsonIgnore]
        public virtual List<Registruje> Registracije { get; set; }
        
    }
}