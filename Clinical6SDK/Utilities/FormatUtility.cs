using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Clinical6SDK.Utilities
{
    public class FormatUtility
    {
        /// <summary>
        /// Converts an Object to a URI Query string
        /// </summary>
        /// <param name="p">Parameter dictionary to convert to uri query</param>
        /// <param name="prefix">A prefix (default empty) used for recursion (associative array query parameters)</param>
        /// <returns></returns>
        static public string ObjectToURIQuery(
            object p,
            string prefix = "")
        {
            List<string> uriQueryList = new List<string>();

            // Populate dictionary with from dictionary or anonymous type
            var dict = new Dictionary<string, object>();
            if (p is Dictionary<string, object>)
            {
                dict = p as Dictionary<string, object>;
            }
            else if (p.GetType().Name.Contains("AnonymousType"))
            {
                dict = p.GetType().GetRuntimeProperties().ToDictionary(x => x.Name, x => x.GetValue(p, null));
            }

            // Go through each item to add to List, determine if it has children, if there are children then recurse with prefix
            foreach (var item in dict)
            {
                if (item.Value is Dictionary<string, object> || item.Value.GetType().Name.Contains("AnonymousType"))
                {
                    string _prefix = prefix.Equals("") ? item.Key : string.Format("{0}[{1}]", prefix, item.Key);
                    uriQueryList.Add(ObjectToURIQuery(item.Value, _prefix));
                }
                else
                {
                    uriQueryList.Add(prefix.Equals("")
                        ? string.Format("{0}={1}", item.Key, item.Value?.ToString())
                        : string.Format("{0}[{1}]={2}", prefix, item.Key, item.Value?.ToString())
                    );
                }
            }
            return string.Join("&", uriQueryList);
        }

    }
}
