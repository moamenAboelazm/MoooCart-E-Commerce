# 🛒 MoooCart API — Enterprise E-Commerce Backend

A high-performance, robust, and pure **RESTful API Backend** built using **ASP.NET Core 9.0**. This repository focuses exclusively on advanced server-side architecture, optimization strategies, and database management patterns. 

**You Can Try it by the link : http://mooocart.runasp.net/swagger/index.html**

Ready accounts :
1. Admin Account (Has full permissions)
{
  "email": "adminNu1@gmail.com",
  "password": "AdminNu#1"
}
2. User Account:
{
  "email": "userNu1@gmail.com",
  "password": "UserNu#1"
}

---

## 🎯 Project Purpose & Architectural Vision
This project was built with a core focus on **applied software engineering principles, scalability, and hands-on learning**. The objective was to design a production-grade, pure backend API that simulates real-world business challenges—such as inventory concurrency, memory management under load, caching strategies, and secure data mapping—without relying on any front-end frameworks (No Blazor, No MVC views).

---

## ⚡ Core Technical Features & Optimization Strategies

### 1. High-Concurrency Asynchronous Programming (`async` / `await`)
The entire application lifecycle is implemented using end-to-end asynchronous programming. From the incoming HTTP requests at the controller level down to the Entity Framework database access layer, non-blocking I/O operations ensure the server maximizes thread pool utilization and handles thousands of concurrent requests smoothly.

### 2. Proactive In-Memory Caching (`IMemoryCache`)
To mitigate database bottlenecks and save server resources, an advanced **Cache-Aside Pattern** has been integrated into highly read-intensive operations:
* **Dashboard Analytics:** Computes expensive historical metrics (total users, lifetime revenue, products sold) and stores them in cache memory for 5 minutes.
* **Category Catalog:** Caches categories with an absolute expiration of 2 hours and a sliding expiration of 30 minutes.
* **Proactive Cache Invalidation:** To prevent stale data, write operations (`Add`, `Update`, `Delete`) instantly trigger cache eviction (`_cache.Remove`), ensuring users experience instantaneous catalog updates without sacrificing look-up performance.

### 3. Advanced LINQ & Query Translations Optimization
To maintain optimal memory consumption, query building avoids memory-heavy execution patterns:
* **Server-Side Evaluation:** Heavily optimized transformations prevent execution runtime exceptions by projecting joins directly into lightweight Anonymous Objects `new { ... }` before applying aggregation routines like `SumAsync`.
* **Prevention of Memory Ballooning:** Deprecated full-table lookups (`GetAllAsync`) have been replaced with queryable boundaries to eliminate accidental buffering of entire data sets into server RAM.

### 4. Enterprise Architecture Patterns
* **Generic Repository Pattern:** Implemented a reusable, abstract data access layer across all database models to enforce clean separation of concerns.
* **Flexible Query Building (`GetQueryable`):** The repository exposes an `IQueryable<T>` wrapper, allowing specific business services to chain custom database filters (`.Include()`, `.Where()`) which compile directly into clean, optimized native SQL on the database side.
* **DTO Pattern & Clean Data Mapping:** AutoMapper handles safe boundaries between the persistence models (`ClsProduct`, `ClsCategory`) and the client-facing contracts, protecting sensitive fields and eliminating nested JSON object reference loop issues.

---

## 📦 Functional Domain Breakdown

### 🔐 1. Identity, Authentication & RBAC (Role-Based Access Control)
* Fully integrated with **ASP.NET Core Identity** utilizing a customized user archetype (`AppUser`).
* Token-based authentication powered by cryptographically signed **JWT (JSON Web Tokens)**.
* Claims-based context routing allowing automated user identification directly from incoming authorization headers.
* Strict role hierarchies ensuring only verified administrators possess full mutation permissions over the system catalog.

### 🛍️ 2. Product Catalog, Searching & Server Pagination
* Replaced traditional client-side heavy filtering with memory-conscious server-side query manipulation.
* Complete catalog capabilities supporting precise parameters:
  * **Pagination:** Dynamic `page` and bounded `pageSize` parameters prevent server flooding.
  * **Searching:** Case-insensitive string parsing executes native indexing lookups directly on the SQL layer.
  * **Filtering:** Explicit contextual separation via `CategoryId`.
  * **Sorting:** Structured indexing options (`priceAsc`, `priceDesc`, `nameDesc`).

### 🛒 3. Persistent Shopping Cart Architecture
* Transitioned from volatile in-memory sessions to database-backed persistence utilizing a resilient schema (`ClsCartItem`).
* Guarantees synchronized state across multi-device user profiles.
* Implements pre-allocation stock validation metrics before registering adding actions to a user's cart bounds.

### 💳 4. Transactional Checkout Logic
* Streamlined API route design via single-action triggers (`POST /api/cart/checkout`) requiring zero body overhead by automatically reconstructing the state from user-tied data rows.
* **Concurrent Stock Controls:** Validates store inventory thresholds immediately before final submission to avoid race conditions.
* **Analytical Schema Persistence:** Transforms transient buying logs cleanly into concrete reporting objects (`ClsProductsHistory`), freezing the exact transactional unit cost (`PurchasedPrice`) at the exact point of checkout completion.

---

## 🛠️ Technology Stack
* **Framework:** .NET 9.0 (ASP.NET Core Web API)
* **Database Management:** Entity Framework Core (EF Core) 9.0
* **Storage Engine:** Microsoft SQL Server
* **Identity Management:** ASP.NET Core Identity
* **Logging System:** Serilog (Structured File & Console Sink Logging)
* **Utilities:** AutoMapper, System.Security.Claims JWT Bearer Middleware

---

## 📂 System Architecture Overview
The system architecture follows a clean, decoupled layout divided by structural responsibilities:
```text
MoooCart Solution
│
├── MoooCart.DB (Data Access Layer)
│   ├── Contexts/          # AppDbContext & Database Configurations
│   ├── Models/            # Database Entities (ClsProduct, ClsCartItem, etc.)
│   └── Base/              # Repository implementations (GenericRepository)
│
├── MoooCart.lib (Library & Contracts Layer)
│   ├── Base/              # Service Contracts & Domain Interfaces (ICartService)
│   └── DTOs/              # Data Transfer Objects & API Request contracts
│
└── MoooCart.API (Presentation Layer)
    ├── Controllers/       # RESTful API Endpoints (CartController, ProductsController)
    ├── Services/          # Concrete Business Logic implementations (CartServices)
    └── Program.cs         # Service Container Registration & Middleware Pipeline

```
---
## ⚖️ Learning Outcomes & Applied Concepts:
Through building this backend from scratch, the following implementation strategies were thoroughly mastered:

1. Building clean **Database Project Isolation** models without exposing physical internal infrastructure directly to web APIs.
2. Managing **EF Core Tracking and Object Attachment States** within generic service scopes to prevent unwanted row insertion loops.
3. Structuring optimal **LINQ Expressions** that cleanly convert to raw T-SQL statements, optimizing performance inside the storage server.
4. Implementing a production-ready **In-Memory Caching infrastructure** that drastically drops server response times down to `<2ms`.
---
## ⚖️ Some screenshots from SwaggerUI:
<img width="1846" height="804" alt="image" src="https://github.com/user-attachments/assets/a644f287-199e-4dd6-ad69-0f1ad0fb33fd" />
<img width="1845" height="556" alt="image" src="https://github.com/user-attachments/assets/7b1a5318-7cfe-45d9-8f44-8430127811e8" />
<img width="1841" height="758" alt="image" src="https://github.com/user-attachments/assets/c785e10c-6581-4350-917b-aba7bf6a88cc" />