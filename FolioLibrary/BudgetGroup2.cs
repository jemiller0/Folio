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
    // uc.budget_groups -> uchicago_mod_finance_storage.group_fund_fiscal_year
    // BudgetGroup2 -> BudgetGroup
    [DisplayColumn(nameof(Id)), DisplayName("Budget Groups"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("budget_groups", Schema = "uc")]
    public partial class BudgetGroup2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.BudgetGroup.json")))
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
        public virtual FinanceGroup2 Group { get; set; }

        [Column("group_id"), Display(Name = "Group", Order = 6), JsonProperty("groupId"), Required]
        public virtual Guid? GroupId { get; set; }

        [Display(Name = "Fiscal Year", Order = 7)]
        public virtual FiscalYear2 FiscalYear { get; set; }

        [Column("fiscal_year_id"), Display(Name = "Fiscal Year", Order = 8), JsonProperty("fiscalYearId"), Required]
        public virtual Guid? FiscalYearId { get; set; }

        [Display(Order = 9)]
        public virtual Fund2 Fund { get; set; }

        [Column("fund_id"), Display(Name = "Fund", Order = 10), JsonProperty("fundId"), Required]
        public virtual Guid? FundId { get; set; }

        [Column("content"), CustomValidation(typeof(BudgetGroup), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 11), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Version)} = {Version}, {nameof(BudgetId)} = {BudgetId}, {nameof(GroupId)} = {GroupId}, {nameof(FiscalYearId)} = {FiscalYearId}, {nameof(FundId)} = {FundId}, {nameof(Content)} = {Content} }}";

        public static BudgetGroup2 FromJObject(JObject jObject) => jObject != null ? new BudgetGroup2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Version = (int?)jObject.SelectToken("_version"),
            BudgetId = (Guid?)jObject.SelectToken("budgetId"),
            GroupId = (Guid?)jObject.SelectToken("groupId"),
            FiscalYearId = (Guid?)jObject.SelectToken("fiscalYearId"),
            FundId = (Guid?)jObject.SelectToken("fundId"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("_version", Version),
            new JProperty("budgetId", BudgetId),
            new JProperty("groupId", GroupId),
            new JProperty("fiscalYearId", FiscalYearId),
            new JProperty("fundId", FundId)).RemoveNullAndEmptyProperties();
    }
}
