import { Takmicenje } from "./Takmicenje.js";
import { Klub } from "./Klub.js";
import { SportskiSavez } from "./SportskiSavez.js";

let listaTakmicenja = [];
fetch("https://localhost:5001/Takmicenje/PreuzmiTakmicenje")
    .then(p => {
        p.json().then(takmicenja => {
            takmicenja.forEach(t => {
                let takmicenje = new Takmicenje(t.id, t.naziv, t.sport, t.kategorija, t.datum_odrzavanja, t.organizator);
                listaTakmicenja.push(takmicenje);
            });

            let listaKlubova = [];
            fetch("https://localhost:5001/Klub/PreuzmiKlub")
                .then(p => {
                    p.json().then(klubovi => {
                        klubovi.forEach(k => {
                            let klub = new Klub(k.id, k.naziv);
                            listaKlubova.push(klub);
                        });
                        let savez = new SportskiSavez(listaTakmicenja, listaKlubova);
                        // let savez1 = new SportskiSavez(listaTakmicenja, listaKlubova);
                        savez.crtaj(document.body);
                        // savez1.crtaj(document.body);
                    })
                })

            console.log(listaKlubova);
        })
    })

console.log(listaTakmicenja);


