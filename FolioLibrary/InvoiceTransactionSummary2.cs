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
    // uc.invoice_transaction_summaries -> diku_mod_finance_storage.invoice_transaction_summaries
    // InvoiceTransactionSummary2 -> InvoiceTransactionSummary
    [JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("invoice_transaction_summaries", Schema = "uc")]
    public partial class InvoiceTransactionSummary2
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

        [Column("id"), Display(Name = "Invoice 2", Order = 1), Editable(false), ForeignKey("Invoice2"), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Name = "Invoice 2", Order = 2)]
        public virtual Invoice2 Invoice2 { get; set; }

        [Column("num_pending_payments"), Display(Name = "Num Pending Payments", Order = 3), JsonProperty("numPendingPayments"), Required]
        public virtual int? NumPendingPayments { get; set; }

        [Column("num_payments_credits"), Display(Name = "Num Payments Credits", Order = 4), JsonProperty("numPaymentsCredits"), Required]
        public virtual int? NumPaymentsCredits { get; set; }

        [Column("content"), CustomValidation(typeof(InvoiceTransactionSummary), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 5), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(NumPendingPayments)} = {NumPendingPayments}, {nameof(NumPaymentsCredits)} = {NumPaymentsCredits}, {nameof(Content)} = {Content} }}";

        public static InvoiceTransactionSummary2 FromJObject(JObject jObject) => jObject != null ? new InvoiceTransactionSummary2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            NumPendingPayments = (int?)jObject.SelectToken("numPendingPayments"),
            NumPaymentsCredits = (int?)jObject.SelectToken("numPaymentsCredits"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("numPendingPayments", NumPendingPayments),
            new JProperty("numPaymentsCredits", NumPaymentsCredits)).RemoveNullAndEmptyProperties();
    }
}
