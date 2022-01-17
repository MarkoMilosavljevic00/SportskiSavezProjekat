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
    public class IspitController : ControllerBase
    {
        public SavezContext Context { get; set; }

        public IspitController(SavezContext context)
        {
            Context = context;
        }


        #region DODAVANJE_REGISTRACIJE
        [Route("DodajRegistraciju")]
        [HttpPost]
        public async Task<ActionResult> DodajRegistraciju(int idTakmicara, int idTakmicenja, int idKluba, DateTime datumRegistracije)
        {
            if (idTakmicara < 0)
            {
                return BadRequest("Greska kod id-a takmicara!");
            }
            if (idKluba < 0)
            {
                return BadRequest("Greska kod id-a kluba!");
            }
            if (idTakmicenja < 0)
            {
                return BadRequest("Greska kod id-a takmicenja!");
            }
            if (datumRegistracije.CompareTo(new DateTime(2000, 1, 1)) < 0
            || datumRegistracije.CompareTo(DateTime.Now) > 0)
            {
                return BadRequest("Greska kod datuma registracije!");
            }

            try
            {
                var takmicar = await Context.Takmicari.Where(p => p.ID == idTakmicara).FirstOrDefaultAsync();
                var takmicenje = await Context.Takmicenja.Where(p => p.ID == idTakmicenja).FirstOrDefaultAsync();
                var klub = await Context.Klubovi.Where(p => p.ID == idKluba).FirstOrDefaultAsync();

                Registruje r = new Registruje()
                {
                    Takmicar = takmicar,
                    Takmicenje = takmicenje,
                    Klub = klub,
                    Datum_registracije = datumRegistracije
                };

                Context.Registracije.Add(r);
                await Context.SaveChangesAsync();

                var podaciOTakmicaru = Context.Registracije
                                        .Include(p=>p.Takmicar)
                                        .Include(p=>p.Takmicenje)
                                        .Include(p=>p.Klub)
                                        .Where(p=>p.Takmicar.ID == idTakmicara)
                                        .Select(p=>
                                        new
                                        {
                                            Ime = p.Takmicar.Ime,
                                            Prezime = p.Takmicar.Prezime,
                                            Klub = p.Klub.Naziv,
                                            Takmicenje = p.Takmicenje.Naziv,
                                            Sport = p.Takmicenje.Sport,
                                            Kategorija = p.Takmicenje.Kategorija
                                        }).ToListAsync();
                return Ok("Uspesno zavedena registracija sa sledecim podacima:\n" + podaciOTakmicaru);
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