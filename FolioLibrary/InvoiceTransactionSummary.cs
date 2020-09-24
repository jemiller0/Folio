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
    [Table("invoice_transaction_summaries", Schema = "diku_mod_finance_storage")]
    public partial class InvoiceTransactionSummary
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.InvoiceTransactionSummary.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(InvoiceTransactionSummary), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<TemporaryInvoiceTransaction> TemporaryInvoiceTransactions { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content} }}";

        public static InvoiceTransactionSummary FromJObject(JObject jObject) => new InvoiceTransactionSummary
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = jObject.ToString()
        };

        public JObject ToJObject() => JObject.Parse(Content);
    }
}
