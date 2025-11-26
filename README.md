# 🌐 **NZFTC Employee Management System (EMS)**  
A full-stack HR, Payroll, and Employee Self-Service portal built using **ASP.NET Core MVC**, **Entity Framework Core**, and **MySQL**.

---

## 🚀 Overview

The **NZFTC Employee Management System (EMS)** streamlines HR operations by providing a secure and modern platform to manage:

- Employees  
- Payroll  
- Leave Management  
- Timesheets  
- Calendar & Events  
- Grievances / Support Tickets  
- Administrative Settings  

Both **Admin** and **Employee** roles are supported with separate dashboards and functionalities.

---

## 📌 Key Features

### 👨‍💼 Admin Features
- Dashboard with company-wide analytics  
- Manage Employees (CRUD)  
- Manage Job Positions & Pay Grades  
- Approve or decline leave requests  
- Manage payroll cycles + employee summaries  
- View and resolve support tickets / grievances  
- Manage leave policies & leave balances  
- Month-view calendar for HR events and leave  
- Announcements administration  

### 👤 Employee Features
- Secure login & first-time password setup  
- Personal dashboard (salary, leave, payslips)  
- Apply for Leave  
- View Leave History  
- Manage Timesheets  
- View Payslips  
- Submit Support Tickets  
- Access personal calendar  
- Manage and update personal profile  

---

## 🛠️ Tech Stack

### **Frontend**
- ASP.NET Core MVC  
- Razor Views  
- Bootstrap 5  
- Custom CSS (portal-stylesheet.css)  
- Responsive UI/UX  

### **Backend**
- ASP.NET Core 9  
- Entity Framework Core  
- Repository/Service pattern  
- Data annotations + validation  

### **Database**
- MySQL  
- EF Core Migration-based schema  
- Seeded data (Employees, Policies, PayGrades, Holidays, etc.)  

---

## 📁 Folder Structure

```NZFTC-EMS/
├── Controllers/
├── Models/
│   ├── Entities/
│   ├── Enums/
│   └── ViewModels/
├── Services/
├── Migrations/
├── Views/
│   ├── Admin/
│   ├── Employee/
│   ├── Shared/
│   └── Website/
├── wwwroot/
├── appsettings.json
└── Program.cs
```

---

## 🗄️ Database & Migrations

This project uses **Entity Framework Core** to manage schema creation and updates.

### Run migrations:


`Add-Migration InitialMigrate`
`Update-Database`

## 🗄️ Database Tables Generated

- **Employees**
- **JobPositions**
- **PayGrades**
- **LeaveRequests**
- **LeaveBalances**
- **LeavePolicies**
- **Timesheets**
- **PayrollRuns**
- **PayrollSummaries**
- **SupportTickets**
- **Announcements**
- **CalendarEvents**

---

## 🔐 Authentication & Security

- Custom login system  
- Role-based access (Admin/Employee)  
- First-time password creation  
- Session authentication  
- Secured routes and restricted pages  

---

## 🔄 Agile Development Summary

### **Sprint 1**
- Setup GitHub + development environment  
- ERD & UML Class Diagram  
- Style guide & wireframes  
- Base layout planning  

### **Sprint 2**
- EF Core + MySQL integration  
- Employee CRUD + Profile  
- Login & password flow  
- Grievances module  
- Payroll & Leave foundations  

### **Sprint 3**
- Calendar (Month View)  
- Timesheets module  
- Dashboards (Admin + Employee)  
- Schema fixes + final UI polish  
- Full system testing  

---

## 📸 Screenshots

<img width="632" height="1388" alt="image" src="https://github.com/user-attachments/assets/f79ddd51-4491-420e-b93a-f88fe8a51d8f" />
<img width="873" height="422" alt="image" src="https://github.com/user-attachments/assets/04e006b7-ee75-4e4a-b318-0c594b24fdc8" />
<img width="875" height="533" alt="image" src="https://github.com/user-attachments/assets/2591fe6b-c17d-4478-8f38-6351bd45430c" />
<img width="875" height="449" alt="image" src="https://github.com/user-attachments/assets/4c804488-8199-4f07-8b69-ecd037d2287d" />
<img width="940" height="454" alt="image" src="https://github.com/user-attachments/assets/ebf558b5-4df2-468b-9a7a-50acef051197" />
<img width="940" height="454" alt="image" src="https://github.com/user-attachments/assets/973428e6-efce-444a-93de-1f2ee4a80e8b" />
<img width="940" height="459" alt="image" src="https://github.com/user-attachments/assets/93480c58-3d87-4519-b740-83682d3031b6" />

---

## 📦 Installation

### **1. Clone the repository**

`git clone https://github.com/your-team/NZFTC-EMS.git`

### **2. Configure MySQL in appsettings.json**

`"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=nzftc_ems;User=root;Password=root;"
}`
### **3. Apply database migrations**
`Update-Database`

### **4. Run the project**
`dotnet run`

### **5. Login (Example Accounts)**

Use the following seeded accounts to access the system:

- **Admin Account:**  
  `admin@nzftc.local`

- **Employee Account:**  
  `employee1@nzftc.local`

---

## 👥 Team Members — Group 3 (SD106)

**Dhona Obina — Lead Frontend/UI Developer**  
Responsible for dashboard layouts, calendar UI, employee views, navigation structure, styling, and component design.

**Aries Tayao — Backend Developer/Tester**  
Worked on EF Core migrations, grievance module, login/password logic, leave and payroll processing, and database fixes.

**Brix Munsayac — Backend Developer/ Scrum Master**  
Developed payroll controllers, leave management logic, support ticket functionality, and various system integrations.

## 📄 License

This project was developed for **Yoobee Colleges — SD106 Integrated Studio II**  
and is intended for **educational use only**.











