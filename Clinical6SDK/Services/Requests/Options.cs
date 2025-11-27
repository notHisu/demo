using System;
using System.Collections.Generic;

namespace Clinical6SDK.Services.Requests
{
    public class Options
    {
        public string Url { set; get; }

        public string Key { set; get; } = "Id";

        public string CacheMode { get; set; }

        public object UriQuery { set; get; }
    }

    //public class UriQuery
    //{
    //    public int per_page;
    //    public int page;
    //    public object filters;
    //    public string search;
    //    public string sort;
    //}

}
