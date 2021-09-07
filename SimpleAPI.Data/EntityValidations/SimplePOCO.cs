using SimpleAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.String;

namespace SimpleAPI.Data.Entities
{
    public partial class SimplePOCO : IValidatableObject
	{
		public bool Validate()
		{
			return Validator.TryValidateObject(this, new ValidationContext(this, null, null), ValidationErrors, true);
		}

		[NotMapped]
		public List<ValidationResult> ValidationErrors { get; set; } = new List<ValidationResult>();
		[NotMapped]
		public bool IsValid => Validate();

		// Field-Level Validation Rules for SimplePOCO
		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			List<ValidationResult> errors = new List<ValidationResult>();

			// Id
			if (Id == Guid.Empty)
			{
				errors.Add(new ValidationResult($"Id cannot be blank or default value.", new[] { nameof(Id) }));
			}

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

			// Children
			if (MyChildren != null)
                foreach (var item in MyChildren)
                {
					if (!item.IsValid)
					{
						errors.AddRange(item.ValidationErrors);
					}
                }
			else
				errors.Add(new ValidationResult($"MyChildren is uninitialized.", new[] { nameof(MyChildren) }));

			return errors;
		}
	}
}
