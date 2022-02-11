using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    // uc.check_ins -> uchicago_mod_circulation_storage.check_in
    // CheckIn2 -> CheckIn
    [DisplayColumn(nameof(Id)), DisplayName("Check Ins"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("check_ins", Schema = "uc")]
    public partial class CheckIn2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.CheckIn.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("occurred_date_time"), DataType(DataType.DateTime), Display(Name = "Occurred Date Time", Order = 2), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("occurredDateTime"), Required]
        public virtual DateTime? OccurredDateTime { get; set; }

        [Display(Order = 3)]
        public virtual Item2 Item { get; set; }

        [Column("item_id"), Display(Name = "Item", Order = 4), JsonProperty("itemId"), Required]
        public virtual Guid? ItemId { get; set; }

        [Column("item_status_prior_to_check_in"), Display(Name = "Item Status Prior To Check In", Order = 5), JsonProperty("itemStatusPriorToCheckIn"), StringLength(1024)]
        public virtual string ItemStatusPriorToCheckIn { get; set; }

        [Column("request_queue_size"), Display(Name = "Request Queue Size", Order = 6), JsonProperty("requestQueueSize")]
        public virtual int? RequestQueueSize { get; set; }

        [Display(Name = "Item Location", Order = 7)]
        public virtual Location2 ItemLocation { get; set; }

        [Column("item_location_id"), Display(Name = "Item Location", Order = 8), JsonProperty("itemLocationId")]
        public virtual Guid? ItemLocationId { get; set; }

        [Display(Name = "Service Point", Order = 9)]
        public virtual ServicePoint2 ServicePoint { get; set; }

        [Column("service_point_id"), Display(Name = "Service Point", Order = 10), JsonProperty("servicePointId"), Required]
        public virtual Guid? ServicePointId { get; set; }

        [Display(Name = "Performed By User", Order = 11)]
        public virtual User2 PerformedByUser { get; set; }

        [Column("performed_by_user_id"), Display(Name = "Performed By User", Order = 12), JsonProperty("performedByUserId"), Required]
        public virtual Guid? PerformedByUserId { get; set; }

        [Column("content"), CustomValidation(typeof(CheckIn), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 13), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OccurredDateTime)} = {OccurredDateTime}, {nameof(ItemId)} = {ItemId}, {nameof(ItemStatusPriorToCheckIn)} = {ItemStatusPriorToCheckIn}, {nameof(RequestQueueSize)} = {RequestQueueSize}, {nameof(ItemLocationId)} = {ItemLocationId}, {nameof(ServicePointId)} = {ServicePointId}, {nameof(PerformedByUserId)} = {PerformedByUserId}, {nameof(Content)} = {Content} }}";

        public static CheckIn2 FromJObject(JObject jObject) => jObject != null ? new CheckIn2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            OccurredDateTime = (DateTime?)jObject.SelectToken("occurredDateTime"),
            ItemId = (Guid?)jObject.SelectToken("itemId"),
            ItemStatusPriorToCheckIn = (string)jObject.SelectToken("itemStatusPriorToCheckIn"),
            RequestQueueSize = (int?)jObject.SelectToken("requestQueueSize"),
            ItemLocationId = (Guid?)jObject.SelectToken("itemLocationId"),
            ServicePointId = (Guid?)jObject.SelectToken("servicePointId"),
            PerformedByUserId = (Guid?)jObject.SelectToken("performedByUserId"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("occurredDateTime", OccurredDateTime?.ToLocalTime()),
            new JProperty("itemId", ItemId),
            new JProperty("itemStatusPriorToCheckIn", ItemStatusPriorToCheckIn),
            new JProperty("requestQueueSize", RequestQueueSize),
            new JProperty("itemLocationId", ItemLocationId),
            new JProperty("servicePointId", ServicePointId),
            new JProperty("performedByUserId", PerformedByUserId)).RemoveNullAndEmptyProperties();
    }
}
