using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SimpleAPI.Public.Filters
{
	public class SimpleFilter
	{
		[MaxLength(50)]
		public string MyField { get; set; }
		public int? MyEnumSelection { get; set; }
	}
}
