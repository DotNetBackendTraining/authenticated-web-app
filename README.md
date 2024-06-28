# Authenticated Web App

## Overview

This is a startup web app with essential functionalities:

1. Authentication with JWT
2. Rate Limiting & CORS settings
3. Dockerization (and docker compose)
4. EFCore & DbContext
5. Production & Development settings

## Development

1. Fill the variables in `example.env` and rename the file to `.env`
2. Run `docker-compose.development.yml`, you can use the following command:
   ```shell
   docker-compose -f "docker-compose.development.yml" up -d
   ```
3. Import `Development.postman_collection.json` to postman for testing.

## Creating this project as an Exercise:

In this exercise, you will learn how to create a minimal ASP.NET Web API and implement JWT (JSON Web Token)
authentication.

### **Step 1: Setting up the Project**

1. Create a new ASP.NET Web API project.
2. Configure the project with the necessary dependencies, such as **`Microsoft.AspNetCore.Authentication.JwtBearer`**
   package, which provides JWT authentication support.

### **Step 2: Implementing JWT Authentication**

1. Create a class called **`JwtTokenGenerator`** that will be responsible for generating and validating JWT tokens.
2. Inside the **`JwtTokenGenerator`** class, implement a method called **`GenerateToken`** that takes in user
   credentials (e.g., username and password) and returns a JWT token.
3. Use the **`System.IdentityModel.Tokens.Jwt`** namespace to create and sign the JWT token. You can use a secure key or
   a certificate to sign the token.
4. Implement another method called **`ValidateToken`** that takes in a JWT token and verifies its validity, including
   the signature and expiration date.

### **Step 3: Creating the API Controller**

1. Create an API controller class that will handle the requests and responses for your API.
2. Apply the **`[Authorize]`** attribute to the controller or specific actions that require authentication.
3. Create a get method that will return today’s weather or a welcome message, just to show the user that he is
   authorized and has access to the system.

### **Step 4: Configuring JWT Authentication**

1. Open the **`Startup.cs`** file in your project.
2. In the **`ConfigureServices`** method, configure JWT authentication using the **`AddAuthentication`** method and
   specify the JWT bearer options.
3. Provide the necessary configuration details such as the issuer, audience, and token validation parameters.
4. In the **`Configure`** method, add the **`UseAuthentication`** middleware to enable authentication in your API.

### **Step 5: Testing the API**

1. Build and run your API project.
2. Use a tool like Postman or curl to send HTTP requests to your API endpoints.
3. For authenticated endpoints, include the JWT token in the request headers using the **`Authorization`** header. The
   token should be in the format **`Bearer <token>`**.
4. Test both authenticated and unauthenticated endpoints to ensure that the authentication is working as expected.

### **Conclusion**

In this exercise, you have learned how to create a minimal ASP.NET Web API and implement JWT authentication. This
provides a secure way to authenticate and authorize requests to your API endpoints. By understanding the concepts and
following the steps outlined in this exercise, you are now equipped with the knowledge to build more complex APIs with
JWT authentication in the future.

Remember to document your code thoroughly and explain any additional features or enhancements you may have implemented.
Good luck with your exercise!