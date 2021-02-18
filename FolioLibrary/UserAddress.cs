using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.user_addresses -> diku_mod_users.users
    // UserAddress -> User
    [DisplayColumn(nameof(StreetAddress1)), DisplayName("User Addresses"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("user_addresses", Schema = "uc")]
    public partial class UserAddress
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual User2 User { get; set; }

        [Column("user_id"), Display(Name = "User", Order = 3), Required]
        public virtual Guid? UserId { get; set; }

        [Column("id2"), JsonProperty("id"), ScaffoldColumn(false)]
        public virtual Guid? Id2 { get; set; }

        [Column("country_id"), Display(Name = "Country Code", Order = 5), JsonProperty("countryId"), StringLength(1024)]
        public virtual string CountryCode { get; set; }

        [Column("address_line1"), Display(Name = "Street Address 1", Order = 6), JsonProperty("addressLine1"), StringLength(1024)]
        public virtual string StreetAddress1 { get; set; }

        [Column("address_line2"), Display(Name = "Street Address 2", Order = 7), JsonProperty("addressLine2"), StringLength(1024)]
        public virtual string StreetAddress2 { get; set; }

        [Column("city"), Display(Order = 8), JsonProperty("city"), StringLength(1024)]
        public virtual string City { get; set; }

        [Column("region"), Display(Order = 9), JsonProperty("region"), StringLength(1024)]
        public virtual string State { get; set; }

        [Column("postal_code"), Display(Name = "Postal Code", Order = 10), JsonProperty("postalCode"), RegularExpression(@"^\d{5}(-\d{4})?$"), StringLength(1024)]
        public virtual string PostalCode { get; set; }

        [Display(Name = "Address Type", Order = 11)]
        public virtual AddressType2 AddressType { get; set; }

        [Column("address_type_id"), Display(Name = "Address Type", Order = 12), JsonProperty("addressTypeId")]
        public virtual Guid? AddressTypeId { get; set; }

        [Column("primary_address"), Display(Order = 13), JsonProperty("primaryAddress")]
        public virtual bool? Default { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(UserId)} = {UserId}, {nameof(Id2)} = {Id2}, {nameof(CountryCode)} = {CountryCode}, {nameof(StreetAddress1)} = {StreetAddress1}, {nameof(StreetAddress2)} = {StreetAddress2}, {nameof(City)} = {City}, {nameof(State)} = {State}, {nameof(PostalCode)} = {PostalCode}, {nameof(AddressTypeId)} = {AddressTypeId}, {nameof(Default)} = {Default} }}";

        public static UserAddress FromJObject(JObject jObject) => jObject != null ? new UserAddress
        {
            Id2 = (Guid?)jObject.SelectToken("id"),
            CountryCode = (string)jObject.SelectToken("countryId"),
            StreetAddress1 = (string)jObject.SelectToken("addressLine1"),
            StreetAddress2 = (string)jObject.SelectToken("addressLine2"),
            City = (string)jObject.SelectToken("city"),
            State = (string)jObject.SelectToken("region"),
            PostalCode = (string)jObject.SelectToken("postalCode"),
            AddressTypeId = (Guid?)jObject.SelectToken("addressTypeId"),
            Default = (bool?)jObject.SelectToken("primaryAddress")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id2),
            new JProperty("countryId", CountryCode),
            new JProperty("addressLine1", StreetAddress1),
            new JProperty("addressLine2", StreetAddress2),
            new JProperty("city", City),
            new JProperty("region", State),
            new JProperty("postalCode", PostalCode),
            new JProperty("addressTypeId", AddressTypeId),
            new JProperty("primaryAddress", Default)).RemoveNullAndEmptyProperties();
    }
}
