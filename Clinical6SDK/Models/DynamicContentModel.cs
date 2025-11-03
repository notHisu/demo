using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Clinical6SDK.Models
{
    /// <summary>
    /// This content model will be removed very soon.
    /// Please see Clinical6SDK.Helpers.Content
    /// </summary>
    /// See <see cref="Helpers.Content"/>
    public class ContentModel : JsonApiModel
	{
        [JsonProperty("type")]
        public override string Type { get; set; } = "dynamic_content__contents";

        [JsonProperty("heart_rate")]
        public int HeartRate { get; set; }

        [JsonProperty("content_type")]
        public ContentTypeModel ContentType { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("policy")]
        public string Policy { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("pdf_upload")]
        public List<PdfUpload>  PdfUpload { get; set; }

        [JsonProperty("file")]
        public List<PdfUpload> File { get; set; }

        public bool Selected { get; set; }

        [JsonProperty("length")]
        public string Length { get; set; }

        [JsonProperty("video")]
        public List<DynamicContentVideo> Videos { get; set; }

        [JsonProperty("article")]
        public List<DynamicContentArticle> Articles { get; set; }

        [JsonProperty("visibility_status")]
        public string VisibilityStatus { get; set; }

        [JsonProperty("article_name")]
        public string ArticleName { get; set; }

        [JsonProperty("published_date")]
        public string PublishedDate { get; set; }

        [JsonProperty("file_type")]
        public string FileType { get; set; }

        // From Content

        [JsonProperty("favorited", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsFavorited { get; set; }

        [JsonProperty("liked", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsLiked { get; set; }

        [JsonProperty("likes", NullValueHandling = NullValueHandling.Ignore)]
        public int Likes { get; set; }

        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        [JsonIgnore]
        public Brand Brand { get; set; }

        [JsonIgnore]
        public ImageResource Image { get; set; }

        [JsonIgnore]
        public Location Location { get; set; }

        [JsonProperty("latitude"), JsonIgnore]
        public string Latitude { get; set; }

        [JsonProperty("longitude"), JsonIgnore]
        public string Longitude { get; set; }

    }


    public class PdfUpload
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("path")]
        public DynamicContentPath Path { get; set; }
    }

    public class ContentTypeModel : JsonApiModel
    {
        [JsonProperty("type")]
        public override string Type { get; set; } = "dynamic_content__content_types";

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("permanent_link")]
        public string PermanentLink { get; set; }

        [JsonProperty("policy")]
        public string Policy { get; set; }
    }

    public class DynamicContentVideo
    {
        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("path")]
        public DynamicContentPath Path { get; set; }
    }
  
    public class DynamicContentArticle
    {
        [JsonProperty("resourceable_id")]
        public string ResourceableId { get; set; }

        [JsonProperty("resourceable_type")]
        public string ResourceableType { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("path")]
        public DynamicContentPath Path { get; set; }
    }

    public class DynamicAttribute
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("length")]
        public string Length { get; set; }

        [JsonProperty("video")]
        public DynamicContentVideo[] Videos { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("visibility_status")]
        public string VisibilityStatus { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("policy")]
        public string Policy { get; set; }
    }

    public class DynamicContentPath
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class ContentKits// : Clinical6ModelBase
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updated_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("position", NullValueHandling = NullValueHandling.Ignore)]
        public int? Position { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonIgnore]
        public ImageResource Image { get; set; }

        [JsonProperty("barcode", NullValueHandling = NullValueHandling.Ignore)]
        public string Barcode { get; set; }

        [JsonProperty("longitude", NullValueHandling = NullValueHandling.Ignore)]
        public string Longitude { get; set; }

        [JsonProperty("latitude", NullValueHandling = NullValueHandling.Ignore)]
        public string Latitude { get; set; }

        [JsonProperty("site_id", NullValueHandling = NullValueHandling.Ignore)]
        public int SiteId { get; set; }

        [JsonProperty("content_type", NullValueHandling = NullValueHandling.Ignore)]
        public string ContentType { get; set; }
    }
}
