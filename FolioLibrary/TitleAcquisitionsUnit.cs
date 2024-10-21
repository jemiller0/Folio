using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.title_acquisitions_units -> uchicago_mod_orders_storage.titles
    // TitleAcquisitionsUnit -> Title
    [DisplayColumn(nameof(Id)), DisplayName("Title Acquisitions Units"), Table("title_acquisitions_units", Schema = "uc")]
    public partial class TitleAcquisitionsUnit
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Title2 Title { get; set; }

        [Column("title_id"), Display(Name = "Title", Order = 3)]
        public virtual Guid? TitleId { get; set; }

        [Display(Name = "Acquisitions Unit", Order = 4)]
        public virtual AcquisitionsUnit2 AcquisitionsUnit { get; set; }

        [Column("acquisitions_unit_id"), Display(Name = "Acquisitions Unit", Order = 5)]
        public virtual Guid? AcquisitionsUnitId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(TitleId)} = {TitleId}, {nameof(AcquisitionsUnitId)} = {AcquisitionsUnitId} }}";

        public static TitleAcquisitionsUnit FromJObject(JValue jObject) => jObject != null ? new TitleAcquisitionsUnit
        {
            AcquisitionsUnitId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(AcquisitionsUnitId);
    }
}
