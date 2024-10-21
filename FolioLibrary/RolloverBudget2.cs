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
    // uc.rollover_budgets -> uchicago_mod_finance_storage.ledger_fiscal_year_rollover_budget
    // RolloverBudget2 -> RolloverBudget
    [DisplayColumn(nameof(Name)), DisplayName("Rollover Budgets"), JsonConverter(typeof(JsonPathJsonConverter<RolloverBudget2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("rollover_budgets", Schema = "uc")]
    public partial class RolloverBudget2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.RolloverBudget.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("_version"), JsonProperty("_version"), ScaffoldColumn(false)]
        public virtual int? Version { get; set; }

        [Display(Order = 3)]
        public virtual Budget2 Budget { get; set; }

        [Column("budget_id"), Display(Name = "Budget", Order = 4), JsonProperty("budgetId")]
        public virtual Guid? BudgetId { get; set; }

        [Display(Order = 5)]
        public virtual Rollover2 Rollover { get; set; }

        [Column("rollover_id"), Display(Name = "Rollover", Order = 6), JsonProperty("ledgerRolloverId")]
        public virtual Guid? RolloverId { get; set; }

        [Column("name"), Display(Order = 7), JsonProperty("name"), StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("fund_details_name"), Display(Name = "Fund Details Name", Order = 8), JsonProperty("fundDetails.name"), StringLength(1024)]
        public virtual string FundDetailsName { get; set; }

        [Column("fund_details_code"), Display(Name = "Fund Details Code", Order = 9), JsonProperty("fundDetails.code"), StringLength(1024)]
        public virtual string FundDetailsCode { get; set; }

        [Column("fund_details_fund_status"), Display(Name = "Fund Details Fund Status", Order = 10), JsonProperty("fundDetails.fundStatus"), RegularExpression(@"^(Active|Frozen|Inactive)$"), StringLength(1024)]
        public virtual string FundDetailsFundStatus { get; set; }

        [Display(Name = "Fund Details Fund Type", Order = 11)]
        public virtual FundType2 FundDetailsFundType { get; set; }

        [Column("fund_details_fund_type_id"), Display(Name = "Fund Details Fund Type", Order = 12), JsonProperty("fundDetails.fundTypeId")]
        public virtual Guid? FundDetailsFundTypeId { get; set; }

        [Column("fund_details_fund_type_name"), Display(Name = "Fund Details Fund Type Name", Order = 13), JsonProperty("fundDetails.fundTypeName"), StringLength(1024)]
        public virtual string FundDetailsFundTypeName { get; set; }

        [Column("fund_details_external_account_no"), Display(Name = "Fund Details External Account No", Order = 14), JsonProperty("fundDetails.externalAccountNo"), StringLength(1024)]
        public virtual string FundDetailsExternalAccountNo { get; set; }

        [Column("fund_details_description"), Display(Name = "Fund Details Description", Order = 15), JsonProperty("fundDetails.description"), StringLength(1024)]
        public virtual string FundDetailsDescription { get; set; }

        [Column("fund_details_restrict_by_locations"), Display(Name = "Fund Details Restrict By Locations", Order = 16), JsonProperty("fundDetails.restrictByLocations")]
        public virtual bool? FundDetailsRestrictByLocations { get; set; }

        [Column("budget_status"), Display(Name = "Budget Status", Order = 17), JsonProperty("budgetStatus"), RegularExpression(@"^(Active|Frozen|Inactive|Planned|Closed)$"), StringLength(1024)]
        public virtual string BudgetStatus { get; set; }

        [Column("allowable_encumbrance"), Display(Name = "Allowable Encumbrance", Order = 18), JsonProperty("allowableEncumbrance")]
        public virtual decimal? AllowableEncumbrance { get; set; }

        [Column("allowable_expenditure"), Display(Name = "Allowable Expenditure", Order = 19), JsonProperty("allowableExpenditure")]
        public virtual decimal? AllowableExpenditure { get; set; }

        [Column("allocated"), Display(Order = 20), JsonProperty("allocated")]
        public virtual decimal? Allocated { get; set; }

        [Column("awaiting_payment"), Display(Name = "Awaiting Payment", Order = 21), JsonProperty("awaitingPayment")]
        public virtual decimal? AwaitingPayment { get; set; }

        [Column("available"), Display(Order = 22), JsonProperty("available")]
        public virtual decimal? Available { get; set; }

        [Column("credits"), Display(Order = 23), JsonProperty("credits")]
        public virtual decimal? Credits { get; set; }

        [Column("encumbered"), Display(Order = 24), JsonProperty("encumbered")]
        public virtual decimal? Encumbered { get; set; }

        [Column("expenditures"), Display(Order = 25), JsonProperty("expenditures")]
        public virtual decimal? Expenditures { get; set; }

        [Column("net_transfers"), Display(Name = "Net Transfers", Order = 26), JsonProperty("netTransfers")]
        public virtual decimal? NetTransfers { get; set; }

        [Column("unavailable"), Display(Order = 27), JsonProperty("unavailable")]
        public virtual decimal? Unavailable { get; set; }

        [Column("over_encumbrance"), Display(Name = "Over Encumbrance", Order = 28), JsonProperty("overEncumbrance")]
        public virtual decimal? OverEncumbrance { get; set; }

        [Column("over_expended"), Display(Name = "Over Expended", Order = 29), JsonProperty("overExpended")]
        public virtual decimal? OverExpended { get; set; }

        [Display(Order = 30)]
        public virtual Fund2 Fund { get; set; }

        [Column("fund_id"), Display(Name = "Fund", Order = 31), JsonProperty("fundId")]
        public virtual Guid? FundId { get; set; }

        [Display(Name = "Fiscal Year", Order = 32)]
        public virtual FiscalYear2 FiscalYear { get; set; }

        [Column("fiscal_year_id"), Display(Name = "Fiscal Year", Order = 33), JsonProperty("fiscalYearId")]
        public virtual Guid? FiscalYearId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 34), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 35), InverseProperty("RolloverBudget2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 36), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 38), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 39), InverseProperty("RolloverBudget2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 40), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("initial_allocation"), Display(Name = "Initial Allocation", Order = 42), JsonProperty("initialAllocation")]
        public virtual decimal? InitialAllocation { get; set; }

        [Column("allocation_to"), Display(Name = "Allocation To", Order = 43), JsonProperty("allocationTo")]
        public virtual decimal? AllocationTo { get; set; }

        [Column("allocation_from"), Display(Name = "Allocation From", Order = 44), JsonProperty("allocationFrom")]
        public virtual decimal? AllocationFrom { get; set; }

        [Column("total_funding"), Display(Name = "Total Funding", Order = 45), JsonProperty("totalFunding")]
        public virtual decimal? TotalFunding { get; set; }

        [Column("cash_balance"), Display(Name = "Cash Balance", Order = 46), JsonProperty("cashBalance")]
        public virtual decimal? CashBalance { get; set; }

        [Column("content"), CustomValidation(typeof(RolloverBudget), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 47), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Rollover Budget Acquisitions Units", Order = 48), JsonConverter(typeof(ArrayJsonConverter<List<RolloverBudgetAcquisitionsUnit2>, RolloverBudgetAcquisitionsUnit2>), "AcquisitionsUnitId"), JsonProperty("acqUnitIds")]
        public virtual ICollection<RolloverBudgetAcquisitionsUnit2> RolloverBudgetAcquisitionsUnit2s { get; set; }

        [Display(Name = "Rollover Budget Acquisitions Units", Order = 49), JsonConverter(typeof(ArrayJsonConverter<List<RolloverBudgetAcquisitionsUnit>, RolloverBudgetAcquisitionsUnit>), "AcquisitionsUnitId"), JsonProperty("fundDetails.acqUnitIds")]
        public virtual ICollection<RolloverBudgetAcquisitionsUnit> RolloverBudgetAcquisitionsUnits { get; set; }

        [Display(Name = "Rollover Budget Allocated From Names", Order = 50), JsonConverter(typeof(ArrayJsonConverter<List<RolloverBudgetAllocatedFromName>, RolloverBudgetAllocatedFromName>), "Content"), JsonProperty("fundDetails.allocatedFromNames")]
        public virtual ICollection<RolloverBudgetAllocatedFromName> RolloverBudgetAllocatedFromNames { get; set; }

        [Display(Name = "Rollover Budget Allocated To Names", Order = 51), JsonConverter(typeof(ArrayJsonConverter<List<RolloverBudgetAllocatedToName>, RolloverBudgetAllocatedToName>), "Content"), JsonProperty("fundDetails.allocatedToNames")]
        public virtual ICollection<RolloverBudgetAllocatedToName> RolloverBudgetAllocatedToNames { get; set; }

        [Display(Name = "Rollover Budget Expense Class Details", Order = 52), JsonProperty("expenseClassDetails")]
        public virtual ICollection<RolloverBudgetExpenseClassDetail> RolloverBudgetExpenseClassDetails { get; set; }

        [Display(Name = "Rollover Budget From Funds", Order = 53), JsonConverter(typeof(ArrayJsonConverter<List<RolloverBudgetFromFund>, RolloverBudgetFromFund>), "FundId"), JsonProperty("fundDetails.allocatedFromIds")]
        public virtual ICollection<RolloverBudgetFromFund> RolloverBudgetFromFunds { get; set; }

        [Display(Name = "Rollover Budget Locations", Order = 54), JsonProperty("fundDetails.locations")]
        public virtual ICollection<RolloverBudgetLocation> RolloverBudgetLocations { get; set; }

        [Display(Name = "Rollover Budget Organizations", Order = 55), JsonConverter(typeof(ArrayJsonConverter<List<RolloverBudgetOrganization>, RolloverBudgetOrganization>), "OrganizationId"), JsonProperty("fundDetails.donorOrganizationIds")]
        public virtual ICollection<RolloverBudgetOrganization> RolloverBudgetOrganizations { get; set; }

        [Display(Name = "Rollover Budget Tags", Order = 56), JsonConverter(typeof(ArrayJsonConverter<List<RolloverBudgetTag>, RolloverBudgetTag>), "Content"), JsonProperty("tags.tagList")]
        public virtual ICollection<RolloverBudgetTag> RolloverBudgetTags { get; set; }

        [Display(Name = "Rollover Budget To Funds", Order = 57), JsonConverter(typeof(ArrayJsonConverter<List<RolloverBudgetToFund>, RolloverBudgetToFund>), "FundId"), JsonProperty("fundDetails.allocatedToIds")]
        public virtual ICollection<RolloverBudgetToFund> RolloverBudgetToFunds { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Version)} = {Version}, {nameof(BudgetId)} = {BudgetId}, {nameof(RolloverId)} = {RolloverId}, {nameof(Name)} = {Name}, {nameof(FundDetailsName)} = {FundDetailsName}, {nameof(FundDetailsCode)} = {FundDetailsCode}, {nameof(FundDetailsFundStatus)} = {FundDetailsFundStatus}, {nameof(FundDetailsFundTypeId)} = {FundDetailsFundTypeId}, {nameof(FundDetailsFundTypeName)} = {FundDetailsFundTypeName}, {nameof(FundDetailsExternalAccountNo)} = {FundDetailsExternalAccountNo}, {nameof(FundDetailsDescription)} = {FundDetailsDescription}, {nameof(FundDetailsRestrictByLocations)} = {FundDetailsRestrictByLocations}, {nameof(BudgetStatus)} = {BudgetStatus}, {nameof(AllowableEncumbrance)} = {AllowableEncumbrance}, {nameof(AllowableExpenditure)} = {AllowableExpenditure}, {nameof(Allocated)} = {Allocated}, {nameof(AwaitingPayment)} = {AwaitingPayment}, {nameof(Available)} = {Available}, {nameof(Credits)} = {Credits}, {nameof(Encumbered)} = {Encumbered}, {nameof(Expenditures)} = {Expenditures}, {nameof(NetTransfers)} = {NetTransfers}, {nameof(Unavailable)} = {Unavailable}, {nameof(OverEncumbrance)} = {OverEncumbrance}, {nameof(OverExpended)} = {OverExpended}, {nameof(FundId)} = {FundId}, {nameof(FiscalYearId)} = {FiscalYearId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(InitialAllocation)} = {InitialAllocation}, {nameof(AllocationTo)} = {AllocationTo}, {nameof(AllocationFrom)} = {AllocationFrom}, {nameof(TotalFunding)} = {TotalFunding}, {nameof(CashBalance)} = {CashBalance}, {nameof(Content)} = {Content}, {nameof(RolloverBudgetAcquisitionsUnit2s)} = {(RolloverBudgetAcquisitionsUnit2s != null ? $"{{ {string.Join(", ", RolloverBudgetAcquisitionsUnit2s)} }}" : "")}, {nameof(RolloverBudgetAcquisitionsUnits)} = {(RolloverBudgetAcquisitionsUnits != null ? $"{{ {string.Join(", ", RolloverBudgetAcquisitionsUnits)} }}" : "")}, {nameof(RolloverBudgetAllocatedFromNames)} = {(RolloverBudgetAllocatedFromNames != null ? $"{{ {string.Join(", ", RolloverBudgetAllocatedFromNames)} }}" : "")}, {nameof(RolloverBudgetAllocatedToNames)} = {(RolloverBudgetAllocatedToNames != null ? $"{{ {string.Join(", ", RolloverBudgetAllocatedToNames)} }}" : "")}, {nameof(RolloverBudgetExpenseClassDetails)} = {(RolloverBudgetExpenseClassDetails != null ? $"{{ {string.Join(", ", RolloverBudgetExpenseClassDetails)} }}" : "")}, {nameof(RolloverBudgetFromFunds)} = {(RolloverBudgetFromFunds != null ? $"{{ {string.Join(", ", RolloverBudgetFromFunds)} }}" : "")}, {nameof(RolloverBudgetLocations)} = {(RolloverBudgetLocations != null ? $"{{ {string.Join(", ", RolloverBudgetLocations)} }}" : "")}, {nameof(RolloverBudgetOrganizations)} = {(RolloverBudgetOrganizations != null ? $"{{ {string.Join(", ", RolloverBudgetOrganizations)} }}" : "")}, {nameof(RolloverBudgetTags)} = {(RolloverBudgetTags != null ? $"{{ {string.Join(", ", RolloverBudgetTags)} }}" : "")}, {nameof(RolloverBudgetToFunds)} = {(RolloverBudgetToFunds != null ? $"{{ {string.Join(", ", RolloverBudgetToFunds)} }}" : "")} }}";

        public static RolloverBudget2 FromJObject(JObject jObject) => jObject != null ? new RolloverBudget2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Version = (int?)jObject.SelectToken("_version"),
            BudgetId = (Guid?)jObject.SelectToken("budgetId"),
            RolloverId = (Guid?)jObject.SelectToken("ledgerRolloverId"),
            Name = (string)jObject.SelectToken("name"),
            FundDetailsName = (string)jObject.SelectToken("fundDetails.name"),
            FundDetailsCode = (string)jObject.SelectToken("fundDetails.code"),
            FundDetailsFundStatus = (string)jObject.SelectToken("fundDetails.fundStatus"),
            FundDetailsFundTypeId = (Guid?)jObject.SelectToken("fundDetails.fundTypeId"),
            FundDetailsFundTypeName = (string)jObject.SelectToken("fundDetails.fundTypeName"),
            FundDetailsExternalAccountNo = (string)jObject.SelectToken("fundDetails.externalAccountNo"),
            FundDetailsDescription = (string)jObject.SelectToken("fundDetails.description"),
            FundDetailsRestrictByLocations = (bool?)jObject.SelectToken("fundDetails.restrictByLocations"),
            BudgetStatus = (string)jObject.SelectToken("budgetStatus"),
            AllowableEncumbrance = (decimal?)jObject.SelectToken("allowableEncumbrance"),
            AllowableExpenditure = (decimal?)jObject.SelectToken("allowableExpenditure"),
            Allocated = (decimal?)jObject.SelectToken("allocated"),
            AwaitingPayment = (decimal?)jObject.SelectToken("awaitingPayment"),
            Available = (decimal?)jObject.SelectToken("available"),
            Credits = (decimal?)jObject.SelectToken("credits"),
            Encumbered = (decimal?)jObject.SelectToken("encumbered"),
            Expenditures = (decimal?)jObject.SelectToken("expenditures"),
            NetTransfers = (decimal?)jObject.SelectToken("netTransfers"),
            Unavailable = (decimal?)jObject.SelectToken("unavailable"),
            OverEncumbrance = (decimal?)jObject.SelectToken("overEncumbrance"),
            OverExpended = (decimal?)jObject.SelectToken("overExpended"),
            FundId = (Guid?)jObject.SelectToken("fundId"),
            FiscalYearId = (Guid?)jObject.SelectToken("fiscalYearId"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            InitialAllocation = (decimal?)jObject.SelectToken("initialAllocation"),
            AllocationTo = (decimal?)jObject.SelectToken("allocationTo"),
            AllocationFrom = (decimal?)jObject.SelectToken("allocationFrom"),
            TotalFunding = (decimal?)jObject.SelectToken("totalFunding"),
            CashBalance = (decimal?)jObject.SelectToken("cashBalance"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            RolloverBudgetAcquisitionsUnit2s = jObject.SelectToken("acqUnitIds")?.Select(jt => RolloverBudgetAcquisitionsUnit2.FromJObject((JValue)jt)).ToArray(),
            RolloverBudgetAcquisitionsUnits = jObject.SelectToken("fundDetails.acqUnitIds")?.Select(jt => RolloverBudgetAcquisitionsUnit.FromJObject((JValue)jt)).ToArray(),
            RolloverBudgetAllocatedFromNames = jObject.SelectToken("fundDetails.allocatedFromNames")?.Select(jt => RolloverBudgetAllocatedFromName.FromJObject((JValue)jt)).ToArray(),
            RolloverBudgetAllocatedToNames = jObject.SelectToken("fundDetails.allocatedToNames")?.Select(jt => RolloverBudgetAllocatedToName.FromJObject((JValue)jt)).ToArray(),
            RolloverBudgetExpenseClassDetails = jObject.SelectToken("expenseClassDetails")?.Where(jt => jt.HasValues).Select(jt => RolloverBudgetExpenseClassDetail.FromJObject((JObject)jt)).ToArray(),
            RolloverBudgetFromFunds = jObject.SelectToken("fundDetails.allocatedFromIds")?.Select(jt => RolloverBudgetFromFund.FromJObject((JValue)jt)).ToArray(),
            RolloverBudgetLocations = jObject.SelectToken("fundDetails.locations")?.Where(jt => jt.HasValues).Select(jt => RolloverBudgetLocation.FromJObject((JObject)jt)).ToArray(),
            RolloverBudgetOrganizations = jObject.SelectToken("fundDetails.donorOrganizationIds")?.Select(jt => RolloverBudgetOrganization.FromJObject((JValue)jt)).ToArray(),
            RolloverBudgetTags = jObject.SelectToken("tags.tagList")?.Select(jt => RolloverBudgetTag.FromJObject((JValue)jt)).ToArray(),
            RolloverBudgetToFunds = jObject.SelectToken("fundDetails.allocatedToIds")?.Select(jt => RolloverBudgetToFund.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("_version", Version),
            new JProperty("budgetId", BudgetId),
            new JProperty("ledgerRolloverId", RolloverId),
            new JProperty("name", Name),
            new JProperty("fundDetails", new JObject(
                new JProperty("name", FundDetailsName),
                new JProperty("code", FundDetailsCode),
                new JProperty("fundStatus", FundDetailsFundStatus),
                new JProperty("fundTypeId", FundDetailsFundTypeId),
                new JProperty("fundTypeName", FundDetailsFundTypeName),
                new JProperty("externalAccountNo", FundDetailsExternalAccountNo),
                new JProperty("description", FundDetailsDescription),
                new JProperty("restrictByLocations", FundDetailsRestrictByLocations),
                new JProperty("acqUnitIds", RolloverBudgetAcquisitionsUnits?.Select(rbau => rbau.ToJObject())),
                new JProperty("allocatedFromNames", RolloverBudgetAllocatedFromNames?.Select(rbafn => rbafn.ToJObject())),
                new JProperty("allocatedToNames", RolloverBudgetAllocatedToNames?.Select(rbatn => rbatn.ToJObject())),
                new JProperty("allocatedFromIds", RolloverBudgetFromFunds?.Select(rbff => rbff.ToJObject())),
                new JProperty("locations", RolloverBudgetLocations?.Select(rbl => rbl.ToJObject())),
                new JProperty("donorOrganizationIds", RolloverBudgetOrganizations?.Select(rbo => rbo.ToJObject())),
                new JProperty("allocatedToIds", RolloverBudgetToFunds?.Select(rbtf => rbtf.ToJObject())))),
            new JProperty("budgetStatus", BudgetStatus),
            new JProperty("allowableEncumbrance", AllowableEncumbrance),
            new JProperty("allowableExpenditure", AllowableExpenditure),
            new JProperty("allocated", Allocated),
            new JProperty("awaitingPayment", AwaitingPayment),
            new JProperty("available", Available),
            new JProperty("credits", Credits),
            new JProperty("encumbered", Encumbered),
            new JProperty("expenditures", Expenditures),
            new JProperty("netTransfers", NetTransfers),
            new JProperty("unavailable", Unavailable),
            new JProperty("overEncumbrance", OverEncumbrance),
            new JProperty("overExpended", OverExpended),
            new JProperty("fundId", FundId),
            new JProperty("fiscalYearId", FiscalYearId),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("initialAllocation", InitialAllocation),
            new JProperty("allocationTo", AllocationTo),
            new JProperty("allocationFrom", AllocationFrom),
            new JProperty("totalFunding", TotalFunding),
            new JProperty("cashBalance", CashBalance),
            new JProperty("acqUnitIds", RolloverBudgetAcquisitionsUnit2s?.Select(rbau2 => rbau2.ToJObject())),
            new JProperty("expenseClassDetails", RolloverBudgetExpenseClassDetails?.Select(rbecd => rbecd.ToJObject())),
            new JProperty("tags", new JObject(
                new JProperty("tagList", RolloverBudgetTags?.Select(rbt => rbt.ToJObject()))))).RemoveNullAndEmptyProperties();
    }
}
