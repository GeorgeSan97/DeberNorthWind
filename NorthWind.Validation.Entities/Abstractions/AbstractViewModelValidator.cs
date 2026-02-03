using NorthWind.Validation.Entities.Enums;
using NorthWind.Validation.Entities.Interfaces;
using NorthWind.Validation.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.Validation.Entities.Abstractions
{
	public abstract class AbstractViewModelValidator<DtoType, ViewModelType> : IModelValidator<ViewModelType>
	{
		private readonly IModelValidatorHub<DtoType> _dtoModelValidatorHub;
		private readonly ValidationConstraint _constraint;

		protected AbstractViewModelValidator(
			IModelValidatorHub<DtoType> dtoModelValidatorHub,
			ValidationConstraint constraint)
		{
			_dtoModelValidatorHub = dtoModelValidatorHub;
			_constraint = constraint;
		}

		public ValidationConstraint Constraint => _constraint;
		public IEnumerable<ValidationError> Errors => _dtoModelValidatorHub.Errors;

		// En caso de que el ViewModel implemente el operador Explicit se puede
		// utilizar este método.
		// Si el ViewModel no implementa el operador Explicit, se podrá remplazar
		// (Override) este método en la clase que implemente esta clase.
		public virtual DtoType Cast(ViewModelType viewModel)
		{
			DtoType DtoModel = default;
			var ExplicitMethod = typeof(ViewModelType).GetMethod("op_Explicit");
			if (ExplicitMethod != null)
				DtoModel = (DtoType)ExplicitMethod.Invoke(
				viewModel, new object[] { viewModel });
			else
				throw new InvalidCastException();
			return DtoModel;
		}
		public Task<bool> Validate(ViewModelType model) =>
			_dtoModelValidatorHub.Validate(Cast(model));
	}
}