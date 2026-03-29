# 🤖 .NET AI Chatbot — Powered by OpenRouter & Mistral 7B

A full-stack AI-powered chatbot web application built with **ASP.NET Core 8.0 MVC**.  
Features user authentication, multi-session chat history, and a professional dark-themed UI  
comparable to ChatGPT — all using free AI models via OpenRouter.



## ✨ Features

- 🔐 **User Authentication** — Register & Login using ASP.NET Core Identity
- 💬 **AI Chat** — Powered by Mistral 7B via OpenRouter API (completely free)
- 📜 **Multi-Session History** — Create, switch, and delete independent conversations
- 🎨 **Professional Dark UI** — ChatGPT-style interface with animations
- ⚡ **Real-time Feel** — Async Fetch API with typing indicator animation
- 🛡️ **Error Handling** — API errors, rate limits, and empty inputs handled gracefully
- 🐳 **Docker Ready** — Multi-stage Dockerfile for containerized deployment

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 8.0 MVC |
| Language | C# 12.0 |
| AI API | OpenRouter (Mistral 7B — Free) |
| Authentication | ASP.NET Core Identity |
| Database | SQLite + Entity Framework Core 8.0 |
| Frontend | Razor Views + Vanilla JavaScript |
| Icons | Font Awesome 6.4 |
| Font | Google Fonts — Inter |
| Container | Docker (multi-stage build) |

---

## 🚀 Setup Instructions

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (or VS Code)
- Free [OpenRouter API Key](https://openrouter.ai/keys)

### Step 1 — Clone the Repository
```bash
git clone https://github.com/Ayesha-Noor-1/.NET-Chatbot-Gemini-.git
cd .NET-Chatbot-Gemini-
```

### Step 2 — Add Your API Key
Open `AIChatbotApp/appsettings.json` and replace the API key:
```json
"GeminiSettings": {
    "ApiKey": "YOUR_OPENROUTER_API_KEY_HERE",
    "ApiUrl": "https://openrouter.ai/api/v1/chat/completions",
    "Model": "mistralai/mistral-7b-instruct:free"
}
```

### Step 3 — Create the Database
Open **Package Manager Console** in Visual Studio and run:
```
Add-Migration InitialCreate
Update-Database
```

### Step 4 — Run the Application
Press **F5** in Visual Studio  
OR run from terminal:
```bash
cd AIChatbotApp
dotnet run
```
Open your browser at `https://localhost:XXXX`

---

## 🐳 Docker Setup

### Build the image
```bash
docker build -t aichatbotapp .
```

### Run the container
```bash
docker run -p 8080:80 aichatbotapp
```

### Open in browser
```
http://localhost:8080
```

---

## 📁 Project Structure
```
AIChatbotApp/
├── Controllers/
│   ├── ChatController.cs        # Chat operations & session management
│   └── AccountController.cs     # Login, register, logout
├── Services/
│   └── GeminiService.cs         # OpenRouter API integration
├── Models/
│   ├── ChatModels.cs            # ChatMessage, ChatSession models
│   └── AuthModels.cs            # Login/Register ViewModels
├── Data/
│   └── ApplicationDbContext.cs  # EF Core database context
├── Views/
│   ├── Chat/Index.cshtml        # Main chat UI
│   └── Account/                 # Login & Register pages
├── appsettings.json             # Configuration (API key goes here)
├── Program.cs                   # App startup & service registration
└── Dockerfile                   # Docker multi-stage build
```

---

## 🔑 Getting a Free API Key

1. Go to [openrouter.ai](https://openrouter.ai)
2. Sign up with Google account (free)
3. Go to [openrouter.ai/keys](https://openrouter.ai/keys)
4. Click **Create Key** → copy the key (starts with `sk-or-...`)
5. Paste it in `appsettings.json`

> ⚠️ **Never commit your API key to GitHub!**  
> The `appsettings.json` is in `.gitignore` for safety.

---

## 📦 NuGet Packages Used
```
Microsoft.AspNetCore.Identity.EntityFrameworkCore  v8.0.11
Microsoft.EntityFrameworkCore.Sqlite               v8.0.11
Microsoft.EntityFrameworkCore.Tools                v8.0.11
Microsoft.Extensions.Http                          v8.0.1
Microsoft.Extensions.Identity.Core                 v8.0.11
```

---

## 👩‍💻 Author

**Ayesha Noor**  
GitHub: [@Ayesha-Noor-1](https://github.com/Ayesha-Noor-1)

---

## 📄 License

This project is for educational purposes — Assignment #2 for Advanced .NET Development.
