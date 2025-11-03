using System;
using Newtonsoft.Json;
using Clinical6SDK.Models;

namespace Clinical6SDK.Common.Converters
{
	public class SearchMethodEnumJsonConverter : JsonConverter
	{
		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
		{
			var method = (ContentFilterSearchMethod)value;

			switch (method) {
				case ContentFilterSearchMethod.EndsWith:
					writer.WriteValue ("ends_with");
					break;
				case ContentFilterSearchMethod.StartsWith:
					writer.WriteValue ("starts_with");
					break;
				case ContentFilterSearchMethod.WholeWord:
					writer.WriteValue ("whole_word");
					break;
				default:
					writer.WriteValue (method.ToString ().ToLower ());
					break;
			}
		}

		public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var enumString = (string)reader.Value;
			ContentFilterSearchMethod? method = null;

			if (enumString == "ends_with")
				method = ContentFilterSearchMethod.EndsWith;

			if (enumString == "starts_with")
				method = ContentFilterSearchMethod.StartsWith;

			if (enumString == "whole_word")
				method = ContentFilterSearchMethod.WholeWord;

			return method;
		}

		public override bool CanConvert (Type objectType)
		{
			return objectType == typeof(string);
		}
	}
}

