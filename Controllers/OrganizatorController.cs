using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganizatorController : ControllerBase
    {
        public SavezContext Context { get; set; }

        public OrganizatorController(SavezContext context)
        {
            Context = context;
        }


        #region PREUZIMANJE_ORGANIZATORA
        [Route("PreuzmiOrganizatora")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiOrganizatora()
        {
            var organizatori = Context.Organizatori
                            .Include(p => p.Takmicenja);

            var organizator = await organizatori.ToListAsync();

            return Ok
                (
                    organizator.Select(p =>
                    new
                    {
                        ID = p.ID,
                        Naziv = p.Naziv,
                        Sredstva = p.Sredstva,
                        Sportski_objekat = p.Sportski_objekat,
                        Takmicenje = p.Takmicenja
                            // .Where(q => rokIDs.Contains(q.IspitniRok.ID))
                            .Select(q =>
                            new
                            {
                                ID = q.ID,
                                Naziv = q.Naziv
                            })
                    }).ToList()
                );
        }
        #endregion


        #region DODAVANJE_ORGANIZATORA
        [Route("DodajOrganizatora/{idTakmicenja}/{naziv}/{sredstva}/")]
        [HttpPost]
        public async Task<ActionResult> DodajOrganizatora(int idTakmicenja, string naziv, double sredstva, string sportskiObj)
        {
            if (idTakmicenja < 0)
            {
                return BadRequest("Greska kod id-a takmicenja!");
            }

            if (string.IsNullOrWhiteSpace(naziv)
            || naziv.Length > 50)
            {
                return BadRequest("Greska kod naziva!");
            }

            if (sredstva < 100000
            || sredstva > 9999999999999)
            {
                return BadRequest("Greska kod sredstava, minimalna vrednost je 100000!");
            }

            if (string.IsNullOrWhiteSpace(sportskiObj)
            || sportskiObj.Length > 100)
            {
                return BadRequest("Greska kod naziva sportskog objekta!");
            }

            try
            {
                var takmicenje = await Context.Takmicenja.Where(p => p.ID == idTakmicenja).ToListAsync();

                Organizator o = new Organizator()
                {
                    Naziv = naziv,
                    Sredstva = sredstva,
                    Sportski_objekat = sportskiObj,
                    Takmicenja = takmicenje
                };

                Context.Organizatori.Add(o);
                await Context.SaveChangesAsync();

                // var podaciOTakmicaru = Context.Registracije
                //                         .Include(p=>p.Takmicar)
                //                         .Include(p=>p.Takmicenje)
                //                         .Include(p=>p.Klub)
                //                         .Where(p=>p.Takmicar.ID == idTakmicara)
                //                         .Select(p=>
                //                         new
                //                         {
                //                             Ime = p.Takmicar.Ime,
                //                             Prezime = p.Takmicar.Prezime,
                //                             Klub = p.Klub.Naziv,
                //                             Takmicenje = p.Takmicenje.Naziv,
                //                             Sport = p.Takmicenje.Sport,
                //                             Kategorija = p.Takmicenje.Kategorija
                //                         }).ToListAsync();
                return Ok($"Uspesno zaveden organizator {naziv} takmicenja {takmicenje.Select(p=>p.Naziv)}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

        #region BRISANJE_ORGANIZATORA
        [Route("BrisiOrganizatora/{id}")]
        [HttpDelete]
        public async Task<ActionResult> BrisiOrganizatora(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Pogrešan ID!");
            }

            try
            {
                var org = await Context.Organizatori.FindAsync(id);
                string naziv = org.Naziv;
                Context.Organizatori.Remove(org);
                await Context.SaveChangesAsync();
                return Ok($"Uspešno izbrisan organizator {naziv}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion

    }
}