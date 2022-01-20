import { Takmicar } from "./Takmicar.js";

export class SportskiSavez {
    constructor(listaTakmicenja, listaKlubova) {
        this.listaTakmicenja = listaTakmicenja;
        this.listaKlubova = listaKlubova;
        this.container = null;
    }


    // Metode crtaj
    crtaj(host) {
        if (this.container != null) {
            host.removeChild(this.container);
        }

        this.container = document.createElement("div");
        this.container.className = "GlavniKontejner";
        host.appendChild(this.container);

        let forma = document.createElement("div");
        forma.className = "Forma";
        this.container.appendChild(forma);

        let btnPrikazTakmicenja = document.createElement("button");
        btnPrikazTakmicenja.innerHTML = "Liste takmicara";
        btnPrikazTakmicenja.onclick = (ev) => this.crtajFormuTakmicenja(forma);
        forma.appendChild(btnPrikazTakmicenja);

        let btnRegistracijaTakmicara = document.createElement("button");
        btnRegistracijaTakmicara.innerHTML = "Registracija takmicara";
        btnRegistracijaTakmicara.onclick = (ev) => this.crtajFormuNovaRegistracija(forma);
        forma.appendChild(btnRegistracijaTakmicara);

        let btnDodajOrganizatora = document.createElement("button");
        btnDodajOrganizatora.innerHTML = "Aplicirati za organizovanje takmicenja";
        btnDodajOrganizatora.onclick = (ev) => this.crtajFormuOrganizuj(forma);
        forma.appendChild(btnDodajOrganizatora);
    }


    crtajFormuNovaRegistracija(host) {
        host = this.osveziFormu(host);


        let btnVrati = document.createElement("button");
        btnVrati.innerHTML = "Vrati na pocetnu";
        btnVrati.onclick = (ev) => this.crtaj(document.body);
        host.appendChild(btnVrati);

        let header = document.createElement("h3");
        header.innerHTML = "Licni podaci";
        host.appendChild(header);

        let red = this.crtajRed(host);
        let l = document.createElement("label");
        l.innerHTML = "Ime: ";
        red.appendChild(l);
        let inputIme = document.createElement("input");
        inputIme.className = "Input"
        inputIme.type = "text";
        red.appendChild(inputIme);


        red = this.crtajRed(host);
        l = document.createElement("label");
        l.innerHTML = "Prezime: ";
        red.appendChild(l);
        let inputPrezime = document.createElement("input");
        inputPrezime.className = "Input"
        inputPrezime.type = "text";
        red.appendChild(inputPrezime);

        red = this.crtajRed(host);
        l = document.createElement("label");
        l.innerHTML = "Pol: ";
        red.appendChild(l);

        let selPol = document.createElement("select");
        selPol.className = "Input"
        red.appendChild(selPol);

        let op
        op = document.createElement("option");
        op.innerHTML = "Zenski";
        op.value = "Z";
        selPol.appendChild(op);
        op = document.createElement("option");
        op.innerHTML = "Muski";
        op.value = "M";
        selPol.appendChild(op);

        red = this.crtajRed(host);
        l = document.createElement("label");
        l.innerHTML = "Sport: ";
        red.appendChild(l);
        let inputSport = document.createElement("input");
        inputSport.className = "Input"
        inputSport.type = "text";
        red.appendChild(inputSport);

        red = this.crtajRed(host);
        l = document.createElement("label");
        l.innerHTML = "Kategorija: ";
        red.appendChild(l);
        let inputKategorija = document.createElement("input");
        inputKategorija.className = "Input"
        inputKategorija.type = "text";
        red.appendChild(inputKategorija);

        red = this.crtajRed(host);
        header = document.createElement("h3");
        header.innerHTML = "Registracija";
        red.appendChild(header);

        red = this.crtajRed(host);
        l = document.createElement("label");
        l.innerHTML = "Takmicenje: ";
        red.appendChild(l);

        let selTakmicenja = document.createElement("select");
        selTakmicenja.setAttribute('id', 'selectTakm')
        selTakmicenja.className = "Input"
        red.appendChild(selTakmicenja);

        this.listaTakmicenja.forEach(p => {
            op = document.createElement("option");
            op.innerHTML = p.sport + " - " + p.naziv;
            op.value = p.id;
            selTakmicenja.appendChild(op);
        })


        red = this.crtajRed(host);
        l = document.createElement("label");
        l.innerHTML = "Klub: ";
        red.appendChild(l);

        let selKlubovi = document.createElement("select");
        selKlubovi.className = "Input"
        red.appendChild(selKlubovi);

        this.listaKlubova.forEach(p => {
            op = document.createElement("option");
            op.innerHTML = p.naziv;
            op.value = p.id;
            selKlubovi.appendChild(op);
        })

        red = this.crtajRed(host);
        let btnRegistruj = document.createElement("button");
        btnRegistruj.innerHTML = "Registruj";
        btnRegistruj.onclick = (ev) => this.dodajTakmicara(inputIme.value, inputPrezime.value,
            selPol.options[selPol.selectedIndex].value, inputSport.value, inputKategorija.value,
            selTakmicenja.options[selTakmicenja.selectedIndex].value, selKlubovi.options[selKlubovi.selectedIndex].value);
        red.appendChild(btnRegistruj);

    }

    crtajFormuOrganizuj(host) {
        host = this.osveziFormu(host);

        let btnVrati = document.createElement("button");
        btnVrati.innerHTML = "Vrati na pocetnu";
        btnVrati.onclick = (ev) => this.crtaj(document.body);
        host.appendChild(btnVrati);

        let header = document.createElement("h3");
        header.innerHTML = "Organizacija takmicenja";
        host.appendChild(header);

        let red = this.crtajRed(host);
        let l = document.createElement("label");
        l.innerHTML = "Naziv: ";
        red.appendChild(l);
        let inputNaziv = document.createElement("input");
        inputNaziv.type = "text";
        red.appendChild(inputNaziv);

        red = this.crtajRed(host);
        l = document.createElement("label");
        l.innerHTML = "Sredstva: ";
        red.appendChild(l);
        let inputSredstva = document.createElement("input");
        inputSredstva.type = "number";
        red.appendChild(inputSredstva);

        red = this.crtajRed(host);
        l = document.createElement("label");
        l.innerHTML = "Sportski objekat : ";
        red.appendChild(l);
        let inputSportskiObjekat = document.createElement("input");
        inputSportskiObjekat.type = "text";
        red.appendChild(inputSportskiObjekat);

        red = this.crtajRed(host);
        l = document.createElement("label");
        l.innerHTML = "Takmicenje : ";
        red.appendChild(l);
        let selTakmicenja = document.createElement("select");
        red.appendChild(selTakmicenja);

        let op;
        this.listaTakmicenja.forEach(p => {
            op = document.createElement("option");
            op.innerHTML = p.sport + " - " + p.naziv;
            op.value = p.id;
            selTakmicenja.appendChild(op);
        })



        let btnApliciraj = document.createElement("button");
        btnApliciraj.innerHTML = "Apliciraj";
        btnApliciraj.onclick = (ev) => this.apliciraj(inputNaziv.value, inputSredstva.value,
            inputSportskiObjekat.value, selTakmicenja.options[selTakmicenja.selectedIndex].value);
        host.appendChild(btnApliciraj);
        
    }

    crtajFormuTakmicenja(host) {

        host = this.osveziFormu(host);


        let btnVrati = document.createElement("button");
        btnVrati.innerHTML = "Vrati na pocetnu";
        btnVrati.onclick = (ev) => this.crtaj(document.body);
        host.appendChild(btnVrati);

        // Takmicenje - Select
        let red = this.crtajRed(host);
        let l1 = document.createElement("label");
        l1.innerHTML = "Takmicenje";
        red.appendChild(l1);

        let selectTakmicenje = document.createElement("select");
        selectTakmicenje.className = "selectTakmicenje"
        red.appendChild(selectTakmicenje);

        let opcijeTakmicenja;
        this.listaTakmicenja.forEach(p => {
            opcijeTakmicenja = document.createElement("option");
            opcijeTakmicenja.innerHTML = p.sport + " - " + p.naziv;
            opcijeTakmicenja.value = p.id;
            selectTakmicenje.appendChild(opcijeTakmicenja);

        })

        // Button nadji sve ucesnike (po takmicenju)
        let btnNadji = document.createElement("button");
        btnNadji.innerHTML = "Nadji";
        btnNadji.onclick = (ev) => this.nadjiSveUcesnikeTakmicenja();
        host.appendChild(btnNadji);
    }


    // Metode pomocne za crtanje
    crtajRed(host) {
        let red = document.createElement("div");
        red.className = "red";
        host.appendChild(red);
        return red;
    }

    crtajPrikaz(host) {

        let provera = this.container.querySelector(".Prikaz");
        if (provera == null) {

            let kontPrikaz = document.createElement("div");
            kontPrikaz.className = "Prikaz";
            host.appendChild(kontPrikaz);

            var tabela = document.createElement("table");
            tabela.className = "tabela";
            kontPrikaz.appendChild(tabela);

            var tabelahead = document.createElement("thead");
            tabela.appendChild(tabelahead);

            var tr = document.createElement("tr");
            tabelahead.appendChild(tr);

            var tabelaBody = document.createElement("tbody");
            tabelaBody.className = "TabelaPodaci";
            tabela.appendChild(tabelaBody);

            let th;
            var zag = ["Klub", "Ime", "Prezime", "Pol", "Takmicenje", "Kategorija", "Datum registracije"];
            zag.forEach(el => {
                th = document.createElement("th");
                th.innerHTML = el;
                tr.appendChild(th);
            })
        }
    }

    osveziSadrzaj() {
        var teloTabele = this.container.querySelector(".TabelaPodaci");
        var roditelj = teloTabele.parentNode;
        roditelj.removeChild(teloTabele);

        teloTabele = document.createElement("tbody");
        teloTabele.className = "TabelaPodaci";
        roditelj.appendChild(teloTabele);
        return teloTabele;
    }

    osveziFormu(host) {
        this.container.removeChild(host);

        let forma = document.createElement("div");
        forma.className = "Forma";
        this.container.appendChild(forma);
        return forma;
    }


    // Metode potrebne za listu takmicenja
    nadjiSveUcesnikeTakmicenja() {
        let optionEl = this.container.querySelector(".selectTakmicenje");
        var idTakmicenja = optionEl.options[optionEl.selectedIndex].value;
        console.log(idTakmicenja);

        this.ucitajTakmicareSaTakmicenja(idTakmicenja)
    }

    ucitajTakmicareSaTakmicenja(idTakmicenja) {

        fetch("https://localhost:5001/Takmicar/NadjiTakmicaraPoTakmicenju/" + idTakmicenja,
            {
                method: "GET"

            }).then(p => {
                if (p.ok) {
                    let forma = this.container.querySelector(".Forma");
                    this.crtajPrikaz(forma);
                    let teloTabele = this.osveziSadrzaj();
                    p.json().then(data => {
                        data.forEach(t => {
                            let takmicar = new Takmicar(t.klub, t.ime, t.prezime, t.pol,t.takmicenje, t.kategorija, t.datum_registracije);
                            console.log(takmicar);
                            takmicar.crtaj(teloTabele);
                        })

                    })
                }
            })
    }

    // Metode potrebne za novu registraciju
    dodajTakmicara(ime, prezime, pol, sport, kategorija, idTakmicenja, idKluba) {
        if(ime===null || ime===undefined || ime===""){
            alert("Unesite ime!");
            return;
        }
        if(prezime===null || prezime===undefined || prezime===""){
            alert("Unesite prezime!");
            return;
        }
        let optionEl = this.container.querySelector("#selectTakm");
        let sportStr = optionEl.options[optionEl.selectedIndex].innerHTML; 
        if(sport===null || sport===undefined || sport==="" ){
            alert("Unesite naziv sporta!");
            return;
        }
        if(!(sportStr.includes(sport)))
        {
            alert("Sport kojim se takmicar bavi se ne poklapa sa takmicenjem za koje ga prijavljujete!");
            return;
        }
        if(kategorija===null || kategorija===undefined || kategorija===""){
            alert("Unesite kategoriju!");
            return;
        }

        fetch("https://localhost:5001/Takmicar/DodajTakmicara/" + ime + "/" + prezime + "/" + pol + "/" + sport + "/" + kategorija,
            {
                method: "POST"
            }).then(s => {
                if (s.status == 200) {
                    s.json().then(data => {
                        console.log(data);
                        alert("Takmicar " +data.ime + " " + data.prezime + " nije postojao u bazi pa je dodat");
                        this.dodajRegistraciju(data.id,idTakmicenja, idKluba);
                    })
                }
                else {
                    if (s.status == 202) {
                        s.json().then(data=>{
                            console.log(data);
                            alert("Takmicar " +data.ime + " " + data.prezime + " postoji u bazi");
                            this.dodajRegistraciju(data.id,idTakmicenja, idKluba);
                        });
                    }
                    if(s.status==404 || s.status==400)
                    {
                        alert("Los unos!");
                    }

                }
            })
            .catch(p => {
                console.log(p);
                alert("Greška u dodavanju takmicara.");
            });
    }

    dodajRegistraciju(idTakmicara,idTakmicenja,idKluba)
    {
        fetch("https://localhost:5001/Registracija/DodajRegistraciju/" + idTakmicara + "/" + idTakmicenja + "/" + idKluba + "/",
            {
                method: "POST"
            }).then(s => {
                if (s.status == 200) {
                    let forma = this.container.querySelector(".Forma");
                    this.crtajPrikaz(forma);
                    let teloTabele = this.osveziSadrzaj();
                    s.json().then(data => {
                        // console.log(data);
                        data.forEach(t=>{
                            let takmicar = new Takmicar(t.klub, t.ime, t.prezime, t.pol,t.takmicenje, t.kategorija, t.datum_registracije)
                            // console.log(takmicar.takmicenje);
                            takmicar.crtaj(teloTabele);
                        })
                        
                    })
                }
            })
            // .catch(p => {
            //     console.log(p);
            //     alert("Greška pri dodavanju registracije.");
            // });
    }

    // Metode potrebne za apliciranje
    apliciraj(naziv, sredstva, sportskiObjekat, idTakmicenja)
    {
        if(naziv===null || naziv===undefined || naziv===""){
            alert("Unesite ime!");
            return;
        }
        if(sredstva===null || sredstva===undefined || sredstva<100000){
            alert("Unesite sredstva! Minimalan iznos za apliciranje je 100 000!");
            return;
        }
        if(sportskiObjekat===null || sportskiObjekat===undefined || sportskiObjekat===""){
            alert("Unesite naziv sporta!");
            return;
        }


        fetch("https://localhost:5001/Organizator/DodajOrganizatora/" + naziv + "/" + sredstva + "/" + sportskiObjekat + "/" + idTakmicenja,
            {
                method: "POST"
            }).then(s => {
                if (s.status == 200) {
                    s.json().then(data => {
                        console.log(data);
                        alert("Uspesno ste aplicirali za organizaciju i vas zahtev je prihvacen!");
                    })
                }
                else {
                    if (s.status == 202) {
                        s.json().then(data=>{
                            console.log(data);
                            alert("Zao nam je vas zahtev nije prihvacen, postoji vec organizator ovog takmicenja koji nudi vise sredstava!");
                        });
                    }
                    if(s.status == 203)
                    {
                        s.json().then(data=>{
                            console.log(data);
                            alert("Cestitamo! Dodeljena vam je organizacija jos jednog takmicenja jer ste ponudili vise od drugih organizatora");
                        });
                    }
                    if(s.status==404 || s.status==400)
                    {
                        alert("Los unos!");
                    }

                }
            })
            .catch(p => {
                console.log(p);
                alert("Greška u dodavanju novog organizatora.");
            });

    }

}