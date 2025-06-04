/// you need to add appsettings.json file with these contents:
{

  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "EmployeeDatabase": "<your connection string>"
  },
  "Jwt":{
    "Issuer": < person who provides tokens>,
    "Audience": < person who is provided with tokens>,
    "Key": <hashing key>,
    "ValidInMinutes": <token's lifetime>
  }
}


**🔐 Authentication & Roles**

**Admin:** 

Full control over devices and employees, and can register accounts.

**User:** 

Can view/update their own data and assigned devices.

📌 Role-based Endpoint Access

   **---USER ---**

Endpoint /	Description

GET /api/accounts/me	View own account & personal info

PUT /api/accounts/me	Update personal info

GET /api/devices/{id}	View assigned device if it belongs to the user

PUT /api/devices/{id}	Update assigned device (validated in service layer)

 **---ADMIN---**

Endpoint /	Description

GET /api/accounts	View all accounts

POST /api/accounts	Register a new account

GET /api/devices	View all devices

POST /api/devices	Add a new device

PUT /api/devices/{id}	Update any device

DELETE /api/devices/{id}	Remove a device

GET /api/employees	View all employees

GET /api/employees/{id}	View specific employee

Admin cannot use /api/accounts/me or /api/accounts/me (PUT) — these are for users.
