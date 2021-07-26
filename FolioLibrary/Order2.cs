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
    // uc.orders -> diku_mod_orders_storage.purchase_order
    // Order2 -> Order
    [CustomValidation(typeof(Order2), nameof(ValidateOrder2)), DisplayColumn(nameof(Number)), DisplayName("Orders"), JsonConverter(typeof(JsonPathJsonConverter<Order2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("orders", Schema = "uc")]
    public partial class Order2
    {
        public static ValidationResult ValidateOrder2(Order2 order2, ValidationContext context)
        {
            var fsc = (FolioServiceContext)context.ObjectInstance;
            if (order2.Number != null && fsc.AnyOrder2s($"id <> \"{order2.Id}\" and poNumber == \"{order2.Number}\"")) return new ValidationResult("Number already exists");
            return ValidationResult.Success;
        }

        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Order.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("approved"), Display(Order = 2), JsonProperty("approved")]
        public virtual bool? Approved { get; set; }

        [Display(Name = "Approved By", Order = 3), InverseProperty("Order2s")]
        public virtual User2 ApprovedBy { get; set; }

        [Column("approved_by_id"), Display(Name = "Approved By", Order = 4), JsonProperty("approvedById")]
        public virtual Guid? ApprovedById { get; set; }

        [Column("approval_date"), DataType(DataType.Date), Display(Name = "Approval Date", Order = 5), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("approvalDate")]
        public virtual DateTime? ApprovalDate { get; set; }

        [Display(Name = "Assigned To", Order = 6), InverseProperty("Order2s1")]
        public virtual User2 AssignedTo { get; set; }

        [Column("assigned_to_id"), Display(Name = "Assigned To", Order = 7), JsonProperty("assignedTo")]
        public virtual Guid? AssignedToId { get; set; }

        [Display(Name = "Bill To", Order = 8), InverseProperty("Order2s")]
        public virtual Address BillTo { get; set; }

        [Column("bill_to_id"), Display(Name = "Bill To", Order = 9), JsonProperty("billTo")]
        public virtual Guid? BillToId { get; set; }

        [Column("close_reason_reason"), Display(Name = "Close Reason Reason", Order = 10), JsonProperty("closeReason.reason"), StringLength(1024)]
        public virtual string CloseReasonReason { get; set; }

        [Column("close_reason_note"), Display(Name = "Close Reason Note", Order = 11), JsonProperty("closeReason.note"), StringLength(1024)]
        public virtual string CloseReasonNote { get; set; }

        [Column("date_ordered"), DataType(DataType.Date), Display(Name = "Order Date", Order = 12), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("dateOrdered")]
        public virtual DateTime? OrderDate { get; set; }

        [Column("manual_po"), Display(Order = 13), JsonProperty("manualPo")]
        public virtual bool? Manual { get; set; }

        [Column("po_number"), Display(Order = 14), JsonProperty("poNumber"), Required, StringLength(1024)]
        public virtual string Number { get; set; }

        [Column("po_number_prefix"), JsonProperty("poNumberPrefix"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string NumberPrefix { get; set; }

        [Column("po_number_suffix"), JsonProperty("poNumberSuffix"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string NumberSuffix { get; set; }

        [Column("order_type"), Display(Name = "Order Type", Order = 17), JsonProperty("orderType"), RegularExpression(@"^(One-Time|Ongoing)$"), StringLength(1024)]
        public virtual string OrderType { get; set; }

        [Column("re_encumber"), Display(Order = 18), JsonProperty("reEncumber")]
        public virtual bool? Reencumber { get; set; }

        [Column("ongoing_interval"), Display(Name = "Ongoing Interval", Order = 19), JsonProperty("ongoing.interval")]
        public virtual int? OngoingInterval { get; set; }

        [Column("ongoing_is_subscription"), Display(Name = "Ongoing Is Subscription", Order = 20), JsonProperty("ongoing.isSubscription")]
        public virtual bool? OngoingIsSubscription { get; set; }

        [Column("ongoing_manual_renewal"), Display(Name = "Ongoing Manual Renewal", Order = 21), JsonProperty("ongoing.manualRenewal")]
        public virtual bool? OngoingManualRenewal { get; set; }

        [Column("ongoing_notes"), Display(Name = "Ongoing Notes", Order = 22), JsonProperty("ongoing.notes"), StringLength(1024)]
        public virtual string OngoingNotes { get; set; }

        [Column("ongoing_review_period"), Display(Name = "Ongoing Review Period", Order = 23), JsonProperty("ongoing.reviewPeriod")]
        public virtual int? OngoingReviewPeriod { get; set; }

        [Column("ongoing_renewal_date"), DataType(DataType.Date), Display(Name = "Ongoing Renewal Date", Order = 24), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("ongoing.renewalDate")]
        public virtual DateTime? OngoingRenewalDate { get; set; }

        [Column("ongoing_review_date"), DataType(DataType.Date), Display(Name = "Ongoing Review Date", Order = 25), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("ongoing.reviewDate")]
        public virtual DateTime? OngoingReviewDate { get; set; }

        [Display(Name = "Ship To", Order = 26), InverseProperty("Order2s1")]
        public virtual Address ShipTo { get; set; }

        [Column("ship_to_id"), Display(Name = "Ship To", Order = 27), JsonProperty("shipTo")]
        public virtual Guid? ShipToId { get; set; }

        [Display(Order = 28)]
        public virtual OrderTemplate2 Template { get; set; }

        [Column("template_id"), Display(Name = "Template", Order = 29), JsonProperty("template")]
        public virtual Guid? TemplateId { get; set; }

        [Display(Order = 30)]
        public virtual Organization2 Vendor { get; set; }

        [Column("vendor_id"), Display(Name = "Vendor", Order = 31), JsonProperty("vendor")]
        public virtual Guid? VendorId { get; set; }

        [Column("status"), Display(Order = 32), JsonProperty("workflowStatus"), RegularExpression(@"^(Pending|Open|Closed)$"), StringLength(1024)]
        public virtual string Status { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 33), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 34), InverseProperty("Order2s2")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 35), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 37), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 38), InverseProperty("Order2s3")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 39), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Order), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 41), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Order Acquisitions Units", Order = 42), JsonConverter(typeof(ArrayJsonConverter<List<OrderAcquisitionsUnit>, OrderAcquisitionsUnit>), "AcquisitionsUnitId"), JsonProperty("acqUnitIds")]
        public virtual ICollection<OrderAcquisitionsUnit> OrderAcquisitionsUnits { get; set; }

        [Display(Name = "Order Invoices", Order = 43)]
        public virtual ICollection<OrderInvoice2> OrderInvoice2s { get; set; }

        [Display(Name = "Order Items", Order = 44)]
        public virtual ICollection<OrderItem2> OrderItem2s { get; set; }

        [Display(Name = "Order Notes", Order = 45), JsonConverter(typeof(ArrayJsonConverter<List<OrderNote>, OrderNote>), "Content"), JsonProperty("notes")]
        public virtual ICollection<OrderNote> OrderNotes { get; set; }

        [Display(Name = "Order Tags", Order = 46), JsonConverter(typeof(ArrayJsonConverter<List<OrderTag>, OrderTag>), "Content"), JsonProperty("tags.tagList")]
        public virtual ICollection<OrderTag> OrderTags { get; set; }

        [Display(Name = "Order Transaction Summary", Order = 47)]
        public virtual OrderTransactionSummary2 OrderTransactionSummary2 { get; set; }

        [Display(Name = "Transactions", Order = 48)]
        public virtual ICollection<Transaction2> Transaction2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Approved)} = {Approved}, {nameof(ApprovedById)} = {ApprovedById}, {nameof(ApprovalDate)} = {ApprovalDate}, {nameof(AssignedToId)} = {AssignedToId}, {nameof(BillToId)} = {BillToId}, {nameof(CloseReasonReason)} = {CloseReasonReason}, {nameof(CloseReasonNote)} = {CloseReasonNote}, {nameof(OrderDate)} = {OrderDate}, {nameof(Manual)} = {Manual}, {nameof(Number)} = {Number}, {nameof(NumberPrefix)} = {NumberPrefix}, {nameof(NumberSuffix)} = {NumberSuffix}, {nameof(OrderType)} = {OrderType}, {nameof(Reencumber)} = {Reencumber}, {nameof(OngoingInterval)} = {OngoingInterval}, {nameof(OngoingIsSubscription)} = {OngoingIsSubscription}, {nameof(OngoingManualRenewal)} = {OngoingManualRenewal}, {nameof(OngoingNotes)} = {OngoingNotes}, {nameof(OngoingReviewPeriod)} = {OngoingReviewPeriod}, {nameof(OngoingRenewalDate)} = {OngoingRenewalDate}, {nameof(OngoingReviewDate)} = {OngoingReviewDate}, {nameof(ShipToId)} = {ShipToId}, {nameof(TemplateId)} = {TemplateId}, {nameof(VendorId)} = {VendorId}, {nameof(Status)} = {Status}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(OrderAcquisitionsUnits)} = {(OrderAcquisitionsUnits != null ? $"{{ {string.Join(", ", OrderAcquisitionsUnits)} }}" : "")}, {nameof(OrderNotes)} = {(OrderNotes != null ? $"{{ {string.Join(", ", OrderNotes)} }}" : "")}, {nameof(OrderTags)} = {(OrderTags != null ? $"{{ {string.Join(", ", OrderTags)} }}" : "")} }}";

        public static Order2 FromJObject(JObject jObject) => jObject != null ? new Order2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Approved = (bool?)jObject.SelectToken("approved"),
            ApprovedById = (Guid?)jObject.SelectToken("approvedById"),
            ApprovalDate = (DateTime?)jObject.SelectToken("approvalDate"),
            AssignedToId = (Guid?)jObject.SelectToken("assignedTo"),
            BillToId = (Guid?)jObject.SelectToken("billTo"),
            CloseReasonReason = (string)jObject.SelectToken("closeReason.reason"),
            CloseReasonNote = (string)jObject.SelectToken("closeReason.note"),
            OrderDate = (DateTime?)jObject.SelectToken("dateOrdered"),
            Manual = (bool?)jObject.SelectToken("manualPo"),
            Number = (string)jObject.SelectToken("poNumber"),
            NumberPrefix = (string)jObject.SelectToken("poNumberPrefix"),
            NumberSuffix = (string)jObject.SelectToken("poNumberSuffix"),
            OrderType = (string)jObject.SelectToken("orderType"),
            Reencumber = (bool?)jObject.SelectToken("reEncumber"),
            OngoingInterval = (int?)jObject.SelectToken("ongoing.interval"),
            OngoingIsSubscription = (bool?)jObject.SelectToken("ongoing.isSubscription"),
            OngoingManualRenewal = (bool?)jObject.SelectToken("ongoing.manualRenewal"),
            OngoingNotes = (string)jObject.SelectToken("ongoing.notes"),
            OngoingReviewPeriod = (int?)jObject.SelectToken("ongoing.reviewPeriod"),
            OngoingRenewalDate = (DateTime?)jObject.SelectToken("ongoing.renewalDate"),
            OngoingReviewDate = (DateTime?)jObject.SelectToken("ongoing.reviewDate"),
            ShipToId = (Guid?)jObject.SelectToken("shipTo"),
            TemplateId = (Guid?)jObject.SelectToken("template"),
            VendorId = (Guid?)jObject.SelectToken("vendor"),
            Status = (string)jObject.SelectToken("workflowStatus"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            OrderAcquisitionsUnits = jObject.SelectToken("acqUnitIds")?.Where(jt => jt.HasValues).Select(jt => OrderAcquisitionsUnit.FromJObject((JValue)jt)).ToArray(),
            OrderNotes = jObject.SelectToken("notes")?.Where(jt => jt.HasValues).Select(jt => OrderNote.FromJObject((JValue)jt)).ToArray(),
            OrderTags = jObject.SelectToken("tags.tagList")?.Where(jt => jt.HasValues).Select(jt => OrderTag.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("approved", Approved),
            new JProperty("approvedById", ApprovedById),
            new JProperty("approvalDate", ApprovalDate),
            new JProperty("assignedTo", AssignedToId),
            new JProperty("billTo", BillToId),
            new JProperty("closeReason", new JObject(
                new JProperty("reason", CloseReasonReason),
                new JProperty("note", CloseReasonNote))),
            new JProperty("dateOrdered", OrderDate),
            new JProperty("manualPo", Manual),
            new JProperty("poNumber", Number),
            new JProperty("poNumberPrefix", NumberPrefix),
            new JProperty("poNumberSuffix", NumberSuffix),
            new JProperty("orderType", OrderType),
            new JProperty("reEncumber", Reencumber),
            new JProperty("ongoing", new JObject(
                new JProperty("interval", OngoingInterval),
                new JProperty("isSubscription", OngoingIsSubscription),
                new JProperty("manualRenewal", OngoingManualRenewal),
                new JProperty("notes", OngoingNotes),
                new JProperty("reviewPeriod", OngoingReviewPeriod),
                new JProperty("renewalDate", OngoingRenewalDate),
                new JProperty("reviewDate", OngoingReviewDate))),
            new JProperty("shipTo", ShipToId),
            new JProperty("template", TemplateId),
            new JProperty("vendor", VendorId),
            new JProperty("workflowStatus", Status),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("acqUnitIds", OrderAcquisitionsUnits?.Select(oau => oau.ToJObject())),
            new JProperty("notes", OrderNotes?.Select(@on => @on.ToJObject())),
            new JProperty("tags", new JObject(
                new JProperty("tagList", OrderTags?.Select(ot => ot.ToJObject()))))).RemoveNullAndEmptyProperties();
    }
}
