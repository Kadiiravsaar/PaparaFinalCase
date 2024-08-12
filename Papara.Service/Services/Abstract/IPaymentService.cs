using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Service.Services.Abstract
{
	public interface IPaymentService
	{
		/// <summary>
		/// İngilizce : Loads balance into the digital wallet of the logged-in user
		/// Türkçe : Giriş yapmış kullanıcının dijital cüzdanına bakiye yükler.
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<PaymentResult> ProcessPayment(PaymentRequest request);
	}
}
