using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.preferred_email_communications -> uchicago_mod_users.users
    // PreferredEmailCommunication -> User
    [DisplayColumn(nameof(Content)), DisplayName("Preferred Email Communications"), Table("preferred_email_communications", Schema = "uc")]
    public partial class PreferredEmailCommunication
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual User2 User { get; set; }

        [Column("user_id"), Display(Name = "User", Order = 3)]
        public virtual Guid? UserId { get; set; }

        [Column("content"), Display(Order = 4), StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(UserId)} = {UserId}, {nameof(Content)} = {Content} }}";

        public static PreferredEmailCommunication FromJObject(JValue jObject) => jObject != null ? new PreferredEmailCommunication
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
