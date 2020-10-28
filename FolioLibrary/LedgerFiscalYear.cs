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
    [Table("ledgerfy", Schema = "diku_mod_finance_storage")]
    public partial class LedgerFiscalYear
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.LedgerFiscalYear.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(LedgerFiscalYear), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Display(Order = 3)]
        public virtual Ledger Ledger { get; set; }

        [Column("ledgerid"), Display(Name = "Ledger", Order = 4), ForeignKey("Ledger")]
        public virtual Guid? Ledgerid { get; set; }

        [Display(Name = "Fiscal Year", Order = 5)]
        public virtual FiscalYear FiscalYear { get; set; }

        [Column("fiscalyearid"), Display(Name = "Fiscal Year", Order = 6), ForeignKey("FiscalYear")]
        public virtual Guid? Fiscalyearid { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(Ledgerid)} = {Ledgerid}, {nameof(Fiscalyearid)} = {Fiscalyearid} }}";

        public static LedgerFiscalYear FromJObject(JObject jObject) => jObject != null ? new LedgerFiscalYear
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = jObject.ToString(),
            Ledgerid = (Guid?)jObject.SelectToken("ledgerId"),
            Fiscalyearid = (Guid?)jObject.SelectToken("fiscalYearId")
        } : null;

        public JObject ToJObject() => JObject.Parse(Content);
    }
}
