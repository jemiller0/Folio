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
    // uc.configurations -> uchicago_mod_configuration.config_data
    // Configuration2 -> Configuration
    [DisplayColumn(nameof(Id)), DisplayName("Configurations"), JsonConverter(typeof(JsonPathJsonConverter<Configuration2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("configurations", Schema = "uc")]
    public partial class Configuration2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Configuration.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("module"), Display(Order = 2), JsonProperty("module"), Required, StringLength(1024)]
        public virtual string Module { get; set; }

        [Column("config_name"), Display(Name = "Config Name", Order = 3), JsonProperty("configName"), Required, StringLength(1024)]
        public virtual string ConfigName { get; set; }

        [Column("code"), Display(Order = 4), JsonProperty("code"), StringLength(1024)]
        public virtual string Code { get; set; }

        [Column("description"), Display(Order = 5), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("default"), Display(Order = 6), JsonProperty("default")]
        public virtual bool? Default { get; set; }

        [Column("enabled"), Display(Order = 7), JsonProperty("enabled")]
        public virtual bool? Enabled { get; set; }

        [Column("value"), Display(Order = 8), JsonProperty("value"), StringLength(1024)]
        public virtual string Value { get; set; }

        [Column("user_id"), Display(Name = "User Id", Order = 9), JsonProperty("userId"), StringLength(128)]
        public virtual string UserId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 10), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 11), InverseProperty("Configuration2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 12), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 14), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 15), InverseProperty("Configuration2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 16), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Configuration), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 18), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Invoices", Order = 19)]
        public virtual ICollection<Invoice2> Invoice2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Module)} = {Module}, {nameof(ConfigName)} = {ConfigName}, {nameof(Code)} = {Code}, {nameof(Description)} = {Description}, {nameof(Default)} = {Default}, {nameof(Enabled)} = {Enabled}, {nameof(Value)} = {Value}, {nameof(UserId)} = {UserId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static Configuration2 FromJObject(JObject jObject) => jObject != null ? new Configuration2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Module = (string)jObject.SelectToken("module"),
            ConfigName = (string)jObject.SelectToken("configName"),
            Code = (string)jObject.SelectToken("code"),
            Description = (string)jObject.SelectToken("description"),
            Default = (bool?)jObject.SelectToken("default"),
            Enabled = (bool?)jObject.SelectToken("enabled"),
            Value = (string)jObject.SelectToken("value"),
            UserId = (string)jObject.SelectToken("userId"),
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
            new JProperty("module", Module),
            new JProperty("configName", ConfigName),
            new JProperty("code", Code),
            new JProperty("description", Description),
            new JProperty("default", Default),
            new JProperty("enabled", Enabled),
            new JProperty("value", Value),
            new JProperty("userId", UserId),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
