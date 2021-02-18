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
    // uc.group_fund_fiscal_years -> diku_mod_finance_storage.group_fund_fiscal_year
    // GroupFundFiscalYear2 -> GroupFundFiscalYear
    [DisplayColumn(nameof(Id)), DisplayName("Group Fund Fiscal Years"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("group_fund_fiscal_years", Schema = "uc")]
    public partial class GroupFundFiscalYear2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.GroupFundFiscalYear.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Order = 2)]
        public virtual Budget2 Budget { get; set; }

        [Column("budget_id"), Display(Name = "Budget", Order = 3), JsonProperty("budgetId")]
        public virtual Guid? BudgetId { get; set; }

        [Display(Order = 4)]
        public virtual FinanceGroup2 Group { get; set; }

        [Column("group_id"), Display(Name = "Group", Order = 5), JsonProperty("groupId"), Required]
        public virtual Guid? GroupId { get; set; }

        [Display(Name = "Fiscal Year", Order = 6)]
        public virtual FiscalYear2 FiscalYear { get; set; }

        [Column("fiscal_year_id"), Display(Name = "Fiscal Year", Order = 7), JsonProperty("fiscalYearId"), Required]
        public virtual Guid? FiscalYearId { get; set; }

        [Display(Order = 8)]
        public virtual Fund2 Fund { get; set; }

        [Column("fund_id"), Display(Name = "Fund", Order = 9), JsonProperty("fundId"), Required]
        public virtual Guid? FundId { get; set; }

        [Column("content"), CustomValidation(typeof(GroupFundFiscalYear), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 10), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(BudgetId)} = {BudgetId}, {nameof(GroupId)} = {GroupId}, {nameof(FiscalYearId)} = {FiscalYearId}, {nameof(FundId)} = {FundId}, {nameof(Content)} = {Content} }}";

        public static GroupFundFiscalYear2 FromJObject(JObject jObject) => jObject != null ? new GroupFundFiscalYear2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            BudgetId = (Guid?)jObject.SelectToken("budgetId"),
            GroupId = (Guid?)jObject.SelectToken("groupId"),
            FiscalYearId = (Guid?)jObject.SelectToken("fiscalYearId"),
            FundId = (Guid?)jObject.SelectToken("fundId"),
            Content = jObject.ToString()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("budgetId", BudgetId),
            new JProperty("groupId", GroupId),
            new JProperty("fiscalYearId", FiscalYearId),
            new JProperty("fundId", FundId)).RemoveNullAndEmptyProperties();
    }
}
