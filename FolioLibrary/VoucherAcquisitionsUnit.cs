using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.voucher_acquisitions_units -> diku_mod_invoice_storage.vouchers
    // VoucherAcquisitionsUnit -> Voucher
    [DisplayColumn(nameof(Id)), DisplayName("Voucher Acquisitions Units"), Table("voucher_acquisitions_units", Schema = "uc")]
    public partial class VoucherAcquisitionsUnit
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Voucher2 Voucher { get; set; }

        [Column("voucher_id"), Display(Name = "Voucher", Order = 3), Required]
        public virtual Guid? VoucherId { get; set; }

        [Display(Name = "Acquisitions Unit", Order = 4)]
        public virtual AcquisitionsUnit2 AcquisitionsUnit { get; set; }

        [Column("acquisitions_unit_id"), Display(Name = "Acquisitions Unit", Order = 5), Required]
        public virtual Guid? AcquisitionsUnitId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(VoucherId)} = {VoucherId}, {nameof(AcquisitionsUnitId)} = {AcquisitionsUnitId} }}";

        public static VoucherAcquisitionsUnit FromJObject(JValue jObject) => jObject != null ? new VoucherAcquisitionsUnit
        {
            AcquisitionsUnitId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(AcquisitionsUnitId);
    }
}
