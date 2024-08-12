using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Service.Constants
{
	public class EmailTemplates
	{
		public const string OrderConfirmationTemplate = @"
        <html>
        <head>
            <style>
                table {{
                    width: 100%;
                    border-collapse: collapse;
                }}
                table, th, td {{
                    border: 1px solid black;
                }}
                th, td {{
                    padding: 8px;
                    text-align: left;
                }}
            </style>
        </head>
        <body>
            <h1>Sipariş Onayı</h1>
            <p>Merhaba {{name}},</p>
            <p>Sipariş Numaranız: <strong>{{orderNumber}}</strong></p>
            <p>Toplam Tutar: <strong>{{totalAmount}} TL</strong></p>
            <p>Kullanılan Puan: <strong>{{pointUsed}}</strong></p>
            <p>Sipariş Tarihi: <strong>{{orderDate}}</strong></p>
        </body>
        </html>";
	}

}
