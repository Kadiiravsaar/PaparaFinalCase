﻿namespace Papara.Core.DTOs.Response
{
	public class LoginResponseDTO 
	{
		public string Token { get; set; }
		public string RefreshToken { get; set; }
		public DateTime Expiration { get; set; }
	}


	



}