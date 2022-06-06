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

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ItemId)} = {ItemId}, {nameof(DonorCode)} = {DonorCode} }}";

        public static ItemDonor FromJObject(JValue jObject) => jObject != null ? new ItemDonor
        {
            DonorCode = (string)jObject
        } : null;

        public JValue ToJObject() => throw new NotImplementedException();
    }
}
