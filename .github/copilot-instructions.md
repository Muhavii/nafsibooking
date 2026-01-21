## ASP.NET Core Web Application Development Guide

This project is a modern ASP.NET Core web application using C# with Razor Pages and Bootstrap UI.

### Quick Reference

- **Build**: `dotnet build`
- **Run**: `dotnet run`
- **Publish**: `dotnet publish -c Release -o ./publish`
- **Add Package**: `dotnet add package PackageName`

### Project Structure

- **Pages/** - Razor Pages for UI pages
- **Properties/** - Project metadata
- **wwwroot/** - Static files (CSS, JS, images)
- **Program.cs** - Application entry point and middleware configuration

### Development Workflow

1. **Make changes** to Pages, Models, or Services
2. **Save files** - Hot reload is enabled for development
3. **Build**: `dotnet build`
4. **Run**: `dotnet run`
5. **Test** - Application runs on https://localhost:5001

### Common Tasks

**Add a new Razor Page:**
```bash
dotnet new page --name MyPage --output Pages
```

**Add a NuGet package:**
```bash
dotnet add package PackageName
```

**Create migrations (if using EF Core):**
```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Troubleshooting

- **Port already in use**: Change in `appsettings.json` or run on different port
- **SSL certificate issues**: Run `dotnet dev-certs https --trust`
- **Build fails**: Run `dotnet restore` first

### References

- [ASP.NET Core Docs](https://learn.microsoft.com/en-us/aspnet/core/)
- [C# Language](https://learn.microsoft.com/en-us/dotnet/csharp/)
- [Razor Pages](https://learn.microsoft.com/en-us/aspnet/core/razor-pages/)
