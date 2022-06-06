using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.holding_donors -> uc.holding_notes
    // HoldingDonor -> HoldingNote
    [DisplayColumn(nameof(Id)), DisplayName("Holding Donors"), Table("holding_donors", Schema = "uc")]
    public partial class HoldingDonor
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string HoldingDonorKey => Id == null || HoldingId == null ? null : $"{Id},{HoldingId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return HoldingDonorKey == ((HoldingDonor)obj).HoldingDonorKey;
        }

        public override int GetHashCode() => HoldingDonorKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Holding2 Holding { get; set; }

        [Column("holding_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? HoldingId { get; set; }

        [Column("donor_code"), Display(Name = "Donor Code", Order = 4), StringLength(1024)]
        public virtual string DonorCode { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(HoldingId)} = {HoldingId}, {nameof(DonorCode)} = {DonorCode} }}";

        public static HoldingDonor FromJObject(JValue jObject) => jObject != null ? new HoldingDonor
        {
            DonorCode = (string)jObject
        } : null;

        public JValue ToJObject() => throw new NotImplementedException();
    }
}
