using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.invoice_item_tags -> uchicago_mod_invoice_storage.invoice_lines
    // InvoiceItemTag -> InvoiceItem
    [DisplayColumn(nameof(Content)), DisplayName("Invoice Item Tags"), Table("invoice_item_tags", Schema = "uc")]
    public partial class InvoiceItemTag
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Invoice Item", Order = 2)]
        public virtual InvoiceItem2 InvoiceItem { get; set; }

        [Column("invoice_item_id"), Display(Name = "Invoice Item", Order = 3), Required]
        public virtual Guid? InvoiceItemId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InvoiceItemId)} = {InvoiceItemId}, {nameof(Content)} = {Content} }}";

        public static InvoiceItemTag FromJObject(JValue jObject) => jObject != null ? new InvoiceItemTag
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
