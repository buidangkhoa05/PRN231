using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Service.Common
{
	public static class QueryExtesions
	{
		public static string ToJsonString<T>(this ICollection<T> data)
		{
			if (data == null || !data.Any())
			{
				return string.Empty;
			}

			IEnumerable<JObject> value = data.Select((T x) => JObject.FromObject(x).ToSnakeCase());
			return JsonConvert.SerializeObject(value);
		}

		public static string ToQueryString(this object obj)
		{
			IEnumerable<string> source = from p in obj.GetType().GetProperties()
										 where p.GetValue(obj, null) != null
										 select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());
			return "?" + string.Join("&", source.ToArray());
		}

		public static string ToQueryString(this IDictionary<string, string> dict)
		{
			List<string> list = new List<string>();
			foreach (var item in dict)
			{
				list.Add(item.Key + "=" + item.Value);
			}

			return "?" + string.Join("&", list);
		}
	}
}
