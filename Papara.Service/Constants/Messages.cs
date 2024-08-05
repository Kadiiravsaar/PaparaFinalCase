using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Service.Constants
{
	public static class Messages
	{
		public static string EntityNotFound = "Entity not found.";
		public static string EntityRetrievedSuccessfully = "Entity retrieved successfully.";
		public static string EntityCreatedSuccessfully = "Entity created successfully.";
		public static string EntityUpdatedSuccessfully = "Entity updated successfully.";
		public static string EntityDeletedSuccessfully = "Entity deleted successfully.";

		public static string InvalidUsernameOrPassword = "Geçersiz kullanıcı adı veya şifre.";  // bunu filter attribute haline getirebiliriz
		public static string UserRoleNotExists = "User role does not exist.";
		public static string RegistrationFailed = "User registration failed.";


		public static string FillAllFields = "All fields must be filled.";
		public static string UserRetrievedSuccessfully = "User retrieved successfully.";
		public static string UserListRetrievedSuccessfully = "User list retrieved successfully.";


		public static string InvalidCardDetails = "Invalid card details.";
		public static string UserNotAuthenticated = "User is not authenticated or ID is not available.";
		public static string UserNotFound = "User not found.";
		public static string PaymentSuccessful = "Payment successful.";
	}
}
