# Vehicle Service Center - Database Mapping Notes

This note documents the Entity Framework Core mapping represented by
`VSC ERD.drawio.png`, `VSC Mapping.png`, the model classes, and
`ProjectContext`.

## Primary keys

Each entity uses a single integer primary key.

| Entity/table | Primary key |
|---|---|
| Users | `UserId` |
| CustomerProfiles | `CustomerProfileId` |
| MechanicProfiles | `MechanicProfileId` |
| Vehicles | `VehicleId` |
| ServiceTypes | `ServiceTypeId` |
| Appointments | `AppointmentId` |
| ServiceOrders | `ServiceOrderId` |
| ServiceOrderItems | `ServiceOrderItemId` |
| Invoices | `InvoiceId` |
| Payments | `PaymentId` |
| SpareParts | `SparePartId` |
| Branches | `BranchId` |

## Foreign keys and relationships

| Dependent entity | Foreign key | Principal entity | Relationship | Required? |
|---|---|---|---|---|
| CustomerProfile | `UserId` | User | One User to zero-or-one CustomerProfile | Required on CustomerProfile |
| MechanicProfile | `UserId` | User | One User to zero-or-one MechanicProfile | Required on MechanicProfile |
| MechanicProfile | `BranchId` | Branch | One Branch to many MechanicProfiles | Required |
| Vehicle | `CustomerProfileId` | CustomerProfile | One CustomerProfile to many Vehicles | Required |
| Appointment | `CustomerProfileId` | CustomerProfile | One CustomerProfile to many Appointments | Required |
| Appointment | `VehicleId` | Vehicle | One Vehicle to many Appointments | Required |
| Appointment | `ServiceTypeId` | ServiceType | One ServiceType to many Appointments | Required |
| Appointment | `MechanicProfileId` | MechanicProfile | One MechanicProfile to many Appointments | Optional |
| Appointment | `BranchId` | Branch | One Branch to many Appointments | Required |
| ServiceOrder | `AppointmentId` | Appointment | Optional one-to-one | Optional and unique |
| ServiceOrder | `CustomerProfileId` | CustomerProfile | One CustomerProfile to many ServiceOrders | Required |
| ServiceOrder | `VehicleId` | Vehicle | One Vehicle to many ServiceOrders | Required |
| ServiceOrder | `MechanicProfileId` | MechanicProfile | One MechanicProfile to many ServiceOrders | Optional |
| ServiceOrder | `BranchId` | Branch | One Branch to many ServiceOrders | Required |
| ServiceOrderItem | `ServiceOrderId` | ServiceOrder | One ServiceOrder to many ServiceOrderItems | Required |
| ServiceOrderItem | `ServiceTypeId` | ServiceType | One ServiceType to many ServiceOrderItems | Optional |
| ServiceOrderItem | `SparePartId` | SparePart | One SparePart to many ServiceOrderItems | Optional |
| Invoice | `ServiceOrderId` | ServiceOrder | One ServiceOrder to zero-or-one Invoice | Required on Invoice and unique |
| Payment | `InvoiceId` | Invoice | One Invoice to many Payments | Required |
| SparePart | `BranchId` | Branch | One Branch to many SpareParts | Required |

## One-to-one relationships

- `User` to `CustomerProfile`: `CustomerProfiles.UserId` has a unique
  index. A customer profile must reference one user, while a user may have
  no customer profile.
- `User` to `MechanicProfile`: `MechanicProfiles.UserId` has a unique
  index. A mechanic profile must reference one user, while a user may have
  no mechanic profile.
- `Appointment` to `ServiceOrder`: `ServiceOrders.AppointmentId` is
  nullable and unique. A service order may be created without an
  appointment, and an appointment may produce at most one service order.
- `ServiceOrder` to `Invoice`: `Invoices.ServiceOrderId` is required and
  unique. Every invoice belongs to one service order, and a service order
  may have at most one invoice.

## One-to-many relationships

- A CustomerProfile owns many Vehicles, Appointments, and ServiceOrders.
- A Branch has many MechanicProfiles, Appointments, ServiceOrders, and
  SpareParts.
- A MechanicProfile may be assigned many Appointments and ServiceOrders.
- A Vehicle may have many Appointments and ServiceOrders.
- A ServiceType may be used by many Appointments and ServiceOrderItems.
- A ServiceOrder contains many ServiceOrderItems.
- A SparePart may be referenced by many ServiceOrderItems.
- An Invoice may receive many Payments.

## Optional relationships

- `Appointment.MechanicProfileId` is nullable because an appointment may
  be booked before a mechanic is assigned.
- `ServiceOrder.AppointmentId` is nullable because a walk-in service order
  may exist without a prior appointment.
- `ServiceOrder.MechanicProfileId` is nullable until a mechanic is
  assigned.
- `ServiceOrderItem.ServiceTypeId` is nullable because an item may
  represent a spare part instead of a service.
- `ServiceOrderItem.SparePartId` is nullable because an item may represent
  labor or a service without a spare part.

Application validation must ensure that a ServiceOrderItem represents a
valid item type and references the appropriate ServiceType and/or
SparePart.

## Unique indexes and column mappings

- `Users.Email` is unique.
- `Vehicles.PlateNumber` and `Vehicles.VIN` are unique.
- `ServiceTypes.Name` is unique.
- `SpareParts.PartNumber` is unique.
- `Invoices.InvoiceNumber` is unique.
- `Payments.TransactionReference` is unique when a value is supplied.
- `UserModel.UserName` maps to the `Users.Name` column.
- `UserModel.Password` stores a BCrypt hash in the
  `Users.PasswordHash` column; plaintext passwords must never be stored.

## EF Core registration

All 12 entities are registered as `DbSet` properties in the single
`ProjectContext`. Data annotations define keys, indexes, maximum lengths,
required fields, decimal column types, and the two renamed User columns.
Relationships are discovered from foreign-key and navigation-property
conventions and are represented in the current EF Core migrations.
