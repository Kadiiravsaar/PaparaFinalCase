using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Papara.Core.Extensions
{
	public static class HttpContextExtensions
	{
		public static string GetUserId(this HttpContext httpContext)
		{
			return httpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		}
	}
}
