# CoreCommerce Backend API

A production-ready, highly scalable headless e-commerce backend built with C# and ASP.NET Core. This API follows **Clean Architecture** principles and implements the **CQRS pattern** using MediatR to ensure strict separation of concerns, high testability, and enterprise-grade performance.

---

## 🏗 Architecture & Technology Stack

The application is structured into four primary layers (Domain, Application, Infrastructure, WebApi) and leverages the following technologies:

| Category | Technology |
| :--- | :--- |
| **Framework** | .NET 8.0 / ASP.NET Core Web API |
| **Language** | C# 12 |
| **Architecture** | Clean Architecture, CQRS (MediatR) |
| **Database** | PostgreSQL |
| **ORM** | Entity Framework Core (Code-First) |
| **Caching / State** | Redis (StackExchange.Redis) |
| **Authentication** | JWT (JSON Web Tokens) with BCrypt |
| **Validation** | FluentValidation |
| **Logging** | Serilog (Structured JSON Logging) |

---

## ✨ Core Features

*   **Identity & Security:** Role-Based Access Control (Admin vs. Customer), secure password hashing, and JWT-based authentication.
*   **Catalog Management:** Admin CRUD operations with soft-delete capabilities, and high-performance public browsing queries with filtering, sorting, and pagination.
*   **High-Performance Cart:** Atomic shopping cart state management offloaded entirely to Redis memory for ultra-low latency.
*   **Transactional Order Engine:** Reliable database transactions preventing inventory drift, locking stock during checkout, and processing payments via an abstracted gateway interface.
*   **Production Hardened:** Global exception handling (RFC 7807 Problem Details), IP-based rate limiting, strict CORS policies, and structured application logging.

---

## 📂 Project Structure

| Layer | Responsibility |
| :--- | :--- |
| **CoreCommerce.Domain** | Enterprise logic, core entities, enums, and global exceptions. Contains zero external dependencies. |
| **CoreCommerce.Application** | Business use cases, CQRS commands/queries (MediatR), DTOs, and interface abstractions. |
| **CoreCommerce.Infrastructure** | External integrations: EF Core DbContext, PostgreSQL repositories, Redis Cache, JWT generation, and Payment mocks. |
| **CoreCommerce.WebApi** | The HTTP delivery mechanism. Contains Controllers, global middleware, Serilog setup, and Swagger configuration. |

---

## 🚀 Getting Started

### Prerequisites
Ensure the following are installed on your development environment:
*   .NET 8.0 SDK
*   PostgreSQL (or Docker to run an instance)
*   Redis (or Docker to run an instance)

### Setup Instructions

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/DEE-PRINCE001/seller-mod.git](https://github.com/your-org/CoreCommerce.git)
   cd CoreCommerce
   ```

2. **Configure Database & Cache:**
   Update the `appsettings.json` in the `CoreCommerce.WebApi` project with your local PostgreSQL and Redis connection strings.
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=CoreCommerceDb;Username=postgres;Password=yourpassword",
     "RedisConnection": "localhost:6379"
   }
   ```

3. **Apply Database Migrations:**
   Navigate to the WebApi folder and run the EF Core migrations to build the PostgreSQL schema.
   ```bash
   cd CoreCommerce.WebApi
   dotnet ef database update --project ../CoreCommerce.Infrastructure
   ```

4. **Run the Application:**
   ```bash
   dotnet run
   ```

---

## 📖 API Documentation

When running in the Development environment, the API documentation is automatically generated and served via Swagger UI. 

Navigate to `https://localhost:port/swagger` in your browser to view the interactive OpenAPI specification. You can authenticate directly within Swagger by clicking the **Authorize** button and pasting your JWT token (format: `Bearer {your_token}`).

---

## 🛡️ Security & Performance Notes

*   **Rate Limiting:** A strict policy limits sensitive endpoints (like checkout and auth) to 100 requests per minute per IP to prevent brute-force attacks.
*   **Asynchronous I/O:** All database and cache operations utilize `async/await` to prevent thread-pool starvation under high concurrent loads.
*   **No-Tracking Queries:** Public read-heavy endpoints utilize EF Core's `AsNoTracking()` for significant memory and performance optimizations.