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
