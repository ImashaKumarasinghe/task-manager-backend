# Mini Task Management App – Backend

##  Project Overview
This is the backend API of the Mini Task Management Application built as part of a Full Stack Developer Intern assessment.

The backend provides secure RESTful endpoints for managing user-specific tasks and integrates with Firebase Authentication to verify users.

Each user can only access and manage their own tasks using Firebase UID-based authorization.

---

##  Tech Stack
- ASP.NET Core Web API
- C#
- Entity Framework Core
- MySQL
- Firebase Admin SDK

---

##  Key Features
-  Firebase Authentication token verification
-  Task management (Create, Read, Update, Delete)
-  User-specific task isolation (multi-user support)
-  RESTful API design
- MySQL database integration
- Input validation and error handling
-  Secure token-based access control

---

##  Architecture Highlights (Recruiter Focus)
- Clean separation of concerns:
  - Controllers → Handle HTTP requests
  - Services → Business logic (Firebase token verification)
  - Data layer → EF Core DbContext
- Dependency Injection used for services
- Secure Firebase token verification using Firebase Admin SDK
- LINQ queries used for efficient database operations
- Scalable and maintainable API structure

---

## Project Structure

Controllers/
├── TasksController.cs

Services/
├── FirebaseTokenService.cs

Models/
├── TaskItem.cs

DTOs/
├── CreateTaskDto.cs
├── UpdateTaskDto.cs

Data/
├── AppDbContext.cs

Program.cs  
appsettings.json  

---

##  Database Setup

1. Install MySQL (if not installed)

2. Create database:

CREATE DATABASE taskmanagementdb;

3. Update connection string in appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=taskmanagementdb;user=root;password=YOUR_PASSWORD"
}

---

## Firebase Setup

1. Go to Firebase Console  
2. Open your project  
3. Go to Project Settings  
4. Select Service Accounts tab  
5. Click Generate new private key  
6. Download the JSON file  
7. Place it in backend project root  
8. Rename it to:

firebase-service-account.json

---

##  Security Note

Add this to your .gitignore file:

firebase-service-account.json

Never upload this file to GitHub.

---

##  Run Database Migrations

dotnet ef migrations add InitialCreate  
dotnet ef database update  

---

##  How to Run Backend

1. Restore dependencies:

dotnet restore  

2. Run the API:

dotnet run  

3. Open Swagger:

http://localhost:5170/swagger  

---

##  API Endpoints

GET /api/tasks  
→ Get all tasks for logged-in user  

POST /api/tasks  
→ Create a new task  

PUT /api/tasks/{id}  
→ Update a task  

DELETE /api/tasks/{id}  
→ Delete a task  

---

## Authentication

All requests must include Firebase ID token:

Authorization: Bearer <Firebase Token>

---

## How Authentication Works

1. User logs in via frontend (Firebase)  
2. Firebase returns ID token  
3. Frontend sends token to backend  
4. Backend verifies token using Firebase Admin SDK  
5. Extracts Firebase UID  
6. Only tasks matching this UID are returned  

---

##  Validation Rules

- Task title is required  
- Title cannot be empty  
- User must be authenticated  
- Users can only access their own tasks  
- Task must exist for update/delete  

---

## Error Handling

400 → Bad Request (invalid input)  
401 → Unauthorized (invalid/missing token)  
404 → Not Found (task not found)  
500 → Internal Server Error  

---

##  Testing Flow

1. Login via frontend  
2. Get Firebase token  
3. Call API with Authorization header  
4. Create task  
5. Fetch tasks  
6. Update task  
7. Delete task  

---

## Additional Notes

- Backend is stateless and scalable  
- Uses industry-standard authentication approach  
- Supports multi-user environment securely  
- Designed for easy frontend integration  

---

