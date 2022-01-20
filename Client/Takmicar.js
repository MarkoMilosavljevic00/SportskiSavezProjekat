export class Takmicar 
{
    constructor(klub, ime, prezime, pol,takmicenje, kategorija, datum_registracije) {
        this.klub=klub;
        this.ime = ime;
        this.prezime = prezime;
        this.pol = pol;
        this.takmicenje=takmicenje
        this.kategorija = kategorija;
        this.datum_registracije = datum_registracije;
    }

    crtaj(host) {

        var tr = document.createElement("tr");
        host.appendChild(tr);

        var el = document.createElement("td");
        el.innerHTML = this.klub;
        tr.appendChild(el);

        el = document.createElement("td");
        el.innerHTML = this.ime;
        tr.appendChild(el);
        el = document.createElement("td");
        el.innerHTML = this.prezime;
        tr.appendChild(el);
        el = document.createElement("td");
        el.innerHTML = this.pol;
        tr.appendChild(el);
        el = document.createElement("td");
        el.innerHTML = this.takmicenje;
        tr.appendChild(el);
        el = document.createElement("td");
        el.innerHTML = this.kategorija;
        tr.appendChild(el);
        el = document.createElement("td");
        el.innerHTML = this.datum_registracije;
        tr.appendChild(el);
    }
}