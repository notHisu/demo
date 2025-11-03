using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms.Clinical6.Core.Models;

namespace Xamarin.Forms.Clinical6.Core.Helpers
{
    public class SubTaskResultsAttributesConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            // Handle only boolean types.
            if (objectType == typeof(SubTaskResultsAttributes))
                return true;

            return false;
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            SubTaskResultsAttributes taskResult = new SubTaskResultsAttributes();

            try
            {
                string answers = string.Empty;
                JObject jObject = JObject.Load(reader);
                Debug.WriteLine(jObject);

                taskResult.CreatedAt = jObject.SelectToken("created_at").ToString();
                taskResult.Question = jObject.SelectToken("question").ToString();
                taskResult.AnswerResultType = jObject.SelectToken("result_type").ToString();

                var answerJToken = jObject.SelectToken("answer");
                answers = answerJToken.Type == JTokenType.Array ? string.Join(",", answerJToken) : answerJToken.ToString();

                taskResult.Answer = answers;

                return taskResult;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return taskResult;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }
    }
}
