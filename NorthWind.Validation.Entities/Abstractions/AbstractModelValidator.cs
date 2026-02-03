using NorthWind.Validation.Entities.Enums;
using NorthWind.Validation.Entities.Interfaces;
using NorthWind.Validation.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Validation.Entities.Abstractions
{
	public abstract class AbstractModelValidator<T> : IModelValidator<T>
	{
		public ValidationConstraint Constraint { get; }
		public IEnumerable<ValidationError> Errors { get; private set; }
		public IValidationService<T> ValidatorService { get; }

		protected AbstractModelValidator(
			IValidationService<T> validationService,
			ValidationConstraint constraint = ValidationConstraint.AlwaysValidate)
		{
			ValidatorService = validationService;
			Constraint = constraint;
		}

		public async Task<bool> Validate(T model)
		{
			Errors = await ValidatorService.Validate(model);
			return Errors == default;
		}

		protected IValidationRules<T, TProperty> AddRuleFor<TProperty>(
			Expression<Func<T, TProperty>> expression) =>
			ValidatorService.AddRuleFor<TProperty>(expression);

		protected ICollectionValidationRules<T, TProperty> AddRuleForEach<TProperty>(
			Expression<Func<T, IEnumerable<TProperty>>> expression) =>
			ValidatorService.AddRuleForEach<TProperty>(expression);
	}
}
