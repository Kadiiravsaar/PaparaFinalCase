using Microsoft.AspNetCore.Diagnostics;
using Papara.Core.DTOs;
using Papara.Core.DTOs.Response;
using Papara.Service.Exceptions;
using System.Text.Json;

namespace Papara.API.Middlewares
{
	public static class UseCustomExceptionHandler
	{
		public static void UseCustomException(this IApplicationBuilder app)
		{
			app.UseExceptionHandler(config =>
			{

				config.Run(async context =>
				{
					context.Response.ContentType = "application/json";

					var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

					var statusCode = exceptionFeature.Error switch
					{
						ClientSideException => 400,
						NotFoundException => 404,
						BusinessException => 422,
						AuthorizationException => 401,
						_ => 500
					};
					context.Response.StatusCode = statusCode;


					var response = CustomResponseDto<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message);


					await context.Response.WriteAsync(JsonSerializer.Serialize(response));

				});
			});
		}
	}
}
