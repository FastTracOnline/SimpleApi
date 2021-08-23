using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SimpleAPI.Common.Extensions
{
	public static class EnumExtensions
	{
		public static string GetDisplayName(this Enum value)
		{
			if (!Enum.IsDefined(value.GetType(), value))
				return "";

			FieldInfo fi = value.GetType().GetField(value.ToString());

			DisplayAttribute[] attributes = (DisplayAttribute[])fi.GetCustomAttributes(typeof(DisplayAttribute), false);

			if (attributes != null && attributes.Length > 0)
				return attributes[0].Name;
			else
				return value.ToString();
		}

		public static string GetDisplayNameNew(this Enum value)
		{
			if (!Enum.IsDefined(value.GetType(), value))
				return "";

			return value.GetType()?.GetMember(value.ToString())?
				.First()?
				.GetCustomAttribute<DisplayAttribute>()?
				.Name ?? "";
		}
	}
}
