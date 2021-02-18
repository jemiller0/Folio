using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.interface_type -> diku_mod_organizations_storage.interfaces
    // InterfaceType -> Interface
    [DisplayColumn(nameof(Content)), DisplayName("Interface Types"), Table("interface_type", Schema = "uc")]
    public partial class InterfaceType
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Interface2 Interface { get; set; }

        [Column("interface_id"), Display(Name = "Interface", Order = 3), Required]
        public virtual Guid? InterfaceId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InterfaceId)} = {InterfaceId}, {nameof(Content)} = {Content} }}";

        public static InterfaceType FromJObject(JValue jObject) => jObject != null ? new InterfaceType
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
