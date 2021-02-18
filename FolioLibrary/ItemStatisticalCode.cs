using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.item_statistical_codes -> diku_mod_inventory_storage.item
    // ItemStatisticalCode -> Item
    [DisplayColumn(nameof(Id)), DisplayName("Item Statistical Codes"), Table("item_statistical_codes", Schema = "uc")]
    public partial class ItemStatisticalCode
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string ItemStatisticalCodeKey => Id == null || ItemId == null ? null : $"{Id},{ItemId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return ItemStatisticalCodeKey == ((ItemStatisticalCode)obj).ItemStatisticalCodeKey;
        }

        public override int GetHashCode() => ItemStatisticalCodeKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Item2 Item { get; set; }

        [Column("item_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? ItemId { get; set; }

        [Display(Name = "Statistical Code", Order = 4)]
        public virtual StatisticalCode2 StatisticalCode { get; set; }

        [Column("statistical_code_id"), Display(Name = "Statistical Code", Order = 5), Required]
        public virtual Guid? StatisticalCodeId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ItemId)} = {ItemId}, {nameof(StatisticalCodeId)} = {StatisticalCodeId} }}";

        public static ItemStatisticalCode FromJObject(JValue jObject) => jObject != null ? new ItemStatisticalCode
        {
            StatisticalCodeId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(StatisticalCodeId);
    }
}
