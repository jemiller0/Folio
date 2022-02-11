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
    // uc.contact_addresses -> uchicago_mod_organizations_storage.contacts
    // ContactAddress -> Contact
    [DisplayColumn(nameof(StreetAddress1)), DisplayName("Contact Addresses"), JsonConverter(typeof(JsonPathJsonConverter<ContactAddress>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("contact_addresses", Schema = "uc")]
    public partial class ContactAddress
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Contact2 Contact { get; set; }

        [Column("contact_id"), Display(Name = "Contact", Order = 3), Required]
        public virtual Guid? ContactId { get; set; }

        [Column("id2"), JsonProperty("id"), ScaffoldColumn(false)]
        public virtual Guid? Id2 { get; set; }

        [Column("address_line1"), Display(Name = "Street Address 1", Order = 5), JsonProperty("addressLine1"), StringLength(1024)]
        public virtual string StreetAddress1 { get; set; }

        [Column("address_line2"), Display(Name = "Street Address 2", Order = 6), JsonProperty("addressLine2"), StringLength(1024)]
        public virtual string StreetAddress2 { get; set; }

        [Column("city"), Display(Order = 7), JsonProperty("city"), StringLength(1024)]
        public virtual string City { get; set; }

        [Column("state_region"), Display(Order = 8), JsonProperty("stateRegion"), StringLength(1024)]
        public virtual string State { get; set; }

        [Column("zip_code"), Display(Name = "Postal Code", Order = 9), JsonProperty("zipCode"), RegularExpression(@"^\d{5}(-\d{4})?$"), StringLength(1024)]
        public virtual string PostalCode { get; set; }

        [Column("country"), Display(Name = "Country Code", Order = 10), JsonProperty("country"), StringLength(1024)]
        public virtual string CountryCode { get; set; }

        [Column("is_primary"), Display(Name = "Is Primary", Order = 11), JsonProperty("isPrimary")]
        public virtual bool? IsPrimary { get; set; }

        [Column("language"), Display(Order = 12), JsonProperty("language"), StringLength(1024)]
        public virtual string Language { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 13), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 14), InverseProperty("ContactAddresses")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 15), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 17), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 18), InverseProperty("ContactAddresses1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 19), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Display(Name = "Contact Address Categories", Order = 21), JsonConverter(typeof(ArrayJsonConverter<List<ContactAddressCategory>, ContactAddressCategory>), "CategoryId"), JsonProperty("categories")]
        public virtual ICollection<ContactAddressCategory> ContactAddressCategories { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ContactId)} = {ContactId}, {nameof(Id2)} = {Id2}, {nameof(StreetAddress1)} = {StreetAddress1}, {nameof(StreetAddress2)} = {StreetAddress2}, {nameof(City)} = {City}, {nameof(State)} = {State}, {nameof(PostalCode)} = {PostalCode}, {nameof(CountryCode)} = {CountryCode}, {nameof(IsPrimary)} = {IsPrimary}, {nameof(Language)} = {Language}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(ContactAddressCategories)} = {(ContactAddressCategories != null ? $"{{ {string.Join(", ", ContactAddressCategories)} }}" : "")} }}";

        public static ContactAddress FromJObject(JObject jObject) => jObject != null ? new ContactAddress
        {
            Id2 = (Guid?)jObject.SelectToken("id"),
            StreetAddress1 = (string)jObject.SelectToken("addressLine1"),
            StreetAddress2 = (string)jObject.SelectToken("addressLine2"),
            City = (string)jObject.SelectToken("city"),
            State = (string)jObject.SelectToken("stateRegion"),
            PostalCode = (string)jObject.SelectToken("zipCode"),
            CountryCode = (string)jObject.SelectToken("country"),
            IsPrimary = (bool?)jObject.SelectToken("isPrimary"),
            Language = (string)jObject.SelectToken("language"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            ContactAddressCategories = jObject.SelectToken("categories")?.Select(jt => ContactAddressCategory.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id2),
            new JProperty("addressLine1", StreetAddress1),
            new JProperty("addressLine2", StreetAddress2),
            new JProperty("city", City),
            new JProperty("stateRegion", State),
            new JProperty("zipCode", PostalCode),
            new JProperty("country", CountryCode),
            new JProperty("isPrimary", IsPrimary),
            new JProperty("language", Language),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("categories", ContactAddressCategories?.Select(cac => cac.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
