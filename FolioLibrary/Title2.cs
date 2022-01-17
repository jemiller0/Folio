using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    // uc.titles -> diku_mod_orders_storage.titles
    // Title2 -> Title
    [DisplayColumn(nameof(Title)), DisplayName("Titles"), JsonConverter(typeof(JsonPathJsonConverter<Title2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("titles", Schema = "uc")]
    public partial class Title2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Title.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("expected_receipt_date"), DataType(DataType.Date), Display(Name = "Expected Receipt Date", Order = 2), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("expectedReceiptDate")]
        public virtual DateTime? ExpectedReceiptDate { get; set; }

        [Column("title"), Display(Order = 3), JsonProperty("title"), Required, StringLength(1024)]
        public virtual string Title { get; set; }

        [Display(Name = "Order Item", Order = 4)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("po_line_id"), Display(Name = "Order Item", Order = 5), JsonProperty("poLineId"), Required]
        public virtual Guid? OrderItemId { get; set; }

        [Display(Order = 6)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id"), Display(Name = "Instance", Order = 7), JsonProperty("instanceId")]
        public virtual Guid? InstanceId { get; set; }

        [Column("publisher"), Display(Order = 8), JsonProperty("publisher"), StringLength(1024)]
        public virtual string Publisher { get; set; }

        [Column("edition"), Display(Order = 9), JsonProperty("edition"), StringLength(1024)]
        public virtual string Edition { get; set; }

        [Column("package_name"), Display(Name = "Package Name", Order = 10), JsonProperty("packageName"), StringLength(1024)]
        public virtual string PackageName { get; set; }

        [Column("po_line_number"), Display(Name = "Order Item Number", Order = 11), JsonProperty("poLineNumber"), StringLength(1024)]
        public virtual string OrderItemNumber { get; set; }

        [Column("published_date"), Display(Name = "Published Date", Order = 12), JsonProperty("publishedDate"), StringLength(1024)]
        public virtual string PublishedDate { get; set; }

        [Column("receiving_note"), Display(Name = "Receiving Note", Order = 13), JsonProperty("receivingNote"), StringLength(1024)]
        public virtual string ReceivingNote { get; set; }

        [Column("subscription_from"), Display(Name = "Subscription From", Order = 14), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("subscriptionFrom")]
        public virtual DateTime? SubscriptionFrom { get; set; }

        [Column("subscription_to"), Display(Name = "Subscription To", Order = 15), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("subscriptionTo")]
        public virtual DateTime? SubscriptionTo { get; set; }

        [Column("subscription_interval"), Display(Name = "Subscription Interval", Order = 16), JsonProperty("subscriptionInterval")]
        public virtual int? SubscriptionInterval { get; set; }

        [Column("is_acknowledged"), Display(Name = "Is Acknowledged", Order = 17), JsonProperty("isAcknowledged")]
        public virtual bool? IsAcknowledged { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 18), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 19), InverseProperty("Title2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 20), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 22), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 23), InverseProperty("Title2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 24), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Title), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 26), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Receivings", Order = 27)]
        public virtual ICollection<Receiving2> Receiving2s { get; set; }

        [Display(Name = "Title Contributors", Order = 28), JsonProperty("contributors")]
        public virtual ICollection<TitleContributor> TitleContributors { get; set; }

        [Display(Name = "Title Product Ids", Order = 29), JsonProperty("productIds")]
        public virtual ICollection<TitleProductId> TitleProductIds { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ExpectedReceiptDate)} = {ExpectedReceiptDate}, {nameof(Title)} = {Title}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(InstanceId)} = {InstanceId}, {nameof(Publisher)} = {Publisher}, {nameof(Edition)} = {Edition}, {nameof(PackageName)} = {PackageName}, {nameof(OrderItemNumber)} = {OrderItemNumber}, {nameof(PublishedDate)} = {PublishedDate}, {nameof(ReceivingNote)} = {ReceivingNote}, {nameof(SubscriptionFrom)} = {SubscriptionFrom}, {nameof(SubscriptionTo)} = {SubscriptionTo}, {nameof(SubscriptionInterval)} = {SubscriptionInterval}, {nameof(IsAcknowledged)} = {IsAcknowledged}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(TitleContributors)} = {(TitleContributors != null ? $"{{ {string.Join(", ", TitleContributors)} }}" : "")}, {nameof(TitleProductIds)} = {(TitleProductIds != null ? $"{{ {string.Join(", ", TitleProductIds)} }}" : "")} }}";

        public static Title2 FromJObject(JObject jObject) => jObject != null ? new Title2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            ExpectedReceiptDate = (DateTime?)jObject.SelectToken("expectedReceiptDate"),
            Title = (string)jObject.SelectToken("title"),
            OrderItemId = (Guid?)jObject.SelectToken("poLineId"),
            InstanceId = (Guid?)jObject.SelectToken("instanceId"),
            Publisher = (string)jObject.SelectToken("publisher"),
            Edition = (string)jObject.SelectToken("edition"),
            PackageName = (string)jObject.SelectToken("packageName"),
            OrderItemNumber = (string)jObject.SelectToken("poLineNumber"),
            PublishedDate = (string)jObject.SelectToken("publishedDate"),
            ReceivingNote = (string)jObject.SelectToken("receivingNote"),
            SubscriptionFrom = (DateTime?)jObject.SelectToken("subscriptionFrom"),
            SubscriptionTo = (DateTime?)jObject.SelectToken("subscriptionTo"),
            SubscriptionInterval = (int?)jObject.SelectToken("subscriptionInterval"),
            IsAcknowledged = (bool?)jObject.SelectToken("isAcknowledged"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            TitleContributors = jObject.SelectToken("contributors")?.Select(jt => TitleContributor.FromJObject((JObject)jt)).ToArray(),
            TitleProductIds = jObject.SelectToken("productIds")?.Select(jt => TitleProductId.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("expectedReceiptDate", ExpectedReceiptDate?.ToLocalTime()),
            new JProperty("title", Title),
            new JProperty("poLineId", OrderItemId),
            new JProperty("instanceId", InstanceId),
            new JProperty("publisher", Publisher),
            new JProperty("edition", Edition),
            new JProperty("packageName", PackageName),
            new JProperty("poLineNumber", OrderItemNumber),
            new JProperty("publishedDate", PublishedDate),
            new JProperty("receivingNote", ReceivingNote),
            new JProperty("subscriptionFrom", SubscriptionFrom?.ToLocalTime()),
            new JProperty("subscriptionTo", SubscriptionTo?.ToLocalTime()),
            new JProperty("subscriptionInterval", SubscriptionInterval),
            new JProperty("isAcknowledged", IsAcknowledged),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("contributors", TitleContributors?.Select(tc => tc.ToJObject())),
            new JProperty("productIds", TitleProductIds?.Select(tpi => tpi.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
