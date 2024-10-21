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
    // uc.actual_cost_records -> uchicago_mod_circulation_storage.actual_cost_record
    // ActualCostRecord2 -> ActualCostRecord
    [DisplayColumn(nameof(Id)), DisplayName("Actual Cost Records"), JsonConverter(typeof(JsonPathJsonConverter<ActualCostRecord2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("actual_cost_records", Schema = "uc")]
    public partial class ActualCostRecord2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.ActualCostRecord.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("loss_type"), Display(Name = "Loss Type", Order = 2), JsonProperty("lossType"), RegularExpression(@"^(Aged to lost|Declared lost)$"), StringLength(1024)]
        public virtual string LossType { get; set; }

        [Column("loss_date"), DataType(DataType.Date), Display(Name = "Loss Date", Order = 3), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("lossDate")]
        public virtual DateTime? LossDate { get; set; }

        [Column("expiration_date"), DataType(DataType.Date), Display(Name = "Expiration Date", Order = 4), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("expirationDate")]
        public virtual DateTime? ExpirationDate { get; set; }

        [Column("user_barcode"), Display(Name = "User Barcode", Order = 5), JsonProperty("user.barcode"), StringLength(1024)]
        public virtual string UserBarcode { get; set; }

        [Column("user_first_name"), Display(Name = "User First Name", Order = 6), JsonProperty("user.firstName"), StringLength(1024)]
        public virtual string UserFirstName { get; set; }

        [Column("user_last_name"), Display(Name = "User Last Name", Order = 7), JsonProperty("user.lastName"), StringLength(1024)]
        public virtual string UserLastName { get; set; }

        [Column("user_middle_name"), Display(Name = "User Middle Name", Order = 8), JsonProperty("user.middleName"), StringLength(1024)]
        public virtual string UserMiddleName { get; set; }

        [Display(Name = "User Group", Order = 9)]
        public virtual Group2 UserGroup { get; set; }

        [Column("user_patron_group_id"), Display(Name = "User Group", Order = 10), JsonProperty("user.patronGroupId")]
        public virtual Guid? UserGroupId { get; set; }

        [Column("user_patron_group"), Display(Name = "User Group Name", Order = 11), JsonProperty("user.patronGroup"), StringLength(1024)]
        public virtual string UserGroupName { get; set; }

        [Column("item_barcode"), Display(Name = "Item Barcode", Order = 12), JsonProperty("item.barcode"), StringLength(1024)]
        public virtual string ItemBarcode { get; set; }

        [Display(Name = "Item Material Type", Order = 13)]
        public virtual MaterialType2 ItemMaterialType { get; set; }

        [Column("item_material_type_id"), Display(Name = "Item Material Type", Order = 14), JsonProperty("item.materialTypeId")]
        public virtual Guid? ItemMaterialTypeId { get; set; }

        [Column("item_material_type"), Display(Name = "Item Material Type Name", Order = 15), JsonProperty("item.materialType"), StringLength(1024)]
        public virtual string ItemMaterialTypeName { get; set; }

        [Display(Name = "Item Permanent Location", Order = 16), InverseProperty("ActualCostRecord2s1")]
        public virtual Location2 ItemPermanentLocation { get; set; }

        [Column("item_permanent_location_id"), Display(Name = "Item Permanent Location", Order = 17), JsonProperty("item.permanentLocationId")]
        public virtual Guid? ItemPermanentLocationId { get; set; }

        [Column("item_permanent_location"), Display(Name = "Item Permanent Location Name", Order = 18), JsonProperty("item.permanentLocation"), StringLength(1024)]
        public virtual string ItemPermanentLocationName { get; set; }

        [Display(Name = "Item Effective Location", Order = 19), InverseProperty("ActualCostRecord2s")]
        public virtual Location2 ItemEffectiveLocation { get; set; }

        [Column("item_effective_location_id"), Display(Name = "Item Effective Location", Order = 20), JsonProperty("item.effectiveLocationId")]
        public virtual Guid? ItemEffectiveLocationId { get; set; }

        [Column("item_effective_location"), Display(Name = "Item Effective Location Name", Order = 21), JsonProperty("item.effectiveLocation"), StringLength(1024)]
        public virtual string ItemEffectiveLocationName { get; set; }

        [Display(Name = "Item Loan Type", Order = 22)]
        public virtual LoanType2 ItemLoanType { get; set; }

        [Column("item_loan_type_id"), Display(Name = "Item Loan Type", Order = 23), JsonProperty("item.loanTypeId")]
        public virtual Guid? ItemLoanTypeId { get; set; }

        [Column("item_loan_type"), Display(Name = "Item Loan Type Name", Order = 24), JsonProperty("item.loanType"), StringLength(1024)]
        public virtual string ItemLoanTypeName { get; set; }

        [Display(Name = "Item Holding", Order = 25)]
        public virtual Holding2 ItemHolding { get; set; }

        [Column("item_holdings_record_id"), Display(Name = "Item Holding", Order = 26), JsonProperty("item.holdingsRecordId")]
        public virtual Guid? ItemHoldingId { get; set; }

        [Column("item_effective_call_number_components_call_number"), Display(Name = "Item Effective Call Number", Order = 27), JsonProperty("item.effectiveCallNumberComponents.callNumber"), StringLength(1024)]
        public virtual string ItemEffectiveCallNumber { get; set; }

        [Column("item_effective_call_number_components_prefix"), Display(Name = "Item Effective Call Number Prefix", Order = 28), JsonProperty("item.effectiveCallNumberComponents.prefix"), StringLength(1024)]
        public virtual string ItemEffectiveCallNumberPrefix { get; set; }

        [Column("item_effective_call_number_components_suffix"), Display(Name = "Item Effective Call Number Suffix", Order = 29), JsonProperty("item.effectiveCallNumberComponents.suffix"), StringLength(1024)]
        public virtual string ItemEffectiveCallNumberSuffix { get; set; }

        [Column("item_volume"), Display(Name = "Item Volume", Order = 30), JsonProperty("item.volume"), StringLength(1024)]
        public virtual string ItemVolume { get; set; }

        [Column("item_enumeration"), Display(Name = "Item Enumeration", Order = 31), JsonProperty("item.enumeration"), StringLength(1024)]
        public virtual string ItemEnumeration { get; set; }

        [Column("item_chronology"), Display(Name = "Item Chronology", Order = 32), JsonProperty("item.chronology"), StringLength(1024)]
        public virtual string ItemChronology { get; set; }

        [Column("item_display_summary"), Display(Name = "Item Display Summary", Order = 33), JsonProperty("item.displaySummary"), StringLength(1024)]
        public virtual string ItemDisplaySummary { get; set; }

        [Column("item_copy_number"), Display(Name = "Item Copy Number", Order = 34), JsonProperty("item.copyNumber"), StringLength(1024)]
        public virtual string ItemCopyNumber { get; set; }

        [Column("instance_title"), Display(Name = "Instance Title", Order = 35), JsonProperty("instance.title"), StringLength(1024)]
        public virtual string InstanceTitle { get; set; }

        [Display(Order = 36)]
        public virtual Fee2 Fee { get; set; }

        [Column("fee_fine_account_id"), Display(Name = "Fee", Order = 37), JsonProperty("feeFine.accountId")]
        public virtual Guid? FeeId { get; set; }

        [Column("fee_fine_billed_amount"), DataType(DataType.Currency), Display(Name = "Fee Billed Amount", Order = 38), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("feeFine.billedAmount")]
        public virtual decimal? FeeBilledAmount { get; set; }

        [Display(Order = 39)]
        public virtual Owner2 Owner { get; set; }

        [Column("fee_fine_owner_id"), Display(Name = "Owner", Order = 40), JsonProperty("feeFine.ownerId")]
        public virtual Guid? OwnerId { get; set; }

        [Column("fee_fine_owner"), Display(Name = "Owner Name", Order = 41), JsonProperty("feeFine.owner"), StringLength(1024)]
        public virtual string OwnerName { get; set; }

        [Display(Name = "Fee Type", Order = 42)]
        public virtual FeeType2 FeeType { get; set; }

        [Column("fee_fine_type_id"), Display(Name = "Fee Type", Order = 43), JsonProperty("feeFine.typeId")]
        public virtual Guid? FeeTypeId { get; set; }

        [Column("fee_fine_type"), Display(Name = "Fee Type Name", Order = 44), JsonProperty("feeFine.type"), StringLength(1024)]
        public virtual string FeeTypeName { get; set; }

        [Column("status"), Display(Order = 45), JsonProperty("status"), RegularExpression(@"^(Open|Billed|Cancelled|Expired)$"), StringLength(1024)]
        public virtual string Status { get; set; }

        [Column("additional_info_for_staff"), Display(Name = "Additional Info For Staff", Order = 46), JsonProperty("additionalInfoForStaff"), StringLength(1024)]
        public virtual string AdditionalInfoForStaff { get; set; }

        [Column("additional_info_for_patron"), Display(Name = "Additional Info For Patron", Order = 47), JsonProperty("additionalInfoForPatron"), StringLength(1024)]
        public virtual string AdditionalInfoForPatron { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 48), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 49), InverseProperty("ActualCostRecord2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 50), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 52), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 53), InverseProperty("ActualCostRecord2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 54), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(ActualCostRecord), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 56), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Actual Cost Record Contributors", Order = 57), JsonProperty("instance.contributors")]
        public virtual ICollection<ActualCostRecordContributor> ActualCostRecordContributors { get; set; }

        [Display(Name = "Actual Cost Record Identifiers", Order = 58), JsonProperty("instance.identifiers")]
        public virtual ICollection<ActualCostRecordIdentifier> ActualCostRecordIdentifiers { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(LossType)} = {LossType}, {nameof(LossDate)} = {LossDate}, {nameof(ExpirationDate)} = {ExpirationDate}, {nameof(UserBarcode)} = {UserBarcode}, {nameof(UserFirstName)} = {UserFirstName}, {nameof(UserLastName)} = {UserLastName}, {nameof(UserMiddleName)} = {UserMiddleName}, {nameof(UserGroupId)} = {UserGroupId}, {nameof(UserGroupName)} = {UserGroupName}, {nameof(ItemBarcode)} = {ItemBarcode}, {nameof(ItemMaterialTypeId)} = {ItemMaterialTypeId}, {nameof(ItemMaterialTypeName)} = {ItemMaterialTypeName}, {nameof(ItemPermanentLocationId)} = {ItemPermanentLocationId}, {nameof(ItemPermanentLocationName)} = {ItemPermanentLocationName}, {nameof(ItemEffectiveLocationId)} = {ItemEffectiveLocationId}, {nameof(ItemEffectiveLocationName)} = {ItemEffectiveLocationName}, {nameof(ItemLoanTypeId)} = {ItemLoanTypeId}, {nameof(ItemLoanTypeName)} = {ItemLoanTypeName}, {nameof(ItemHoldingId)} = {ItemHoldingId}, {nameof(ItemEffectiveCallNumber)} = {ItemEffectiveCallNumber}, {nameof(ItemEffectiveCallNumberPrefix)} = {ItemEffectiveCallNumberPrefix}, {nameof(ItemEffectiveCallNumberSuffix)} = {ItemEffectiveCallNumberSuffix}, {nameof(ItemVolume)} = {ItemVolume}, {nameof(ItemEnumeration)} = {ItemEnumeration}, {nameof(ItemChronology)} = {ItemChronology}, {nameof(ItemDisplaySummary)} = {ItemDisplaySummary}, {nameof(ItemCopyNumber)} = {ItemCopyNumber}, {nameof(InstanceTitle)} = {InstanceTitle}, {nameof(FeeId)} = {FeeId}, {nameof(FeeBilledAmount)} = {FeeBilledAmount}, {nameof(OwnerId)} = {OwnerId}, {nameof(OwnerName)} = {OwnerName}, {nameof(FeeTypeId)} = {FeeTypeId}, {nameof(FeeTypeName)} = {FeeTypeName}, {nameof(Status)} = {Status}, {nameof(AdditionalInfoForStaff)} = {AdditionalInfoForStaff}, {nameof(AdditionalInfoForPatron)} = {AdditionalInfoForPatron}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(ActualCostRecordContributors)} = {(ActualCostRecordContributors != null ? $"{{ {string.Join(", ", ActualCostRecordContributors)} }}" : "")}, {nameof(ActualCostRecordIdentifiers)} = {(ActualCostRecordIdentifiers != null ? $"{{ {string.Join(", ", ActualCostRecordIdentifiers)} }}" : "")} }}";

        public static ActualCostRecord2 FromJObject(JObject jObject) => jObject != null ? new ActualCostRecord2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            LossType = (string)jObject.SelectToken("lossType"),
            LossDate = ((DateTime?)jObject.SelectToken("lossDate"))?.ToUniversalTime(),
            ExpirationDate = ((DateTime?)jObject.SelectToken("expirationDate"))?.ToUniversalTime(),
            UserBarcode = (string)jObject.SelectToken("user.barcode"),
            UserFirstName = (string)jObject.SelectToken("user.firstName"),
            UserLastName = (string)jObject.SelectToken("user.lastName"),
            UserMiddleName = (string)jObject.SelectToken("user.middleName"),
            UserGroupId = (Guid?)jObject.SelectToken("user.patronGroupId"),
            UserGroupName = (string)jObject.SelectToken("user.patronGroup"),
            ItemBarcode = (string)jObject.SelectToken("item.barcode"),
            ItemMaterialTypeId = (Guid?)jObject.SelectToken("item.materialTypeId"),
            ItemMaterialTypeName = (string)jObject.SelectToken("item.materialType"),
            ItemPermanentLocationId = (Guid?)jObject.SelectToken("item.permanentLocationId"),
            ItemPermanentLocationName = (string)jObject.SelectToken("item.permanentLocation"),
            ItemEffectiveLocationId = (Guid?)jObject.SelectToken("item.effectiveLocationId"),
            ItemEffectiveLocationName = (string)jObject.SelectToken("item.effectiveLocation"),
            ItemLoanTypeId = (Guid?)jObject.SelectToken("item.loanTypeId"),
            ItemLoanTypeName = (string)jObject.SelectToken("item.loanType"),
            ItemHoldingId = (Guid?)jObject.SelectToken("item.holdingsRecordId"),
            ItemEffectiveCallNumber = (string)jObject.SelectToken("item.effectiveCallNumberComponents.callNumber"),
            ItemEffectiveCallNumberPrefix = (string)jObject.SelectToken("item.effectiveCallNumberComponents.prefix"),
            ItemEffectiveCallNumberSuffix = (string)jObject.SelectToken("item.effectiveCallNumberComponents.suffix"),
            ItemVolume = (string)jObject.SelectToken("item.volume"),
            ItemEnumeration = (string)jObject.SelectToken("item.enumeration"),
            ItemChronology = (string)jObject.SelectToken("item.chronology"),
            ItemDisplaySummary = (string)jObject.SelectToken("item.displaySummary"),
            ItemCopyNumber = (string)jObject.SelectToken("item.copyNumber"),
            InstanceTitle = (string)jObject.SelectToken("instance.title"),
            FeeId = (Guid?)jObject.SelectToken("feeFine.accountId"),
            FeeBilledAmount = (decimal?)jObject.SelectToken("feeFine.billedAmount"),
            OwnerId = (Guid?)jObject.SelectToken("feeFine.ownerId"),
            OwnerName = (string)jObject.SelectToken("feeFine.owner"),
            FeeTypeId = (Guid?)jObject.SelectToken("feeFine.typeId"),
            FeeTypeName = (string)jObject.SelectToken("feeFine.type"),
            Status = (string)jObject.SelectToken("status"),
            AdditionalInfoForStaff = (string)jObject.SelectToken("additionalInfoForStaff"),
            AdditionalInfoForPatron = (string)jObject.SelectToken("additionalInfoForPatron"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            ActualCostRecordContributors = jObject.SelectToken("instance.contributors")?.Where(jt => jt.HasValues).Select(jt => ActualCostRecordContributor.FromJObject((JObject)jt)).ToArray(),
            ActualCostRecordIdentifiers = jObject.SelectToken("instance.identifiers")?.Where(jt => jt.HasValues).Select(jt => ActualCostRecordIdentifier.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("lossType", LossType),
            new JProperty("lossDate", LossDate?.ToLocalTime()),
            new JProperty("expirationDate", ExpirationDate?.ToLocalTime()),
            new JProperty("user", new JObject(
                new JProperty("barcode", UserBarcode),
                new JProperty("firstName", UserFirstName),
                new JProperty("lastName", UserLastName),
                new JProperty("middleName", UserMiddleName),
                new JProperty("patronGroupId", UserGroupId),
                new JProperty("patronGroup", UserGroupName))),
            new JProperty("item", new JObject(
                new JProperty("barcode", ItemBarcode),
                new JProperty("materialTypeId", ItemMaterialTypeId),
                new JProperty("materialType", ItemMaterialTypeName),
                new JProperty("permanentLocationId", ItemPermanentLocationId),
                new JProperty("permanentLocation", ItemPermanentLocationName),
                new JProperty("effectiveLocationId", ItemEffectiveLocationId),
                new JProperty("effectiveLocation", ItemEffectiveLocationName),
                new JProperty("loanTypeId", ItemLoanTypeId),
                new JProperty("loanType", ItemLoanTypeName),
                new JProperty("holdingsRecordId", ItemHoldingId),
                new JProperty("effectiveCallNumberComponents", new JObject(
                    new JProperty("callNumber", ItemEffectiveCallNumber),
                    new JProperty("prefix", ItemEffectiveCallNumberPrefix),
                    new JProperty("suffix", ItemEffectiveCallNumberSuffix))),
                new JProperty("volume", ItemVolume),
                new JProperty("enumeration", ItemEnumeration),
                new JProperty("chronology", ItemChronology),
                new JProperty("displaySummary", ItemDisplaySummary),
                new JProperty("copyNumber", ItemCopyNumber))),
            new JProperty("instance", new JObject(
                new JProperty("title", InstanceTitle),
                new JProperty("contributors", ActualCostRecordContributors?.Select(acrc => acrc.ToJObject())),
                new JProperty("identifiers", ActualCostRecordIdentifiers?.Select(acri => acri.ToJObject())))),
            new JProperty("feeFine", new JObject(
                new JProperty("accountId", FeeId),
                new JProperty("billedAmount", FeeBilledAmount),
                new JProperty("ownerId", OwnerId),
                new JProperty("owner", OwnerName),
                new JProperty("typeId", FeeTypeId),
                new JProperty("type", FeeTypeName))),
            new JProperty("status", Status),
            new JProperty("additionalInfoForStaff", AdditionalInfoForStaff),
            new JProperty("additionalInfoForPatron", AdditionalInfoForPatron),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
