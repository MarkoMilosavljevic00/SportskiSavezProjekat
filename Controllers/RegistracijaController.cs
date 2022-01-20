using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Microsoft.AspNetCore.Http;


namespace Proba.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistracijaController : ControllerBase
    {
        public SavezContext Context { get; set; }

        public RegistracijaController(SavezContext context)
        {
            Context = context;
        }


        #region DODAVANJE_REGISTRACIJE
        [Route("DodajRegistraciju/{idTakmicara}/{idTakmicenja}/{idKluba}")]
        [HttpPost]
        public async Task<ActionResult> DodajRegistraciju(int idTakmicara, int idTakmicenja, int idKluba)
        {
            // if (idTakmicara < 0)
            // {
            //     return BadRequest("Greska kod id-a takmicara!");
            // }
            // if (idKluba < 0)
            // {
            //     return BadRequest("Greska kod id-a kluba!");
            // }
            // if (idTakmicenja < 0)
            // {
            //     return BadRequest("Greska kod id-a takmicenja!");
            // }

            try
            {
                var takmicar = await Context.Takmicari.Where(p => p.ID == idTakmicara).FirstOrDefaultAsync();
                var takmicenje = await Context.Takmicenja.Where(p => p.ID == idTakmicenja).FirstOrDefaultAsync();
                var klub = await Context.Klubovi.Where(p => p.ID == idKluba).FirstOrDefaultAsync();


                var postojiLiRegistracija = await Context.Registracije
                                        .Include(p => p.Takmicar)
                                        .Include(p => p.Takmicenje)
                                        .Include(p => p.Klub)
                                        .Where(p => p.Takmicar.ID == idTakmicara
                                                && p.Takmicenje.ID == idTakmicenja).FirstOrDefaultAsync();

                if (postojiLiRegistracija == null)
                {

                    Registruje r = new Registruje()
                    {
                        Takmicar = takmicar,
                        Takmicenje = takmicenje,
                        Klub = klub,
                        Datum_registracije = DateTime.Now
                    };

                    Context.Registracije.Add(r);
                    await Context.SaveChangesAsync();
                }

                var podaciOTakmicaru = await Context.Registracije
                                        .Include(p => p.Takmicar)
                                        .Include(p => p.Takmicenje)
                                        .Include(p => p.Klub)
                                        .Where(p => p.Takmicar.ID == idTakmicara)
                                        .Select(p =>
                                        new
                                        {
                                            Klub = p.Klub.Naziv,
                                            Ime = p.Takmicar.Ime,
                                            Prezime = p.Takmicar.Prezime,
                                            Pol = p.Takmicar.Pol,
                                            Takmicenje = p.Takmicenje.Naziv,
                                            Kategorija = p.Takmicar.Kategorija,
                                            Datum_registracije = p.Datum_registracije
                                        }).ToListAsync();
                return Ok(podaciOTakmicaru);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion


        #region BRISI_REGISTRACIJU
        [Route("BrisiRegistraciju/{id}")]
        [HttpDelete]
        public async Task<ActionResult> BrisiRegistraciju(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Pogrešan ID!");
            }

            try
            {
                var reg = await Context.Registracije.FindAsync(id);
                int id1 = reg.ID;
                Context.Registracije.Remove(reg);
                await Context.SaveChangesAsync();
                return Ok($"Uspešno izbrisana registracija sa id-om {id1}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        #endregion
    }
}