using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.administrative_notes -> uchicago_mod_inventory_storage.instance
    // AdministrativeNote -> Instance
    [DisplayColumn(nameof(Content)), DisplayName("Administrative Notes"), Table("administrative_notes", Schema = "uc")]
    public partial class AdministrativeNote
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Instance2 Instance { get; set; }

        [Column("instance_id"), Display(Name = "Instance", Order = 3)]
        public virtual Guid? InstanceId { get; set; }

        [Column("content"), Display(Order = 4), StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(InstanceId)} = {InstanceId}, {nameof(Content)} = {Content} }}";

        public static AdministrativeNote FromJObject(JValue jObject) => jObject != null ? new AdministrativeNote
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
