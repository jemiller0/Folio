using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.voucher_item_source_ids -> diku_mod_invoice_storage.voucher_lines
    // VoucherItemSourceId -> VoucherItem
    [DisplayColumn(nameof(Content)), DisplayName("Voucher Item Source Ids"), Table("voucher_item_source_ids", Schema = "uc")]
    public partial class VoucherItemSourceId
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Voucher Item", Order = 2)]
        public virtual VoucherItem2 VoucherItem { get; set; }

        [Column("voucher_item_id"), Display(Name = "Voucher Item", Order = 3), Required]
        public virtual Guid? VoucherItemId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(VoucherItemId)} = {VoucherItemId}, {nameof(Content)} = {Content} }}";

        public static VoucherItemSourceId FromJObject(JValue jObject) => jObject != null ? new VoucherItemSourceId
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
