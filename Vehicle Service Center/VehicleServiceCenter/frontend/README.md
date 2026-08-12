# Vehicle Service Center Frontend

React and Vite frontend for the Vehicle Service Center API. It provides separate Admin, Customer, and Mechanic journeys, JWT-authenticated API access, role-aware navigation, and management workflows for every project controller.

## Requirements

- Node.js 20 or newer
- The ASP.NET Core API running locally

## Installation

From the `frontend` directory:

```powershell
npm install
Copy-Item .env.example .env
npm run dev
```

Open the URL printed by Vite. By default, API requests use `http://localhost:5248`.

## Environment variables

`VITE_API_BASE_URL` is the public base URL of the backend API. Vite variables are included in browser code, so never put database passwords, SMTP passwords, JWT signing keys, or any other secret in `frontend/.env`.

Backend secrets belong in the root `.env` file or a secure deployment secret store. The committed `.env.example` files contain placeholders only.

## Commands

```powershell
npm run dev      # development server
npm run build    # production build in dist/
npm run preview  # preview the production build
npm run lint     # Oxlint checks
```

## Main journeys

- Admin: users, customer profiles, vehicles, service types, mechanics, appointments, service orders/items, invoices, payments, spare parts, and branches.
- Customer: profile and vehicles, appointment booking/history/details, and owned service-order/invoice/payment records.
- Mechanic: dashboard and availability, assigned appointments and service orders, order status/items, and spare-part inventory.

The frontend hides actions that do not apply to the signed-in role. The backend remains responsible for authorization and ownership checks. A `401` clears the expired session and returns to login; a `403` opens the unauthorized page with a clear explanation.

## Manual verification

Use [SPRINT_2_USER_TEST_CHECKLIST.md](./SPRINT_2_USER_TEST_CHECKLIST.md) to record browser evidence for the three roles, all CRUD workflows, loading/empty/error states, and authorization failures.
