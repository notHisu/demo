using System;
using Clinical6SDK.Models;
using JsonApiSerializer.JsonConverters;
using Newtonsoft.Json;

namespace Clinical6SDK.Common.Converters
{
    public class MyTypeDeterminingResourceObjectConvertor : ResourceObjectConverter
    {
        protected override object CreateObject(Type objectType, string jsonapiType, JsonSerializer serializer)
        {
            switch (jsonapiType)
            {
                case "edc__connections":
                    return new EDCConnection();
                case "data_collection__flow_processes":
                    return new Flow();
                case "c6__flow_connections":
                    return new FlowConnection();
                default:
                    return base.CreateObject(objectType, jsonapiType, serializer);
            }
        }
    }
}
