using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleAPI.Common
{
	public enum ChangeType
	{
		LogOnly = 0,
		Create = 1,
		Update = 2,
		Delete = 3
	}

	public enum SimpleEnum
	{
		[Display(Name="TBD")]
		Unknown = 0,
		GoodValue = 1
	}
}
