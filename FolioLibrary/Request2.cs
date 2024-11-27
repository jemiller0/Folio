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

        [Column("request_level"), Display(Name = "Request Level", Order = 2), JsonProperty("requestLevel"), RegularExpression(@"^(Item|Title)$"), StringLength(1024)]
        public virtual string RequestLevel { get; set; }

        [Column("request_type"), Display(Name = "Request Type", Order = 3), JsonProperty("requestType"), RegularExpression(@"^(Hold|Recall|Page)$"), Required, StringLength(1024)]
        public virtual string RequestType { get; set; }

        [Column("ecs_request_phase"), Display(Name = "Ecs Request Phase", Order = 4), JsonProperty("ecsRequestPhase"), RegularExpression(@"^(Primary|Secondary)$"), StringLength(1024)]
        public virtual string EcsRequestPhase { get; set; }

        [Column("request_date"), DataType(DataType.Date), Display(Name = "Request Date", Order = 5), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("requestDate"), Required]
        public virtual DateTime? RequestDate { get; set; }

        [Column("patron_comments"), Display(Name = "Patron Comments", Order = 6), JsonProperty("patronComments"), StringLength(1024)]
        public virtual string PatronComments { get; set; }

        [Display(Order = 7), InverseProperty("Request2s4")]
        public virtual User2 Requester { get; set; }

        [Column("requester_id"), Display(Name = "Requester", Order = 8), JsonProperty("requesterId"), Required]
        public virtual Guid? RequesterId { get; set; }

        [Display(Name = "Proxy User", Order = 9), InverseProperty("Request2s3")]
        public virtual User2 ProxyUser { get; set; }

        [Column("proxy_user_id"), Display(Name = "Proxy User", Order = 10), JsonProperty("proxyUserId")]
        public virtual Guid? ProxyUserId { get; set; }

        [Display(Order = 11)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id"), Display(Name = "Instance", Order = 12), JsonProperty("instanceId")]
        public virtual Guid? InstanceId { get; set; }

        [Display(Order = 13)]
        public virtual Holding2 Holding { get; set; }

        [Column("holding_id"), Display(Name = "Holding", Order = 14), JsonProperty("holdingsRecordId")]
        public virtual Guid? HoldingId { get; set; }

        [Display(Order = 15)]
        public virtual Item2 Item { get; set; }

        [Column("item_id"), Display(Name = "Item", Order = 16), JsonProperty("itemId"), Required]
        public virtual Guid? ItemId { get; set; }

        [Column("status"), Display(Order = 17), JsonProperty("status"), RegularExpression(@"^(Open - Not yet filled|Open - Awaiting pickup|Open - In transit|Open - Awaiting delivery|Closed - Filled|Closed - Cancelled|Closed - Unfilled|Closed - Pickup expired)$"), StringLength(1024)]
        public virtual string Status { get; set; }

        [Display(Name = "Cancellation Reason", Order = 18)]
        public virtual CancellationReason2 CancellationReason { get; set; }

        [Column("cancellation_reason_id"), Display(Name = "Cancellation Reason", Order = 19), JsonProperty("cancellationReasonId")]
        public virtual Guid? CancellationReasonId { get; set; }

        [Display(Name = "Cancelled By User", Order = 20), InverseProperty("Request2s1")]
        public virtual User2 CancelledByUser { get; set; }

        [Column("cancelled_by_user_id"), Display(Name = "Cancelled By User", Order = 21), JsonProperty("cancelledByUserId")]
        public virtual Guid? CancelledByUserId { get; set; }

        [Column("cancellation_additional_information"), Display(Name = "Cancellation Additional Information", Order = 22), JsonProperty("cancellationAdditionalInformation"), StringLength(1024)]
        public virtual string CancellationAdditionalInformation { get; set; }

        [Column("cancelled_date"), DataType(DataType.Date), Display(Name = "Cancelled Date", Order = 23), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("cancelledDate")]
        public virtual DateTime? CancelledDate { get; set; }

        [Column("position"), Display(Order = 24), JsonProperty("position")]
        public virtual int? Position { get; set; }

        [Column("instance_title"), Display(Name = "Instance Title", Order = 25), JsonProperty("instance.title"), StringLength(1024)]
        public virtual string InstanceTitle { get; set; }

        [Column("item_barcode"), Display(Name = "Item Barcode", Order = 26), JsonProperty("item.barcode"), StringLength(1024)]
        public virtual string ItemBarcode { get; set; }

        [Column("requester_first_name"), Display(Name = "Requester First Name", Order = 27), JsonProperty("requester.firstName"), StringLength(1024)]
        public virtual string RequesterFirstName { get; set; }

        [Column("requester_last_name"), Display(Name = "Requester Last Name", Order = 28), JsonProperty("requester.lastName"), StringLength(1024)]
        public virtual string RequesterLastName { get; set; }

        [Column("requester_middle_name"), Display(Name = "Requester Middle Name", Order = 29), JsonProperty("requester.middleName"), StringLength(1024)]
        public virtual string RequesterMiddleName { get; set; }

        [Column("requester_barcode"), Display(Name = "Requester Barcode", Order = 30), JsonProperty("requester.barcode"), StringLength(1024)]
        public virtual string RequesterBarcode { get; set; }

        [Column("requester_patron_group"), Display(Name = "Requester Patron Group", Order = 31), JsonProperty("requester.patronGroup"), StringLength(1024)]
        public virtual string RequesterPatronGroup { get; set; }

        [Column("proxy_first_name"), Display(Name = "Proxy First Name", Order = 32), JsonProperty("proxy.firstName"), StringLength(1024)]
        public virtual string ProxyFirstName { get; set; }

        [Column("proxy_last_name"), Display(Name = "Proxy Last Name", Order = 33), JsonProperty("proxy.lastName"), StringLength(1024)]
        public virtual string ProxyLastName { get; set; }

        [Column("proxy_middle_name"), Display(Name = "Proxy Middle Name", Order = 34), JsonProperty("proxy.middleName"), StringLength(1024)]
        public virtual string ProxyMiddleName { get; set; }

        [Column("proxy_barcode"), Display(Name = "Proxy Barcode", Order = 35), JsonProperty("proxy.barcode"), StringLength(1024)]
        public virtual string ProxyBarcode { get; set; }

        [Column("proxy_patron_group"), Display(Name = "Proxy Patron Group", Order = 36), JsonProperty("proxy.patronGroup"), StringLength(1024)]
        public virtual string ProxyPatronGroup { get; set; }

        [Column("fulfillment_preference"), Display(Name = "Fulfillment Preference", Order = 37), JsonProperty("fulfillmentPreference"), RegularExpression(@"^(Hold Shelf|Delivery)$"), StringLength(1024)]
        public virtual string FulfillmentPreference { get; set; }

        [Display(Name = "Delivery Address Type", Order = 38)]
        public virtual AddressType2 DeliveryAddressType { get; set; }

        [Column("delivery_address_type_id"), Display(Name = "Delivery Address Type", Order = 39), JsonProperty("deliveryAddressTypeId")]
        public virtual Guid? DeliveryAddressTypeId { get; set; }

        [Column("request_expiration_date"), DataType(DataType.Date), Display(Name = "Request Expiration Date", Order = 40), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("requestExpirationDate")]
        public virtual DateTime? RequestExpirationDate { get; set; }

        [Column("hold_shelf_expiration_date"), DataType(DataType.Date), Display(Name = "Hold Shelf Expiration Date", Order = 41), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("holdShelfExpirationDate")]
        public virtual DateTime? HoldShelfExpirationDate { get; set; }

        [Display(Name = "Pickup Service Point", Order = 42)]
        public virtual ServicePoint2 PickupServicePoint { get; set; }

        [Column("pickup_service_point_id"), Display(Name = "Pickup Service Point", Order = 43), JsonProperty("pickupServicePointId")]
        public virtual Guid? PickupServicePointId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 44), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 45), InverseProperty("Request2s2")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 46), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 48), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 49), InverseProperty("Request2s5")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 50), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("print_details_print_count"), Display(Name = "Print Details Print Count", Order = 52), JsonProperty("printDetails.printCount")]
        public virtual int? PrintDetailsPrintCount { get; set; }

        [Display(Name = "Print Details Requester", Order = 53), InverseProperty("Request2s")]
        public virtual User2 PrintDetailsRequester { get; set; }

        [Column("print_details_requester_id"), Display(Name = "Print Details Requester", Order = 54), JsonProperty("printDetails.requesterId")]
        public virtual Guid? PrintDetailsRequesterId { get; set; }

        [Column("print_details_is_printed"), Display(Name = "Print Details Is Printed", Order = 55), JsonProperty("printDetails.isPrinted")]
        public virtual bool? PrintDetailsIsPrinted { get; set; }

        [Column("print_details_print_event_date"), DataType(DataType.Date), Display(Name = "Print Details Print Event Date", Order = 56), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("printDetails.printEventDate")]
        public virtual DateTime? PrintDetailsPrintEventDate { get; set; }

        [Column("awaiting_pickup_request_closed_date"), DataType(DataType.Date), Display(Name = "Awaiting Pickup Request Closed Date", Order = 57), DisplayFormat(DataFormatString = "{0:d}"), Editable(false), JsonProperty("awaitingPickupRequestClosedDate")]
        public virtual DateTime? AwaitingPickupRequestClosedDate { get; set; }

        [Column("search_index_call_number_components_call_number"), Display(Name = "Search Index Call Number Components Call Number", Order = 58), JsonProperty("searchIndex.callNumberComponents.callNumber"), StringLength(1024)]
        public virtual string SearchIndexCallNumberComponentsCallNumber { get; set; }

        [Column("search_index_call_number_components_prefix"), Display(Name = "Search Index Call Number Components Prefix", Order = 59), JsonProperty("searchIndex.callNumberComponents.prefix"), StringLength(1024)]
        public virtual string SearchIndexCallNumberComponentsPrefix { get; set; }

        [Column("search_index_call_number_components_suffix"), Display(Name = "Search Index Call Number Components Suffix", Order = 60), JsonProperty("searchIndex.callNumberComponents.suffix"), StringLength(1024)]
        public virtual string SearchIndexCallNumberComponentsSuffix { get; set; }

        [Column("search_index_shelving_order"), Display(Name = "Search Index Shelving Order", Order = 61), JsonProperty("searchIndex.shelvingOrder"), StringLength(1024)]
        public virtual string SearchIndexShelvingOrder { get; set; }

        [Column("search_index_pickup_service_point_name"), Display(Name = "Search Index Pickup Service Point Name", Order = 62), JsonProperty("searchIndex.pickupServicePointName"), StringLength(1024)]
        public virtual string SearchIndexPickupServicePointName { get; set; }

        [Column("item_location_code"), Display(Name = "Item Location Code", Order = 63), JsonProperty("itemLocationCode"), StringLength(1024)]
        public virtual string ItemLocationCode { get; set; }

        [Column("content"), CustomValidation(typeof(Request), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 64), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Request Identifiers", Order = 65), JsonProperty("instance.identifiers")]
        public virtual ICollection<RequestIdentifier> RequestIdentifiers { get; set; }

        [Display(Name = "Request Notes", Order = 66)]
        public virtual ICollection<RequestNote> RequestNotes { get; set; }

        [Display(Name = "Request Tags", Order = 67), JsonConverter(typeof(ArrayJsonConverter<List<RequestTag>, RequestTag>), "Content"), JsonProperty("tags.tagList")]
        public virtual ICollection<RequestTag> RequestTags { get; set; }

        [Display(Name = "Scheduled Notices", Order = 68)]
        public virtual ICollection<ScheduledNotice2> ScheduledNotice2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(RequestLevel)} = {RequestLevel}, {nameof(RequestType)} = {RequestType}, {nameof(EcsRequestPhase)} = {EcsRequestPhase}, {nameof(RequestDate)} = {RequestDate}, {nameof(PatronComments)} = {PatronComments}, {nameof(RequesterId)} = {RequesterId}, {nameof(ProxyUserId)} = {ProxyUserId}, {nameof(InstanceId)} = {InstanceId}, {nameof(HoldingId)} = {HoldingId}, {nameof(ItemId)} = {ItemId}, {nameof(Status)} = {Status}, {nameof(CancellationReasonId)} = {CancellationReasonId}, {nameof(CancelledByUserId)} = {CancelledByUserId}, {nameof(CancellationAdditionalInformation)} = {CancellationAdditionalInformation}, {nameof(CancelledDate)} = {CancelledDate}, {nameof(Position)} = {Position}, {nameof(InstanceTitle)} = {InstanceTitle}, {nameof(ItemBarcode)} = {ItemBarcode}, {nameof(RequesterFirstName)} = {RequesterFirstName}, {nameof(RequesterLastName)} = {RequesterLastName}, {nameof(RequesterMiddleName)} = {RequesterMiddleName}, {nameof(RequesterBarcode)} = {RequesterBarcode}, {nameof(RequesterPatronGroup)} = {RequesterPatronGroup}, {nameof(ProxyFirstName)} = {ProxyFirstName}, {nameof(ProxyLastName)} = {ProxyLastName}, {nameof(ProxyMiddleName)} = {ProxyMiddleName}, {nameof(ProxyBarcode)} = {ProxyBarcode}, {nameof(ProxyPatronGroup)} = {ProxyPatronGroup}, {nameof(FulfillmentPreference)} = {FulfillmentPreference}, {nameof(DeliveryAddressTypeId)} = {DeliveryAddressTypeId}, {nameof(RequestExpirationDate)} = {RequestExpirationDate}, {nameof(HoldShelfExpirationDate)} = {HoldShelfExpirationDate}, {nameof(PickupServicePointId)} = {PickupServicePointId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(PrintDetailsPrintCount)} = {PrintDetailsPrintCount}, {nameof(PrintDetailsRequesterId)} = {PrintDetailsRequesterId}, {nameof(PrintDetailsIsPrinted)} = {PrintDetailsIsPrinted}, {nameof(PrintDetailsPrintEventDate)} = {PrintDetailsPrintEventDate}, {nameof(AwaitingPickupRequestClosedDate)} = {AwaitingPickupRequestClosedDate}, {nameof(SearchIndexCallNumberComponentsCallNumber)} = {SearchIndexCallNumberComponentsCallNumber}, {nameof(SearchIndexCallNumberComponentsPrefix)} = {SearchIndexCallNumberComponentsPrefix}, {nameof(SearchIndexCallNumberComponentsSuffix)} = {SearchIndexCallNumberComponentsSuffix}, {nameof(SearchIndexShelvingOrder)} = {SearchIndexShelvingOrder}, {nameof(SearchIndexPickupServicePointName)} = {SearchIndexPickupServicePointName}, {nameof(ItemLocationCode)} = {ItemLocationCode}, {nameof(Content)} = {Content}, {nameof(RequestIdentifiers)} = {(RequestIdentifiers != null ? $"{{ {string.Join(", ", RequestIdentifiers)} }}" : "")}, {nameof(RequestTags)} = {(RequestTags != null ? $"{{ {string.Join(", ", RequestTags)} }}" : "")} }}";

        public static Request2 FromJObject(JObject jObject) => jObject != null ? new Request2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            RequestLevel = (string)jObject.SelectToken("requestLevel"),
            RequestType = (string)jObject.SelectToken("requestType"),
            EcsRequestPhase = (string)jObject.SelectToken("ecsRequestPhase"),
            RequestDate = ((DateTime?)jObject.SelectToken("requestDate"))?.ToUniversalTime(),
            PatronComments = (string)jObject.SelectToken("patronComments"),
            RequesterId = (Guid?)jObject.SelectToken("requesterId"),
            ProxyUserId = (Guid?)jObject.SelectToken("proxyUserId"),
            InstanceId = (Guid?)jObject.SelectToken("instanceId"),
            HoldingId = (Guid?)jObject.SelectToken("holdingsRecordId"),
            ItemId = (Guid?)jObject.SelectToken("itemId"),
            Status = (string)jObject.SelectToken("status"),
            CancellationReasonId = (Guid?)jObject.SelectToken("cancellationReasonId"),
            CancelledByUserId = (Guid?)jObject.SelectToken("cancelledByUserId"),
            CancellationAdditionalInformation = (string)jObject.SelectToken("cancellationAdditionalInformation"),
            CancelledDate = ((DateTime?)jObject.SelectToken("cancelledDate"))?.ToUniversalTime(),
            Position = (int?)jObject.SelectToken("position"),
            InstanceTitle = (string)jObject.SelectToken("instance.title"),
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
            FulfillmentPreference = (string)jObject.SelectToken("fulfillmentPreference"),
            DeliveryAddressTypeId = (Guid?)jObject.SelectToken("deliveryAddressTypeId"),
            RequestExpirationDate = ((DateTime?)jObject.SelectToken("requestExpirationDate"))?.ToUniversalTime(),
            HoldShelfExpirationDate = ((DateTime?)jObject.SelectToken("holdShelfExpirationDate"))?.ToUniversalTime(),
            PickupServicePointId = (Guid?)jObject.SelectToken("pickupServicePointId"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            PrintDetailsPrintCount = (int?)jObject.SelectToken("printDetails.printCount"),
            PrintDetailsRequesterId = (Guid?)jObject.SelectToken("printDetails.requesterId"),
            PrintDetailsIsPrinted = (bool?)jObject.SelectToken("printDetails.isPrinted"),
            PrintDetailsPrintEventDate = ((DateTime?)jObject.SelectToken("printDetails.printEventDate"))?.ToUniversalTime(),
            AwaitingPickupRequestClosedDate = ((DateTime?)jObject.SelectToken("awaitingPickupRequestClosedDate"))?.ToUniversalTime(),
            SearchIndexCallNumberComponentsCallNumber = (string)jObject.SelectToken("searchIndex.callNumberComponents.callNumber"),
            SearchIndexCallNumberComponentsPrefix = (string)jObject.SelectToken("searchIndex.callNumberComponents.prefix"),
            SearchIndexCallNumberComponentsSuffix = (string)jObject.SelectToken("searchIndex.callNumberComponents.suffix"),
            SearchIndexShelvingOrder = (string)jObject.SelectToken("searchIndex.shelvingOrder"),
            SearchIndexPickupServicePointName = (string)jObject.SelectToken("searchIndex.pickupServicePointName"),
            ItemLocationCode = (string)jObject.SelectToken("itemLocationCode"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            RequestIdentifiers = jObject.SelectToken("instance.identifiers")?.Where(jt => jt.HasValues).Select(jt => RequestIdentifier.FromJObject((JObject)jt)).ToArray(),
            RequestTags = jObject.SelectToken("tags.tagList")?.Select(jt => RequestTag.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("requestLevel", RequestLevel),
            new JProperty("requestType", RequestType),
            new JProperty("ecsRequestPhase", EcsRequestPhase),
            new JProperty("requestDate", RequestDate?.ToLocalTime()),
            new JProperty("patronComments", PatronComments),
            new JProperty("requesterId", RequesterId),
            new JProperty("proxyUserId", ProxyUserId),
            new JProperty("instanceId", InstanceId),
            new JProperty("holdingsRecordId", HoldingId),
            new JProperty("itemId", ItemId),
            new JProperty("status", Status),
            new JProperty("cancellationReasonId", CancellationReasonId),
            new JProperty("cancelledByUserId", CancelledByUserId),
            new JProperty("cancellationAdditionalInformation", CancellationAdditionalInformation),
            new JProperty("cancelledDate", CancelledDate?.ToLocalTime()),
            new JProperty("position", Position),
            new JProperty("instance", new JObject(
                new JProperty("title", InstanceTitle),
                new JProperty("identifiers", RequestIdentifiers?.Select(ri => ri.ToJObject())))),
            new JProperty("item", new JObject(
                new JProperty("barcode", ItemBarcode))),
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
            new JProperty("fulfillmentPreference", FulfillmentPreference),
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
            new JProperty("printDetails", new JObject(
                new JProperty("printCount", PrintDetailsPrintCount),
                new JProperty("requesterId", PrintDetailsRequesterId),
                new JProperty("isPrinted", PrintDetailsIsPrinted),
                new JProperty("printEventDate", PrintDetailsPrintEventDate?.ToLocalTime()))),
            new JProperty("awaitingPickupRequestClosedDate", AwaitingPickupRequestClosedDate?.ToLocalTime()),
            new JProperty("searchIndex", new JObject(
                new JProperty("callNumberComponents", new JObject(
                    new JProperty("callNumber", SearchIndexCallNumberComponentsCallNumber),
                    new JProperty("prefix", SearchIndexCallNumberComponentsPrefix),
                    new JProperty("suffix", SearchIndexCallNumberComponentsSuffix))),
                new JProperty("shelvingOrder", SearchIndexShelvingOrder),
                new JProperty("pickupServicePointName", SearchIndexPickupServicePointName))),
            new JProperty("itemLocationCode", ItemLocationCode),
            new JProperty("tags", new JObject(
                new JProperty("tagList", RequestTags?.Select(rt => rt.ToJObject()))))).RemoveNullAndEmptyProperties();
    }
}
