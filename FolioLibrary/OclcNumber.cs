using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.oclc_numbers -> uc.identifiers
    // OclcNumber -> Identifier
    [DisplayColumn(nameof(Content)), DisplayName("OCLC Numbers"), Table("oclc_numbers", Schema = "uc")]
    public partial class OclcNumber
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string OclcNumberKey => Id == null || InstanceId == null ? null : $"{Id},{InstanceId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return OclcNumberKey == ((OclcNumber)obj).OclcNumberKey;
        }

        public override int GetHashCode() => OclcNumberKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id", Order = 3), Display(Name = "Instance", Order = 3)]
        public virtual Guid? InstanceId { get; set; }

        [Column("content"), Display(Order = 4), Editable(false)]
        public virtual int? Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InstanceId)} = {InstanceId}, {nameof(Content)} = {Content} }}";

        public static OclcNumber FromJObject(JValue jObject) => jObject != null ? new OclcNumber
        {
            Content = (int?)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
