using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.finance_group_acquisitions_units -> diku_mod_finance_storage.groups
    // FinanceGroupAcquisitionsUnit -> FinanceGroup
    [DisplayColumn(nameof(Id)), DisplayName("Finance Group Acquisitions Units"), Table("finance_group_acquisitions_units", Schema = "uc")]
    public partial class FinanceGroupAcquisitionsUnit
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Finance Group", Order = 2)]
        public virtual FinanceGroup2 FinanceGroup { get; set; }

        [Column("finance_group_id"), Display(Name = "Finance Group", Order = 3), Required]
        public virtual Guid? FinanceGroupId { get; set; }

        [Display(Name = "Acquisitions Unit", Order = 4)]
        public virtual AcquisitionsUnit2 AcquisitionsUnit { get; set; }

        [Column("acquisitions_unit_id"), Display(Name = "Acquisitions Unit", Order = 5), Required]
        public virtual Guid? AcquisitionsUnitId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(FinanceGroupId)} = {FinanceGroupId}, {nameof(AcquisitionsUnitId)} = {AcquisitionsUnitId} }}";

        public static FinanceGroupAcquisitionsUnit FromJObject(JValue jObject) => jObject != null ? new FinanceGroupAcquisitionsUnit
        {
            AcquisitionsUnitId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(AcquisitionsUnitId);
    }
}
