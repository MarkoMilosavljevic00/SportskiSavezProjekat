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
    public class KlubController : ControllerBase
    {
        public SavezContext Context { get; set; }

        public KlubController(SavezContext context)
        {
            Context = context;
        }


        #region PREUZIMANJE_KLUBA
        [Route("PreuzmiKlub")]
        [HttpGet]
        public async Task<ActionResult>PreuzmiKlub()
        {
            return Ok(await Context.Klubovi.Select(p =>
                new
                {
                    p.ID,
                    p.Naziv
                }).ToListAsync());
        }
        #endregion


        #region DODAVANJE_KLUBA
        [Route("DodajKlub")]
        [HttpPost]
        public async Task<ActionResult> DodajKlub([FromBody] Klub klub)
        {
            if (string.IsNullOrWhiteSpace(klub.Naziv)
            || klub.Naziv.Length > 50)
            {
                return BadRequest("Greska kod naziva!");
            }

            try
            {
                Context.Klubovi.Add(klub);
                await Context.SaveChangesAsync();
                return Ok($"Dodat klub {klub.Naziv}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion
    
        #region BRISANJE_KLUBA
        [Route("BrisiKlub/{id}")]
        [HttpDelete]
        public async Task<ActionResult> BrisiKlub(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Pogrešan ID!");
            }

            try
            {
                var klub = await Context.Klubovi.FindAsync(id);
                string naziv = klub.Naziv;
                Context.Klubovi.Remove(klub);
                await Context.SaveChangesAsync();
                return Ok($"Uspešno izbrisan klub {naziv}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        #endregion

    
    }
}