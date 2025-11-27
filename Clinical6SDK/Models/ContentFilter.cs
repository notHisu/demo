using System;
using Newtonsoft.Json;
using Clinical6SDK.Common.Converters;

namespace Clinical6SDK.Models
{
	public enum ContentFilterSearchMethod {
		StartsWith,
		EndsWith,
		WholeWord
	}

	public interface IContentFilter
	{
		[JsonProperty ("attribute")]
		string Attribute { get; }
	}

	public interface IStringValueContentFilter : IContentFilter
	{
		[JsonProperty ("values")]
		string[] Values { get; set; }

		[JsonProperty ("whole_word")]
		bool MatchWholeWordOnly { get; set; }

		[JsonProperty ("case_sensitive")]
		bool IsCaseSensitive { get; set; }

		[JsonProperty ("search_method")]
		[JsonConverter (typeof(SearchMethodEnumJsonConverter))]
		ContentFilterSearchMethod SearchMethod { get; set; }
	}

	public interface IIntValueContentFilter : IContentFilter
	{
		int[] Values { get; set; }
	}

	public class SearchContentFilter : IStringValueContentFilter
	{
		public string Attribute {
			get { return "search"; }
		}

		public string[] Values { get; set; }
		public bool MatchWholeWordOnly { get; set; }
		public bool IsCaseSensitive { get; set; }
		public ContentFilterSearchMethod SearchMethod { get; set; }
	}

	#region Tag Filters
	public class TagContentFilter : IStringValueContentFilter
	{
		public string Attribute {
			get { return "tag_name"; }
		}

		public string[] Values { get; set; }
		public bool MatchWholeWordOnly { get; set; }
		public bool IsCaseSensitive { get; set; }
		public ContentFilterSearchMethod SearchMethod { get; set; }
	}

	public class TagIdContentFilter : IIntValueContentFilter 
	{
		public string Attribute {
			get { return "tag"; }
		}

		public int[] Values { get; set; }
	}	
	#endregion

	#region Location Filters
	public abstract class LocationContentFilter : IContentFilter 
	{
		public string Attribute {
			get { return "location"; }
		}
	}

	public class LocationRadiusContentFilter : LocationContentFilter
	{
		[JsonProperty ("exact_lat")]
		public double ExactLatitude { get; set; }

		[JsonProperty ("exact_lng")]
		public double ExactLongitude { get; set; }

		[JsonProperty ("radius")]
		public double Radius { get; set; }
	}

	public class LocationBoxContentFilter : LocationContentFilter
	{
		[JsonProperty ("sw_lat")]
		public double SouthWestLatitude { get; set; }

		[JsonProperty ("sw_lng")]
		public double SouthWestLongitude { get; set; }

		[JsonProperty ("ne_lat")]
		public double NorthEastLatitude { get; set; }

		[JsonProperty ("ne_lng")]
		public double NorthEastLongitude { get; set; }
	}

	public class LocationAddressContentFilter : IStringValueContentFilter
	{
		public string Attribute {
			get { return "location"; }
		}

		public string[] Values { get; set; }
		public bool MatchWholeWordOnly { get; set; }
		public bool IsCaseSensitive { get; set; }
		public ContentFilterSearchMethod SearchMethod { get; set; }
	}
	#endregion

	public class TextContentFilter : IStringValueContentFilter
	{
		public string Attribute {
			get { return "text"; }
		}

		public string[] Values { get; set; }
		public bool MatchWholeWordOnly { get; set; }
		public bool IsCaseSensitive { get; set; }
		public ContentFilterSearchMethod SearchMethod { get; set; }
	}

	public class DateContentFilter : IContentFilter
	{
		public string Attribute {
			get { return "date"; }
		}

		[JsonProperty ("min")]
		public string Min { get; set; }

		[JsonProperty ("max")]
		public string Max { get; set; }
	}

	public class NumericContentFilter : IContentFilter 
	{
		public string Attribute {
			get { return "numeric"; }
		}

		[JsonProperty ("min")]
		public string Min { get; set; }

		[JsonProperty ("max")]
		public string Max { get; set; }
	}
}

