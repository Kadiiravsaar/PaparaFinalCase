using FluentValidation;
using Papara.Service.Constants;
using Papara.Service.Exceptions;

namespace Papara.Service.Rules
{
	public class BusinessRules
	{
		public static void CheckEntityExists<TEntity>(TEntity entity)
		{
			if (entity == null)
			{
				throw new BusinessException(Messages.EntityNotFound);
			}
			
		}
	}
}
