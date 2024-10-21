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
    [Table("ledger_fiscal_year_rollover_budget", Schema = "uchicago_mod_finance_storage")]
    public partial class RolloverBudget
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

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(RolloverBudget), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual string CreationUserId { get; set; }

        [Column("ledgerrolloverid"), Display(Name = "Rollover", Order = 5), ForeignKey("Rollover")]
        public virtual Guid? Ledgerrolloverid { get; set; }

        [Display(Order = 6)]
        public virtual Rollover Rollover { get; set; }

        [Display(Order = 7)]
        public virtual Fund Fund { get; set; }

        [Column("fundid"), Display(Name = "Fund", Order = 8), ForeignKey("Fund")]
        public virtual Guid? Fundid { get; set; }

        [Display(Name = "Fiscal Year", Order = 9)]
        public virtual FiscalYear FiscalYear { get; set; }

        [Column("fiscalyearid"), Display(Name = "Fiscal Year", Order = 10), ForeignKey("FiscalYear")]
        public virtual Guid? Fiscalyearid { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(Ledgerrolloverid)} = {Ledgerrolloverid}, {nameof(Fundid)} = {Fundid}, {nameof(Fiscalyearid)} = {Fiscalyearid} }}";

        public static RolloverBudget FromJObject(JObject jObject) => jObject != null ? new RolloverBudget
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToUniversalTime(),
            CreationUserId = (string)jObject.SelectToken("metadata.createdByUserId"),
            Ledgerrolloverid = (Guid?)jObject.SelectToken("ledgerRolloverId"),
            Fundid = (Guid?)jObject.SelectToken("fundId"),
            Fiscalyearid = (Guid?)jObject.SelectToken("fiscalYearId")
        } : null;

        public JObject ToJObject() => JsonConvert.DeserializeObject<JObject>(Content, FolioDapperContext.LocalTimeJsonSerializationSettings);
    }
}
