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
- **AutoMapper**: A library used for Object-Object Mapping
- **Validation**: FluentValidation
- **Containerization**: Docker
- **SMTP**: Email service for notifications.

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

## Screenshots

Here are some screenshots that illustrate the API requests.

**If you make requests as shown below, you can quickly test the functionality.**

- **Token usage**
  
![token1](https://github.com/user-attachments/assets/1be4f304-8bfe-4f19-8e34-00ef740bba39)


![token2](https://github.com/user-attachments/assets/20db1525-3633-4a6b-9bcc-1470e839f153)


- **POST** `/api/Categories` (Roles = "Admin")

![KullaniciKayit](https://github.com/user-attachments/assets/524267b8-8d3d-4373-a68c-2c9d735d84dd)

- **POST** `/api/Products` (Roles = "Admin")

![AdminUrunEklemek](https://github.com/user-attachments/assets/c4c268c7-e208-4a49-b1ea-fe0d468db753)

- **POST** `/api/Coupons` (Roles = "Admin")

![AdminKuponEklemek](https://github.com/user-attachments/assets/b2be22ce-87ea-4155-b169-58663af193a7)

- **POST** `/api/Auth/register`

![KullaniciKayit](https://github.com/user-attachments/assets/89a518d3-c69e-46b2-8b78-fc3373b6bc8c)

- **POST** `/api/Auth/login` (User)

![KullaniciGiris](https://github.com/user-attachments/assets/2e418b3a-fc43-4041-9e63-54542c300a83)

- **POST** `/api/DigitalWallets` (User)

![KullaniciHesapCüzdani](https://github.com/user-attachments/assets/af57d2d8-1993-4536-9de1-80772f808575)

- **POST** `/api/Payments/process` (User)

![KullaniciHesapCuzdaninaBakiyeEklemekFake](https://github.com/user-attachments/assets/996c24c9-4d76-422d-845b-67e1d452cfb3)

- **POST** `/api/BasketItems` (User)

![KullaniciSepeteUrunEklemek](https://github.com/user-attachments/assets/9297884d-b880-4011-b37d-ba6e016f74bb)

- **POST** `/api/Baskets/calculatePrice` (User)

![KullaniciKuponKodKullanmadan](https://github.com/user-attachments/assets/14a55908-6d04-4efb-aa1a-293f4194c1fe)

- **POST** `/api/Baskets/calculatePrice` (User)-Applied coupon code `SAVE40`

![KullaniciSepeteKuponUygularsaHesaplanan](https://github.com/user-attachments/assets/0fe23e4f-4580-49a6-9a03-7dcb18382914)

- **POST** `/api/Orders` (User) 
  
![KullaniciSiparişVerme](https://github.com/user-attachments/assets/a841c302-3374-433a-a2f3-f25ca86cc3f2)

- **POST** `/api/Orders` (User) - Coupon applied order
  
![KullaniciSepeteKuponUyguladiktanSonrakiSipariş](https://github.com/user-attachments/assets/6ef63b2b-76fe-472f-ad95-9438201bf850)

- **Order** confirmation email

![SiparisOnayMaili](https://github.com/user-attachments/assets/ff136e66-2402-4844-8201-b64e7dd296a7)

**For more details, please refer to the documentation.**

