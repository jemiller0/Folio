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
    // uc.contact_urls -> diku_mod_organizations_storage.contacts
    // ContactUrl -> Contact
    [DisplayColumn(nameof(Value)), DisplayName("Contact URLs"), JsonConverter(typeof(JsonPathJsonConverter<ContactUrl>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("contact_urls", Schema = "uc")]
    public partial class ContactUrl
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Contact2 Contact { get; set; }

        [Column("contact_id"), Display(Name = "Contact", Order = 3), Required]
        public virtual Guid? ContactId { get; set; }

        [Column("id2"), Display(Name = "Id 2", Order = 4), JsonProperty("id")]
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

        [Display(Name = "Creation User", Order = 11), InverseProperty("ContactUrls")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 12), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 14), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 15), InverseProperty("ContactUrls1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 16), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Display(Name = "Contact URL Categories", Order = 18), JsonConverter(typeof(ArrayJsonConverter<List<ContactUrlCategory>, ContactUrlCategory>), "CategoryId"), JsonProperty("categories")]
        public virtual ICollection<ContactUrlCategory> ContactUrlCategories { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ContactId)} = {ContactId}, {nameof(Id2)} = {Id2}, {nameof(Value)} = {Value}, {nameof(Description)} = {Description}, {nameof(Language)} = {Language}, {nameof(IsPrimary)} = {IsPrimary}, {nameof(Notes)} = {Notes}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(ContactUrlCategories)} = {(ContactUrlCategories != null ? $"{{ {string.Join(", ", ContactUrlCategories)} }}" : "")} }}";

        public static ContactUrl FromJObject(JObject jObject) => jObject != null ? new ContactUrl
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
            ContactUrlCategories = jObject.SelectToken("categories")?.Where(jt => jt.HasValues).Select(jt => ContactUrlCategory.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id2),
            new JProperty("value", Value),
            new JProperty("description", Description),
            new JProperty("language", Language),
            new JProperty("isPrimary", IsPrimary),
            new JProperty("notes", Notes),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("categories", ContactUrlCategories?.Select(cuc => cuc.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
