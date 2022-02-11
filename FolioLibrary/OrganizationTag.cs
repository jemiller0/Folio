using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.organization_tags -> uchicago_mod_organizations_storage.organizations
    // OrganizationTag -> Organization
    [DisplayColumn(nameof(Content)), DisplayName("Organization Tags"), Table("organization_tags", Schema = "uc")]
    public partial class OrganizationTag
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Organization2 Organization { get; set; }

        [Column("organization_id"), Display(Name = "Organization", Order = 3), Required]
        public virtual Guid? OrganizationId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationId)} = {OrganizationId}, {nameof(Content)} = {Content} }}";

        public static OrganizationTag FromJObject(JValue jObject) => jObject != null ? new OrganizationTag
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
