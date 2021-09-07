using SimpleAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.String;

namespace SimpleAPI.Data.Entities
{
    public partial class SimpleChildPOCO : IValidatableObject
	{
		public bool Validate()
		{
			return Validator.TryValidateObject(this, new ValidationContext(this, null, null), ValidationErrors, true);
		}

		[NotMapped]
		public List<ValidationResult> ValidationErrors { get; set; } = new List<ValidationResult>();
		[NotMapped]
		public bool IsValid => Validate();

		// Field-Level Validation Rules for SimpleChildPOCO
		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			List<ValidationResult> errors = new List<ValidationResult>();

			// MyEnumField
			if (!Enum.IsDefined(typeof(SimpleEnum), MyEnumField))
			{
				errors.Add(new ValidationResult($"MyEnumField is an unknown value.", new[] { nameof(MyEnumField) }));
			}

			// MyField
			if (IsNullOrWhiteSpace(MyField))
			{
				errors.Add(new ValidationResult($"MyField is required.", new[] { nameof(MyField) }));
			}
			else
			{
				MyField = MyField.Trim();
				if (MyField.Length > 75)
					errors.Add(new ValidationResult($"MyField is too long.  Must be <= 75 characters.", new[] { nameof(MyField) }));
			}

			// Children - None

			return errors;
		}
	}
}
