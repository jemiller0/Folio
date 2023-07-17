using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("ledger_fiscal_year_rollover_budget", Schema = "uchicago_mod_finance_storage")]
    public partial class LedgerFiscalYearRolloverBudget
    {
        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual string CreationUserId { get; set; }

        [Display(Name = "Ledger Rollover", Order = 5)]
        public virtual LedgerRollover LedgerRollover { get; set; }

        [Column("ledgerrolloverid"), Display(Name = "Ledger Rollover", Order = 6), ForeignKey("LedgerRollover")]
        public virtual Guid? Ledgerrolloverid { get; set; }

        [Display(Order = 7)]
        public virtual Fund Fund { get; set; }

        [Column("fundid"), Display(Name = "Fund", Order = 8), ForeignKey("Fund")]
        public virtual Guid? Fundid { get; set; }

        [Display(Name = "Fiscal Year", Order = 9)]
        public virtual FiscalYear FiscalYear { get; set; }

        [Column("fiscalyearid"), Display(Name = "Fiscal Year", Order = 10), ForeignKey("FiscalYear")]
        public virtual Guid? Fiscalyearid { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(Ledgerrolloverid)} = {Ledgerrolloverid}, {nameof(Fundid)} = {Fundid}, {nameof(Fiscalyearid)} = {Fiscalyearid} }}";

        public static LedgerFiscalYearRolloverBudget FromJObject(JValue jObject) => jObject != null ? new LedgerFiscalYearRolloverBudget
        {
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => JsonConvert.DeserializeObject<JObject>(Content, FolioDapperContext.LocalTimeJsonSerializationSettings);
    }
}
