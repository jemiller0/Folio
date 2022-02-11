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
    [Table("transaction", Schema = "uchicago_mod_finance_storage")]
    public partial class Transaction
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Transaction.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(Transaction), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual string CreationUserId { get; set; }

        [Display(Name = "Fiscal Year", Order = 5), InverseProperty("Transactions")]
        public virtual FiscalYear FiscalYear { get; set; }

        [Column("fiscalyearid"), Display(Name = "Fiscal Year", Order = 6), ForeignKey("FiscalYear")]
        public virtual Guid? Fiscalyearid { get; set; }

        [Column("fromfundid"), Display(Name = "Fund", Order = 7), ForeignKey("Fund")]
        public virtual Guid? Fromfundid { get; set; }

        [Display(Order = 8), InverseProperty("Transactions")]
        public virtual Fund Fund { get; set; }

        [Display(Name = "Fiscal Year 1", Order = 9), InverseProperty("Transactions1")]
        public virtual FiscalYear FiscalYear1 { get; set; }

        [Column("sourcefiscalyearid"), Display(Name = "Fiscal Year 1", Order = 10), ForeignKey("FiscalYear1")]
        public virtual Guid? Sourcefiscalyearid { get; set; }

        [Display(Name = "Fund 1", Order = 11), InverseProperty("Transactions1")]
        public virtual Fund Fund1 { get; set; }

        [Column("tofundid"), Display(Name = "Fund 1", Order = 12), ForeignKey("Fund1")]
        public virtual Guid? Tofundid { get; set; }

        [Display(Name = "Expense Class", Order = 13)]
        public virtual ExpenseClass ExpenseClass { get; set; }

        [Column("expenseclassid"), Display(Name = "Expense Class", Order = 14), ForeignKey("ExpenseClass")]
        public virtual Guid? Expenseclassid { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(Fiscalyearid)} = {Fiscalyearid}, {nameof(Fromfundid)} = {Fromfundid}, {nameof(Sourcefiscalyearid)} = {Sourcefiscalyearid}, {nameof(Tofundid)} = {Tofundid}, {nameof(Expenseclassid)} = {Expenseclassid} }}";

        public static Transaction FromJObject(JObject jObject) => jObject != null ? new Transaction
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToUniversalTime(),
            CreationUserId = (string)jObject.SelectToken("metadata.createdByUserId"),
            Fiscalyearid = (Guid?)jObject.SelectToken("fiscalYearId"),
            Fromfundid = (Guid?)jObject.SelectToken("fromFundId"),
            Sourcefiscalyearid = (Guid?)jObject.SelectToken("sourceFiscalYearId"),
            Tofundid = (Guid?)jObject.SelectToken("toFundId"),
            Expenseclassid = (Guid?)jObject.SelectToken("expenseClassId")
        } : null;

        public JObject ToJObject() => JsonConvert.DeserializeObject<JObject>(Content, FolioDapperContext.LocalTimeJsonSerializationSettings);
    }
}
