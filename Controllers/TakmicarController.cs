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
    public class TakmicarController : ControllerBase
    {
        public SavezContext Context { get; set; }

        public TakmicarController(SavezContext context)
        {
            Context = context;
        }

        #region PREUZIMANJE_TAKMICARA
        [Route("PreuzmiTakmicara")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiTakmicara()
        {
            var takmicari = Context.Takmicari
                            .Include(p => p.Registracije.Where(p=>p.Klub.ID == 1 || p.Takmicar.Ime.Equals("Novak")))
                            .ThenInclude(p => p.Takmicenje)
                            .Include(p => p.Registracije)
                            .ThenInclude(p => p.Klub);

            var takmicar = await takmicari.ToListAsync();

            return Ok
                (
                    takmicar.Select(p =>
                    new
                    {
                        ID = p.ID,
                        Ime = p.Ime,
                        Prezime = p.Prezime,
                        Pol = p.Pol,
                        Registracija = p.Registracije
                            // .Where(q => rokIDs.Contains(q.IspitniRok.ID))
                            .Select(q =>
                            new
                            {
                                ID = q.ID,
                                Klub = q.Klub.Naziv,
                                Takmicenje = q.Takmicenje.Naziv,
                                Datum_odrzavanja = q.Takmicenje.Datum_odrzavanja,
                                Datum_registracije = q.Datum_registracije,
                                Sport = q.Takmicenje.Sport,
                                Kategorija = q.Takmicenje.Kategorija
                            })
                    }).ToList()
                );

            // return Ok(Context.Takmicari);
        }
        #endregion


        #region DODAVANJE_TAKMICARA
        [Route("DodajTakmicara")]
        [HttpPost]
        public async Task<ActionResult> DodajTakmicara([FromBody] Takmicar takmicar)
        {
            if (string.IsNullOrWhiteSpace(takmicar.Ime)
            || takmicar.Ime.Length > 50)
            {
                return BadRequest("Greska kod imena!");
            }

            if (string.IsNullOrWhiteSpace(takmicar.Prezime)
           || takmicar.Prezime.Length > 50)
            {
                return BadRequest("Greska kod prezimena!");
            }

            if (string.IsNullOrWhiteSpace(takmicar.Pol)
            || takmicar.Pol.Length > 1
               || !(takmicar.Pol.Equals("M") || takmicar.Pol.Equals("Z")))
            {
                return BadRequest("Greska kod pola!");
            }
            if (string.IsNullOrWhiteSpace(takmicar.Sport)
            || takmicar.Sport.Length > 50)
            {
                return BadRequest("Greska kod sporta!");
            }

            if (string.IsNullOrWhiteSpace(takmicar.Kategorija)
                         || takmicar.Kategorija.Length > 50)
            {
                return BadRequest("Greska kod kategorije!");
            }

            try
            {
                Context.Takmicari.Add(takmicar);
                await Context.SaveChangesAsync();
                return Ok($"Dodat je takmicar {takmicar.Ime} {takmicar.Prezime}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion



        #region PROMENA_TAKMICARA


        [Route("PromenitiTakmicara/{ime}/{prezime}/{pol}/{sport}/{kategorija}")]
        [HttpPut]
        public async Task<ActionResult> PromeniTakmicara(string ime, string prezime, string pol, string sport, string kategorija)
        {
            if (string.IsNullOrWhiteSpace(ime)
            || ime.Length > 50)
            {
                return BadRequest("Greska kod imena!");
            }

            if (string.IsNullOrWhiteSpace(prezime)
           || prezime.Length > 50)
            {
                return BadRequest("Greska kod prezimena!");
            }

            if (string.IsNullOrWhiteSpace(pol)
            || pol.Length > 1
               || !(pol.Equals("M") || pol.Equals("Z")))
            {
                return BadRequest("Greska kod pola!");
            }
            if (string.IsNullOrWhiteSpace(sport)
            || sport.Length > 50)
            {
                return BadRequest("Greska kod sporta!");
            }

            if (string.IsNullOrWhiteSpace(kategorija)
                         || kategorija.Length > 50)
            {
                return BadRequest("Greska kod kategorije!");
            }
            try
            {
                var takmicar = Context.Takmicari.Where(p => p.Ime.Equals(ime) && p.Prezime.Equals(prezime)).FirstOrDefault();

                if (takmicar != null)
                {
                    takmicar.Pol = pol;
                    takmicar.Sport = sport;
                    takmicar.Kategorija = kategorija;

                    await Context.SaveChangesAsync();
                    return Ok($"Takmicar {ime} {prezime} uspesno azuriran!");
                }
                else
                {
                    return BadRequest($"Takmicar {ime} {prezime} nije pronadjen!");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [Route("PromenitiTakmicara(FromBody)")]
        [HttpPut]
        public async Task<ActionResult> PromeniBody([FromBody] Takmicar takmicar)
        {

            if (string.IsNullOrWhiteSpace(takmicar.Ime)
            || takmicar.Ime.Length > 50)
            {
                return BadRequest("Greska kod imena!");
            }

            if (string.IsNullOrWhiteSpace(takmicar.Prezime)
           || takmicar.Prezime.Length > 50)
            {
                return BadRequest("Greska kod prezimena!");
            }

            if (string.IsNullOrWhiteSpace(takmicar.Pol)
            || takmicar.Pol.Length > 1
               || !(takmicar.Pol.Equals("M") || takmicar.Pol.Equals("Z")))
            {
                return BadRequest("Greska kod pola!");
            }
            if (string.IsNullOrWhiteSpace(takmicar.Sport)
            || takmicar.Sport.Length > 50)
            {
                return BadRequest("Greska kod sporta!");
            }

            if (string.IsNullOrWhiteSpace(takmicar.Kategorija)
                         || takmicar.Kategorija.Length > 50)
            {
                return BadRequest("Greska kod kategorije!");
            }

            try
            {
                var takmicarZaPromenu = await Context.Takmicari.FindAsync(takmicar.ID);

                if (takmicarZaPromenu != null)
                {
                    takmicarZaPromenu.Ime = takmicar.Ime;
                    takmicarZaPromenu.Prezime = takmicar.Prezime;
                    takmicarZaPromenu.Pol = takmicar.Pol;
                    takmicarZaPromenu.Sport = takmicar.Sport;
                    takmicarZaPromenu.Kategorija = takmicar.Kategorija;

                    await Context.SaveChangesAsync();
                    return Ok($"Takmicar {takmicar.Ime} {takmicar.Prezime} uspesno azuriran!");
                }
                else
                {
                    return BadRequest($"Takmicar {takmicar.Ime} {takmicar.Prezime} nije pronadjen!");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion


        #region BRISANJE_TAKMICARA

        [Route("BrisiTakmicara(FromBody)")]
        [HttpDelete]
        public async Task<ActionResult> ObrisiTakmicara([FromBody] Takmicar takmicar)
        {

            if (string.IsNullOrWhiteSpace(takmicar.Ime)
            || takmicar.Ime.Length > 50)
            {
                return BadRequest("Greska kod imena!");
            }

            if (string.IsNullOrWhiteSpace(takmicar.Prezime)
           || takmicar.Prezime.Length > 50)
            {
                return BadRequest("Greska kod prezimena!");
            }

            if (string.IsNullOrWhiteSpace(takmicar.Pol)
            || takmicar.Pol.Length > 1
               || !(takmicar.Pol.Equals("M") || takmicar.Pol.Equals("Z")))
            {
                return BadRequest("Greska kod pola!");
            }
            if (string.IsNullOrWhiteSpace(takmicar.Sport)
            || takmicar.Sport.Length > 50)
            {
                return BadRequest("Greska kod sporta!");
            }

            if (string.IsNullOrWhiteSpace(takmicar.Kategorija)
                         || takmicar.Kategorija.Length > 50)
            {
                return BadRequest("Greska kod kategorije!");
            }

            try
            {

                var takmicarZaBrisanje = await Context.Takmicari.FindAsync(takmicar.ID);

                if (takmicar != null)
                {
                    string ime = takmicar.Ime;
                    string prezime = takmicar.Prezime;

                    Context.Takmicari.Remove(takmicarZaBrisanje);
                    await Context.SaveChangesAsync();
                    return Ok($"Takmicar {ime} {prezime} je uspesno obrisan");
                }
                else
                {
                    return BadRequest($"Takmicar {takmicar.Ime} {takmicar.Prezime} nije pronadjen!");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("BrisiTakmicara/{id}")]
        [HttpDelete]
        public async Task<ActionResult> BrisiTakmicara(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Pogrešan ID!");
            }

            try
            {
                var takmicar = await Context.Takmicari.FindAsync(id);
                string imeprezime = takmicar.Ime;
                imeprezime += " ";
                imeprezime += takmicar.Prezime;
                Context.Takmicari.Remove(takmicar);
                await Context.SaveChangesAsync();
                return Ok($"Uspešno izbrisan takmicar {imeprezime}");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        #endregion

    }

}
