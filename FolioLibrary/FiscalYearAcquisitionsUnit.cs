using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.fiscal_year_acquisitions_units -> diku_mod_finance_storage.fiscal_year
    // FiscalYearAcquisitionsUnit -> FiscalYear
    [DisplayColumn(nameof(Id)), DisplayName("Fiscal Year Acquisitions Units"), Table("fiscal_year_acquisitions_units", Schema = "uc")]
    public partial class FiscalYearAcquisitionsUnit
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Fiscal Year", Order = 2)]
        public virtual FiscalYear2 FiscalYear { get; set; }

        [Column("fiscal_year_id"), Display(Name = "Fiscal Year", Order = 3), Required]
        public virtual Guid? FiscalYearId { get; set; }

        [Display(Name = "Acquisitions Unit", Order = 4)]
        public virtual AcquisitionsUnit2 AcquisitionsUnit { get; set; }

        [Column("acquisitions_unit_id"), Display(Name = "Acquisitions Unit", Order = 5), Required]
        public virtual Guid? AcquisitionsUnitId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(FiscalYearId)} = {FiscalYearId}, {nameof(AcquisitionsUnitId)} = {AcquisitionsUnitId} }}";

        public static FiscalYearAcquisitionsUnit FromJObject(JValue jObject) => jObject != null ? new FiscalYearAcquisitionsUnit
        {
            AcquisitionsUnitId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(AcquisitionsUnitId);
    }
}
