using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class SavezContext :  DbContext
    {
        public DbSet<Takmicar> Takmicari{get; set;}

        public DbSet<Takmicenje> Takmicenja{get;set;}
        public DbSet<Klub> Klubovi { get; set; }
        public DbSet<Registruje> Registracije { get; set; }
        public DbSet<Organizator> Organizatori { get; set; }
    
        public SavezContext(DbContextOptions options):base(options)
        {

        }
    }
}