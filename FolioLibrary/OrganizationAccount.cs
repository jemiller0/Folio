using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FolioLibrary
{
    // uc.organization_accounts -> diku_mod_organizations_storage.organizations
    // OrganizationAccount -> Organization
    [DisplayColumn(nameof(Name)), DisplayName("Organization Accounts"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("organization_accounts", Schema = "uc")]
    public partial class OrganizationAccount
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Organization2 Organization { get; set; }

        [Column("organization_id"), Display(Name = "Organization", Order = 3), Required]
        public virtual Guid? OrganizationId { get; set; }

        [Column("name"), Display(Order = 4), JsonProperty("name"), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("account_no"), Display(Name = "Account Number", Order = 5), JsonProperty("accountNo"), Required, StringLength(1024)]
        public virtual string AccountNumber { get; set; }

        [Column("description"), Display(Order = 6), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("app_system_no"), Display(Name = "App System No", Order = 7), JsonProperty("appSystemNo"), StringLength(1024)]
        public virtual string AppSystemNo { get; set; }

        [Column("payment_method"), Display(Name = "Payment Method", Order = 8), JsonProperty("paymentMethod"), Required, StringLength(1024)]
        public virtual string PaymentMethod { get; set; }

        [Column("account_status"), Display(Name = "Account Status", Order = 9), JsonProperty("accountStatus"), Required, StringLength(1024)]
        public virtual string AccountStatus { get; set; }

        [Column("contact_info"), Display(Name = "Contact Info", Order = 10), JsonProperty("contactInfo"), StringLength(1024)]
        public virtual string ContactInfo { get; set; }

        [Column("library_code"), Display(Name = "Library Code", Order = 11), JsonProperty("libraryCode"), Required, StringLength(1024)]
        public virtual string LibraryCode { get; set; }

        [Column("library_edi_code"), Display(Name = "Library EDI Code", Order = 12), JsonProperty("libraryEdiCode"), Required, StringLength(1024)]
        public virtual string LibraryEdiCode { get; set; }

        [Column("notes"), Display(Order = 13), JsonProperty("notes"), StringLength(1024)]
        public virtual string Notes { get; set; }

        [Display(Name = "Organization Account Acquisitions Units", Order = 14), JsonConverter(typeof(ArrayJsonConverter<List<OrganizationAccountAcquisitionsUnit>, OrganizationAccountAcquisitionsUnit>), "AcquisitionsUnitId"), JsonProperty("acqUnitIds")]
        public virtual ICollection<OrganizationAccountAcquisitionsUnit> OrganizationAccountAcquisitionsUnits { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationId)} = {OrganizationId}, {nameof(Name)} = {Name}, {nameof(AccountNumber)} = {AccountNumber}, {nameof(Description)} = {Description}, {nameof(AppSystemNo)} = {AppSystemNo}, {nameof(PaymentMethod)} = {PaymentMethod}, {nameof(AccountStatus)} = {AccountStatus}, {nameof(ContactInfo)} = {ContactInfo}, {nameof(LibraryCode)} = {LibraryCode}, {nameof(LibraryEdiCode)} = {LibraryEdiCode}, {nameof(Notes)} = {Notes}, {nameof(OrganizationAccountAcquisitionsUnits)} = {(OrganizationAccountAcquisitionsUnits != null ? $"{{ {string.Join(", ", OrganizationAccountAcquisitionsUnits)} }}" : "")} }}";

        public static OrganizationAccount FromJObject(JObject jObject) => jObject != null ? new OrganizationAccount
        {
            Name = (string)jObject.SelectToken("name"),
            AccountNumber = (string)jObject.SelectToken("accountNo"),
            Description = (string)jObject.SelectToken("description"),
            AppSystemNo = (string)jObject.SelectToken("appSystemNo"),
            PaymentMethod = (string)jObject.SelectToken("paymentMethod"),
            AccountStatus = (string)jObject.SelectToken("accountStatus"),
            ContactInfo = (string)jObject.SelectToken("contactInfo"),
            LibraryCode = (string)jObject.SelectToken("libraryCode"),
            LibraryEdiCode = (string)jObject.SelectToken("libraryEdiCode"),
            Notes = (string)jObject.SelectToken("notes"),
            OrganizationAccountAcquisitionsUnits = jObject.SelectToken("acqUnitIds")?.Where(jt => jt.HasValues).Select(jt => OrganizationAccountAcquisitionsUnit.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("name", Name),
            new JProperty("accountNo", AccountNumber),
            new JProperty("description", Description),
            new JProperty("appSystemNo", AppSystemNo),
            new JProperty("paymentMethod", PaymentMethod),
            new JProperty("accountStatus", AccountStatus),
            new JProperty("contactInfo", ContactInfo),
            new JProperty("libraryCode", LibraryCode),
            new JProperty("libraryEdiCode", LibraryEdiCode),
            new JProperty("notes", Notes),
            new JProperty("acqUnitIds", OrganizationAccountAcquisitionsUnits?.Select(oaau => oaau.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
