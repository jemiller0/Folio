using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    // uc.categories -> uchicago_mod_organizations_storage.categories
    // Category2 -> Category
    [DisplayColumn(nameof(Name)), DisplayName("Categories"), JsonConverter(typeof(JsonPathJsonConverter<Category2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("categories", Schema = "uc")]
    public partial class Category2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Category.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("value"), Display(Order = 2), JsonProperty("value"), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 4), InverseProperty("Category2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 5), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 7), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 8), InverseProperty("Category2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 9), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Category), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 11), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Contact Address Categories", Order = 12)]
        public virtual ICollection<ContactAddressCategory> ContactAddressCategories { get; set; }

        [Display(Name = "Contact Categories", Order = 13)]
        public virtual ICollection<ContactCategory> ContactCategories { get; set; }

        [Display(Name = "Contact Email Categories", Order = 14)]
        public virtual ICollection<ContactEmailCategory> ContactEmailCategories { get; set; }

        [Display(Name = "Contact Phone Number Categories", Order = 15)]
        public virtual ICollection<ContactPhoneNumberCategory> ContactPhoneNumberCategories { get; set; }

        [Display(Name = "Contact URL Categories", Order = 16)]
        public virtual ICollection<ContactUrlCategory> ContactUrlCategories { get; set; }

        [Display(Name = "Organization Address Categories", Order = 17)]
        public virtual ICollection<OrganizationAddressCategory> OrganizationAddressCategories { get; set; }

        [Display(Name = "Organization Email Categories", Order = 18)]
        public virtual ICollection<OrganizationEmailCategory> OrganizationEmailCategories { get; set; }

        [Display(Name = "Organization Phone Number Categories", Order = 19)]
        public virtual ICollection<OrganizationPhoneNumberCategory> OrganizationPhoneNumberCategories { get; set; }

        [Display(Name = "Organization URL Categories", Order = 20)]
        public virtual ICollection<OrganizationUrlCategory> OrganizationUrlCategories { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static Category2 FromJObject(JObject jObject) => jObject != null ? new Category2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("value"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("value", Name),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
