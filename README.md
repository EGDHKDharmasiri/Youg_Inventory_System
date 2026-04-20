# 📦 Young Inventory System

> **An Enterprise-Level Inventory Management Application built with Blazor Web App & .NET Core Web API**

---

## 📋 Table of Contents

- [Project Overview](#-project-overview)
- [Architecture](#-architecture)
- [Tech Stack](#-tech-stack--tools)
- [Key Features](#-key-features-implemented)
- [System Requirements](#-system-requirements)

---

## 🎯 Project Overview

**Young Inventory System** is a professional-grade inventory management application designed to streamline product catalog management, stock level monitoring, and sales operations for fashion retail businesses. The system provides real-time inventory visibility, automated low-stock alerts, and comprehensive product analytics through an intuitive, modern web interface.

### Primary Objectives:
- ✅ Centralized product catalog management with image support
- ✅ Real-time stock level tracking and alerts
- ✅ Secure user authentication with role-based access control
- ✅ RESTful API for scalable, modular architecture
- ✅ Interactive dashboard with business metrics
- ✅ Professional UI/UX with responsive design

**Target Users:** Retail managers, inventory coordinators, and business administrators

---

## 🏗️ Architecture

The Young Inventory System follows a **three-tier architecture** with clear separation of concerns:

```
┌─────────────────────────────────────────────────────────────┐
│                    CLIENT LAYER (Presentation)             │
│  ┌──────────────────────────────────────────────────────┐  │
│  │   Blazor Web App (Interactive Server Rendering)      │  │
│  │   - Components: Dashboard, Products, Orders, Reports │  │
│  │   - Real-time UI updates via WebSocket (SignalR)     │  │
│  │   - Bootstrap 5 Responsive Design                    │  │
│  └──────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                              ↓ HTTP/HTTPS
┌─────────────────────────────────────────────────────────────┐
│                   BUSINESS LOGIC LAYER (API)               │
│  ┌──────────────────────────────────────────────────────┐  │
│  │   ASP.NET Core Web API (RESTful Controllers)         │  │
│  │   - ProductsController: CRUD operations              │  │
│  │   - AuthController: Authentication management        │  │
│  │   - Structured logging & error handling              │  │
│  │   - Input validation & business rules                │  │
│  └──────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                              ↓ Entity Framework Core
┌─────────────────────────────────────────────────────────────┐
│              DATA ACCESS LAYER (Database)                   │
│  ┌──────────────────────────────────────────────────────┐  │
│  │   SQLite Database (Code-First Migrations)            │  │
│  │   - Users Table: Authentication & credentials        │  │
│  │   - Products Table: Catalog & inventory              │  │
│  │   - Orders Table: Transaction history                │  │
│  └──────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
```

### Architecture Benefits:
- **Separation of Concerns**: Each layer has distinct responsibilities
- **Scalability**: API can serve multiple client types (web, mobile, desktop)
- **Testability**: API endpoints can be unit tested independently
- **Maintainability**: Clear structure for code organization and updates
- **Reusability**: Components and services can be shared across projects

---

## 🛠️ Tech Stack & Tools

### Frontend
| Technology | Version | Purpose |
|-----------|---------|---------|
| **Blazor Web App** | .NET 10 | Interactive Server-side UI framework |
| **Bootstrap** | 5.3 | Responsive CSS framework |
| **Bootstrap Icons** | 1.11 | Icon library for UI elements |
| **Fluent Validation** | Latest | Input validation framework |
| **Microsoft.AspNetCore.Components.Authorization** | Latest | Authentication state management |
| **JavaScript Interop** | Built-in | Browser API integration (drag-drop, scrolling) |

### Backend
| Technology | Version | Purpose |
|-----------|---------|---------|
| **.NET** | 10 | Modern open-source framework |
| **ASP.NET Core** | Latest | Web framework for APIs |
| **Entity Framework Core** | Latest | ORM for database operations |
| **Code-First Migrations** | Built-in | Database schema versioning |

### Database
| Technology | Purpose |
|-----------|---------|
| **SQLite** | Lightweight, file-based relational database |
| **Code-First Approach** | Database generated from C# models |
| **Migrations** | Version control for schema changes |

### Security & Authentication
| Feature | Implementation |
|---------|----------------|
| **Cookie-Based Auth** | ASP.NET Core Identity cookies |
| **Custom Auth Provider** | CustomAuthenticationStateProvider |
| **Protected Session Storage** | Encrypted browser storage |
| **Recovery Key System** | Master key (YOUNG-ADMIN-999) for account recovery |

### Development Tools
| Tool | Purpose |
|------|---------|
| **Visual Studio 2026** | IDE |
| **Git** | Version control |
| **NuGet** | Package manager |
| **Entity Framework CLI** | Database migrations |
| **PowerShell** | Task automation |

---

## ✨ Key Features Implemented

### 1. **Authentication & Authorization**
- ✅ Secure login system with database credential validation
- ✅ Custom authentication state provider with prerendering support
- ✅ Cookie-based session management
- ✅ Protected session storage with encryption
- ✅ Recovery key mechanism for account access
- ✅ Automatic logout with session clearing
- ✅ Graceful handling of JavaScript interop during prerendering


### 2. **Product Management (CRUD Operations)**

#### CREATE
- Add new products via quick-entry form
- Duplicate product code validation (409 Conflict)
- Image upload with Base64 conversion
- Form validation with error messages

#### READ
- Display all products in responsive data grid
- Get product details by ID
- Search products by name or code
- Filter by stock status (In Stock, Low Stock, Out of Stock)
- View low stock products via dedicated API endpoint

#### UPDATE
- Edit existing product details
- Modify pricing, stock levels, and metadata
- Prevent ProductCode duplication
- Atomic database transactions

#### DELETE
- Remove products from inventory
- Confirmation dialog before deletion
- Automatic list refresh after deletion


### 3. **Interactive Dashboard**

Real-time metrics displayed on landing page:

| Metric | Purpose |
|--------|---------|
| **Total Products** | Count of all items in catalog |
| **Total Inventory Value** | Sum of (Price × Stock Quantity) |
| **Low Stock Alerts** | Products below minimum threshold |

Features:
- ✅ Auto-calculated from database
- ✅ Visual indicators with color coding
- ✅ Icon-based card design
- ✅ Responsive grid layout

### 4. **Advanced Search & Filtering**

**Search Capabilities:**
- Real-time search as user types
- Case-insensitive matching
- Searches across ProductCode and Name fields

**Filter Options:**
- All Items
- In Stock (Quantity > 0)
- Low Stock (Quantity ≤ LowStockLevel)
- Out of Stock (Quantity = 0)



### 5. **Image Handling & Upload**

- ✅ Drag-and-drop image upload support
- ✅ Click-to-upload file input
- ✅ Automatic image resizing (500×500px)
- ✅ Base64 encoding for database storage
- ✅ Preview with clear button
- ✅ Supported formats: JPEG, PNG, GIF
- ✅ Max file size: 10MB


### 6. **Settings & Credential Management**

- ✅ Change username and password
- ✅ Automatic logout after credential change
- ✅ Forced re-login with new credentials
- ✅ Password strength validation
- ✅ Confirmation matching

### 7. **Responsive & Professional UI**

Features:
- ✅ Bootstrap 5 responsive framework
- ✅ Modern gradient backgrounds
- ✅ Smooth animations and transitions
- ✅ Loading spinners for async operations
- ✅ Success/error alert notifications
- ✅ Mobile-friendly navigation
- ✅ Accessible color contrast
- ✅ Icon integration (Bootstrap Icons)

### 8. **API Standards & Best Practices**

- ✅ RESTful endpoint design
- ✅ Proper HTTP status codes (200, 201, 204, 400, 404, 409, 500)
- ✅ JSON request/response format
- ✅ Comprehensive error messages
- ✅ Structured logging with serilog patterns
- ✅ Async/await throughout
- ✅ Input validation at API layer
- ✅ Exception handling with try-catch blocks

---

## 💾 System Requirements

### Development Environment
- **OS**: Windows 10/11
- **.NET SDK**: .NET 10.0 or later
- **Visual Studio**: 2026 Community Edition or higher
- **Browser**: Modern browser with WebSocket support
- **RAM**: 4GB minimum (8GB recommended)
- **Disk Space**: 2GB for .NET and dependencies

### Runtime Environment
- **.NET Runtime**: .NET 10.0 or later
- **Browser**: Any modern browser (Chrome, Edge, Firefox, Safari)
- **Database**: SQLite (included with application)

---



## 👨‍💻 Author

**Your Name**
- GitHub: [@DhanushkaHarsha](https://github.com/EGDHKDharmasiri)
- LinkedIn: [Dhanushka Harsha](https://www.linkedin.com/in/dhanushka-harsha-389875228?utm_source=share_via&utm_content=profile&utm_medium=member_android)
- Email: egddharmasiri@gmail.com

---

## 📞 Support

For issues, questions, or suggestions:
1. **Email**: egddharmasiri@gmail.com
2. **LinkedIn**: Direct message

---

## 🙏 Acknowledgments

- **Blazor Documentation**: Microsoft Learn
- **Bootstrap Team**: Bootstrap 5 framework
- **Entity Framework Core**: Microsoft Data Access
- **Community**: .NET community for tools and libraries

---

## 🎓 Learning Outcomes

This project demonstrates expertise in:

✅ **Full-Stack Web Development**
- Frontend: Blazor Web App with Interactive Server rendering
- Backend: RESTful API with ASP.NET Core
- Database: SQLite with Entity Framework Core

✅ **Software Architecture**
- Three-tier architecture (Presentation, Business Logic, Data)
- Separation of concerns
- SOLID principles

✅ **Authentication & Security**
- Cookie-based authentication
- Custom authentication state management
- Protected session storage
- Authorization attributes

✅ **Database Design**
- Code-First approach
- Entity relationships
- Schema versioning with migrations
- Normalization principles

✅ **API Development**
- RESTful endpoint design
- HTTP status codes
- Error handling
- Input validation

✅ **UI/UX Development**
- Responsive design with Bootstrap
- Component-based architecture
- Real-time validation
- Professional user experience

✅ **DevOps & Deployment**
- Build and deployment pipelines
- Database migration in production
- FTP deployment
- Environment configuration

---


