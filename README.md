# Item Ordering Web Application

A full-stack web application built with ASP.NET Core and PostgreSQL, 
featuring a multi-role system for managing orders between users, 
sellers, and couriers.

## Features

- **Secure authentication** — login system with email verification support
- **Role-based access control** — four distinct roles: Admin, User, Courier, Seller
- **Role application system** — users can apply for roles, admins approve requests
- **Order management** — users can place and track the status of their orders
- **Admin dashboard** — full control over users, roles, and orders
- **Chat system** — ongoing

## Tech Stack

- **Backend:** C# / ASP.NET Core (Razor Pages)
- **Database:** PostgreSQL with Entity Framework Core
- **Frontend:** HTML, CSS, JavaScript
- **Auth:** ASP.NET Identity with email verification

## Getting Started

### Prerequisites
- .NET 8 SDK
- PostgreSQL

### Setup
1. Clone the repository
2. Update the connection string in `appsettings.json` with your PostgreSQL credentials
3. Apply migrations
4. Run the application

## Roles

| Role | Permissions |
|------|-------------|
| Admin | Manage users, approve role requests, full access |
| Seller | List and manage items |
| Courier | View and update delivery status |
| User | Browse items, place orders, track status |
