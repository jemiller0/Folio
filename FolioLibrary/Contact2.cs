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
    // uc.contacts -> diku_mod_organizations_storage.contacts
    // Contact2 -> Contact
    [DisplayColumn(nameof(Name)), DisplayName("Contacts"), JsonConverter(typeof(JsonPathJsonConverter<Contact2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("contacts", Schema = "uc")]
    public partial class Contact2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Contact.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("name"), Display(Order = 2), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("prefix"), Display(Order = 3), JsonProperty("prefix"), StringLength(1024)]
        public virtual string Prefix { get; set; }

        [Column("first_name"), Display(Name = "First Name", Order = 4), JsonProperty("firstName"), Required, StringLength(1024)]
        public virtual string FirstName { get; set; }

        [Column("last_name"), Display(Name = "Last Name", Order = 5), JsonProperty("lastName"), Required, StringLength(1024)]
        public virtual string LastName { get; set; }

        [Column("language"), Display(Order = 6), JsonProperty("language"), StringLength(1024)]
        public virtual string Language { get; set; }

        [Column("notes"), Display(Order = 7), JsonProperty("notes"), StringLength(1024)]
        public virtual string Notes { get; set; }

        [Column("inactive"), Display(Order = 8), JsonProperty("inactive")]
        public virtual bool? Inactive { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 9), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 10), InverseProperty("Contact2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 11), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 13), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 14), InverseProperty("Contact2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 15), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Contact), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 17), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Contact Addresses", Order = 18), JsonProperty("addresses")]
        public virtual ICollection<ContactAddress> ContactAddresses { get; set; }

        [Display(Name = "Contact Categories", Order = 19), JsonConverter(typeof(ArrayJsonConverter<List<ContactCategory>, ContactCategory>), "CategoryId"), JsonProperty("categories")]
        public virtual ICollection<ContactCategory> ContactCategories { get; set; }

        [Display(Name = "Contact Emails", Order = 20), JsonProperty("emails")]
        public virtual ICollection<ContactEmail> ContactEmails { get; set; }

        [Display(Name = "Contact Phone Numbers", Order = 21), JsonProperty("phoneNumbers")]
        public virtual ICollection<ContactPhoneNumber> ContactPhoneNumbers { get; set; }

        [Display(Name = "Contact URLs", Order = 22), JsonProperty("urls")]
        public virtual ICollection<ContactUrl> ContactUrls { get; set; }

        [Display(Name = "Organization Contacts", Order = 23)]
        public virtual ICollection<OrganizationContact> OrganizationContacts { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Prefix)} = {Prefix}, {nameof(FirstName)} = {FirstName}, {nameof(LastName)} = {LastName}, {nameof(Language)} = {Language}, {nameof(Notes)} = {Notes}, {nameof(Inactive)} = {Inactive}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(ContactAddresses)} = {(ContactAddresses != null ? $"{{ {string.Join(", ", ContactAddresses)} }}" : "")}, {nameof(ContactCategories)} = {(ContactCategories != null ? $"{{ {string.Join(", ", ContactCategories)} }}" : "")}, {nameof(ContactEmails)} = {(ContactEmails != null ? $"{{ {string.Join(", ", ContactEmails)} }}" : "")}, {nameof(ContactPhoneNumbers)} = {(ContactPhoneNumbers != null ? $"{{ {string.Join(", ", ContactPhoneNumbers)} }}" : "")}, {nameof(ContactUrls)} = {(ContactUrls != null ? $"{{ {string.Join(", ", ContactUrls)} }}" : "")} }}";

        public static Contact2 FromJObject(JObject jObject) => jObject != null ? new Contact2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = $"{jObject["firstName"]} {jObject["lastName"]}",
            Prefix = (string)jObject.SelectToken("prefix"),
            FirstName = (string)jObject.SelectToken("firstName"),
            LastName = (string)jObject.SelectToken("lastName"),
            Language = (string)jObject.SelectToken("language"),
            Notes = (string)jObject.SelectToken("notes"),
            Inactive = (bool?)jObject.SelectToken("inactive"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            ContactAddresses = jObject.SelectToken("addresses")?.Where(jt => jt.HasValues).Select(jt => ContactAddress.FromJObject((JObject)jt)).ToArray(),
            ContactCategories = jObject.SelectToken("categories")?.Where(jt => jt.HasValues).Select(jt => ContactCategory.FromJObject((JValue)jt)).ToArray(),
            ContactEmails = jObject.SelectToken("emails")?.Where(jt => jt.HasValues).Select(jt => ContactEmail.FromJObject((JObject)jt)).ToArray(),
            ContactPhoneNumbers = jObject.SelectToken("phoneNumbers")?.Where(jt => jt.HasValues).Select(jt => ContactPhoneNumber.FromJObject((JObject)jt)).ToArray(),
            ContactUrls = jObject.SelectToken("urls")?.Where(jt => jt.HasValues).Select(jt => ContactUrl.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("prefix", Prefix),
            new JProperty("firstName", FirstName),
            new JProperty("lastName", LastName),
            new JProperty("language", Language),
            new JProperty("notes", Notes),
            new JProperty("inactive", Inactive),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("addresses", ContactAddresses?.Select(ca => ca.ToJObject())),
            new JProperty("categories", ContactCategories?.Select(cc => cc.ToJObject())),
            new JProperty("emails", ContactEmails?.Select(ce => ce.ToJObject())),
            new JProperty("phoneNumbers", ContactPhoneNumbers?.Select(cpn => cpn.ToJObject())),
            new JProperty("urls", ContactUrls?.Select(cu => cu.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
