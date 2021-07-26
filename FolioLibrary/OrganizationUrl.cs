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
    // uc.organization_urls -> diku_mod_organizations_storage.organizations
    // OrganizationUrl -> Organization
    [DisplayColumn(nameof(Value)), DisplayName("Organization URLs"), JsonConverter(typeof(JsonPathJsonConverter<OrganizationUrl>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("organization_urls", Schema = "uc")]
    public partial class OrganizationUrl
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Organization2 Organization { get; set; }

        [Column("organization_id"), Display(Name = "Organization", Order = 3), Required]
        public virtual Guid? OrganizationId { get; set; }

        [Column("id2"), JsonProperty("id"), ScaffoldColumn(false)]
        public virtual Guid? Id2 { get; set; }

        [Column("value"), DataType(DataType.Url), Display(Order = 5), JsonProperty("value"), Required, StringLength(1024)]
        public virtual string Value { get; set; }

        [Column("description"), Display(Order = 6), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("language"), Display(Order = 7), JsonProperty("language"), StringLength(1024)]
        public virtual string Language { get; set; }

        [Column("is_primary"), Display(Name = "Is Primary", Order = 8), JsonProperty("isPrimary")]
        public virtual bool? IsPrimary { get; set; }

        [Column("notes"), Display(Order = 9), JsonProperty("notes"), StringLength(1024)]
        public virtual string Notes { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 10), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 11), InverseProperty("OrganizationUrls")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 12), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 14), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 15), InverseProperty("OrganizationUrls1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 16), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Display(Name = "Organization URL Categories", Order = 18), JsonConverter(typeof(ArrayJsonConverter<List<OrganizationUrlCategory>, OrganizationUrlCategory>), "CategoryId"), JsonProperty("categories")]
        public virtual ICollection<OrganizationUrlCategory> OrganizationUrlCategories { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrganizationId)} = {OrganizationId}, {nameof(Id2)} = {Id2}, {nameof(Value)} = {Value}, {nameof(Description)} = {Description}, {nameof(Language)} = {Language}, {nameof(IsPrimary)} = {IsPrimary}, {nameof(Notes)} = {Notes}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(OrganizationUrlCategories)} = {(OrganizationUrlCategories != null ? $"{{ {string.Join(", ", OrganizationUrlCategories)} }}" : "")} }}";

        public static OrganizationUrl FromJObject(JObject jObject) => jObject != null ? new OrganizationUrl
        {
            Id2 = (Guid?)jObject.SelectToken("id"),
            Value = (string)jObject.SelectToken("value"),
            Description = (string)jObject.SelectToken("description"),
            Language = (string)jObject.SelectToken("language"),
            IsPrimary = (bool?)jObject.SelectToken("isPrimary"),
            Notes = (string)jObject.SelectToken("notes"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            OrganizationUrlCategories = jObject.SelectToken("categories")?.Where(jt => jt.HasValues).Select(jt => OrganizationUrlCategory.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id2),
            new JProperty("value", Value),
            new JProperty("description", Description),
            new JProperty("language", Language),
            new JProperty("isPrimary", IsPrimary),
            new JProperty("notes", Notes),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("categories", OrganizationUrlCategories?.Select(ouc => ouc.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
