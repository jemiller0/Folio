using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.invoice_acquisitions_units -> uchicago_mod_invoice_storage.invoices
    // InvoiceAcquisitionsUnit -> Invoice
    [DisplayColumn(nameof(Id)), DisplayName("Invoice Acquisitions Units"), Table("invoice_acquisitions_units", Schema = "uc")]
    public partial class InvoiceAcquisitionsUnit
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Invoice2 Invoice { get; set; }

        [Column("invoice_id"), Display(Name = "Invoice", Order = 3), Required]
        public virtual Guid? InvoiceId { get; set; }

        [Display(Name = "Acquisitions Unit", Order = 4)]
        public virtual AcquisitionsUnit2 AcquisitionsUnit { get; set; }

        [Column("acquisitions_unit_id"), Display(Name = "Acquisitions Unit", Order = 5), Required]
        public virtual Guid? AcquisitionsUnitId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InvoiceId)} = {InvoiceId}, {nameof(AcquisitionsUnitId)} = {AcquisitionsUnitId} }}";

        public static InvoiceAcquisitionsUnit FromJObject(JValue jObject) => jObject != null ? new InvoiceAcquisitionsUnit
        {
            AcquisitionsUnitId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(AcquisitionsUnitId);
    }
}
