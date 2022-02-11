using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    // uc.interface_credentials -> uchicago_mod_organizations_storage.interface_credentials
    // InterfaceCredential2 -> InterfaceCredential
    [DisplayColumn(nameof(Id)), DisplayName("Interface Credentials"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("interface_credentials", Schema = "uc")]
    public partial class InterfaceCredential2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.InterfaceCredential.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("username"), Display(Order = 2), JsonProperty("username"), Required, StringLength(1024)]
        public virtual string Username { get; set; }

        [Column("password"), DataType(DataType.Password), Display(Order = 3), JsonProperty("password"), Required, StringLength(1024)]
        public virtual string Password { get; set; }

        [Display(Order = 4)]
        public virtual Interface2 Interface { get; set; }

        [Column("interface_id"), Display(Name = "Interface", Order = 5), JsonProperty("interfaceId"), Required]
        public virtual Guid? InterfaceId { get; set; }

        [Column("content"), CustomValidation(typeof(InterfaceCredential), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 6), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Username)} = {Username}, {nameof(Password)} = {Password}, {nameof(InterfaceId)} = {InterfaceId}, {nameof(Content)} = {Content} }}";

        public static InterfaceCredential2 FromJObject(JObject jObject) => jObject != null ? new InterfaceCredential2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Username = (string)jObject.SelectToken("username"),
            Password = (string)jObject.SelectToken("password"),
            InterfaceId = (Guid?)jObject.SelectToken("interfaceId"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("username", Username),
            new JProperty("password", Password),
            new JProperty("interfaceId", InterfaceId)).RemoveNullAndEmptyProperties();
    }
}
