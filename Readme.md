# Setting Up and Running the Application

This project is a web API built using .NET Core 8 with Entity Framework Core, authorization, and follows clean code architecture. Docker is used for containerization.

### About the Application

The application is designed to perform CRUD operations and utilize data effectively. It provides a set of API endpoints for CRUD operations on restaurant-related information, including:

- Managing shop details (name, address, contact info, etc.).
- Storing and retrieving shop data from a SQL Server database.
- Built with RESTful principles for easy consumption by frontend applications.

---

# Running the App with Docker

Follow these steps to run the application using Docker and `docker-compose`.

## 1. Prerequisites

Make sure you have the following installed on your machine:

- [Docker](https://www.docker.com/get-started) (includes Docker Compose)

You can verify that Docker is installed by running:

```bash
docker --version
docker-compose --version
git clone https://github.com/loganathansellappa/dotnetcore-8-api.git
cd dotnetcore-8-api
docker-compose up --build
```

## Swagger

http://localhost:8080/swagger/index.html

# Credentials:
testuser@testdevlogan.com / abcTest123!


