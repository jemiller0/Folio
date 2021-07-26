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
    // uc.organization_emails -> diku_mod_organizations_storage.organizations
    // OrganizationEmail -> Organization
    [DisplayColumn(nameof(Value)), DisplayName("Organization Emails"), JsonConverter(typeof(JsonPathJsonConverter<OrganizationEmail>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("organization_emails", Schema = "uc")]
    public partial class OrganizationEmail
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Organization2 Organization { get; set; }

        [Column("organization_id"), Display(Name = "Organization", Order = 3), Required]
        public virtual Guid? OrganizationId { get; set; }

        [Column("id2"), JsonProperty("id"), ScaffoldColumn(false)]
        public virtual Guid? Id2 { get; set; }

        [Column("value"), DataType(DataType.EmailAddress), Display(Order = 5), JsonProperty("value"), RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"), Required, StringLength(1024)]
        public virtual string Value { get; set; }

        [Column("description"), Display(Order = 6), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("is_primary"), Display(Name = "Is Primary", Order = 7), JsonProperty("isPrimary")]
        public virtual bool? IsPrimary { get; set; }

        [Column("language"), Display(Order = 8), JsonProperty("language"), StringLength(1024)]
        public virtual string Language { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 9), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 10), InverseProperty("OrganizationEmails")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 11), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 13), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 14), InverseProperty("OrganizationEmails1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 15), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Display(Name = "Organization Email Categories", Order = 17), JsonConverter(typeof(ArrayJsonConverter<List<OrganizationEmailCategory>, OrganizationEmailCategory>), "CategoryId"), JsonProperty("categories")]
        public virtual ICollection<OrganizationEmailCategory> OrganizationEmailCategories { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationId)} = {OrganizationId}, {nameof(Id2)} = {Id2}, {nameof(Value)} = {Value}, {nameof(Description)} = {Description}, {nameof(IsPrimary)} = {IsPrimary}, {nameof(Language)} = {Language}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(OrganizationEmailCategories)} = {(OrganizationEmailCategories != null ? $"{{ {string.Join(", ", OrganizationEmailCategories)} }}" : "")} }}";

        public static OrganizationEmail FromJObject(JObject jObject) => jObject != null ? new OrganizationEmail
        {
            Id2 = (Guid?)jObject.SelectToken("id"),
            Value = (string)jObject.SelectToken("value"),
            Description = (string)jObject.SelectToken("description"),
            IsPrimary = (bool?)jObject.SelectToken("isPrimary"),
            Language = (string)jObject.SelectToken("language"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            OrganizationEmailCategories = jObject.SelectToken("categories")?.Where(jt => jt.HasValues).Select(jt => OrganizationEmailCategory.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id2),
            new JProperty("value", Value),
            new JProperty("description", Description),
            new JProperty("isPrimary", IsPrimary),
            new JProperty("language", Language),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("categories", OrganizationEmailCategories?.Select(oec => oec.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
