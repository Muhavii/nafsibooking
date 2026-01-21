# Nafsi Booking

A ticket booking platform. Browse events, select tickets, and book.

## Quick Start

1. **Run the app**
   ```bash
   dotnet run
   ```
   Opens at `http://localhost:5270`

2. **Browse events**
   - Home page shows all upcoming events
   - Search by city, venue, or title
   - Filter by date

3. **Book tickets**
   - Click "View & book" on any event
   - Select ticket tier and quantity
   - Enter name, email, accept terms
   - Get confirmation code

4. **Add events** (Admin)
   - Click "Admin" in the navbar
   - Fill event details and ticket tiers
   - Save to make live

## Requirements

- .NET 10.0 or later

## Structure

- `Pages/` — Razor Pages (home, event details, booking)
- `Pages/Admin/` — Event creation
- `Services/` — Event and booking service
- `Models/` — Event, ticket, and booking data
- `wwwroot/css/` — Styling
