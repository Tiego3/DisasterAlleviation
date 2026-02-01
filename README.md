# Disaster Alleviation Foundation

A comprehensive web-based platform designed to streamline disaster relief operations by managing donations, volunteers, and resource allocation with complete transparency.

---

## 📋 Project Overview

The Disaster Alleviation Foundation is an ASP.NET Core Razor Pages application that serves as a centralized platform for managing disaster relief efforts. The system enables public donors to contribute monetary funds or goods donations, volunteers to register and be assigned to disasters, and administrators to efficiently allocate resources and track relief operations in real-time.

### Purpose

In times of crisis, efficient resource management is critical. This platform bridges the gap between donors, volunteers, and disaster response teams by providing:

- **Transparent donation tracking** - Every contribution is recorded and visible
- **Efficient resource allocation** - Administrators can quickly assign funds and goods to active disasters
- **Volunteer coordination** - Streamlined application and assignment process
- **Real-time reporting** - Live dashboard showing impact metrics and active operations

---

## ✨ Key Features

### For Public Users

- **Monetary Donations**
  - Secure donation processing with instant confirmation
  - Anonymous donation support with unique ID generation
  - Automatic receipt generation
  
- **Goods Donations**
  - Category-based donation system (Food, Clothing, Medical Supplies, etc.)
  - Flexible drop-off scheduling (immediate or scheduled)
  - Reference number tracking for all goods donations
  - Google Places API integration for location autocomplete

- **Volunteer Registration**
  - Comprehensive volunteer application system
  - Skills and availability tracking
  - Emergency contact information management
  - Transportation and travel willingness indicators
  - Date range availability specification

### For Administrators

- **Dashboard & Analytics**
  - Real-time statistics (current balance, goods available, active disasters, total donors)
  - Visual impact metrics
  - Comprehensive overview of all operations

- **Disaster Management**
  - Register new disasters with detailed information
  - Track disaster status (Active/Closed)
  - Assign required aid types
  - Monitor resource allocations per disaster
  - View volunteer assignments

- **Resource Allocation**
  - Assign monetary funds to specific disasters
  - Allocate goods donations to relief efforts
  - Track allocation history
  - Monitor available vs. allocated resources

- **Volunteer Management**
  - Review and approve/reject volunteer applications
  - View detailed volunteer profiles
  - Assign volunteers to specific disasters
  - Define volunteer roles and responsibilities
  - Track volunteer availability and assignments

- **Donation Management**
  - View all monetary and goods donations
  - Filter by donor, date range, amount, or category
  - Sort and search functionality
  - Anonymous donor tracking

- **Category Management**
  - Create and manage goods donation categories
  - Track usage statistics per category
  - Prevent deletion of categories with active donations

- **Drop-off Management**
  - View scheduled goods drop-offs
  - Confirm completed drop-offs
  - Record on-site donations
  - Track drop-off status and reference numbers

---

## 🛠️ Tech Stack

### Backend

- **Framework**: ASP.NET Core 6.0+ (Razor Pages)
- **Language**: C# 10.0
- **ORM**: Entity Framework Core
- **Database**: Microsoft SQL Server
- **Authentication**: ASP.NET Core Identity

### Frontend

- **UI Framework**: Bootstrap 5.3.3
- **Icons**: Bootstrap Icons 1.11.3
- **JavaScript**: Vanilla JavaScript (ES6+)
- **Notifications**: SweetAlert2
- **Location Services**: Google Places API

### Additional Technologies

- **File Upload**: Built-in ASP.NET Core file handling
- **Validation**: Data Annotations & Client-side validation
- **Toast Notifications**: Bootstrap Toast with custom styling
- **Modal Dialogs**: Bootstrap Modals

---

## 🏗️ High-Level Architecture

### System Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                      Presentation Layer                      │
│              (Razor Pages + Bootstrap UI)                    │
├─────────────────────────────────────────────────────────────┤
│                      Application Layer                       │
│         (Page Models + Business Logic + Validation)          │
├─────────────────────────────────────────────────────────────┤
│                         Data Layer                           │
│           (Entity Framework Core + DbContext)                │
├─────────────────────────────────────────────────────────────┤
│                        Database Layer                        │
│                  (Microsoft SQL Server)                      │
└─────────────────────────────────────────────────────────────┘
```

### Application Flow

1. **Public Access Flow**
   - User visits homepage → Views active disasters and impact metrics
   - User navigates to Donate → Chooses monetary or goods donation
   - Form submission → Data validation → Database storage → Confirmation

2. **Volunteer Flow**
   - User applies via Volunteer page → Application stored as "Pending"
   - Admin reviews → Approves/Rejects application
   - Admin allocates volunteer to disaster → Assignment recorded

3. **Admin Management Flow**
   - Admin logs in → Access to Admin Dashboard
   - Views statistics and quick actions
   - Performs operations (register disaster, allocate resources, etc.)
   - Changes reflected in real-time across the system

### Data Models

**Core Entities:**

- `Donor` - Stores donor information (supports anonymous donors)
- `MonetaryDonation` - Records cash contributions
- `GoodsDonation` - Tracks physical item donations
- `Category` - Defines donation categories
- `Disaster` - Stores disaster information and status
- `Volunteer` - Contains volunteer applications and assignments
- `ResourceAllocation` - Links resources to specific disasters

---

## 🚀 Setup and Installation

### Prerequisites

- .NET 6.0 SDK or later
- Microsoft SQL Server (Express, Developer, or full edition)
- Visual Studio 2022 or VS Code (with C# extension)
- Node.js (for Bootstrap dependencies, optional)
- Google Maps API Key (for location autocomplete)

### Installation Steps

1. **Clone the Repository**
   ```bash
   git clone https://github.com/Tiego3/DisasterAlleviation.git
   cd DisasterAlleviation
   ```

2. **Configure Database Connection**
   
   Open `appsettings.json` and update the connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=DisasterAlleviationDB;Trusted_Connection=True;MultipleActiveResultSets=true"
     }
   }
   ```

3. **Configure Google Maps API (Optional but Recommended)**
   
   Add your Google Maps API key to `appsettings.json`:
   ```json
   {
     "GoogleMapsApiKey": "YOUR_API_KEY_HERE"
   }
   ```

4. **Database Setup**
   
   The application automatically runs migrations on startup. The database will be created with the following default categories:
   - Clothing
   - Food
   - Medical Supplies

5. **Build and Run**
   ```bash
   dotnet restore
   dotnet build
   dotnet run
   ```

6. **Access the Application**
   
   Navigate to `https://localhost:5001` (or the port specified in your launch settings)

### Creating Admin Account

1. Navigate to `/Admin/AdminRegister`
2. Register with an email ending in `@mailinator.com` (security requirement)
3. The account will automatically be assigned the "Admin" role
4. Login at `/Admin/AdminLogin`

---

## 📱 How to Use the Application

### For Donors

**Making a Monetary Donation:**
1. Click "Donate Now" on homepage or navigate to `/Donate`
2. Select "Monetary Donation" tab
3. Choose to donate anonymously or with name/email
4. Enter donation amount
5. Submit and save your confirmation ID

**Donating Goods:**
1. Navigate to `/Donate` and select "Goods Donation" tab
2. Choose anonymous or named donation
3. Select category and enter item details
4. Schedule drop-off time or choose immediate drop-off
5. Save your reference number for drop-off

### For Volunteers

1. Navigate to `/Volunteer`
2. Complete the application form with:
   - Personal information
   - Skills and experience
   - Availability dates and schedule
   - Emergency contact
   - Transportation details
3. Submit application (status: Pending)
4. Wait for admin approval

### For Administrators

**Dashboard Navigation:**
- Access via `/Admin/AdminDashboard` after login
- View key metrics and quick action buttons

**Register a Disaster:**
1. Click "Register New Disaster"
2. Fill in disaster details (type, location, description, dates)
3. Select required aid types
4. Submit to activate disaster

**Allocate Resources:**
1. Navigate to "Allocate Resources"
2. Select target disaster
3. Choose resource type (Monetary or Goods)
4. Enter allocation details
5. Confirm allocation

**Manage Volunteers:**
1. Go to "View All Volunteers"
2. Review applications
3. Click "View Details" to see full profile
4. Approve or reject application
5. Allocate approved volunteers to disasters via "Allocate Volunteers"

---

## 🎯 Design Decisions & Assumptions

### Design Decisions

1. **Anonymous Donations**
   - System generates unique anonymous IDs for privacy
   - Allows returning donors to link multiple donations
   - Balances privacy with tracking needs

2. **Role-Based Access Control**
   - Admin role required for management functions
   - Public pages accessible without authentication
   - Email domain restriction (@mailinator.com) for admin registration

3. **Database-First Migrations**
   - Automatic migration on application start
   - Ensures database schema is always current
   - Prevents deployment issues

4. **Client-Side Enhancements**
   - Google Places API for location autocomplete
   - Real-time form validation
   - Dynamic UI updates without page reloads

5. **Status Management**
   - Disasters: Active/Closed
   - Volunteers: Pending/Approved/Rejected
   - Drop-offs: Pending/Scheduled/Completed/Cancelled

### Assumptions

1. **Security**
   - Admin accounts are limited to mailinator.com emails (for demo/testing purposes)
   - Production deployment would use proper authentication providers
   - HTTPS is assumed for production deployment

2. **Data Integrity**
   - Closed disasters remain in database for historical tracking
   - All monetary amounts are in South African Rand (R)

3. **Geographic Scope**
   - Primary focus on South African disasters
   - Adaptable to other regions by changing Google Places API restrictions

4. **Workflow**
   - Volunteer approval is manual (no automatic approval)
   - Resource allocation is admin-driven (no automatic distribution)
   - Goods donations require physical drop-off (no shipping integration)

5. **Scalability**
   - Designed for medium-scale operations (hundreds of disasters/donations)
   - Database can be scaled for larger deployments
   - Frontend uses client-side filtering for performance

---

## 🔮 Future Enhancements

**Short-term Improvements:**
- Email notifications for volunteers and donors
- Downloadable donation receipts (PDF generation)
- SMS notifications for drop-off confirmations
- Multi-language support

**Medium-term Features:**
- Mobile application (React Native or Flutter)
- Advanced reporting and analytics dashboard
- Integration with payment gateways
- Volunteer scheduling and shift management
- Disaster impact assessments and reporting

**Long-term Vision:**
- AI-powered resource allocation recommendations
- Predictive analytics for disaster preparedness
- Integration with government emergency systems
- Blockchain-based donation tracking for transparency
- International disaster response coordination

---

## Acknowledgments

- Bootstrap  for the  UI framework
- Microsoft for ASP.NET Core and Entity Framework Core
- Google Maps team for Places API
- SweetAlert2 for beautiful alert dialogs
- The open-source community 

---

**Built with ❤️ by Tiego Mathobela**
