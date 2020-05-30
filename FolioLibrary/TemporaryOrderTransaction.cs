using NJsonSchema;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    [Table("temporary_order_transactions", Schema = "diku_mod_finance_storage")]
    public partial class TemporaryOrderTransaction
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.TemporaryOrderTransaction.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(TemporaryOrderTransaction), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("encumbrance_sourcepurchaseorderid"), Display(Name = "Order Transaction Summary", Order = 3), Editable(false), ForeignKey("OrderTransactionSummary")]
        public virtual Guid? EncumbranceSourcepurchaseorderid { get; set; }

        [Display(Name = "Order Transaction Summary", Order = 4)]
        public virtual OrderTransactionSummary OrderTransactionSummary { get; set; }

        [Display(Name = "Fiscal Year", Order = 5)]
        public virtual FiscalYear FiscalYear { get; set; }

        [Column("fiscalyearid"), Display(Name = "Fiscal Year", Order = 6), Editable(false), ForeignKey("FiscalYear")]
        public virtual Guid? Fiscalyearid { get; set; }

        [Column("fromfundid"), Display(Name = "Fund", Order = 7), Editable(false), ForeignKey("Fund")]
        public virtual Guid? Fromfundid { get; set; }

        [Display(Order = 8)]
        public virtual Fund Fund { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(EncumbranceSourcepurchaseorderid)} = {EncumbranceSourcepurchaseorderid}, {nameof(Fiscalyearid)} = {Fiscalyearid}, {nameof(Fromfundid)} = {Fromfundid} }}";
    }
}
