# Papara Final Case

## Project Overview
Papara Final Case is an e-commerce platform focused on the sale of digital products and licenses. 
Available on Android, iOS, and Web platforms, this project provides a user-friendly and secure environment for purchasing digital items, 
with features that enhance the overall shopping experience.


## Key Features
- **Multi-Platform Accessibility**: The platform is available on Android, iOS, and Web, offering a consistent experience across all devices.

- **Digital Product Sales**: Users can browse and purchase various digital products and licenses after registering on the platform.

- **Loyalty Points System**: Integrated with a loyalty points system, users earn points with every purchase, which can be redeemed for discounts on future transactions.

- **Coupon System**: The platform also supports a coupon system where users can apply gift coupons for additional discounts on their purchases.

- **Secure and Scalable Architecture**: Built with a focus on security, performance, and scalability using industry-leading technologies.

## Technologies Used
- **Database**: MS SQL Server
- **Authorization**: JWT Token
- **ORM**: Entity Framework Core (EF Core) with Repository and Unit of Work patterns
- **Message** Queue: RabbitMQ
- **Caching**: Redis (StackExchangeRedis)
- **Background Jobs**: Hangfire
- **Dependency Injection**: Autofac
- **Validation**: FluentValidation
- **Containerization**: Docker

## Development & Testing:

- **API Documentation**: [Postman Documentation](https://documenter.getpostman.com/view/26479579/2sA3s3JX5A)
- **API Testing**: Use Postman or Swagger for API documentation and testing.


## Getting Started

### Prerequisites
Ensure you have the following installed on your local machine:

- **Docker**: For containerization and running services like Redis and RabbitMQ locally.
- **.NET Core SDK**: For building and running the backend.
- **MS SQL Server**: For the database.

- **Hangfire Setup**
To use Hangfire in the project, you need to manually create the necessary tables in the database.
Ensure that the database specified in the **appsettings.json** file is correctly set up and accessible.

### Installation
1. Clone the repository:
    ```sh
     git clone https://github.com/Kadiiravsaar/PaparaFinalCase.git
    ```
    
2. Navigate to the project directory:
    ```sh
    cd PaparaFinalCase
    ```

3. Set up the database:

- Ensure MS SQL Server is running.
- Update the connection strings in **appsettings.json**.


4. Running RabbitMQ and Redis with Docker

- Update the connection strings in **appsettings.json**.
  
- Run **RabbitMQ** with management interface:
  ```bash
  docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.13-management
  ```
  
   **Port 5672**: AMQP protocol port used for messaging.
   
   **Port 15672**: Management interface port to monitor RabbitMQ.


- Run **Redis** container:
  ```bash
  docker run --name redis -p {port}:6379 -d redis
  ```

   **Port 6379**: Redis default port for data operations.
  
   **Port 1453**: Local port mapping to access Redis from your local environment.

  

5. Install the dependencies: 
    ```sh
    dotnet restore
    ```

6. Run the application:
    ```sh
    dotnet run
    ```

7. Optionally, run the services in Docker:
    ```sh
    docker-compose up --build
    ```


### Running the Application
The backend services can be accessed locally via **http://localhost:{port}**. Use Postman or Swagger for API documentation and testing.


## Project Structure
- **Core**: Contains business logic, domain models, and DTOs.
- **Repository**: Includes the data access layer, repository, and Unit of Work implementations.
- **Service**: Encompasses application services, business rules, and validations.
- **API**: Houses the Web API layer, controllers, and related configurations.


## Usage
1. Register an account on the platform.
2. Browse available digital products and licenses.
3. Add desired items to your cart. Discounts from coupons are applied automatically during this stage.
4. Proceed to checkout.
5. Earn loyalty points with every purchase, redeemable in future transactions.


## Contact
For questions or feedback, please contact [kadiiravsar@gmail.com](mailto:kadiiravsar@gmail.com).

Thank you for exploring Papara Final Case!



