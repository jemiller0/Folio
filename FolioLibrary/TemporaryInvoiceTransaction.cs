using NJsonSchema;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    [Table("temporary_invoice_transactions", Schema = "diku_mod_finance_storage")]
    public partial class TemporaryInvoiceTransaction
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.TemporaryInvoiceTransaction.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(TemporaryInvoiceTransaction), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Display(Name = "Invoice Transaction Summary", Order = 3)]
        public virtual InvoiceTransactionSummary InvoiceTransactionSummary { get; set; }

        [Column("sourceinvoiceid"), Display(Name = "Invoice Transaction Summary", Order = 4), Editable(false), ForeignKey("InvoiceTransactionSummary")]
        public virtual Guid? Sourceinvoiceid { get; set; }

        [Column("paymentencumbranceid"), Display(Name = "Transaction", Order = 5), Editable(false), ForeignKey("Transaction")]
        public virtual Guid? Paymentencumbranceid { get; set; }

        [Display(Order = 6)]
        public virtual Transaction Transaction { get; set; }

        [Column("fromfundid"), Display(Name = "Fund", Order = 7), Editable(false), ForeignKey("Fund")]
        public virtual Guid? Fromfundid { get; set; }

        [Display(Order = 8), InverseProperty("TemporaryInvoiceTransactions")]
        public virtual Fund Fund { get; set; }

        [Display(Name = "Fund 1", Order = 9), InverseProperty("TemporaryInvoiceTransactions1")]
        public virtual Fund Fund1 { get; set; }

        [Column("tofundid"), Display(Name = "Fund 1", Order = 10), Editable(false), ForeignKey("Fund1")]
        public virtual Guid? Tofundid { get; set; }

        [Display(Name = "Fiscal Year", Order = 11)]
        public virtual FiscalYear FiscalYear { get; set; }

        [Column("fiscalyearid"), Display(Name = "Fiscal Year", Order = 12), Editable(false), ForeignKey("FiscalYear")]
        public virtual Guid? Fiscalyearid { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(Sourceinvoiceid)} = {Sourceinvoiceid}, {nameof(Paymentencumbranceid)} = {Paymentencumbranceid}, {nameof(Fromfundid)} = {Fromfundid}, {nameof(Tofundid)} = {Tofundid}, {nameof(Fiscalyearid)} = {Fiscalyearid} }}";
    }
}
