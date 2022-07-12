using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.item_donors -> uc.item_notes
    // ItemDonor -> ItemNote
    [DisplayColumn(nameof(Id)), DisplayName("Item Donors"), Table("item_donors", Schema = "uc")]
    public partial class ItemDonor
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string ItemDonorKey => Id == null || ItemId == null ? null : $"{Id},{ItemId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return ItemDonorKey == ((ItemDonor)obj).ItemDonorKey;
        }

        public override int GetHashCode() => ItemDonorKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Item2 Item { get; set; }

        [Column("item_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? ItemId { get; set; }

        [Column("donor_code"), Display(Name = "Donor Code", Order = 4), StringLength(1024)]
        public virtual string DonorCode { get; set; }

        [Column("created_date"), ScaffoldColumn(false)]
        public virtual DateTime? CreationTime { get; set; }

        [InverseProperty("ItemDonors"), ScaffoldColumn(false)]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 7), Editable(false)]
        public virtual Guid? CreationUserId { get; set; }

        [Column("updated_date"), ScaffoldColumn(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [InverseProperty("ItemDonors1"), ScaffoldColumn(false)]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 10), Editable(false)]
        public virtual Guid? LastWriteUserId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ItemId)} = {ItemId}, {nameof(DonorCode)} = {DonorCode}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId} }}";

        public static ItemDonor FromJObject(JObject jObject) => throw new NotImplementedException();

        public JValue ToJObject() => throw new NotImplementedException();
    }
}
