# TAR25ProgrammeerimineII

EntityFrameworkCore\

# Raamatukogu infosüsteem (ASP.NET Core MVC + EF Core)

Raamatukogu rakendus: raamatud, lugejad ja laenutused.
Andmebaas: SQL Server LocalDB. Ühendus on failis `appsettings.json`
(`ConnectionStrings:DefaultConnection`). Käivitamisel rakendatakse migratsioonid
automaatselt (`Database.Migrate()`).

## Mida rakendus oskab
- Raamatu lisamine (saadavus on alati true), muutmine ja kustutamine
- Otsing pealkirja, ilmumisaasta ja saadavuse järgi
- Lugejate lisamine ja nimekiri
- Raamatu laenutamine ja tagastamine (tagastamise kuupäev salvestatakse)
- Lugeja kõikide laenutuste vaatamine
- Laenutatud raamatut ei saa kustutada

## Ülesanne 6. Vastused

**Milline on DbContexti ülesanne?**
DbContext on sild C# objektide ja andmebaasi vahel. Ta hoiab andmebaasiühendust,
jälgib objektide muudatusi (change tracking), tõlgib LINQ-päringud SQL-iks ning
salvestab muudatused andmebaasi (`SaveChanges`). Ühtlasi kirjeldab ta mudeli seosed
(`OnModelCreating`) ja tabelid (`DbSet`).

**Miks ei ole hea kirjutada kõiki andmebaasipäringuid otse kasutajaliidese koodi?**
- Vastutused lähevad segamini: kontroller/vaade peaks tegelema päringu ja kuvamisega,
  mitte andmebaasi loogikaga.
- Sama päring kordub mitmes kohas ja selle muutmine tähendab mitme faili parandamist.
- Koodi on raske testida (ei saa teenust ilma päris andmebaasita kontrollida).
- Ärireeglid (nt "laenutatud raamatut ei kustutata") võivad ühes kohas ununeda.
- Andmebaasi vahetamine või päringu muutmine puudutaks kogu UI-d.

**Millisesse kihti paigutaksid andmebaasiga suhtlemise MVC- või VIPER-arhitektuuris?**
- MVC-s: Model-kihti, täpsemalt teenuste/repositooriumi kihti (siin `BookServices`),
  mida kontroller kutsub liidese (`IBookServices`) kaudu. Kontroller jääb õhukeseks.
- VIPERis: Interactor'isse (äriloogika ja andmete hankimine); Presenter ja View
  ei suhtle andmebaasiga.

Põhjus: andmeligipääs ja ärireeglid on koos eraldi kihis, UI-st sõltumatud ning
testitavad.
