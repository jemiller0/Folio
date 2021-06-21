using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.voucher_item_invoice_items -> diku_mod_invoice_storage.voucher_lines
    // VoucherItemInvoiceItem -> VoucherItem
    [DisplayColumn(nameof(Id)), DisplayName("Voucher Item Invoice Items"), Table("voucher_item_invoice_items", Schema = "uc")]
    public partial class VoucherItemInvoiceItem
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Voucher Item", Order = 2)]
        public virtual VoucherItem2 VoucherItem { get; set; }

        [Column("voucher_item_id"), Display(Name = "Voucher Item", Order = 3)]
        public virtual Guid? VoucherItemId { get; set; }

        [Display(Name = "Invoice Item", Order = 4)]
        public virtual InvoiceItem2 InvoiceItem { get; set; }

        [Column("invoice_item_id"), Display(Name = "Invoice Item", Order = 5)]
        public virtual Guid? InvoiceItemId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(VoucherItemId)} = {VoucherItemId}, {nameof(InvoiceItemId)} = {InvoiceItemId} }}";

        public static VoucherItemInvoiceItem FromJObject(JValue jObject) => jObject != null ? new VoucherItemInvoiceItem
        {
            InvoiceItemId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(InvoiceItemId);
    }
}
