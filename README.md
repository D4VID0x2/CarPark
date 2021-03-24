# Vozový park

### Autor: David Bláha

### Třída: 3.K

### Rok: 2020/21

#### v3

* * *

## Typy uživatelů

### Běžný uživatel
- Přihlášení / odhlášení
- Změna hesla
- Přehled rezervací
- Vytvoření nové rezervace
- Zrušení rezervace


### Administrátor
- Založení nového uživatele
- Zrušení uživatele
- Vytvoření nového auta
- Zrušení auta
- Vytvoření rezervace jménem uživatele
- Vynucení změny hesla uživatele



* * *

## Funkce běžného uživatele

### Přihlášení

```
Email: email@example.com
Heslo: ********

$
```

Heslo se zobrazuje jako hvězdičky

### Změna hesla

```
$ zmena-hesla

Zadejte staré heslo: ********
Zadejte nové heslo: **********
Potvrďte nové heslo: **********

$
```

Nové heslo se musí zadat dvakrát pro potvrzení


### Přehled aktuálních rezervací

```
$ seznam rezervaci

2:
  Auto: Honda Odyssey
  Od: 10.4.2021 6:00
  Do: 10.4.2021 22:00

Starší rezervace:
1:
  Auto: Škoda Octacia
  Od: 1.3.2021 12:00
  Do: 5.3.2021 12:00

$
```


### Vytvoření nové rezervace

```
$ pridat rezervaci

Od: 20.4.2021 6:00
Do: 20.4.2021 18:00

Volná auta:

2:
  Značka:       Honda
  Model:        Odyssey
  Typ:          Personal
  Spotřeba:     4l/100km

Vyberte auto: 2

Rezervace byla úspěšně přidána

$
```

Systém zobrazí pouze auta, která jsou v tu dobu dostupná


### Zrušení rezervace

```
$ zrusit rezervaci

Vaše rezervace:

2:
  Auto: Honda Odyssey
  Od: 10.4.2021 6:00
  Do: 10.4.2021 22:00

3:
  Auto: Honda Odyssey
  Od: 20.4.2021 6:00
  Do: 20.4.2021 18:00

Vyberte rezervaci pro zrušení: 3


Rezervace byla úspěšně zrušena

$
```


* * *

## Funkce administrátora


### Vytvoření nového uživatele

```
# pridat uzivatele

Jméno: Jan
Příjmení: Novák
Email: email@example.com
Heslo: ********
Potvrďte heslo: ********
Admin (A/N): N

Uživatel byl úspěšně přidán

#
```

Nový uživatel bude automaticky při prvním přihlášení požádán o změnu hesla


### Zrušení uživatele

```
# seznam uzivatelu

1:
  Jméno: Jan
  Příjmení: Novák
  Email: email@example.com
  Naposledy přihlášen: 2.3.2021 14:23
  Admin: Ne
  Nutná změna hesla: Ne

# zrusit uzivatele

Uživatel zrušen

#
```

Po odstranění uživatele budou automaticky jeho budoucí rezervace zrušeny a budou smazána jeho osobní data
Minulé rezervace zůstanou v databázi pod jeho anonymním UID


### Vytvoření rezervace jménem uživatele

```
# pridat rezervaci

Od: 20.4.2021 6:00
Do: 20.4.2021 18:00

Volná auta:

2:
  Značka:       Honda
  Model:        Odyssey
  Typ:          Personal
  Spotřeba:     4l/100km

Vyberte auto: 2

Zadejte ID uživatele: 1

Rezervace byla úspěšně přidána

#
```


### Přidání nového auta

```
# pridat auto

Značka: Honda
Model: Odyssey
Typ: Osobni
Spotřeba: 4

Auto přidáno

#
```


### Zrušení auta

```
# seznam aut

1:
  Značka:       Škoda
  Model:        Octacia
  Typ:          Osobní
  Spotřeba:     3.7l/100km

2:
  Značka:       Honda
  Model:        Odyssey
  Typ:          Personal
  Spotřeba:     4l/100km

# zrusit auto 2

Auto #2 zrušeno

#
```

Systém neumožní odtranění auta, které je zarezervované a bude ještě použito


### Přidání servisního úkonu k autu

```
# cars service 1

Popis: Nutná výměna starých pneumatik
Od: 24.4.2021 9:00
Do: 24.4.2021 12:00
Cena: 500 Kč
Číslo faktury: 123456789


Auto #1 bylo označeno jako v servisu a nebude možné ho zarezervovat

#
```

Pokud v daný čas je již auto zarezervované, upozorní na to administrátora a dostane možnost rezervaci zrušit


### Vynucení změny hesla uživatele

```
# zmena hesla uzivatele 1

Uživatel #1 bude při příštím přihlášení požádán o změnu hesla

#
```


* * *

## Struktura databáze

XML dokument

data uložena jako serializovaný object v podobě xml pole

hesla jsou uložena bezpečně jako hash, takže je nelze zneužít

`database.xml`

```xml
<?xml version="1.0" encoding="utf-8"?>  
<Databaze xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">  
  <Uzivatel id="0" email="admin" jmeno="admin" prijmeni="admin" hash="GWsTR5DVeb4d/ZoAYLfNPa3y2M1NVDUICt7WynzQjB7cKm1m" admin="true" lastlogin="1615565616"/>  
  <Uzivatel id="1" email="email@example.com" jmeno="Jan" prijmeni="Novák" hash="GWsTR5DVeb4d/ZoAYLfNPa3y2M1NVDUICt7WynzQjB7cKm1m" admin="false" lastlogin="1615565616"/>  
  <Uzivatel id="2" email="michal@svoboda.cz" jmeno="Michal" prijmeni="Svoboda" hash="GWsTR5DVeb4d/ZoAYLfNPa3y2M1NVDUICt7WynzQjB7cKm1m" admin="false" lastlogin="1615565616"/>  

  <Auto id="1" znacka="Skoda" model="Octavia" typ="osobni" spotreba="3.5" />
  <Auto id="2" znacka="Honda" model="Odyssey" typ="osobni" spotreba="4" />

  <Rezervace id="1" idAuta="2" od="1615564751" do="1615568762" />
  <Rezervace id="2" idAuta="1" od="1615564751" do="1615568762" />
</Databaze>  
```




## Datové struktury

### Uživatel

- UID - unikátní anonymní identifikátor
- email - unikátní identifikátor pro přihlašování
- jméno
- příjmení
- datum a čas posledního přihlášení
- zahashované heslo
- je admin?
- je nutná změna hesla?


### Auto

- UID - unikátní identifikátor
- Značka
- Model
- Typ (osobní, nákladní)
- Spotřeba na 100km
- Servisní úkony


### Rezervace

- UID - unikátní identifikátor
- Uživatel
- Auto
- Od
- Do


### Servisni Úkon

- UID - unikátní identifikátor
- Od
- Do
- Cena
- Popis
- Číslo faktury
 

