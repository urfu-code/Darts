using System;
using Darts.Domain.Models;
using Newtonsoft.Json;

namespace Darts.Domain.Repositories
{
	public class ThrowJsonConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteToken(JsonToken.String, ((ThrowResult)value).ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType != JsonToken.String)
				throw new JsonSerializationException("ThrowResult should be string in json");
			return ThrowResult.Parse(reader.Value.ToString());
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(ThrowResult);
		}
	}
}