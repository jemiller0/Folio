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
    // uc.organization_addresses -> diku_mod_organizations_storage.organizations
    // OrganizationAddress -> Organization
    [DisplayColumn(nameof(StreetAddress1)), DisplayName("Organization Addresses"), JsonConverter(typeof(JsonPathJsonConverter<OrganizationAddress>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("organization_addresses", Schema = "uc")]
    public partial class OrganizationAddress
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Organization2 Organization { get; set; }

        [Column("organization_id"), Display(Name = "Organization", Order = 3), Required]
        public virtual Guid? OrganizationId { get; set; }

        [Column("id2"), JsonProperty("id"), ScaffoldColumn(false)]
        public virtual Guid? Id2 { get; set; }

        [Column("address_line1"), Display(Name = "Street Address 1", Order = 5), JsonProperty("addressLine1"), StringLength(1024)]
        public virtual string StreetAddress1 { get; set; }

        [Column("address_line2"), Display(Name = "Street Address 2", Order = 6), JsonProperty("addressLine2"), StringLength(1024)]
        public virtual string StreetAddress2 { get; set; }

        [Column("city"), Display(Order = 7), JsonProperty("city"), StringLength(1024)]
        public virtual string City { get; set; }

        [Column("state_region"), Display(Name = "State Region", Order = 8), JsonProperty("stateRegion"), StringLength(1024)]
        public virtual string StateRegion { get; set; }

        [Column("zip_code"), Display(Name = "Zip Code", Order = 9), JsonProperty("zipCode"), RegularExpression(@"^\d{5}(-\d{4})?$"), StringLength(1024)]
        public virtual string ZipCode { get; set; }

        [Column("country"), Display(Order = 10), JsonProperty("country"), StringLength(1024)]
        public virtual string Country { get; set; }

        [Column("is_primary"), Display(Name = "Is Primary", Order = 11), JsonProperty("isPrimary")]
        public virtual bool? IsPrimary { get; set; }

        [Column("language"), Display(Order = 12), JsonProperty("language"), StringLength(1024)]
        public virtual string Language { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 13), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 14), InverseProperty("OrganizationAddresses")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 15), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 17), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 18), InverseProperty("OrganizationAddresses1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 19), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Display(Name = "Organization Address Categories", Order = 21), JsonConverter(typeof(ArrayJsonConverter<List<OrganizationAddressCategory>, OrganizationAddressCategory>), "CategoryId"), JsonProperty("categories")]
        public virtual ICollection<OrganizationAddressCategory> OrganizationAddressCategories { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationId)} = {OrganizationId}, {nameof(Id2)} = {Id2}, {nameof(StreetAddress1)} = {StreetAddress1}, {nameof(StreetAddress2)} = {StreetAddress2}, {nameof(City)} = {City}, {nameof(StateRegion)} = {StateRegion}, {nameof(ZipCode)} = {ZipCode}, {nameof(Country)} = {Country}, {nameof(IsPrimary)} = {IsPrimary}, {nameof(Language)} = {Language}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(OrganizationAddressCategories)} = {(OrganizationAddressCategories != null ? $"{{ {string.Join(", ", OrganizationAddressCategories)} }}" : "")} }}";

        public static OrganizationAddress FromJObject(JObject jObject) => jObject != null ? new OrganizationAddress
        {
            Id2 = (Guid?)jObject.SelectToken("id"),
            StreetAddress1 = (string)jObject.SelectToken("addressLine1"),
            StreetAddress2 = (string)jObject.SelectToken("addressLine2"),
            City = (string)jObject.SelectToken("city"),
            StateRegion = (string)jObject.SelectToken("stateRegion"),
            ZipCode = (string)jObject.SelectToken("zipCode"),
            Country = (string)jObject.SelectToken("country"),
            IsPrimary = (bool?)jObject.SelectToken("isPrimary"),
            Language = (string)jObject.SelectToken("language"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            OrganizationAddressCategories = jObject.SelectToken("categories")?.Where(jt => jt.HasValues).Select(jt => OrganizationAddressCategory.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id2),
            new JProperty("addressLine1", StreetAddress1),
            new JProperty("addressLine2", StreetAddress2),
            new JProperty("city", City),
            new JProperty("stateRegion", StateRegion),
            new JProperty("zipCode", ZipCode),
            new JProperty("country", Country),
            new JProperty("isPrimary", IsPrimary),
            new JProperty("language", Language),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("categories", OrganizationAddressCategories?.Select(oac => oac.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
