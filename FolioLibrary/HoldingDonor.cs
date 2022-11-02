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
        public virtual Guid? Id { get; set; }

        [Display(Order = 2)]
        public virtual Holding2 Holding { get; set; }

        [Column("holding_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? HoldingId { get; set; }

        [Column("donor_code"), Display(Name = "Donor Code", Order = 4), StringLength(1024)]
        public virtual string DonorCode { get; set; }

        [Column("created_date"), ScaffoldColumn(false)]
        public virtual DateTime? CreationTime { get; set; }

        [InverseProperty("HoldingDonors"), ScaffoldColumn(false)]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 7), Editable(false)]
        public virtual Guid? CreationUserId { get; set; }

        [Column("updated_date"), ScaffoldColumn(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [InverseProperty("HoldingDonors1"), ScaffoldColumn(false)]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 10), Editable(false)]
        public virtual Guid? LastWriteUserId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(HoldingId)} = {HoldingId}, {nameof(DonorCode)} = {DonorCode}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId} }}";

        public static HoldingDonor FromJObject(JObject jObject) => throw new NotImplementedException();

        public JValue ToJObject() => throw new NotImplementedException();
    }
}
