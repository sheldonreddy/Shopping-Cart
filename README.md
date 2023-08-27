# Game Global Shopping Cart API

Welcome to the Games Global Shopping Cart API documentation! This README provides an overview of the API's specifications, implementation details, and how to get started using it.

## Table of Contents

- [Introduction](#introduction)
- [API Specification](#api-specification)
- [Implementation Details](#implementation-details)
- [Getting Started](#getting-started)
- [Contributing](#contributing)
- [License](#license)

## Introduction

The Games Global Shopping Cart API is a secure RESTful API designed to provide seamless shopping cart management for users. It's built using ASP.NET Core and follows best practices to ensure security, performance, and maintainability.

## API Specification

The API features the following functionalities:

1. **Authentication:**
   - Users can log in and be authenticated. Upon a successful authentication a JWT Token is generated and returned to the user. This token must be passed in all the subsequent requests made by the user to get access to the API resources.

2. **Shopping Cart Operations:**
   - The API provides endpoints to perform CRUD operations on shopping cart items.
   - Each user's shopping cart items are scoped to their own account.

   The API implements 5 main entities: User, Product, Product Category, LineItem and Cart. A LineItem is essentially composed of a product details and the quantity of the product in the line item.
   A cart is essentially a collection of lineItems. To insure that a user is able to view only the cart created by him/her, his/her Frontend application session
   needs to be passed to the API while creating a cart and this cart can only retrieve using the session related to it. To delete a cart, the user must first delete
   all the items in it.

3. **Image Attachment:**
   - Shopping cart items can have images attached.
   - These images when added return an url which can be used when adding the details of a product associated to the image.

4. **Swagger Interface:**
   - The API is documented using the Swagger/OpenAPI standard.
   - Swagger UI is accessible for interactive API exploration and testing.

5. **Model Validation:**
   - API requests are validated for proper input to prevent invalid data from being processed.

6. **Unit Testing:**
   - Unit tests are implemented to ensure the reliability and correctness of API functionality.

7. **Relational Database:**
   - The application uses Entity Framework Core to interact with a relational database for data storage.

8. **Containerization:**
   - Docker is used to containerize the application, allowing for easy deployment and scalability.

9. **HTTP Status Codes:**
   - The API returns appropriate HTTP status codes to indicate the success or failure of requests.

## Implementation Details

- The API is developed using ASP.NET Core, C#, and Entity Framework Core.
- OAuth2 authentication is integrated for user login and authentication.
- Amazon S3 is used for image storage and retrieval.
- API endpoints are organized in controllers following RESTful conventions.
- Dependency injection is used to manage service instances.
- Unit tests are written using testing frameworks like Xunit and Moq.
- The API is built and packaged as a Docker container for deployment.

## Getting Started

To get started with the Games Global Shopping Cart API, follow these steps:

1. Clone the repository:

   ```bash
   git clone https://github.com/your-username/GG-shopping-cart-api.git
   ```
2. Install dependencies
    ```bash
    cd GG-shopping-cart-api

    dotnet restore

    ```
3. Configure appsettings.json:

 a. Database settings

     - You will need to have a running instance of MS SQL Server
     - Replace the '''ConnectionString''' value in the appsettings.json by your own connection string
     - Then run the following commands to initialise and create the Database and its tables:

     ```bash
     dotnet ef migrations add InitialCreate

     dotnet ef database updat
     ```
 
  b. Authentication

    This API is secured using JSON Web Tokens (JWT) for authentication. To access protected endpoints, you need to include an authorization token in the `Authorization` header of your HTTP requests.

    Obtaining a JWT Token:

    To obtain a JWT token, you need to follow these steps:

    1. **User Registration:** Register a new user using the `/api/users/register` endpoint by providing the required user information.

    2. **User Login:** Use the `/api/users/login` endpoint to log in with your registered credentials. This endpoint will return a JWT token upon successful authentication.

    Using the JWT Token

    Once you have obtained a JWT token, you should include it in the `Authorization` header of your requests in the following format:

    Authorization: Bearer YourJWTTokenHere

  c. Cart creation

    To create a new cart you must provide the client browser session in the request body and to fetch a cart the session must be attached to the header in this form:

    Client_App_Session: your-client-session


4. Docker configuration:
    To run this API in Docker run the following commands:

    ```bash
    docker build -t gg-shopping-cart-api .

    docker run -d -p 8080:80 --name gg-shopping-cart-api-container  gg-shopping-cart-api
    ```


## API Documentation

API documentation is available via Swagger. Once the application is running, navigate to `/swagger` in your browser to interact with and explore the API endpoints.


## Contributing

Contributions to the Games Global Shopping Cart API are welcome! Whether you find a bug, want to add a feature, or improve the documentation, your contributions will be appreciated.

Fork the repository.
Create a new branch for your feature/bugfix.
Commit and push your changes.
Open a pull request with a detailed description of your changes.

## License

The Games Global Shopping Cart API is open-source and available under the MIT License.

Feel free to reach out to us at novhakaze@gmail.com for any questions or support. We hope you enjoy using the Games Global Shopping Cart API for your shopping cart management needs!
