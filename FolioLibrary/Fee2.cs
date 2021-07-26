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
    // uc.fees -> diku_mod_feesfines.accounts
    // Fee2 -> Fee
    [DisplayColumn(nameof(Title)), DisplayName("Fees"), JsonConverter(typeof(JsonPathJsonConverter<Fee2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("fees", Schema = "uc")]
    public partial class Fee2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Fee.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("amount"), DataType(DataType.Currency), Display(Order = 2), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("amount")]
        public virtual decimal? Amount { get; set; }

        [Column("remaining"), DataType(DataType.Currency), Display(Name = "Remaining Amount", Order = 3), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("remaining")]
        public virtual decimal? RemainingAmount { get; set; }

        [Column("date_created"), JsonProperty("dateCreated"), ScaffoldColumn(false)]
        public virtual DateTime? DateCreated { get; set; }

        [Column("date_updated"), JsonProperty("dateUpdated"), ScaffoldColumn(false)]
        public virtual DateTime? DateUpdated { get; set; }

        [Column("status_name"), Display(Name = "Status Name", Order = 6), JsonProperty("status.name"), Required, StringLength(1024)]
        public virtual string StatusName { get; set; }

        [Column("payment_status_name"), Display(Name = "Payment Status Name", Order = 7), JsonProperty("paymentStatus.name"), Required, StringLength(1024)]
        public virtual string PaymentStatusName { get; set; }

        [Column("fee_fine_type"), JsonProperty("feeFineType"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string FeeFineType { get; set; }

        [Column("fee_fine_owner"), JsonProperty("feeFineOwner"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string FeeFineOwner { get; set; }

        [Column("title"), Display(Order = 10), JsonProperty("title"), StringLength(1024)]
        public virtual string Title { get; set; }

        [Column("call_number"), Display(Name = "Call Number", Order = 11), JsonProperty("callNumber"), StringLength(1024)]
        public virtual string CallNumber { get; set; }

        [Column("barcode"), Display(Order = 12), JsonProperty("barcode"), StringLength(1024)]
        public virtual string Barcode { get; set; }

        [Column("material_type"), Display(Name = "Material Type", Order = 13), JsonProperty("materialType"), StringLength(1024)]
        public virtual string MaterialType { get; set; }

        [Column("item_status_name"), Display(Name = "Item Status Name", Order = 14), JsonProperty("itemStatus.name"), Required, StringLength(1024)]
        public virtual string ItemStatusName { get; set; }

        [Column("location"), Display(Order = 15), JsonProperty("location"), StringLength(1024)]
        public virtual string Location { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 16), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 17), InverseProperty("Fee2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 18), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 20), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 21), InverseProperty("Fee2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 22), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("due_date"), DataType(DataType.DateTime), Display(Name = "Due Time", Order = 24), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("dueDate")]
        public virtual DateTime? DueTime { get; set; }

        [Column("returned_date"), DataType(DataType.DateTime), Display(Name = "Returned Time", Order = 25), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("returnedDate")]
        public virtual DateTime? ReturnedTime { get; set; }

        [Display(Order = 26)]
        public virtual Loan2 Loan { get; set; }

        [Column("loan_id"), Display(Name = "Loan", Order = 27), JsonProperty("loanId")]
        public virtual Guid? LoanId { get; set; }

        [Display(Order = 28), InverseProperty("Fee2s2")]
        public virtual User2 User { get; set; }

        [Column("user_id"), Display(Name = "User", Order = 29), JsonProperty("userId"), Required]
        public virtual Guid? UserId { get; set; }

        [Display(Order = 30)]
        public virtual Item2 Item { get; set; }

        [Column("item_id"), Display(Name = "Item", Order = 31), JsonProperty("itemId"), Required]
        public virtual Guid? ItemId { get; set; }

        [Display(Name = "Material Type 1", Order = 32)]
        public virtual MaterialType2 MaterialType1 { get; set; }

        [Column("material_type_id"), Display(Name = "Material Type 1", Order = 33), ForeignKey("MaterialType1"), JsonProperty("materialTypeId"), Required]
        public virtual Guid? MaterialTypeId { get; set; }

        [Display(Name = "Fee Type", Order = 34)]
        public virtual FeeType2 FeeType { get; set; }

        [Column("fee_type_id"), Display(Name = "Fee Type", Order = 35), JsonProperty("feeFineId"), Required]
        public virtual Guid? FeeTypeId { get; set; }

        [Display(Order = 36)]
        public virtual Owner2 Owner { get; set; }

        [Column("owner_id"), Display(Name = "Owner", Order = 37), JsonProperty("ownerId"), Required]
        public virtual Guid? OwnerId { get; set; }

        [Display(Order = 38)]
        public virtual Holding2 Holding { get; set; }

        [Column("holding_id"), Display(Name = "Holding", Order = 39), JsonProperty("holdingsRecordId")]
        public virtual Guid? HoldingId { get; set; }

        [Display(Order = 40)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id"), Display(Name = "Instance", Order = 41), JsonProperty("instanceId")]
        public virtual Guid? InstanceId { get; set; }

        [Column("content"), CustomValidation(typeof(Fee), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 42), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Payments", Order = 43)]
        public virtual ICollection<Payment2> Payment2s { get; set; }

        [Display(Name = "Refund Reasons", Order = 44)]
        public virtual ICollection<RefundReason2> RefundReason2s { get; set; }

        [Display(Name = "User Summary Open Fees Fines", Order = 45)]
        public virtual ICollection<UserSummaryOpenFeesFine> UserSummaryOpenFeesFines { get; set; }

        [Display(Name = "Waive Reasons", Order = 46)]
        public virtual ICollection<WaiveReason2> WaiveReason2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Amount)} = {Amount}, {nameof(RemainingAmount)} = {RemainingAmount}, {nameof(DateCreated)} = {DateCreated}, {nameof(DateUpdated)} = {DateUpdated}, {nameof(StatusName)} = {StatusName}, {nameof(PaymentStatusName)} = {PaymentStatusName}, {nameof(FeeFineType)} = {FeeFineType}, {nameof(FeeFineOwner)} = {FeeFineOwner}, {nameof(Title)} = {Title}, {nameof(CallNumber)} = {CallNumber}, {nameof(Barcode)} = {Barcode}, {nameof(MaterialType)} = {MaterialType}, {nameof(ItemStatusName)} = {ItemStatusName}, {nameof(Location)} = {Location}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(DueTime)} = {DueTime}, {nameof(ReturnedTime)} = {ReturnedTime}, {nameof(LoanId)} = {LoanId}, {nameof(UserId)} = {UserId}, {nameof(ItemId)} = {ItemId}, {nameof(MaterialTypeId)} = {MaterialTypeId}, {nameof(FeeTypeId)} = {FeeTypeId}, {nameof(OwnerId)} = {OwnerId}, {nameof(HoldingId)} = {HoldingId}, {nameof(InstanceId)} = {InstanceId}, {nameof(Content)} = {Content} }}";

        public static Fee2 FromJObject(JObject jObject) => jObject != null ? new Fee2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Amount = (decimal?)jObject.SelectToken("amount"),
            RemainingAmount = (decimal?)jObject.SelectToken("remaining"),
            DateCreated = (DateTime?)jObject.SelectToken("dateCreated"),
            DateUpdated = (DateTime?)jObject.SelectToken("dateUpdated"),
            StatusName = (string)jObject.SelectToken("status.name"),
            PaymentStatusName = (string)jObject.SelectToken("paymentStatus.name"),
            FeeFineType = (string)jObject.SelectToken("feeFineType"),
            FeeFineOwner = (string)jObject.SelectToken("feeFineOwner"),
            Title = (string)jObject.SelectToken("title"),
            CallNumber = (string)jObject.SelectToken("callNumber"),
            Barcode = (string)jObject.SelectToken("barcode"),
            MaterialType = (string)jObject.SelectToken("materialType"),
            ItemStatusName = (string)jObject.SelectToken("itemStatus.name"),
            Location = (string)jObject.SelectToken("location"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            DueTime = (DateTime?)jObject.SelectToken("dueDate"),
            ReturnedTime = (DateTime?)jObject.SelectToken("returnedDate"),
            LoanId = (string)jObject["loanId"] != "0" ? (Guid?)jObject["loanId"] : null,
            UserId = (Guid?)jObject.SelectToken("userId"),
            ItemId = (string)jObject["itemId"] != "0" ? (Guid?)jObject["itemId"] : null,
            MaterialTypeId = (string)jObject["materialTypeId"] != "0" ? (Guid?)jObject["materialTypeId"] : null,
            FeeTypeId = (Guid?)jObject.SelectToken("feeFineId"),
            OwnerId = (Guid?)jObject.SelectToken("ownerId"),
            HoldingId = (string)jObject["holdingsRecordId"] != "" ? (Guid?)jObject["holdingsRecordId"] : null,
            InstanceId = (string)jObject["instanceId"] != "" ? (Guid?)jObject["instanceId"] : null,
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("amount", Amount),
            new JProperty("remaining", RemainingAmount),
            new JProperty("dateCreated", DateCreated),
            new JProperty("dateUpdated", DateUpdated),
            new JProperty("status", new JObject(
                new JProperty("name", StatusName))),
            new JProperty("paymentStatus", new JObject(
                new JProperty("name", PaymentStatusName))),
            new JProperty("feeFineType", FeeFineType),
            new JProperty("feeFineOwner", FeeFineOwner),
            new JProperty("title", Title),
            new JProperty("callNumber", CallNumber),
            new JProperty("barcode", Barcode),
            new JProperty("materialType", MaterialType),
            new JProperty("itemStatus", new JObject(
                new JProperty("name", ItemStatusName))),
            new JProperty("location", Location),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("dueDate", DueTime),
            new JProperty("returnedDate", ReturnedTime),
            new JProperty("loanId", LoanId),
            new JProperty("userId", UserId),
            new JProperty("itemId", ItemId),
            new JProperty("materialTypeId", MaterialTypeId),
            new JProperty("feeFineId", FeeTypeId),
            new JProperty("ownerId", OwnerId),
            new JProperty("holdingsRecordId", HoldingId),
            new JProperty("instanceId", InstanceId)).RemoveNullAndEmptyProperties();
    }
}
