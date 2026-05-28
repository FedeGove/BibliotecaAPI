# BibliotecaAPI

API REST per la gestione di una biblioteca, con autenticazione JWT. Costruita con ASP.NET Core, Entity Framework Core e PostgreSQL.

## Tecnologie

- ASP.NET Core 9
- Entity Framework Core + Npgsql
- PostgreSQL
- JWT (JSON Web Token)
- BCrypt.Net

## Prerequisiti

- .NET 9 SDK
- PostgreSQL installato e in esecuzione

## Configurazione

1. Clona il repository
2. Apri `appsettings.json` e aggiorna la stringa di connessione:

```json
"ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=biblioteca;Username=tuousername;Password=tuapassword"
},
"Jwt": {
    "Key": "chiave-segreta-molto-lunga-almeno-32-caratteri",
    "Issuer": "BibliotecaAPI",
    "Audience": "BibliotecaAPI"
}
```

3. Crea il database e applica le migrations:

```bash
dotnet ef database update
```

4. Avvia il server:

```bash
dotnet watch
```

## Autenticazione

Tutti gli endpoint tranne `/api/auth` richiedono un token JWT.

**Registrazione**
```
POST /api/auth/register
Body: { "username": "nome", "password": "password" }
```

**Login**
```
POST /api/auth/login
Body: { "username": "nome", "password": "password" }
```

Il login restituisce un token da includere in ogni richiesta protetta:
```
Authorization: Bearer <token>
```

## Endpoint

### Libri

| Metodo | URL | Descrizione |
|--------|-----|-------------|
| GET | `/api/libri` | Lista tutti i libri |
| GET | `/api/libri/{id}` | Dettaglio libro |
| POST | `/api/libri` | Aggiunge un libro |
| DELETE | `/api/libri/{id}` | Elimina un libro |
| POST | `/api/libri/{libroId}/prestito/{utenteId}` | Registra un prestito |
| POST | `/api/libri/{libroId}/restituzione` | Registra una restituzione |

### Utenti

| Metodo | URL | Descrizione |
|--------|-----|-------------|
| GET | `/api/utenti` | Lista tutti gli utenti |
| GET | `/api/utenti/{id}` | Dettaglio utente |
| POST | `/api/utenti` | Aggiunge un utente |
| DELETE | `/api/utenti/{id}` | Elimina un utente |

## Esempio richiesta autenticata (Postman)

1. Fai login su `POST /api/auth/login`
2. Copia il token dalla risposta
3. In Postman vai su Authorization → Bearer Token
4. Incolla il token e invia la richiesta
