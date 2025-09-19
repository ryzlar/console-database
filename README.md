# CSD Console App

Een simpele C# console-applicatie met login- en gebruikersbeheer, gebouwd met **.NET** en **Entity Framework**.  
De app maakt verbinding met een **MySQL database** en ondersteunt gebruikersauthenticatie, profielbeheer en een dynamisch menu.

## ✨ Features
- 🔑 Inloggen en uitloggen met gebruikersgegevens
- 👤 Profielpagina met naam, email en hashed wachtwoord
- 📋 Dynamisch menu afhankelijk van loginstatus
- 💾 Opslag van data in **MySQL database** via Entity Framework
- 🎨 Console UI met kleuren

## 🛠️ Installatie


Pas je database connection string aan in AppDbContext.cs:

csharp
Code kopiëren
optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=;database=csd_consoledb");
Voer de database migraties uit:

bash
Code kopiëren
dotnet ef database update
Start de applicatie:

bash
Code kopiëren
dotnet run
📦 Build naar .exe
Om een uitvoerbaar bestand te maken:

bash
Code kopiëren
dotnet publish -c Release -r win-x64 --self-contained true
Het .exe bestand komt in de map:

bash
Code kopiëren
bin/Release/net8.0/win-x64/publish/
📄 License
Dit project valt onder de MIT License – zie het bestand LICENSE voor details.

🚀 Veel plezier met dit project!
Pull requests en verbeteringen zijn altijd welkom.

yaml
Code kopiëren

---

Wil je dat ik hem **meer technisch en uitgebreid** maak (met codevoorbeelden van loginmenu, filtering enz.), of hou je hem liever **simpel zoals hierboven**?







Vragen aan ChatGPT
