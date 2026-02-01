# Disaster Alleviation Foundation  
**A Web-Based Disaster Relief Management System**

---

## Project Overview

The **Disaster Alleviation Foundation** is a full-stack web application designed to support humanitarian organisations in managing disaster relief operations.

The platform enables the public to donate money or goods, apply to volunteer, and allows administrators to manage disasters, track resources, and allocate aid in a structured and transparent way.

The core problem this application addresses is **coordination and accountability in disaster response** — ensuring that donations, volunteers, and relief resources are properly recorded, monitored, and distributed.

This project simulates a real-world NGO operations system and is built as a portfolio-grade application showcasing full-stack web development with enterprise-style patterns.

---

## Key Features

### Public Users
- Submit **monetary donations**
- Submit **goods donations**
- Donate **anonymously** with reusable donor IDs
- Schedule or immediately drop off goods
- Register as a **volunteer**

### Administrators
- Secure admin login system
- Admin dashboard with live statistics
- Register and manage disasters
- View all donations (monetary & goods)
- Allocate money and goods to disasters
- Approve or reject volunteers
- Close completed disasters
- View volunteers assigned to disasters

### System Capabilities
- Role-based access (Admin vs Public)
- SQL database persistence
- Real-time dashboard metrics
- Resource allocation tracking
- Volunteer management workflow

---

## Tech Stack

| Layer | Technology |
|------|------------|
| Frontend | Razor Pages (ASP.NET Core) |
| Backend | ASP.NET Core |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Authentication | ASP.NET Identity |
| UI Framework | Bootstrap 5 |
| Client Scripts | JavaScript |
| Architecture | MVC-style Razor Pages |

---

## High-Level Architecture

```

Browser (User/Admin)
↓
Razor Pages UI
↓
Page Models (C#)
↓
Entity Framework Core
↓
SQL Server Database

```

### Core Domains
- Donations (Monetary, Goods)
- Disasters
- Volunteers
- Categories
- Users (Identity)

Each domain is represented by a model class and persisted using EF Core.

---

## Setup & Installation

### Prerequisites
- .NET 7 or later
- SQL Server / LocalDB
- Visual Studio or VS Code

### Steps

1. Clone the repository
```bash
git clone https://github.com/Tiego3/DisasterAlleviation
```

2. Configure the database connection
   Update `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DisasterAlleviationDb;Trusted_Connection=True;"
}
```

3. Apply database migrations

```bash
dotnet ef database update
```

4. Run the application

```bash
dotnet run
```

5. Open in browser

```
https://localhost:5001
```

---

## How to Use the Application

### For Donors

1. Navigate to **Donate**
2. Choose:

   * Monetary or Goods
3. Enter required details
4. Submit donation
5. Save your Anonymous ID (if applicable)

### For Volunteers

1. Go to **Volunteer**
2. Complete the form
3. Submit application
4. Wait for admin approval

### For Admins

1. Login via **Admin Sign In**
2. Access the **Admin Dashboard**
3. Manage:

   * Disasters
   * Donations
   * Volunteers
   * Resource allocation

---

## Assumptions & Inferred Design Decisions

> These are inferred from the codebase and explicitly stated.

* Admin accounts are manually created.
* Disaster lifecycle: `Active → Closed`.
* Monetary donations are recorded numerically (no payment gateway).
* Goods donations are stored by quantity, not individual inventory units.
* Resource allocation does not automatically deduct stock.
* Volunteers must be approved before being allocated.

---

## Limitations

* No real payment integration.
* No automated email notifications.
* No mobile application.
* Inventory is not physically tracked.
* Admin actions are manual.

---

## Potential Future Enhancements

* Payment gateway integration (PayFast / Stripe)
* Real-time inventory management
* Email/SMS notifications
* Disaster heatmap visualisation
* Public transparency reports
* Advanced analytics dashboard
* Mobile-friendly UI redesign

---

## Why This Project Matters (Recruiter View)

This project demonstrates:

* Full-stack web development
* Real-world domain modelling
* Secure authentication systems
* Role-based access control
* SQL database design
* Admin workflow design
* UI/UX for business systems
* Professional application architecture

It simulates a **production-grade NGO operations platform**, not just a simple CRUD application.

---

## Portfolio Summary

> *A full-stack ASP.NET Core web platform for managing disaster relief operations, including donations, volunteers, and resource allocation with secure admin workflows and SQL-backed persistence.*
