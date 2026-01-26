🎰 SafeCasino - Online Casino Platform

SafeCasino is een volledig functioneel online casino platform gebouwd met ASP.NET Core 9.0 MVC. De applicatie biedt een moderne, veilige en gebruiksvriendelijke omgeving voor het spelen van casino games met uitgebreide functies voor verantwoord gokken.

📋 Inhoudsopgave

- Overzicht
- Functionaliteiten
- Technische Stack
- Project Structuur
- Installatie
- Database Configuratie
- SMTP Configuratie
- Seeding
- Standaard Accounts
- Admin Panel
- Screenshots

🎯 Overzicht

SafeCasino is een volledig uitgewerkte casino website met:

- 25 Casino Games verdeeld over 5 categorieën
- Meerdere Game Providers (NetEnt, Evolution Gaming, Microgaming, Play'n GO, Pragmatic Play)
- Verantwoord Gokken features en limieten
- Review Systeem voor spelers
- Tournament Systeem met dagelijkse prijzenpotten
- **Admin Panel voor gebruikersbeheer**
- **SMTP Email integratie**
- Volledig Responsive design met moderne UI

✨ Functionaliteiten

👥 Gebruikersbeheer

- Registratie & Authenticatie met ASP.NET Core Identity
- Rol-gebaseerde autorisatie (Admin, Moderator, Player)
- Leeftijdsverificatie (18+)
- Email verificatie via SMTP
- Wachtwoord herstel via email
- Profiel beheer met saldo tracking
- Admin panel voor gebruikersbeheer

🎮 Casino Games

5 Hoofdcategorieën:

- 🃏 Blackjack (5 varianten)
- 🎰 Slots (5 populaire games)
- 🎲 Roulette (5 varianten)
- ♠️ Poker (5 varianten)
- 📹 Live Casino (5 live games)

Game Features:

- Gedetailleerde game informatie (RTP, min/max inzet)
- Spel statistieken (aantal keer gespeeld)
- Filterfunctionaliteit (categorie, provider, populariteit)
- Zoekfunctie
- Populaire & nieuwe games badges

⭐ Review Systeem

- Spelers kunnen reviews schrijven
- Sterren beoordeling (1-5 sterren)
- Moderatie systeem (goedkeuring vereist)
- Gemiddelde ratings per game
- Bewerken en verwijderen van eigen reviews

🏆 Tournament Systeem

- Dagelijkse tournaments met €30,000 prijzenpot
- Animal Tournament thema
- Punten systeem (10 punten per €1 inzet)
- Top 100 spelers ontvangen prijzen
- Live leaderboard tracking

🛡️ Verantwoord Gokken

- Inzet limieten (dagelijks, wekelijks, maandelijks)
- Verlies limieten
- Sessie tijd limieten
- Zelf-uitsluiting opties
- Hulplijnen en professionele hulp informatie
- Waarschuwingssignalen detectie

🔧 Admin Panel

- Gebruikersoverzicht met zoek- en filterfunctionaliteit
- Gebruiker details bekijken (profiel, saldo, transacties)
- Rol management (toewijzen/verwijderen van rollen)
- Account blokkering/deblokkering
- Email verificatie status beheren
- Statistieken dashboard

📱 Responsive Design

- Volledig responsive layout
- Modern dark theme met purple/gold accent
- Professionele casino esthetiek
- Geoptimaliseerd voor desktop, tablet en mobiel
- Font Awesome icons integratie

🛠 Technische Stack

Backend

- Framework: ASP.NET Core 9.0 MVC
- Database: SQL Server (publiek bereikbare server)
- ORM: Entity Framework Core 9.0
- Authentication: ASP.NET Core Identity
- Email: SMTP (Gmail/Outlook configureerbaar)
- Taal: C# (.NET 9.0)

Frontend

- Framework: Bootstrap 5
- Icons: Font Awesome 6.4.0
- JavaScript: jQuery
- Styling: Custom CSS met CSS Variables

📁 Project Structuur
SafeCasino/
├── SafeCasino.Web/              # Hoofd web applicatie
│   ├── Controllers/             # MVC Controllers
│   │   ├── AdminController.cs   # Admin panel controller
│   │   └── ...
│   ├── Views/                   # Razor Views
│   │   ├── Admin/               # Admin panel views
│   │   └── ...
│   ├── ViewModels/              # View Models
│   ├── wwwroot/                 # Static files (CSS, JS, images)
│   ├── appsettings.json         # Configuratie (Database, SMTP)
│   └── Program.cs               # Application entry point
│
├── SafeCasino.Api/              # REST API
│   ├── Controllers/             # API Controllers
│   ├── appsettings.json         # API Configuratie
│   └── Program.cs               # API entry point
│
├── SafeCasino.Data/             # Data Layer
│   ├── Configurations/          # EF Core Fluent API configuraties
│   ├── Data/                    # DbContext & Seeding
│   ├── Entities/                # Domain Models
│   ├── Identity/                # Identity Models
│   ├── Migrations/              # EF Core Migrations
│   └── Seed/                    # Database Seed Data
│
└── SafeCasino.Shared/           # Gedeelde Code
    ├── Constants/               # API Routes & Constants
    ├── DTOs/                    # Data Transfer Objects
    ├── Requests/                # Request Models
    └── Responses/               # Response Models

🚀 Installatie

Vereisten

- .NET 9.0 SDK of hoger
- SQL Server (publiek bereikbare instantie)
- Visual Studio 2022 (optioneel maar aanbevolen)
- Gmail of Outlook account voor SMTP (optioneel, voor email functies)
- Git

Stap 1: Clone Repository

```bash
git clone https://github.com/SoufianeAbk/safecasino.git
cd safecasino
```

Stap 2: NuGet Packages Herstellen

```bash
dotnet restore
```

Stap 3: Connection String & SMTP Configureren

Open `SafeCasino.Web/appsettings.json` en configureer de database en SMTP instellingen:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=jouw-server.database.windows.net;Database=SafeCasinoDB;User Id=jouw-username;Password=jouw-wachtwoord;Encrypt=True;TrustServerCertificate=False;"
  },
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SenderEmail": "jouw-email@gmail.com",
    "SenderName": "SafeCasino",
    "Username": "jouw-email@gmail.com",
    "Password": "jouw-app-wachtwoord"
  }
}
```

SMTP Providers:

Gmail:
- SmtpServer: `smtp.gmail.com`
- SmtpPort: `587`
- Gebruik App-specifiek wachtwoord (niet je normale wachtwoord)

Outlook/Hotmail:
- SmtpServer: `smtp-mail.outlook.com`
- SmtpPort: `587`
- Gebruik je normale account credentials

Stap 4: Database Aanmaken

```bash
cd SafeCasino.Web
dotnet ef database update
```

Stap 5: Applicatie Starten

```bash
dotnet run
```

De applicatie is nu beschikbaar op:
- HTTPS: https://localhost:7243
- HTTP: http://localhost:5136

💾 Database Configuratie

Entity Relaties

```
GameCategory (1) ──────< (N) CasinoGame (N) >────── (1) GameProvider
                                 │
                                 │
                         (1) ────┴──── (N)
                                 │
                              Review
                                 │
                         (N) ────┴──── (1)
                                 │
                         ApplicationUser
```

Belangrijke Tabellen

- Users - Gebruikers (Identity)
- Roles - Rollen (Admin, Moderator, Player)
- CasinoGames - Casino spellen (25 vooraf ingevuld)
- GameCategories - Spelcategorieën (5 categorieën)
- GameProviders - Game providers (5 providers)
- Reviews - Speler reviews

Database Hosting

De applicatie gebruikt een publiek bereikbare SQL Server in plaats van LocalDB, waardoor:
- De database toegankelijk is vanaf meerdere machines
- Deployment naar productie eenvoudiger is
- Betere schaalbaarheid en performance mogelijk is
- Externe backups en monitoring mogelijk zijn

📧 SMTP Configuratie

SafeCasino gebruikt SMTP voor het verzenden van emails:

Ondersteunde Email Functies

- ✅ Email verificatie bij registratie
- ✅ Wachtwoord herstel emails
- ✅ Account notificaties
- ✅ Welkomst emails

SMTP Setup

1. Gmail gebruiken:
   - Ga naar Google Account instellingen
   - Schakel 2-staps verificatie in
   - Genereer een App-specifiek wachtwoord
   - Gebruik dit wachtwoord in `appsettings.json`

2. Outlook gebruiken:
   - Gebruik je normale account credentials
   - Mogelijk moet "Less secure apps" worden ingeschakeld

3. Andere providers:
   - Pas `SmtpServer` en `SmtpPort` aan
   - Configureer credentials in `EmailSettings`

🌱 Seeding

De database wordt automatisch gevuld met testdata bij eerste start:

### Categorieën (5)

- Blackjack - Klassieke kaartspellen
- Live Casino - Live dealer games
- Roulette - Verschillende roulette varianten
- Poker - Video en table poker
- Slots - Slot machines

### Game Providers (5)

- NetEnt - Premium gaming oplossingen
- Evolution Gaming - Live casino specialist
- Microgaming - Gerespecteerde developer
- Play'n GO - Mobile-first developer
- Pragmatic Play - Multi-product provider

### Casino Games (25)

- 5 Blackjack games
- 5 Live Casino games
- 5 Roulette games
- 5 Poker games
- 5 Slots games

Alle games hebben realistische data:
- RTP percentages (88% - 99.7%)
- Min/Max inzetten
- Spel statistieken
- Beschrijvingen in het Nederlands

👤 Standaard Accounts

Admin Account

- Email: admin@safecasino.be
- Wachtwoord: Admin123!
- Rol: Administrator
- Saldo: €10,000
- Rechten: Volledige toegang tot systeem + Admin Panel

Test Speler Account

- Email: speler@safecasino.be
- Wachtwoord: Speler123!
- Rol: Player
- Saldo: €500
- Rechten: Standaard speler rechten

🔧 Admin Panel

Het Admin Panel is toegankelijk via `/Admin` en biedt uitgebreide gebruikersbeheer functionaliteit:

Features

Gebruikersoverzicht:
- Paginering en zoekfunctionaliteit
- Filteren op rol en status
- Sorteren op verschillende velden
- Quick actions voor veelvoorkomende taken

Gebruiker Details:
- Volledige profielinformatie
- Saldo overzicht
- Rol management
- Account status (actief/geblokkeerd)
- Email verificatie status
- Registratiedatum en laatste login

Acties:
- Rollen toewijzen/verwijderen
- Account blokkeren/deblokkeren
- Email verificatie forceren
- Gebruikersstatistieken bekijken
- Saldo aanpassingen (toekomstige feature)

Toegang

Alleen gebruikers met de **Administrator** rol hebben toegang tot het Admin Panel.

🎨 Design Features

```css
--primary-purple: #7c3aed      /* Hoofd paars */
--secondary-purple: #6d28d9    /* Donkerder paars */
--accent-purple: #8b5cf6       /* Accent paars */
--gold-accent: #ffd700         /* Goud accent */
--dark-bg: #0f0f23            /* Donkere achtergrond */
--card-bg: #1a1a2e            /* Card achtergrond */
```

Animaties

- Fade-in animaties voor cards
- Hover effects op game cards
- Smooth transitions
- Responsive navigation

Responsive Breakpoints

- Mobile: < 768px
- Tablet: 768px - 1024px
- Desktop: > 1024px

🗺️ Routes Overzicht

Authenticatie

- Login (`/Account/Login`) - Inloggen
- Registratie (`/Account/Register`) - Account aanmaken
- Profiel (`/Account/Profile`) - Gebruikersprofiel (vereist login)

Admin (vereist Administrator rol)

- Dashboard (`/Admin/Index`) - Gebruikersoverzicht
- Details (`/Admin/Details/{id}`) - Gebruiker details
- Rol Management (`/Admin/AssignRole/{id}`) - Rollen beheren

Reviews (vereist login)

- Review Schrijven (`/Reviews/Create`) - Nieuwe review
- Review Bewerken (`/Reviews/Edit/{id}`) - Review aanpassen
- Review Verwijderen (`/Reviews/Delete/{id}`) - Review verwijderen

🔒 Beveiliging

- Password Hashing met Identity
- Anti-Forgery Tokens op alle forms
- SSL/HTTPS enforced in productie
- Rol-gebaseerde autorisatie
- Email verificatie voor nieuwe accounts
- Lockout policy na mislukte login pogingen
- Beveiligde database verbinding met encryptie
- SMTP credentials veilig opgeslagen

📊 Features Roadmap

- [ ] Live chat support
- [ ] Game demo modes
- [ ] VIP programma
- [ ] Cashback systeem
- [ ] Mobile app (iOS/Android)
- [ ] Realtime tournament leaderboards
- [ ] Social features (vrienden, chat)
- [ ] Bonussen en promoties management
- [ ] Bulk email functionaliteit

💾 AI Assistentie

- https://chatgpt.com/c/69454bd9-f518-832a-9342-00eb9f9067ea
- https://chatgpt.com/c/69404b79-7b48-832c-80c1-6b937b394a61
- https://chatgpt.com/c/69455355-ea38-832e-8458-77374bfeac7d

**⚠️ Verantwoord Gokken Waarschuwing:**
Gokken kan verslavend zijn, speel verantwoord. 18+
