using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    // uc.order_transaction_summaries -> diku_mod_finance_storage.order_transaction_summaries
    // OrderTransactionSummary2 -> OrderTransactionSummary
    [DisplayColumn(nameof(Id)), DisplayName("Order Transaction Summaries"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("order_transaction_summaries", Schema = "uc")]
    public partial class OrderTransactionSummary2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.OrderTransactionSummary.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Name = "Order 2", Order = 1), Editable(false), ForeignKey("Order2"), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Name = "Order 2", Order = 2)]
        public virtual Order2 Order2 { get; set; }

        [Column("num_transactions"), Display(Name = "Num Transactions", Order = 3), JsonProperty("numTransactions"), Required]
        public virtual int? NumTransactions { get; set; }

        [Column("content"), CustomValidation(typeof(OrderTransactionSummary), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 4), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(NumTransactions)} = {NumTransactions}, {nameof(Content)} = {Content} }}";

        public static OrderTransactionSummary2 FromJObject(JObject jObject) => jObject != null ? new OrderTransactionSummary2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            NumTransactions = (int?)jObject.SelectToken("numTransactions"),
            Content = jObject.ToString()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("numTransactions", NumTransactions)).RemoveNullAndEmptyProperties();
    }
}
