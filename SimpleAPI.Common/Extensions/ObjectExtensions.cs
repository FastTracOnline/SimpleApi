using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SimpleAPI.Common.Extensions
{
	public static class ObjectExtensions
	{
		public static object GetPropertyValue(this object obj, string property)
		{
			var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
			if (prop == null)
				return null;
			else
				return prop.GetValue(obj, null);
		}

		public static bool TrySetProperty(this object obj, string property, object value)
		{
			var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
			if (prop != null && prop.CanWrite)
			{
				prop.SetValue(obj, value, null);
				return true;
			}
			else
				return false;
		}
	}
}
