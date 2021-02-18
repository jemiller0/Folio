using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.invoice_order_numbers -> diku_mod_invoice_storage.invoices
    // InvoiceOrderNumber -> Invoice
    [DisplayColumn(nameof(Content)), DisplayName("Invoice Order Numbers"), Table("invoice_order_numbers", Schema = "uc")]
    public partial class InvoiceOrderNumber
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Invoice2 Invoice { get; set; }

        [Column("invoice_id"), Display(Name = "Invoice", Order = 3), Required]
        public virtual Guid? InvoiceId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InvoiceId)} = {InvoiceId}, {nameof(Content)} = {Content} }}";

        public static InvoiceOrderNumber FromJObject(JValue jObject) => jObject != null ? new InvoiceOrderNumber
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
