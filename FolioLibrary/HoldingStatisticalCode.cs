using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.holding_statistical_codes -> uchicago_mod_inventory_storage.holdings_record
    // HoldingStatisticalCode -> Holding
    [DisplayColumn(nameof(Id)), DisplayName("Holding Statistical Codes"), Table("holding_statistical_codes", Schema = "uc")]
    public partial class HoldingStatisticalCode
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string HoldingStatisticalCodeKey => Id == null || HoldingId == null ? null : $"{Id},{HoldingId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return HoldingStatisticalCodeKey == ((HoldingStatisticalCode)obj).HoldingStatisticalCodeKey;
        }

        public override int GetHashCode() => HoldingStatisticalCodeKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Holding2 Holding { get; set; }

        [Column("holding_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? HoldingId { get; set; }

        [Display(Name = "Statistical Code", Order = 4)]
        public virtual StatisticalCode2 StatisticalCode { get; set; }

        [Column("statistical_code_id"), Display(Name = "Statistical Code", Order = 5), Required]
        public virtual Guid? StatisticalCodeId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(HoldingId)} = {HoldingId}, {nameof(StatisticalCodeId)} = {StatisticalCodeId} }}";

        public static HoldingStatisticalCode FromJObject(JValue jObject) => jObject != null ? new HoldingStatisticalCode
        {
            StatisticalCodeId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(StatisticalCodeId);
    }
}
