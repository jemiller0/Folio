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
    // uc.lost_item_fee_policies -> uchicago_mod_feesfines.lost_item_fee_policy
    // LostItemFeePolicy2 -> LostItemFeePolicy
    [DisplayColumn(nameof(Name)), DisplayName("Lost Item Fee Policies"), JsonConverter(typeof(JsonPathJsonConverter<LostItemFeePolicy2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("lost_item_fee_policies", Schema = "uc")]
    public partial class LostItemFeePolicy2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.LostItemFeePolicy.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("name"), Display(Order = 2), JsonProperty("name"), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("description"), Display(Order = 3), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("item_aged_lost_overdue_duration"), Display(Name = "Item Aged Lost Overdue Duration", Order = 4), JsonProperty("itemAgedLostOverdue.duration")]
        public virtual int? ItemAgedLostOverdueDuration { get; set; }

        [Column("item_aged_lost_overdue_interval_id"), Display(Name = "Item Aged Lost Overdue Interval", Order = 5), JsonProperty("itemAgedLostOverdue.intervalId"), RegularExpression(@"^(Minutes|Hours|Days|Weeks|Months)$"), StringLength(1024)]
        public virtual string ItemAgedLostOverdueInterval { get; set; }

        [Column("patron_billed_after_aged_lost_duration"), Display(Name = "Patron Billed After Aged Lost Duration", Order = 6), JsonProperty("patronBilledAfterAgedLost.duration")]
        public virtual int? PatronBilledAfterAgedLostDuration { get; set; }

        [Column("patron_billed_after_aged_lost_interval_id"), Display(Name = "Patron Billed After Aged Lost Interval", Order = 7), JsonProperty("patronBilledAfterAgedLost.intervalId"), RegularExpression(@"^(Minutes|Hours|Days|Weeks|Months)$"), StringLength(1024)]
        public virtual string PatronBilledAfterAgedLostInterval { get; set; }

        [Column("recalled_item_aged_lost_overdue_duration"), Display(Name = "Recalled Item Aged Lost Overdue Duration", Order = 8), JsonProperty("recalledItemAgedLostOverdue.duration")]
        public virtual int? RecalledItemAgedLostOverdueDuration { get; set; }

        [Column("recalled_item_aged_lost_overdue_interval_id"), Display(Name = "Recalled Item Aged Lost Overdue Interval", Order = 9), JsonProperty("recalledItemAgedLostOverdue.intervalId"), RegularExpression(@"^(Minutes|Hours|Days|Weeks|Months)$"), StringLength(1024)]
        public virtual string RecalledItemAgedLostOverdueInterval { get; set; }

        [Column("patron_billed_after_recalled_item_aged_lost_duration"), Display(Name = "Patron Billed After Recalled Item Aged Lost Duration", Order = 10), JsonProperty("patronBilledAfterRecalledItemAgedLost.duration")]
        public virtual int? PatronBilledAfterRecalledItemAgedLostDuration { get; set; }

        [Column("patron_billed_after_recalled_item_aged_lost_interval_id"), Display(Name = "Patron Billed After Recalled Item Aged Lost Interval", Order = 11), JsonProperty("patronBilledAfterRecalledItemAgedLost.intervalId"), RegularExpression(@"^(Minutes|Hours|Days|Weeks|Months)$"), StringLength(1024)]
        public virtual string PatronBilledAfterRecalledItemAgedLostInterval { get; set; }

        [Column("charge_amount_item_charge_type"), Display(Name = "Charge Amount Item Charge Type", Order = 12), JsonProperty("chargeAmountItem.chargeType"), StringLength(1024)]
        public virtual string ChargeAmountItemChargeType { get; set; }

        [Column("charge_amount_item_amount"), DataType(DataType.Currency), Display(Name = "Charge Amount Item Amount", Order = 13), DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = true), JsonProperty("chargeAmountItem.amount")]
        public virtual decimal? ChargeAmountItemAmount { get; set; }

        [Column("lost_item_processing_fee"), Display(Name = "Lost Item Processing Fee", Order = 14), JsonProperty("lostItemProcessingFee")]
        public virtual decimal? LostItemProcessingFee { get; set; }

        [Column("charge_amount_item_patron"), Display(Name = "Charge Amount Item Patron", Order = 15), JsonProperty("chargeAmountItemPatron")]
        public virtual bool? ChargeAmountItemPatron { get; set; }

        [Column("charge_amount_item_system"), Display(Name = "Charge Amount Item System", Order = 16), JsonProperty("chargeAmountItemSystem")]
        public virtual bool? ChargeAmountItemSystem { get; set; }

        [Column("lost_item_charge_fee_fine_duration"), Display(Name = "Lost Item Charge Fee Fine Duration", Order = 17), JsonProperty("lostItemChargeFeeFine.duration")]
        public virtual int? LostItemChargeFeeFineDuration { get; set; }

        [Column("lost_item_charge_fee_fine_interval_id"), Display(Name = "Lost Item Charge Fee Fine Interval", Order = 18), JsonProperty("lostItemChargeFeeFine.intervalId"), RegularExpression(@"^(Minutes|Hours|Days|Weeks|Months)$"), StringLength(1024)]
        public virtual string LostItemChargeFeeFineInterval { get; set; }

        [Column("returned_lost_item_processing_fee"), Display(Name = "Returned Lost Item Processing Fee", Order = 19), JsonProperty("returnedLostItemProcessingFee")]
        public virtual bool? ReturnedLostItemProcessingFee { get; set; }

        [Column("replaced_lost_item_processing_fee"), Display(Name = "Replaced Lost Item Processing Fee", Order = 20), JsonProperty("replacedLostItemProcessingFee")]
        public virtual bool? ReplacedLostItemProcessingFee { get; set; }

        [Column("replacement_processing_fee"), Display(Name = "Replacement Processing Fee", Order = 21), JsonProperty("replacementProcessingFee")]
        public virtual decimal? ReplacementProcessingFee { get; set; }

        [Column("replacement_allowed"), Display(Name = "Replacement Allowed", Order = 22), JsonProperty("replacementAllowed")]
        public virtual bool? ReplacementAllowed { get; set; }

        [Column("lost_item_returned"), Display(Name = "Lost Item Returned", Order = 23), JsonProperty("lostItemReturned"), StringLength(1024)]
        public virtual string LostItemReturned { get; set; }

        [Column("fees_fines_shall_refunded_duration"), Display(Name = "Fees Fines Shall Refunded Duration", Order = 24), JsonProperty("feesFinesShallRefunded.duration")]
        public virtual int? FeesFinesShallRefundedDuration { get; set; }

        [Column("fees_fines_shall_refunded_interval_id"), Display(Name = "Fees Fines Shall Refunded Interval", Order = 25), JsonProperty("feesFinesShallRefunded.intervalId"), RegularExpression(@"^(Minutes|Hours|Days|Weeks|Months)$"), StringLength(1024)]
        public virtual string FeesFinesShallRefundedInterval { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 26), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 27), InverseProperty("LostItemFeePolicy2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 28), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 30), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 31), InverseProperty("LostItemFeePolicy2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 32), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(LostItemFeePolicy), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 34), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Loans", Order = 35)]
        public virtual ICollection<Loan2> Loan2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Description)} = {Description}, {nameof(ItemAgedLostOverdueDuration)} = {ItemAgedLostOverdueDuration}, {nameof(ItemAgedLostOverdueInterval)} = {ItemAgedLostOverdueInterval}, {nameof(PatronBilledAfterAgedLostDuration)} = {PatronBilledAfterAgedLostDuration}, {nameof(PatronBilledAfterAgedLostInterval)} = {PatronBilledAfterAgedLostInterval}, {nameof(RecalledItemAgedLostOverdueDuration)} = {RecalledItemAgedLostOverdueDuration}, {nameof(RecalledItemAgedLostOverdueInterval)} = {RecalledItemAgedLostOverdueInterval}, {nameof(PatronBilledAfterRecalledItemAgedLostDuration)} = {PatronBilledAfterRecalledItemAgedLostDuration}, {nameof(PatronBilledAfterRecalledItemAgedLostInterval)} = {PatronBilledAfterRecalledItemAgedLostInterval}, {nameof(ChargeAmountItemChargeType)} = {ChargeAmountItemChargeType}, {nameof(ChargeAmountItemAmount)} = {ChargeAmountItemAmount}, {nameof(LostItemProcessingFee)} = {LostItemProcessingFee}, {nameof(ChargeAmountItemPatron)} = {ChargeAmountItemPatron}, {nameof(ChargeAmountItemSystem)} = {ChargeAmountItemSystem}, {nameof(LostItemChargeFeeFineDuration)} = {LostItemChargeFeeFineDuration}, {nameof(LostItemChargeFeeFineInterval)} = {LostItemChargeFeeFineInterval}, {nameof(ReturnedLostItemProcessingFee)} = {ReturnedLostItemProcessingFee}, {nameof(ReplacedLostItemProcessingFee)} = {ReplacedLostItemProcessingFee}, {nameof(ReplacementProcessingFee)} = {ReplacementProcessingFee}, {nameof(ReplacementAllowed)} = {ReplacementAllowed}, {nameof(LostItemReturned)} = {LostItemReturned}, {nameof(FeesFinesShallRefundedDuration)} = {FeesFinesShallRefundedDuration}, {nameof(FeesFinesShallRefundedInterval)} = {FeesFinesShallRefundedInterval}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static LostItemFeePolicy2 FromJObject(JObject jObject) => jObject != null ? new LostItemFeePolicy2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            Description = (string)jObject.SelectToken("description"),
            ItemAgedLostOverdueDuration = (int?)jObject.SelectToken("itemAgedLostOverdue.duration"),
            ItemAgedLostOverdueInterval = (string)jObject.SelectToken("itemAgedLostOverdue.intervalId"),
            PatronBilledAfterAgedLostDuration = (int?)jObject.SelectToken("patronBilledAfterAgedLost.duration"),
            PatronBilledAfterAgedLostInterval = (string)jObject.SelectToken("patronBilledAfterAgedLost.intervalId"),
            RecalledItemAgedLostOverdueDuration = (int?)jObject.SelectToken("recalledItemAgedLostOverdue.duration"),
            RecalledItemAgedLostOverdueInterval = (string)jObject.SelectToken("recalledItemAgedLostOverdue.intervalId"),
            PatronBilledAfterRecalledItemAgedLostDuration = (int?)jObject.SelectToken("patronBilledAfterRecalledItemAgedLost.duration"),
            PatronBilledAfterRecalledItemAgedLostInterval = (string)jObject.SelectToken("patronBilledAfterRecalledItemAgedLost.intervalId"),
            ChargeAmountItemChargeType = (string)jObject.SelectToken("chargeAmountItem.chargeType"),
            ChargeAmountItemAmount = (decimal?)jObject.SelectToken("chargeAmountItem.amount"),
            LostItemProcessingFee = (decimal?)jObject.SelectToken("lostItemProcessingFee"),
            ChargeAmountItemPatron = (bool?)jObject.SelectToken("chargeAmountItemPatron"),
            ChargeAmountItemSystem = (bool?)jObject.SelectToken("chargeAmountItemSystem"),
            LostItemChargeFeeFineDuration = (int?)jObject.SelectToken("lostItemChargeFeeFine.duration"),
            LostItemChargeFeeFineInterval = (string)jObject.SelectToken("lostItemChargeFeeFine.intervalId"),
            ReturnedLostItemProcessingFee = (bool?)jObject.SelectToken("returnedLostItemProcessingFee"),
            ReplacedLostItemProcessingFee = (bool?)jObject.SelectToken("replacedLostItemProcessingFee"),
            ReplacementProcessingFee = (decimal?)jObject.SelectToken("replacementProcessingFee"),
            ReplacementAllowed = (bool?)jObject.SelectToken("replacementAllowed"),
            LostItemReturned = (string)jObject.SelectToken("lostItemReturned"),
            FeesFinesShallRefundedDuration = (int?)jObject.SelectToken("feesFinesShallRefunded.duration"),
            FeesFinesShallRefundedInterval = (string)jObject.SelectToken("feesFinesShallRefunded.intervalId"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("name", Name),
            new JProperty("description", Description),
            new JProperty("itemAgedLostOverdue", new JObject(
                new JProperty("duration", ItemAgedLostOverdueDuration),
                new JProperty("intervalId", ItemAgedLostOverdueInterval))),
            new JProperty("patronBilledAfterAgedLost", new JObject(
                new JProperty("duration", PatronBilledAfterAgedLostDuration),
                new JProperty("intervalId", PatronBilledAfterAgedLostInterval))),
            new JProperty("recalledItemAgedLostOverdue", new JObject(
                new JProperty("duration", RecalledItemAgedLostOverdueDuration),
                new JProperty("intervalId", RecalledItemAgedLostOverdueInterval))),
            new JProperty("patronBilledAfterRecalledItemAgedLost", new JObject(
                new JProperty("duration", PatronBilledAfterRecalledItemAgedLostDuration),
                new JProperty("intervalId", PatronBilledAfterRecalledItemAgedLostInterval))),
            new JProperty("chargeAmountItem", new JObject(
                new JProperty("chargeType", ChargeAmountItemChargeType),
                new JProperty("amount", ChargeAmountItemAmount))),
            new JProperty("lostItemProcessingFee", LostItemProcessingFee),
            new JProperty("chargeAmountItemPatron", ChargeAmountItemPatron),
            new JProperty("chargeAmountItemSystem", ChargeAmountItemSystem),
            new JProperty("lostItemChargeFeeFine", new JObject(
                new JProperty("duration", LostItemChargeFeeFineDuration),
                new JProperty("intervalId", LostItemChargeFeeFineInterval))),
            new JProperty("returnedLostItemProcessingFee", ReturnedLostItemProcessingFee),
            new JProperty("replacedLostItemProcessingFee", ReplacedLostItemProcessingFee),
            new JProperty("replacementProcessingFee", ReplacementProcessingFee),
            new JProperty("replacementAllowed", ReplacementAllowed),
            new JProperty("lostItemReturned", LostItemReturned),
            new JProperty("feesFinesShallRefunded", new JObject(
                new JProperty("duration", FeesFinesShallRefundedDuration),
                new JProperty("intervalId", FeesFinesShallRefundedInterval))),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
