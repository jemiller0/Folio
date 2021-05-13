using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    [Table("budget", Schema = "diku_mod_finance_storage")]
    public partial class Budget
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Budget.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(Budget), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual string CreationUserId { get; set; }

        [Display(Order = 5)]
        public virtual Fund Fund { get; set; }

        [Column("fundid"), Display(Name = "Fund", Order = 6)]
        public virtual Guid? FundId { get; set; }

        [Display(Name = "Fiscal Year", Order = 7)]
        public virtual FiscalYear FiscalYear { get; set; }

        [Column("fiscalyearid"), Display(Name = "Fiscal Year", Order = 8)]
        public virtual Guid? FiscalYearId { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<BudgetExpenseClass> BudgetExpenseClasses { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<BudgetGroup> BudgetGroups { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(FundId)} = {FundId}, {nameof(FiscalYearId)} = {FiscalYearId} }}";

        public static Budget FromJObject(JObject jObject) => jObject != null ? new Budget
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = jObject.ToString(),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (string)jObject.SelectToken("metadata.createdByUserId"),
            FundId = (Guid?)jObject.SelectToken("fundId"),
            FiscalYearId = (Guid?)jObject.SelectToken("fiscalYearId")
        } : null;

        public JObject ToJObject() => JObject.Parse(Content);
    }
}
