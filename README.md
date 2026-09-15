# BibliotecaAPI

BibliotecaAPI è una REST API sviluppata in C# con ASP.NET Core per la gestione di una biblioteca.

Il progetto permette di gestire utenti e libri, effettuare il login tramite JWT e gestire il prestito e la restituzione dei libri.

L'ho realizzato principalmente per approfondire lo sviluppo backend, l'utilizzo di Entity Framework Core, PostgreSQL e l'autenticazione tramite token JWT.

## Tecnologie utilizzate

- C#
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- JWT
- BCrypt
- Postman

## Funzionalità

### Autenticazione

- Registrazione di nuovi utenti
- Login tramite username e password
- Password salvate tramite hash BCrypt
- Generazione di JWT dopo il login
- Ruoli `utente` e `admin`
- Protezione degli endpoint tramite `[Authorize]`

### Gestione libri

Gli utenti autenticati possono:

- visualizzare l'elenco dei libri
- visualizzare un singolo libro
- prendere in prestito un libro
- restituire un libro

Gli amministratori possono inoltre:

- aggiungere nuovi libri
- eliminare libri

## Prestiti

Quando un utente prende in prestito un libro:

- il libro viene impostato come non disponibile
- viene associato all'utente autenticato
- viene impostata una data di restituzione a 30 giorni

L'identità dell'utente viene recuperata direttamente dal JWT, senza dover passare manualmente l'ID dell'utente nella richiesta.

Durante la restituzione viene inoltre controllato che il libro sia effettivamente stato preso in prestito dall'utente che sta effettuando la richiesta.

## Struttura del progetto

```text
BibliotecaAPI/
│
├── Controllers/
│   ├── AuthController.cs
│   └── LibriController.cs
│
├── Data/
│   └── BibliotecaContext.cs
│
├── Models/
│   ├── Libro.cs
│   ├── Utente.cs
│   └── LoginRequest.cs
│
├── Migrations/
├── Program.cs
├── appsettings.json
└── BibliotecaAPI.csproj
```

## Database

Il progetto utilizza PostgreSQL.

Entity Framework Core viene utilizzato per gestire l'accesso al database e le migration.

### Utente

La tabella degli utenti contiene:

- `Id`
- `Username`
- `PasswordHash`
- `Ruolo`

### Libro

La tabella dei libri contiene:

- `Id`
- `Titolo`
- `Autore`
- `Anno`
- `Genere`
- `Disponibile`
- `UtenteId`
- `DataRestituzione`

## Endpoint principali

### Registrazione

```http
POST /api/auth/register
```

Registra un nuovo utente.

### Login

```http
POST /api/auth/login
```

Effettua il login e restituisce un token JWT.

### Visualizzazione libri

```http
GET /api/libri
```

Restituisce l'elenco dei libri.

Richiede autenticazione.

### Visualizzazione singolo libro

```http
GET /api/libri/{id}
```

Restituisce un libro tramite il suo ID.

### Creazione libro

```http
POST /api/libri
```

Aggiunge un nuovo libro.

Richiede il ruolo `admin`.

### Eliminazione libro

```http
DELETE /api/libri/{id}
```

Elimina un libro.

Richiede il ruolo `admin`.

### Prestito libro

```http
POST /api/libri/{libroId}/prestito
```

Permette all'utente autenticato di prendere in prestito un libro disponibile.

L'ID dell'utente viene recuperato direttamente dal JWT.

### Restituzione libro

```http
POST /api/libri/{libroId}/restituzione
```

Permette all'utente che ha preso in prestito il libro di restituirlo.

L'API controlla che il libro sia effettivamente associato all'utente autenticato.

## Autenticazione

Dopo il login viene generato un JWT contenente alcune informazioni sull'utente:

- username
- ID
- ruolo

Il token deve essere inviato nelle richieste protette tramite header:

```http
Authorization: Bearer TOKEN
```

Gli endpoint possono essere protetti tramite:

```csharp
[Authorize]
```

oppure, per operazioni riservate agli amministratori:

```csharp
[Authorize(Roles = "admin")]
```

## Sicurezza delle password

Le password non vengono salvate in chiaro nel database.

Viene utilizzato BCrypt per generare un hash della password durante la registrazione.

Durante il login BCrypt verifica che la password inserita corrisponda all'hash salvato nel database.

## Configurazione

Il progetto utilizza .NET User Secrets per evitare di salvare credenziali e chiavi direttamente nel repository.

Devono essere configurati:

- connection string PostgreSQL
- chiave JWT
- password dell'amministratore iniziale

### Connection string

```bash
dotnet user-secrets set ConnectionStrings:DefaultConnection 'Host=localhost;Port=5432;Database=biblioteca;Username=utente;Password=password'
```

### Chiave JWT

```bash
dotnet user-secrets set Jwt:Key 'inserire-una-chiave-jwt-sufficientemente-lunga'
```

### Password admin

```bash
dotnet user-secrets set Admin:Password 'password-admin'
```

È possibile visualizzare i secrets configurati con:

```bash
dotnet user-secrets list
```

## Avvio del progetto

Clonare il repository:

```bash
git clone <URL_REPOSITORY>
```

Entrare nella cartella del progetto:

```bash
cd BibliotecaAPI
```

Ripristinare i pacchetti:

```bash
dotnet restore
```

Applicare le migration al database:

```bash
dotnet ef database update
```

Avviare l'API:

```bash
dotnet run
```

L'indirizzo su cui viene avviata l'API viene mostrato nel terminale.

## Test

Gli endpoint sono stati testati tramite Postman.

Il flusso principale testato comprende:

1. registrazione di un nuovo utente
2. login dell'utente
3. generazione del JWT
4. login dell'amministratore
5. creazione di un libro
6. visualizzazione dei libri
7. prestito di un libro
8. verifica dell'utente associato al prestito
9. restituzione del libro
10. eliminazione del libro da parte dell'amministratore

## Obiettivo del progetto

Questo progetto è stato realizzato per approfondire alcuni concetti dello sviluppo backend, tra cui:

- REST API
- richieste HTTP
- autenticazione
- autorizzazione
- JWT
- hashing delle password
- Entity Framework Core
- PostgreSQL
- migration
- ruoli utente
- sviluppo asincrono in C#
