using Newtonsoft.Json;
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
    [Table("fiscal_year", Schema = "uchicago_mod_finance_storage")]
    public partial class FiscalYear
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.FiscalYear.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(FiscalYear), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual string CreationUserId { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<BudgetGroup> BudgetGroups { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Budget> Budgets { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Ledger> Ledgers { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<RolloverBudget> RolloverBudgets { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Rollover> Rollovers { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Rollover> Rollovers1 { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Transaction> Transactions { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Transaction> Transactions1 { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId} }}";

        public static FiscalYear FromJObject(JObject jObject) => jObject != null ? new FiscalYear
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToUniversalTime(),
            CreationUserId = (string)jObject.SelectToken("metadata.createdByUserId")
        } : null;

        public JObject ToJObject() => JsonConvert.DeserializeObject<JObject>(Content, FolioDapperContext.LocalTimeJsonSerializationSettings);
    }
}
