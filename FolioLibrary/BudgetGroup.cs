using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    [Table("group_fund_fiscal_year", Schema = "uchicago_mod_finance_storage")]
    public partial class BudgetGroup
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

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(BudgetGroup), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Display(Order = 3)]
        public virtual Budget Budget { get; set; }

        [Column("budgetid"), Display(Name = "Budget", Order = 4), ForeignKey("Budget")]
        public virtual Guid? Budgetid { get; set; }

        [Display(Name = "Finance Group", Order = 5)]
        public virtual FinanceGroup FinanceGroup { get; set; }

        [Column("groupid"), Display(Name = "Finance Group", Order = 6), ForeignKey("FinanceGroup")]
        public virtual Guid? Groupid { get; set; }

        [Display(Order = 7)]
        public virtual Fund Fund { get; set; }

        [Column("fundid"), Display(Name = "Fund", Order = 8), ForeignKey("Fund")]
        public virtual Guid? Fundid { get; set; }

        [Display(Name = "Fiscal Year", Order = 9)]
        public virtual FiscalYear FiscalYear { get; set; }

        [Column("fiscalyearid"), Display(Name = "Fiscal Year", Order = 10), ForeignKey("FiscalYear")]
        public virtual Guid? Fiscalyearid { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(Budgetid)} = {Budgetid}, {nameof(Groupid)} = {Groupid}, {nameof(Fundid)} = {Fundid}, {nameof(Fiscalyearid)} = {Fiscalyearid} }}";

        public static BudgetGroup FromJObject(JObject jObject) => jObject != null ? new BudgetGroup
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            Budgetid = (Guid?)jObject.SelectToken("budgetId"),
            Groupid = (Guid?)jObject.SelectToken("groupId"),
            Fundid = (Guid?)jObject.SelectToken("fundId"),
            Fiscalyearid = (Guid?)jObject.SelectToken("fiscalYearId")
        } : null;

        public JObject ToJObject() => JsonConvert.DeserializeObject<JObject>(Content, FolioDapperContext.LocalTimeJsonSerializationSettings);
    }
}
