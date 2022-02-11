using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.currencies -> uchicago_mod_organizations_storage.organizations
    // Currency -> Organization
    [DisplayColumn(nameof(Content)), Table("currencies", Schema = "uc")]
    public partial class Currency
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

        public static Currency FromJObject(JValue jObject) => jObject != null ? new Currency
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
