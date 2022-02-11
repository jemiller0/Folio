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
    // uc.requests -> uchicago_mod_circulation_storage.request
    // Request2 -> Request
    [DisplayColumn(nameof(Id)), DisplayName("Requests"), JsonConverter(typeof(JsonPathJsonConverter<Request2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("requests", Schema = "uc")]
    public partial class Request2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Request.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("request_type"), Display(Name = "Request Type", Order = 2), JsonProperty("requestType"), RegularExpression(@"^(Hold|Recall|Page)$"), Required, StringLength(1024)]
        public virtual string RequestType { get; set; }

        [Column("request_date"), DataType(DataType.Date), Display(Name = "Request Date", Order = 3), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("requestDate"), Required]
        public virtual DateTime? RequestDate { get; set; }

        [Column("patron_comments"), Display(Name = "Patron Comments", Order = 4), JsonProperty("patronComments"), StringLength(1024)]
        public virtual string PatronComments { get; set; }

        [Display(Order = 5), InverseProperty("Request2s3")]
        public virtual User2 Requester { get; set; }

        [Column("requester_id"), Display(Name = "Requester", Order = 6), JsonProperty("requesterId"), Required]
        public virtual Guid? RequesterId { get; set; }

        [Display(Name = "Proxy User", Order = 7), InverseProperty("Request2s2")]
        public virtual User2 ProxyUser { get; set; }

        [Column("proxy_user_id"), Display(Name = "Proxy User", Order = 8), JsonProperty("proxyUserId")]
        public virtual Guid? ProxyUserId { get; set; }

        [Display(Order = 9)]
        public virtual Item2 Item { get; set; }

        [Column("item_id"), Display(Name = "Item", Order = 10), JsonProperty("itemId"), Required]
        public virtual Guid? ItemId { get; set; }

        [Column("status"), Display(Order = 11), JsonProperty("status"), RegularExpression(@"^(Open - Not yet filled|Open - Awaiting pickup|Open - In transit|Open - Awaiting delivery|Closed - Filled|Closed - Cancelled|Closed - Unfilled|Closed - Pickup expired)$"), StringLength(1024)]
        public virtual string Status { get; set; }

        [Display(Name = "Cancellation Reason", Order = 12)]
        public virtual CancellationReason2 CancellationReason { get; set; }

        [Column("cancellation_reason_id"), Display(Name = "Cancellation Reason", Order = 13), JsonProperty("cancellationReasonId")]
        public virtual Guid? CancellationReasonId { get; set; }

        [Display(Name = "Cancelled By User", Order = 14), InverseProperty("Request2s")]
        public virtual User2 CancelledByUser { get; set; }

        [Column("cancelled_by_user_id"), Display(Name = "Cancelled By User", Order = 15), JsonProperty("cancelledByUserId")]
        public virtual Guid? CancelledByUserId { get; set; }

        [Column("cancellation_additional_information"), Display(Name = "Cancellation Additional Information", Order = 16), JsonProperty("cancellationAdditionalInformation"), StringLength(1024)]
        public virtual string CancellationAdditionalInformation { get; set; }

        [Column("cancelled_date"), DataType(DataType.Date), Display(Name = "Cancelled Date", Order = 17), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("cancelledDate")]
        public virtual DateTime? CancelledDate { get; set; }

        [Column("position"), Display(Order = 18), JsonProperty("position")]
        public virtual int? Position { get; set; }

        [Column("item_title"), Display(Name = "Item Title", Order = 19), JsonProperty("item.title"), StringLength(1024)]
        public virtual string ItemTitle { get; set; }

        [Column("item_barcode"), Display(Name = "Item Barcode", Order = 20), JsonProperty("item.barcode"), StringLength(1024)]
        public virtual string ItemBarcode { get; set; }

        [Column("requester_first_name"), Display(Name = "Requester First Name", Order = 21), JsonProperty("requester.firstName"), StringLength(1024)]
        public virtual string RequesterFirstName { get; set; }

        [Column("requester_last_name"), Display(Name = "Requester Last Name", Order = 22), JsonProperty("requester.lastName"), StringLength(1024)]
        public virtual string RequesterLastName { get; set; }

        [Column("requester_middle_name"), Display(Name = "Requester Middle Name", Order = 23), JsonProperty("requester.middleName"), StringLength(1024)]
        public virtual string RequesterMiddleName { get; set; }

        [Column("requester_barcode"), Display(Name = "Requester Barcode", Order = 24), JsonProperty("requester.barcode"), StringLength(1024)]
        public virtual string RequesterBarcode { get; set; }

        [Column("requester_patron_group"), Display(Name = "Requester Patron Group", Order = 25), JsonProperty("requester.patronGroup"), StringLength(1024)]
        public virtual string RequesterPatronGroup { get; set; }

        [Column("proxy_first_name"), Display(Name = "Proxy First Name", Order = 26), JsonProperty("proxy.firstName"), StringLength(1024)]
        public virtual string ProxyFirstName { get; set; }

        [Column("proxy_last_name"), Display(Name = "Proxy Last Name", Order = 27), JsonProperty("proxy.lastName"), StringLength(1024)]
        public virtual string ProxyLastName { get; set; }

        [Column("proxy_middle_name"), Display(Name = "Proxy Middle Name", Order = 28), JsonProperty("proxy.middleName"), StringLength(1024)]
        public virtual string ProxyMiddleName { get; set; }

        [Column("proxy_barcode"), Display(Name = "Proxy Barcode", Order = 29), JsonProperty("proxy.barcode"), StringLength(1024)]
        public virtual string ProxyBarcode { get; set; }

        [Column("proxy_patron_group"), Display(Name = "Proxy Patron Group", Order = 30), JsonProperty("proxy.patronGroup"), StringLength(1024)]
        public virtual string ProxyPatronGroup { get; set; }

        [Column("fulfilment_preference"), Display(Name = "Fulfilment Preference", Order = 31), JsonProperty("fulfilmentPreference"), RegularExpression(@"^(Hold Shelf|Delivery)$"), Required, StringLength(1024)]
        public virtual string FulfilmentPreference { get; set; }

        [Display(Name = "Delivery Address Type", Order = 32)]
        public virtual AddressType2 DeliveryAddressType { get; set; }

        [Column("delivery_address_type_id"), Display(Name = "Delivery Address Type", Order = 33), JsonProperty("deliveryAddressTypeId")]
        public virtual Guid? DeliveryAddressTypeId { get; set; }

        [Column("request_expiration_date"), DataType(DataType.Date), Display(Name = "Request Expiration Date", Order = 34), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("requestExpirationDate")]
        public virtual DateTime? RequestExpirationDate { get; set; }

        [Column("hold_shelf_expiration_date"), DataType(DataType.Date), Display(Name = "Hold Shelf Expiration Date", Order = 35), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("holdShelfExpirationDate")]
        public virtual DateTime? HoldShelfExpirationDate { get; set; }

        [Display(Name = "Pickup Service Point", Order = 36)]
        public virtual ServicePoint2 PickupServicePoint { get; set; }

        [Column("pickup_service_point_id"), Display(Name = "Pickup Service Point", Order = 37), JsonProperty("pickupServicePointId")]
        public virtual Guid? PickupServicePointId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 38), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 39), InverseProperty("Request2s1")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 40), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 42), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 43), InverseProperty("Request2s4")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 44), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("awaiting_pickup_request_closed_date"), DataType(DataType.Date), Display(Name = "Awaiting Pickup Request Closed Date", Order = 46), DisplayFormat(DataFormatString = "{0:d}"), Editable(false), JsonProperty("awaitingPickupRequestClosedDate")]
        public virtual DateTime? AwaitingPickupRequestClosedDate { get; set; }

        [Column("content"), CustomValidation(typeof(Request), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 47), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Request Identifiers", Order = 48), JsonProperty("item.identifiers")]
        public virtual ICollection<RequestIdentifier> RequestIdentifiers { get; set; }

        [Display(Name = "Request Tags", Order = 49), JsonConverter(typeof(ArrayJsonConverter<List<RequestTag>, RequestTag>), "Content"), JsonProperty("tags.tagList")]
        public virtual ICollection<RequestTag> RequestTags { get; set; }

        [Display(Name = "Scheduled Notices", Order = 50)]
        public virtual ICollection<ScheduledNotice2> ScheduledNotice2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(RequestType)} = {RequestType}, {nameof(RequestDate)} = {RequestDate}, {nameof(PatronComments)} = {PatronComments}, {nameof(RequesterId)} = {RequesterId}, {nameof(ProxyUserId)} = {ProxyUserId}, {nameof(ItemId)} = {ItemId}, {nameof(Status)} = {Status}, {nameof(CancellationReasonId)} = {CancellationReasonId}, {nameof(CancelledByUserId)} = {CancelledByUserId}, {nameof(CancellationAdditionalInformation)} = {CancellationAdditionalInformation}, {nameof(CancelledDate)} = {CancelledDate}, {nameof(Position)} = {Position}, {nameof(ItemTitle)} = {ItemTitle}, {nameof(ItemBarcode)} = {ItemBarcode}, {nameof(RequesterFirstName)} = {RequesterFirstName}, {nameof(RequesterLastName)} = {RequesterLastName}, {nameof(RequesterMiddleName)} = {RequesterMiddleName}, {nameof(RequesterBarcode)} = {RequesterBarcode}, {nameof(RequesterPatronGroup)} = {RequesterPatronGroup}, {nameof(ProxyFirstName)} = {ProxyFirstName}, {nameof(ProxyLastName)} = {ProxyLastName}, {nameof(ProxyMiddleName)} = {ProxyMiddleName}, {nameof(ProxyBarcode)} = {ProxyBarcode}, {nameof(ProxyPatronGroup)} = {ProxyPatronGroup}, {nameof(FulfilmentPreference)} = {FulfilmentPreference}, {nameof(DeliveryAddressTypeId)} = {DeliveryAddressTypeId}, {nameof(RequestExpirationDate)} = {RequestExpirationDate}, {nameof(HoldShelfExpirationDate)} = {HoldShelfExpirationDate}, {nameof(PickupServicePointId)} = {PickupServicePointId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(AwaitingPickupRequestClosedDate)} = {AwaitingPickupRequestClosedDate}, {nameof(Content)} = {Content}, {nameof(RequestIdentifiers)} = {(RequestIdentifiers != null ? $"{{ {string.Join(", ", RequestIdentifiers)} }}" : "")}, {nameof(RequestTags)} = {(RequestTags != null ? $"{{ {string.Join(", ", RequestTags)} }}" : "")} }}";

        public static Request2 FromJObject(JObject jObject) => jObject != null ? new Request2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            RequestType = (string)jObject.SelectToken("requestType"),
            RequestDate = (DateTime?)jObject.SelectToken("requestDate"),
            PatronComments = (string)jObject.SelectToken("patronComments"),
            RequesterId = (Guid?)jObject.SelectToken("requesterId"),
            ProxyUserId = (Guid?)jObject.SelectToken("proxyUserId"),
            ItemId = (Guid?)jObject.SelectToken("itemId"),
            Status = (string)jObject.SelectToken("status"),
            CancellationReasonId = (Guid?)jObject.SelectToken("cancellationReasonId"),
            CancelledByUserId = (Guid?)jObject.SelectToken("cancelledByUserId"),
            CancellationAdditionalInformation = (string)jObject.SelectToken("cancellationAdditionalInformation"),
            CancelledDate = (DateTime?)jObject.SelectToken("cancelledDate"),
            Position = (int?)jObject.SelectToken("position"),
            ItemTitle = (string)jObject.SelectToken("item.title"),
            ItemBarcode = (string)jObject.SelectToken("item.barcode"),
            RequesterFirstName = (string)jObject.SelectToken("requester.firstName"),
            RequesterLastName = (string)jObject.SelectToken("requester.lastName"),
            RequesterMiddleName = (string)jObject.SelectToken("requester.middleName"),
            RequesterBarcode = (string)jObject.SelectToken("requester.barcode"),
            RequesterPatronGroup = (string)jObject.SelectToken("requester.patronGroup"),
            ProxyFirstName = (string)jObject.SelectToken("proxy.firstName"),
            ProxyLastName = (string)jObject.SelectToken("proxy.lastName"),
            ProxyMiddleName = (string)jObject.SelectToken("proxy.middleName"),
            ProxyBarcode = (string)jObject.SelectToken("proxy.barcode"),
            ProxyPatronGroup = (string)jObject.SelectToken("proxy.patronGroup"),
            FulfilmentPreference = (string)jObject.SelectToken("fulfilmentPreference"),
            DeliveryAddressTypeId = (Guid?)jObject.SelectToken("deliveryAddressTypeId"),
            RequestExpirationDate = (DateTime?)jObject.SelectToken("requestExpirationDate"),
            HoldShelfExpirationDate = (DateTime?)jObject.SelectToken("holdShelfExpirationDate"),
            PickupServicePointId = (Guid?)jObject.SelectToken("pickupServicePointId"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            AwaitingPickupRequestClosedDate = (DateTime?)jObject.SelectToken("awaitingPickupRequestClosedDate"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            RequestIdentifiers = jObject.SelectToken("item.identifiers")?.Where(jt => jt.HasValues).Select(jt => RequestIdentifier.FromJObject((JObject)jt)).ToArray(),
            RequestTags = jObject.SelectToken("tags.tagList")?.Select(jt => RequestTag.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("requestType", RequestType),
            new JProperty("requestDate", RequestDate?.ToLocalTime()),
            new JProperty("patronComments", PatronComments),
            new JProperty("requesterId", RequesterId),
            new JProperty("proxyUserId", ProxyUserId),
            new JProperty("itemId", ItemId),
            new JProperty("status", Status),
            new JProperty("cancellationReasonId", CancellationReasonId),
            new JProperty("cancelledByUserId", CancelledByUserId),
            new JProperty("cancellationAdditionalInformation", CancellationAdditionalInformation),
            new JProperty("cancelledDate", CancelledDate?.ToLocalTime()),
            new JProperty("position", Position),
            new JProperty("item", new JObject(
                new JProperty("title", ItemTitle),
                new JProperty("barcode", ItemBarcode),
                new JProperty("identifiers", RequestIdentifiers?.Select(ri => ri.ToJObject())))),
            new JProperty("requester", new JObject(
                new JProperty("firstName", RequesterFirstName),
                new JProperty("lastName", RequesterLastName),
                new JProperty("middleName", RequesterMiddleName),
                new JProperty("barcode", RequesterBarcode),
                new JProperty("patronGroup", RequesterPatronGroup))),
            new JProperty("proxy", new JObject(
                new JProperty("firstName", ProxyFirstName),
                new JProperty("lastName", ProxyLastName),
                new JProperty("middleName", ProxyMiddleName),
                new JProperty("barcode", ProxyBarcode),
                new JProperty("patronGroup", ProxyPatronGroup))),
            new JProperty("fulfilmentPreference", FulfilmentPreference),
            new JProperty("deliveryAddressTypeId", DeliveryAddressTypeId),
            new JProperty("requestExpirationDate", RequestExpirationDate?.ToLocalTime()),
            new JProperty("holdShelfExpirationDate", HoldShelfExpirationDate?.ToLocalTime()),
            new JProperty("pickupServicePointId", PickupServicePointId),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("awaitingPickupRequestClosedDate", AwaitingPickupRequestClosedDate?.ToLocalTime()),
            new JProperty("tags", new JObject(
                new JProperty("tagList", RequestTags?.Select(rt => rt.ToJObject()))))).RemoveNullAndEmptyProperties();
    }
}
