<div align="center">

# Vehicle Service Center

**A full-stack platform for managing vehicle service operations from booking to payment.**

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)
![React](https://img.shields.io/badge/React-19-61DAFB?style=flat-square&logo=react&logoColor=20232A)
![Vite](https://img.shields.io/badge/Vite-8-646CFF?style=flat-square&logo=vite&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-EF_Core-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5-7952B3?style=flat-square&logo=bootstrap&logoColor=white)
![JWT](https://img.shields.io/badge/Auth-JWT-000000?style=flat-square&logo=jsonwebtokens)

</div>

## About the project

Vehicle Service Center brings the daily work of a vehicle workshop into one application. Customers can manage vehicles and appointments, mechanics can work with assigned jobs, and administrators can manage the complete operation through role-specific dashboards.

The project combines an ASP.NET Core REST API with a React frontend and a SQL Server database. It includes JWT authentication, resource ownership checks, email notifications, inventory, invoicing, payments, filtering, sorting, and reporting summaries.

## Main features

- Secure registration and login with JWT authentication and BCrypt password hashing
- Role-based access for Admin, Customer, and Mechanic accounts
- Ownership protection for customer vehicles, appointments, service orders, invoices, and payments
- Customer profiles and vehicle management
- Appointment booking, editing, status tracking, and confirmation email notifications
- Service orders with validated status transitions and detailed service/spare-part items
- Mechanic profiles, branch assignments, and availability management
- Spare-part inventory with stock updates and low-stock warnings
- Invoice and payment workflows with totals and revenue summaries
- Search, filtering, sorting, aggregation, and responsive CRUD pages
- Swagger documentation for backend API testing
- Clear handling for loading, empty, validation, `401`, and `403` states

## User roles

| Role | Main capabilities |
|---|---|
| **Admin** | Manage users, profiles, vehicles, branches, service types, mechanics, appointments, service orders, inventory, invoices, and payments. |
| **Customer** | Manage their profile and vehicles, book appointments, follow owned service orders, view invoices, and record payments. |
| **Mechanic** | Manage availability, view assigned appointments and service orders, update allowed statuses, and maintain service-order items. |

> Frontend role checks improve the user experience. The backend remains responsible for authorization and ownership security.

## Technology stack

| Layer | Technology |
|---|---|
| Frontend | React 19, React Router, Vite 8, Bootstrap 5, Axios |
| Backend | ASP.NET Core 10, C# 14, Entity Framework Core 10 |
| Database | Microsoft SQL Server |
| Authentication | JWT Bearer tokens and BCrypt password hashing |
| Email | MailKit with configurable SMTP settings |
| API documentation | Swagger / OpenAPI |
| Quality checks | Oxlint, Vite production build, .NET build |

## Project structure

```text
Vehicle-Service-Center/
└── Vehicle Service Center/
    └── VehicleServiceCenter/
        ├── Controllers/       # REST API endpoints
        ├── Data/              # Seed data
        ├── Database/          # ERD images and relationship notes
        ├── DTOs/              # API request and response contracts
        ├── Migrations/        # Entity Framework migrations
        ├── Models/            # Database entities
        ├── Services/          # JWT, authorization, and email services
        ├── frontend/
        │   ├── src/api/       # Axios API modules
        │   ├── src/components/# Shared UI and layout components
        │   ├── src/pages/     # Pages organized by feature
        │   └── src/routes/    # Application routes
        ├── Program.cs
        └── VehicleServiceCenter.csproj
```

## Database design

The database contains 12 connected entities covering users, customer and mechanic profiles, vehicles, appointments, services, work orders, inventory, branches, invoices, and payments.

![Vehicle Service Center ERD](./Vehicle%20Service%20Center/VehicleServiceCenter/Database/VSC%20ERD.drawio.png)

The complete primary-key, foreign-key, one-to-one, one-to-many, and optional relationship explanation is available in [Database Mapping Notes](./Vehicle%20Service%20Center/VehicleServiceCenter/Database/MAPPING_NOTES.md).

## Getting started

### Prerequisites

Install the following software:

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20 or newer](https://nodejs.org/)
- [Microsoft SQL Server](https://www.microsoft.com/sql-server/)
- Entity Framework CLI: `dotnet tool install --global dotnet-ef`

### 1. Clone the repository

```bash
git clone https://github.com/SparkToCode2026/Vehicle-Service-Center.git
cd Vehicle-Service-Center/"Vehicle Service Center"/VehicleServiceCenter
```

### 2. Configure the backend

Copy the safe example file and replace its placeholder values locally:

```powershell
Copy-Item .env.example .env
```

For macOS or Linux:

```bash
cp .env.example .env
```

The backend requires these settings:

| Variable | Purpose |
|---|---|
| `ConnectionStrings__DefaultConnection` | SQL Server connection string |
| `Jwt__Key` | Private JWT signing key of at least 32 characters |
| `Jwt__Issuer` | Token issuer |
| `Jwt__Audience` | Token audience |
| `Jwt__ExpireMinutes` | Token lifetime |
| `EmailSettings__*` | SMTP server and sender configuration |

Apply the migrations and run the API:

```powershell
dotnet restore
dotnet ef database update
dotnet run --launch-profile http
```

The API runs at `http://localhost:5248`, and Swagger is available at `http://localhost:5248/swagger` in development.

### 3. Configure the frontend

Open another terminal:

```powershell
cd frontend
Copy-Item .env.example .env
npm install
npm run dev
```

For macOS or Linux, replace `Copy-Item` with `cp`. The frontend runs at `http://localhost:5173` and uses `VITE_API_BASE_URL` to locate the API.

## Useful commands

Run backend checks from `VehicleServiceCenter`:

```powershell
dotnet build
dotnet ef database update
dotnet run --launch-profile http
```

Run frontend checks from `VehicleServiceCenter/frontend`:

```powershell
npm run dev
npm run lint
npm run build
npm run preview
```

## Security notes

- Never commit `.env`, `frontend/.env`, SMTP passwords, database credentials, or JWT signing keys.
- The committed `.env.example` files contain placeholders only.
- Protected API operations require a valid JWT.
- Backend policies enforce roles and resource ownership; UI visibility is not treated as a security boundary.
- Expired sessions are cleared on `401 Unauthorized`, while forbidden actions show a dedicated `403` page.

## Documentation

- [Frontend setup and commands](./Vehicle%20Service%20Center/VehicleServiceCenter/frontend/README.md)
- [Database relationship mapping](./Vehicle%20Service%20Center/VehicleServiceCenter/Database/MAPPING_NOTES.md)
- [Complete database ERD](./Vehicle%20Service%20Center/VehicleServiceCenter/Database/VSC%20ERD.drawio.png)

## Verification

The project currently passes:

- Frontend lint checks
- Frontend production build
- Backend .NET build

Live database, role, ownership, and SMTP journeys should also be checked in the browser before deployment.

---

<div align="center">
Built as part of the Spark to Code 2026 program.
</div>
