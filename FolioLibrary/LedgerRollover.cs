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
    [Table("ledger_fiscal_year_rollover", Schema = "diku_mod_finance_storage")]
    public partial class LedgerRollover
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.LedgerRollover.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(LedgerRollover), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual string CreationUserId { get; set; }

        [Display(Order = 5)]
        public virtual Ledger Ledger { get; set; }

        [Column("ledgerid"), Display(Name = "Ledger", Order = 6), ForeignKey("Ledger")]
        public virtual Guid? Ledgerid { get; set; }

        [Display(Name = "Fiscal Year", Order = 7), InverseProperty("LedgerRollovers")]
        public virtual FiscalYear FiscalYear { get; set; }

        [Column("fromfiscalyearid"), Display(Name = "Fiscal Year", Order = 8), ForeignKey("FiscalYear")]
        public virtual Guid? Fromfiscalyearid { get; set; }

        [Display(Name = "Fiscal Year 1", Order = 9), InverseProperty("LedgerRollovers1")]
        public virtual FiscalYear FiscalYear1 { get; set; }

        [Column("tofiscalyearid"), Display(Name = "Fiscal Year 1", Order = 10), ForeignKey("FiscalYear1")]
        public virtual Guid? Tofiscalyearid { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<LedgerRolloverError> LedgerRolloverErrors { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<LedgerRolloverProgress> LedgerRolloverProgresses { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(Ledgerid)} = {Ledgerid}, {nameof(Fromfiscalyearid)} = {Fromfiscalyearid}, {nameof(Tofiscalyearid)} = {Tofiscalyearid} }}";

        public static LedgerRollover FromJObject(JObject jObject) => jObject != null ? new LedgerRollover
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = jObject.ToString(),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (string)jObject.SelectToken("metadata.createdByUserId"),
            Ledgerid = (Guid?)jObject.SelectToken("ledgerId"),
            Fromfiscalyearid = (Guid?)jObject.SelectToken("fromFiscalYearId"),
            Tofiscalyearid = (Guid?)jObject.SelectToken("toFiscalYearId")
        } : null;

        public JObject ToJObject() => JObject.Parse(Content);
    }
}
