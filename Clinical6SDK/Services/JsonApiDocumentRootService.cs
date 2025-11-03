using System;
using System.Net.Http;
using System.Threading.Tasks;
using JsonApiSerializer.JsonApi;
using Clinical6SDK.Models;
using Clinical6SDK.Services.Requests;
using System.Collections;
using System.Reflection;
using System.Linq;

namespace Clinical6SDK.Services
{
    /// <summary>
    /// Wraps JsonApiHttpService to expose the DocumentRoot for meta, links,
    /// tags, and other document data in the transaction.
    /// </summary>
    public class JsonApiDocumentRootHttpService : JsonApiHttpService
    {
        /// <summary>
        /// Invokes a Get call to the server.
        /// </summary>
        /// <returns>A IJsonApiModel object</returns>
        /// <param name="obj">Object.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public new async Task<DocumentRoot<T>> Get<T>(
            IJsonApiModel obj = null,
            Options options = null
        ) where T : new()
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.Type))
            {
                T t = new T();

                Type type = t is IEnumerable ? t.GetType().GenericTypeArguments[0] : t.GetType();

                // If the type isn't IJsonApiModel, then throw an exception
                if (!type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IJsonApiModel)))
                    throw new Exception("Type Param must implement the IJsonApiModel interface");

                obj = (IJsonApiModel)Activator.CreateInstance(type);
            }

            return await Send<DocumentRoot<T>>(GenerateUrl(obj, options: options));
        }


        /// <summary>
        /// Invokes a Get call to the server to get children of an object.
        /// </summary>
        /// <returns>An object with Success, Response and Error</returns>
        /// <param name="obj">Object.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public new async Task<DocumentRoot<T>> GetChildren<T>(
            IJsonApiModel obj,
            string childType = null,
            Options options = null
        ) where T : new()
        {
            if (childType == null || string.IsNullOrWhiteSpace(childType))
            {
                T t = new T();

                Type type = t is IEnumerable ? t.GetType().GenericTypeArguments[0] : t.GetType();

                // If the type isn't IJsonApiModel, then throw an exception
                if (!type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IJsonApiModel)))
                    throw new Exception("Type Param must implement the IJsonApiModel interface");

                obj = (IJsonApiModel)Activator.CreateInstance(type);
            }
            return await Send<DocumentRoot<T>>(GenerateUrl(obj, childType, options));
        }


        /// <summary>
        /// Invokes an Insert(POST) call to the server.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public new async Task<DocumentRoot<T>> Insert<T>(
            IJsonApiModel obj,
            Options options = null
        ) where T : IJsonApiModel
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Parameter obj is required");

            return await Send<DocumentRoot<T>>(
                GenerateUrl(obj, options: options),
                httpMethod: HttpMethod.Post,
                requestData: ModelToJObject(obj));
        }


        /// <summary>
        /// Invokes an Update(PATCH) call to the server.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public new async Task<DocumentRoot<T>> Update<T>(
            IJsonApiModel obj,
            Options options = null
        ) where T : IJsonApiModel
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Parameter obj is required");

            return await Send<DocumentRoot<T>>(
                GenerateUrl(obj, options: options),
                httpMethod: new HttpMethod("PATCH"),
                requestData: ModelToJObject(obj));
        }
    }
}
