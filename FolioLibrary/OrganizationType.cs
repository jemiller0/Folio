using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.organization_organization_types -> uchicago_mod_organizations_storage.organizations
    // OrganizationType -> Organization
    [DisplayColumn(nameof(Content)), DisplayName("Organization Types"), Table("organization_organization_types", Schema = "uc")]
    public partial class OrganizationType
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Organization2 Organization { get; set; }

        [Column("organization_id"), Display(Name = "Organization", Order = 3)]
        public virtual Guid? OrganizationId { get; set; }

        [Column("content"), Display(Order = 4), StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationId)} = {OrganizationId}, {nameof(Content)} = {Content} }}";

        public static OrganizationType FromJObject(JValue jObject) => jObject != null ? new OrganizationType
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
