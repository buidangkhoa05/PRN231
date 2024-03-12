using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service.Common
{
	public static class StringExtensoins
	{
		public static string ToSnakeCase(this string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}

			return Regex.Match(input, "^_+")?.ToString() + Regex.Replace(input, "([a-z0-9])([A-Z])", "$1_$2").ToLower();
		}

		public static JObject ToSnakeCase(this JObject original)
		{
			JObject jObject = new JObject();
			foreach (JProperty item in original.Properties())
			{
				string propertyName = item.Name.ToSnakeCase();
				jObject[propertyName] = item.Value;
			}

			return jObject;
		}
	}
}
