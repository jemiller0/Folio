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
    // uc.order_invoices -> diku_mod_orders_storage.order_invoice_relationship
    // OrderInvoice2 -> OrderInvoice
    [DisplayColumn(nameof(Id)), DisplayName("Order Invoices"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("order_invoices", Schema = "uc")]
    public partial class OrderInvoice2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.OrderInvoice.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Order = 2)]
        public virtual Order2 Order { get; set; }

        [Column("order_id"), Display(Name = "Order", Order = 3), JsonProperty("purchaseOrderId"), Required]
        public virtual Guid? OrderId { get; set; }

        [Display(Order = 4)]
        public virtual Invoice2 Invoice { get; set; }

        [Column("invoice_id"), Display(Name = "Invoice", Order = 5), JsonProperty("invoiceId"), Required]
        public virtual Guid? InvoiceId { get; set; }

        [Column("content"), CustomValidation(typeof(OrderInvoice), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 6), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrderId)} = {OrderId}, {nameof(InvoiceId)} = {InvoiceId}, {nameof(Content)} = {Content} }}";

        public static OrderInvoice2 FromJObject(JObject jObject) => jObject != null ? new OrderInvoice2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            OrderId = (Guid?)jObject.SelectToken("purchaseOrderId"),
            InvoiceId = (Guid?)jObject.SelectToken("invoiceId"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("purchaseOrderId", OrderId),
            new JProperty("invoiceId", InvoiceId)).RemoveNullAndEmptyProperties();
    }
}
