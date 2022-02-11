using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.source_marc_fields -> uchicago_mod_inventory_storage.instance_source_marc
    // SourceMarcField -> InstanceSourceMarc
    [DisplayColumn(nameof(Content)), DisplayName("Source Marc Fields"), Table("source_marc_fields", Schema = "uc")]
    public partial class SourceMarcField
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Source Marc", Order = 2)]
        public virtual SourceMarc SourceMarc { get; set; }

        [Column("source_marc_id"), Display(Name = "Source Marc", Order = 3), Required]
        public virtual Guid? SourceMarcId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(SourceMarcId)} = {SourceMarcId}, {nameof(Content)} = {Content} }}";

        public static SourceMarcField FromJObject(JValue jObject) => jObject != null ? new SourceMarcField
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
