using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Models
{
    public class Registruje
    {
        [Key]
        public int ID { get; set; }

        public DateTime Datum_registracije { get; set; }

        [JsonIgnore]
        public virtual Klub Klub { get; set; }

        [JsonIgnore]
        public virtual Takmicar Takmicar { get; set; }

        [JsonIgnore]
        public virtual Takmicenje Takmicenje { get; set; }
    }
}