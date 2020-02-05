using System;
using System.Collections.Generic;
using System.Text;
using Ladon;

namespace Yort.Humm.InStore.Infrastructure
{
	internal static class HummModelPropertyReader
	{
		private static readonly Dictionary<Type, Dictionary<string, Func<object, object>>> _CachedPropertyInfo = new Dictionary<Type, Dictionary<string, Func<object, object>>>();

		public static Dictionary<string, object> GetPropertyValues<T>(T request) where T : class
		{
			request.GuardNull(nameof(request));

			var sourceType = typeof(T);
			if (sourceType == typeof(RequestBase) || sourceType == typeof(ResponseBase))
				sourceType = request.GetType();

			var properties = GetProperties(sourceType);
			var retVal = new Dictionary<string, object>(properties.Count);
			foreach (var property in properties)
			{
				retVal[property.Key] = property.Value(request);
			}
			return retVal;
		}

		private static Dictionary<string, Func<object, object>> GetProperties(Type sourceType)
		{
			if (_CachedPropertyInfo.TryGetValue(sourceType, out var retVal))
				return retVal;

			var allProps = sourceType.GetProperties();
			retVal = new Dictionary<string, Func<object, object>>(allProps.Length + 1);
			foreach (var property in allProps)
			{
				var name = property.Name;
				var attrs = property.GetCustomAttributes(typeof(Newtonsoft.Json.JsonIgnoreAttribute), true);
				if (attrs != null && attrs.Length > 0) continue;

				attrs = property.GetCustomAttributes(typeof(Newtonsoft.Json.JsonPropertyAttribute), true);
				if (attrs != null && attrs.Length > 0)
					name = ((Newtonsoft.Json.JsonPropertyAttribute)attrs[0]).PropertyName;

				retVal[name] = new Func<object, object>((t) => property.GetValue(t));
			}

			_CachedPropertyInfo[sourceType] = retVal;

			return retVal;
		}

	}
}
