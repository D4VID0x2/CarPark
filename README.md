# Vozový park

### Autor: David Bláha

### Třída: 3.K

### Rok: 2020/21


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
Password: ********

$
```

### Změna hesla

```
$ changepassword

Enter old password: ********
Enter new password: **********
Enter new password to confirm: **********

$ 
```

### Přehled aktuálních rezervací

```
$ reservations list

2:
  Car: Honda Odyssey
  From: 10.4.2021 6:00
  Until: 10.4.2021 22:00

Older reservations: 
1:
  Car: Škoda Octacia
  From: 1.3.2021 12:00
  Until: 5.3.2021 12:00

$ 
```


### Vytvoření nové rezervace

```
$ reservations add

From: 20.4.2021 6:00
Until: 20.4.2021 18:00

Available cars:

2:
  Brand:        Honda
  Model:        Odyssey
  Type:         Personal
  Consumption:  4l/100km

Choose car: 2

Reservation created successfully

$ 
```

Systém zobrazí pouze auta, která jsou v tu dobu volná


### Zrušení rezervace

```
$ reservations cancel 

Your upcomming reservations:

2:
  Car: Honda Odyssey
  From: 10.4.2021 6:00
  Until: 10.4.2021 22:00

3:
  Car: Honda Odyssey
  From: 20.4.2021 6:00
  Until: 20.4.2021 18:00

Choose reservation to cancel: 3


Reservation has been canceled successfully

$ 
```


* * *

## Funkce administrátora


### Vytvoření nového uživatele

```
# users add

Enter name: Jan
Enter surname: Novák
Enter email: email@example.com
Enter password: ********
Confirm password: ********

User created successfully

# 
```

Nový uživatel bude automaticky při prvním přihlášení požádán o změnu hesla

### Zrušení uživatele

```
# users list

1:
  Name: Jan
  Surname: Novák
  Email: email@example.com 
  Last logged in: 2.3.2021 14:23

# users remove 1

User removed successfully

# 
```

Po odstranění uživatele budou automaticky jeho budoucí rezervace zrušeny

### Vytvoření rezervace jménem uživatele

```
# reservations add

From: 20.4.2021 6:00
Until: 20.4.2021 18:00

Available cars:

2:
  Brand:        Honda
  Model:        Odyssey
  Type:         Personal
  Consumption:  4l/100km

Choose car: 2

Enter user ID: 1

Reservation created successfully

# 
```

### Přidání nového auta

```
# cars add
Brand: Honda
Model: Odyssey
Type: Personal
Consumption per 100km: 4

Car added successfully

# 
```

### Zrušení auta

```
# cars list

1:
  Brand:        Škoda
  Model:        Octacia
  Type:         Personal
  Consumption:  3.7l/100km

2:
  Brand:        Honda
  Model:        Odyssey
  Type:         Personal
  Consumption:  4l/100km

# cars remove 2

Car removed successfully

# 
```

Systém neumožní odtranění auta, které je zarezervované a bude ještě použito


### Přidání servisního úkonu k autu

```
# cars service 1

Description: Nutná výměna starých pneumatik
From: 24.4.2021 9:00
Until: 24.4.2021 12:00

Car #1 was marked as being serviced and cannot be reserved during that time

#
```

Pokud v daný čas je již auto zarezervované, upozorní na to administrátora a dostane možnost rezervaci zrušit


### Vynucení změny hesla uživatele

```
# users force-password-renew 1

User #1 will be prompted for password change next time they log in.

# 
```


* * *

## Struktura databáze

XML dokument

`database.xml`

```xml
<?xml version="1.0" encoding="utf-8"?>  
<Database xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">  
  <User email="email@example.com" name="Jan" name="Novák" hash="GWsTR5DVeb4d/ZoAYLfNPa3y2M1NVDUICt7WynzQjB7cKm1m" type="user" lastlogin="1615565616"/>  

  <Car id="1" brand="Honda" model="Odyssey" type="personal" consumption="4" />

  <Reservation id="1" carId="2" from="1615564751" until="1615568762" />
</Database>  
```

