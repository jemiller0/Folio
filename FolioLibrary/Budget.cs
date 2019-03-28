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
                var js = JsonSchema4.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
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

        [Column("fund_id"), Display(Name = "Fund", Order = 6), Editable(false)]
        public virtual Guid? FundId { get; set; }

        [Display(Name = "Fiscal Year", Order = 7)]
        public virtual FiscalYear FiscalYear { get; set; }

        [Column("fiscal_year_id"), Display(Name = "Fiscal Year", Order = 8), Editable(false)]
        public virtual Guid? FiscalYearId { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<FundDistribution> FundDistributions { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Transaction> Transactions { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(FundId)} = {FundId}, {nameof(FiscalYearId)} = {FiscalYearId} }}";
    }
}
