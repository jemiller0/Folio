using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.languages -> uchicago_mod_inventory_storage.instance
    // Language -> Instance
    [DisplayColumn(nameof(Content)), Table("languages", Schema = "uc")]
    public partial class Language
    {
        [NotMapped, ScaffoldColumn(false)]
        public virtual string LanguageKey => Id == null || InstanceId == null ? null : $"{Id},{InstanceId}";

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            return LanguageKey == ((Language)obj).LanguageKey;
        }

        public override int GetHashCode() => LanguageKey?.GetHashCode() ?? 0;

        [Column("id", Order = 1), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id", Order = 3), ScaffoldColumn(false)]
        public virtual Guid? InstanceId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InstanceId)} = {InstanceId}, {nameof(Content)} = {Content} }}";

        public static Language FromJObject(JValue jObject) => jObject != null ? new Language
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
