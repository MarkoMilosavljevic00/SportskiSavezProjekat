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
        [Route("DodajOrganizatora/{naziv}/{sredstva}/{sportskiObj}/{idTakmicenja}")]
        [HttpPost]
        public async Task<ActionResult> DodajOrganizatora(string naziv, double sredstva, string sportskiObj, int idTakmicenja)
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

            if (sportskiObj.Length > 100)
            {
                return BadRequest("Greska kod naziva sportskog objekta!");
            }

            try
            {
                // var postojiLiTakmicenje = await Context.Takmicenja.Where(p => p.ID == idTakmicenja).FirstOrDefaultAsync();
                // if (postojiLiTakmicenje == null)
                // {
                //     return BadRequest("Takmicenje ne postoji!");
                // }

                var organizator = await Context.Organizatori
                                    .Include(p => p.Takmicenja)
                                    .Where(p => p.Naziv == naziv).FirstOrDefaultAsync();


                var takmicenje = await Context.Takmicenja
                                                .Include(p => p.Organizator)
                                                .Where(p => p.ID == idTakmicenja).FirstOrDefaultAsync();

                var takmicenjeLista = await Context.Takmicenja
                                                .Include(p=>p.Organizator)
                                                .Where(p=>p.ID == idTakmicenja).ToListAsync();


                if (takmicenje == null)
                {
                    return BadRequest("Takmicenje ne postoji!");
                }

                if (takmicenje.Organizator != null && sredstva <= takmicenje.Organizator.Sredstva)
                {
                    return StatusCode(202, takmicenje.Organizator);
                }
                else
                {
                    if (organizator == null)
                    {
                        Organizator o = new Organizator()
                        {
                            Naziv = naziv,
                            Sredstva = sredstva,
                            Sportski_objekat = sportskiObj,
                            Takmicenja = takmicenjeLista
                        };
                        //  o.Takmicenja.Add(takmicenje);
                        // takmicenje.Organizator = o;
                        Context.Organizatori.Add(o);
                        await Context.SaveChangesAsync();
                        return Ok(o);
                    }
                    else
                    {
                        organizator.Takmicenja.Add(takmicenje);
                        // takmicenje.Organizator = organizator;
                        await Context.SaveChangesAsync();
                        return StatusCode(203, organizator);
                    }
                }
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
                org.Takmicenja.ForEach(p=>p.Organizator=null);
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