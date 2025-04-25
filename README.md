**Run the Application**:
   - Open the project in Visual Studio.
   - Press `F5` or run the project to start the application.

---

#### Usage

1. **Register a New User**:
   - Navigate to the registration page and create a new account.

2. **Login**:
   - Use your registered email and password to log in.

3. **Manage Tasks**:
   - Create, edit, and delete tasks from the dashboard.
   - View task statistics and update history.

4. **Generate Reports**:
   - Access the reports section to view task summaries and user statistics.

---

#### **Folder Structure**

- **Controllers**: Contains controllers for handling user and task-related operations.
- **Models**: Defines the data models for users, tasks, and task updates.
- **Views**: Razor views for rendering the UI.
- **Data**: Contains the database context and migrations.
- **Services**: Includes services for user authentication and other business logic.

---

#### **Security Considerations**

- **Password Storage**: Passwords are currently stored in plain text. It is recommended to hash passwords using a secure algorithm like `bcrypt`.
- **Session Management**: Ensure proper session expiration and protection against session hijacking.

---

#### **Future Enhancements**

- Implement password hashing for secure storage.
- Add client-side validation for forms.
- Improve error handling and logging.
- Add unit tests for controllers and services.

---
#### **Install Entity Framework Core**
Install-Package Microsoft.EntityFrameworkCore

### **Install SQL Server provider for EF Core**
Install-Package Microsoft.EntityFrameworkCore.SqlServer

### **Install Identity Framework**
Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore

### **Install Toastr (optional, for notifications)**
Install-Package Toastr
### **Configure Connection String**
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=SimpleTaskTrackrDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}

### **Add initial migration**
Add-Migration InitialMigration

### **Apply the migration to create the database**
Update-Database

## **License**

This project is licensed under the MIT License. See the `LICENSE` file for details.

---

#### **Contact**

For questions or support, please contact [iamrishabhsharma0301@gmail.com].
Whatsapp[8789352863]


