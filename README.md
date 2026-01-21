# Nafsi Booking

A modern ticket booking platform built with ASP.NET Core, C#, Razor Pages, and Bootstrap. Find and book tickets to upcoming events with ease.

## Project Structure

```
C#Proj/
├── Pages/                          # Razor Pages (UI pages)
│   ├── Index.cshtml               # Home page
│   ├── Privacy.cshtml             # Privacy policy
│   ├── Error.cshtml               # Error handling page
│   └── Shared/                    # Shared layouts and components
├── Properties/                     # Project properties and settings
├── wwwroot/                        # Static files (CSS, JS, images)
│   ├── css/                       # Stylesheets
│   ├── js/                        # JavaScript files
│   └── lib/                       # Third-party libraries
├── appsettings.json               # Configuration settings
├── appsettings.Development.json   # Development-specific settings
├── Program.cs                     # Application startup and configuration
└── C#Proj.csproj                 # Project file with dependencies
```

## Features

- **Razor Pages**: Modern page-based architecture
- **Bootstrap 5**: Responsive UI framework
- **Authentication Ready**: Built-in support for authentication
- **Static Assets**: CSS and JavaScript bundling support
- **Error Handling**: Global error handling and logging

## Prerequisites

- .NET 10.0 or later
- Visual Studio Code or Visual Studio 2022+

## Getting Started

### 1. Restore Dependencies
```bash
dotnet restore
```

### 2. Build the Project
```bash
dotnet build
```

### 3. Run the Application
```bash
dotnet run
```

The application will be available at `https://localhost:5001` (or `http://localhost:5000`).

## Development

### Project Configuration
- **Framework**: ASP.NET Core 10.0
- **Language**: C# 13
- **Target**: .NET 10.0

### Key Files

- **Program.cs**: Application startup, middleware configuration, and dependency injection
- **appsettings.json**: Application settings and configuration
- **Pages/**: Razor Pages for UI

## Building and Deployment

### Development Build
```bash
dotnet build
```

### Release Build
```bash
dotnet build -c Release
```

### Publish
```bash
dotnet publish -c Release -o ./publish
```

## Testing

To run tests (if added):
```bash
dotnet test
```

## Adding Features

### Add a New Razor Page
```bash
dotnet new page --name MyPage --namespace C#Proj.Pages --output Pages
```

### Add NuGet Package
```bash
dotnet add package PackageName
```

## Learn More

- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [Razor Pages](https://learn.microsoft.com/en-us/aspnet/core/razor-pages/)
- [Bootstrap Documentation](https://getbootstrap.com/)

## License

MIT
