using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.ledger_acquisitions_units -> uchicago_mod_finance_storage.ledger
    // LedgerAcquisitionsUnit -> Ledger
    [DisplayColumn(nameof(Id)), DisplayName("Ledger Acquisitions Units"), Table("ledger_acquisitions_units", Schema = "uc")]
    public partial class LedgerAcquisitionsUnit
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Ledger2 Ledger { get; set; }

        [Column("ledger_id"), Display(Name = "Ledger", Order = 3), Required]
        public virtual Guid? LedgerId { get; set; }

        [Display(Name = "Acquisitions Unit", Order = 4)]
        public virtual AcquisitionsUnit2 AcquisitionsUnit { get; set; }

        [Column("acquisitions_unit_id"), Display(Name = "Acquisitions Unit", Order = 5), Required]
        public virtual Guid? AcquisitionsUnitId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(LedgerId)} = {LedgerId}, {nameof(AcquisitionsUnitId)} = {AcquisitionsUnitId} }}";

        public static LedgerAcquisitionsUnit FromJObject(JValue jObject) => jObject != null ? new LedgerAcquisitionsUnit
        {
            AcquisitionsUnitId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(AcquisitionsUnitId);
    }
}
