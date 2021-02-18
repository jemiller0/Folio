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
    // uc.organization_phone_numbers -> diku_mod_organizations_storage.organizations
    // OrganizationPhoneNumber -> Organization
    [DisplayColumn(nameof(PhoneNumber)), DisplayName("Organization Phone Numbers"), JsonConverter(typeof(JsonPathJsonConverter<OrganizationPhoneNumber>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("organization_phone_numbers", Schema = "uc")]
    public partial class OrganizationPhoneNumber
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Organization2 Organization { get; set; }

        [Column("organization_id"), Display(Name = "Organization", Order = 3), Required]
        public virtual Guid? OrganizationId { get; set; }

        [Column("id2"), JsonProperty("id"), ScaffoldColumn(false)]
        public virtual Guid? Id2 { get; set; }

        [Column("phone_number"), DataType(DataType.PhoneNumber), Display(Name = "Phone Number", Order = 5), JsonProperty("phoneNumber"), RegularExpression(@"^(\+\d{1,3} ?)?(\(?\d{3}\)?[ \-]?)?(\d{2})?\d[ \-]?\d{4}$"), Required, StringLength(1024)]
        public virtual string PhoneNumber { get; set; }

        [Column("type"), Display(Order = 6), JsonProperty("type"), StringLength(1024)]
        public virtual string Type { get; set; }

        [Column("is_primary"), Display(Name = "Is Primary", Order = 7), JsonProperty("isPrimary")]
        public virtual bool? IsPrimary { get; set; }

        [Column("language"), Display(Order = 8), JsonProperty("language"), StringLength(1024)]
        public virtual string Language { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 9), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 10), InverseProperty("OrganizationPhoneNumbers")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 11), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 13), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 14), InverseProperty("OrganizationPhoneNumbers1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 15), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Display(Name = "Organization Phone Number Categories", Order = 17), JsonConverter(typeof(ArrayJsonConverter<List<OrganizationPhoneNumberCategory>, OrganizationPhoneNumberCategory>), "CategoryId"), JsonProperty("categories")]
        public virtual ICollection<OrganizationPhoneNumberCategory> OrganizationPhoneNumberCategories { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationId)} = {OrganizationId}, {nameof(Id2)} = {Id2}, {nameof(PhoneNumber)} = {PhoneNumber}, {nameof(Type)} = {Type}, {nameof(IsPrimary)} = {IsPrimary}, {nameof(Language)} = {Language}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(OrganizationPhoneNumberCategories)} = {(OrganizationPhoneNumberCategories != null ? $"{{ {string.Join(", ", OrganizationPhoneNumberCategories)} }}" : "")} }}";

        public static OrganizationPhoneNumber FromJObject(JObject jObject) => jObject != null ? new OrganizationPhoneNumber
        {
            Id2 = (Guid?)jObject.SelectToken("id"),
            PhoneNumber = (string)jObject.SelectToken("phoneNumber"),
            Type = (string)jObject.SelectToken("type"),
            IsPrimary = (bool?)jObject.SelectToken("isPrimary"),
            Language = (string)jObject.SelectToken("language"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            OrganizationPhoneNumberCategories = jObject.SelectToken("categories")?.Where(jt => jt.HasValues).Select(jt => OrganizationPhoneNumberCategory.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id2),
            new JProperty("phoneNumber", PhoneNumber),
            new JProperty("type", Type),
            new JProperty("isPrimary", IsPrimary),
            new JProperty("language", Language),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("categories", OrganizationPhoneNumberCategories?.Select(opnc => opnc.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
