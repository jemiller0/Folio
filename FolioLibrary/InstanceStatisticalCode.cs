using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.instance_statistical_codes -> diku_mod_inventory_storage.instance
    // InstanceStatisticalCode -> Instance
    [DisplayColumn(nameof(Id)), DisplayName("Instance Statistical Codes"), Table("instance_statistical_codes", Schema = "uc")]
    public partial class InstanceStatisticalCode
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string InstanceStatisticalCodeKey => Id == null || InstanceId == null ? null : $"{Id},{InstanceId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return InstanceStatisticalCodeKey == ((InstanceStatisticalCode)obj).InstanceStatisticalCodeKey;
        }

        public override int GetHashCode() => InstanceStatisticalCodeKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? InstanceId { get; set; }

        [Display(Name = "Statistical Code", Order = 4)]
        public virtual StatisticalCode2 StatisticalCode { get; set; }

        [Column("statistical_code_id"), Display(Name = "Statistical Code", Order = 5), Required]
        public virtual Guid? StatisticalCodeId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InstanceId)} = {InstanceId}, {nameof(StatisticalCodeId)} = {StatisticalCodeId} }}";

        public static InstanceStatisticalCode FromJObject(JValue jObject) => jObject != null ? new InstanceStatisticalCode
        {
            StatisticalCodeId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(StatisticalCodeId);
    }
}
