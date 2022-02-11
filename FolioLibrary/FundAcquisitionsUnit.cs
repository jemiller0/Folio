using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.fund_acquisitions_units -> uchicago_mod_finance_storage.fund
    // FundAcquisitionsUnit -> Fund
    [DisplayColumn(nameof(Id)), DisplayName("Fund Acquisitions Units"), Table("fund_acquisitions_units", Schema = "uc")]
    public partial class FundAcquisitionsUnit
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Fund2 Fund { get; set; }

        [Column("fund_id"), Display(Name = "Fund", Order = 3), Required]
        public virtual Guid? FundId { get; set; }

        [Display(Name = "Acquisitions Unit", Order = 4)]
        public virtual AcquisitionsUnit2 AcquisitionsUnit { get; set; }

        [Column("acquisitions_unit_id"), Display(Name = "Acquisitions Unit", Order = 5), Required]
        public virtual Guid? AcquisitionsUnitId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(FundId)} = {FundId}, {nameof(AcquisitionsUnitId)} = {AcquisitionsUnitId} }}";

        public static FundAcquisitionsUnit FromJObject(JValue jObject) => jObject != null ? new FundAcquisitionsUnit
        {
            AcquisitionsUnitId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(AcquisitionsUnitId);
    }
}
