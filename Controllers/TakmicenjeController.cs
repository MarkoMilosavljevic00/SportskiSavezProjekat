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
    public class TakmicenjeController : ControllerBase
    {
        public SavezContext Context { get; set; }

        public TakmicenjeController(SavezContext context)
        {
            Context = context;
        }


        #region PREUZIMANJE_TAKMICENJA
        [Route("PreuzmiTakmicenje")]
        [HttpGet]
        public async Task<ActionResult>PreuzmiTakmicenje(/*[FromQuery]int id*/)
        {
            return Ok(await Context.Takmicenja./*Where(s=>s.ID = id).*/Select(p =>
                new
                {
                    ID = p.ID,
                    Naziv = p.Naziv,
                    Sport = p.Sport,
                    Kategorija = p.Kategorija,
                    DatumOdrzavanja = p.Datum_odrzavanja,
                    Organizacija = p.Organizator
                }).ToListAsync());
        }
        #endregion


         #region DODAVANJE_TAKMICENJA
        [Route("DodajTakmicenje")]
        [HttpPost]
        public async Task<ActionResult> DodajTakmicenje([FromBody] Takmicenje takmicenje)
        {
            if (string.IsNullOrWhiteSpace(takmicenje.Naziv)
            || takmicenje.Naziv.Length > 100)
            {
                return BadRequest("Greska kod naziva!");
            }

            if (string.IsNullOrWhiteSpace(takmicenje.Sport)
           || takmicenje.Sport.Length > 50)
            {
                return BadRequest("Greska kod sporta!");
            }

            if (string.IsNullOrWhiteSpace(takmicenje.Kategorija)
            || takmicenje.Kategorija.Length > 50)
            {
                return BadRequest("Greska kod kategorije!");
            }
            if (takmicenje.Datum_odrzavanja.CompareTo(new DateTime(2000,01,01)) < 0
            || takmicenje.Datum_odrzavanja.CompareTo(DateTime.Now) > 0)
            {
                return BadRequest("Greska kod datuma!");
            }

            try
            {

                Context.Takmicenja.Add(takmicenje);
                await Context.SaveChangesAsync();
                return Ok($"Dodato je takmicenje {takmicenje.Naziv} za sport {takmicenje.Sport}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion
    
        #region BRISANJE_TAKMICENJA
        [Route("BrisiTakmicenje/{id}")]
        [HttpDelete]
        public async Task<ActionResult> BrisiTakmicenje(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Pogrešan ID!");
            }

            try
            {
                var takmicenje = await Context.Takmicenja.FindAsync(id);
                string naziv = takmicenje.Naziv;
                Context.Takmicenja.Remove(takmicenje);
                await Context.SaveChangesAsync();
                return Ok($"Uspešno izbrisan takmicenje {naziv}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        #endregion

    
    }
}