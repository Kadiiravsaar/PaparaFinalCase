using System;
using System.Collections;
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

		public const string AdminRegistrationSuccess = "Admin registration completed, please log in.";

		public const string RegistrationSuccess = "Registration successful, please log in.";

		public static string InvalidUsernameOrPassword = "Geçersiz kullanıcı adı veya şifre.";

		public static string UserRoleNotExists = "User role does not exist.";

		public static string RegistrationFailed = "User registration failed.";

		public const string SessionExpired = "Session has expired. Please log in again.";

		public const string EmailNotFound = "Email Not Found";

		public const string ProductNotFound = "Product Not Found";

		public const string BasketNotFound = "Basket Not Found";

		public const string BasketItemNotFound = "Basket Item Not Found";

		public const string NotEnoughStockAvailable = "Not Enough Stock Available";

		public const string EmailAlreadyInUse = "Email already in use.";

		public const string UserIdNotFoundInToken = "User ID not found in token.";

		public const string CouponCannotBeDeleted = "Kupon başka bir kayıtla ilişkili olduğu için silinemez.";

		public const string CouponCodeAlreadyExists = "Coupon code already exists.";

		public const string CouponNotFoundOrExpired = "Coupon not found or expired.";

		public const string NoCouponUsagesFound = "No coupon usages found.";

		public const string CouponUsageNotFound = "Coupon usage not found.";

		public const string InsufficientBalance = "Insufficient balance.";

		public const string NoActiveBasketOrBasketIsEmpty = "No active basket or basket is empty.";

		public const string UserDoesNotHaveDigitalWallet = "User does not have a digital wallet.";

		public const string ProductCategoryNotFound = "Product category not found.";

		public const string OrderDetailNotFound = "Order detail not found.";

		public const string OrderNotFound = "Order not found.";

		public const string CategoryNotLinkedToProduct = "Category not linked to this product.";

		public const string AtLeastOneCategoryIdRequired = "At least one category ID must be provided when adding a product.";

		public const string ProductNotLinkedToAnyCategory = "The product is not linked to any category.";

		public const string CouponAlreadyUsedByUser = "This coupon has already been used by this user.";

		public const string CannotDeleteRecordDueToRelatedData = "Record cannot be deleted due to related data.";

		public const string InvalidOrExpiredCoupon = "Invalid or expired coupon.";

		public const string CouponAlreadyUsed = "Coupon has already been used.";

		public const string UserAlreadyHasActiveBasket = "User already has an active basket.";

		public static string CouponAlreadyHasBeen = "Bu kuponu daha önce kullandınız.";

		public static string CouponNotFound = "Kupon bulunamadı";

		public static string FillAllFields = "All fields must be filled.";

		public static string UserRetrievedSuccessfully = "User retrieved successfully.";

		public static string UserListRetrievedSuccessfully = "User list retrieved successfully.";

		public static string InvalidCardDetails = "Invalid card details.";

		public static string UserNotAuthenticated = "User is not authenticated or ID is not available.";

		public static string UserNotFound = "User not found.";

		public static string PaymentSuccessful = "Payment successful.";

		public const string UserAlreadyHasDigitalWallet = "User already has a digital wallet.";

		public const string DigitalWalletNotFound = "Digital wallet not found.";
	}
}
