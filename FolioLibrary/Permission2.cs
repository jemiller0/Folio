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
    // uc.permissions -> uchicago_mod_permissions.permissions
    // Permission2 -> Permission
    [CustomValidation(typeof(Permission2), nameof(ValidatePermission2)), DisplayColumn(nameof(Name)), DisplayName("Permissions"), JsonConverter(typeof(JsonPathJsonConverter<Permission2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("permissions", Schema = "uc")]
    public partial class Permission2
    {
        public static ValidationResult ValidatePermission2(Permission2 permission2, ValidationContext context)
        {
            var fsc = (FolioServiceContext)context.ObjectInstance;
            if (permission2.Code != null && fsc.AnyPermission2s($"id <> \"{permission2.Id}\" and permissionName == \"{permission2.Code}\"")) return new ValidationResult("Code already exists");
            if (permission2.Name != null && fsc.AnyPermission2s($"id <> \"{permission2.Id}\" and displayName == \"{permission2.Name}\"")) return new ValidationResult("Name already exists");
            return ValidationResult.Success;
        }

        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Permission.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("permission_name"), Display(Order = 2), JsonProperty("permissionName"), Required, StringLength(1024)]
        public virtual string Code { get; set; }

        [Column("display_name"), Display(Order = 3), JsonProperty("displayName"), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("description"), Display(Order = 4), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("mutable"), Display(Order = 5), JsonProperty("mutable")]
        public virtual bool? Editable { get; set; }

        [Column("visible"), Display(Order = 6), JsonProperty("visible")]
        public virtual bool? Visible { get; set; }

        [Column("dummy"), Display(Order = 7), JsonProperty("dummy")]
        public virtual bool? Dummy { get; set; }

        [Column("deprecated"), Display(Order = 8), JsonProperty("deprecated")]
        public virtual bool? Deprecated { get; set; }

        [Column("module_name"), Display(Name = "Module Name", Order = 9), JsonProperty("moduleName"), StringLength(1024)]
        public virtual string ModuleName { get; set; }

        [Column("module_version"), Display(Name = "Module Version", Order = 10), JsonProperty("moduleVersion"), StringLength(1024)]
        public virtual string ModuleVersion { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 11), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 12), InverseProperty("Permission2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 13), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 15), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 16), InverseProperty("Permission2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 17), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Permission), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 19), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Permission Child Ofs", Order = 20), JsonConverter(typeof(ArrayJsonConverter<List<PermissionChildOf>, PermissionChildOf>), "Content"), JsonProperty("childOf")]
        public virtual ICollection<PermissionChildOf> PermissionChildOfs { get; set; }

        [Display(Name = "Permission Granted Tos", Order = 21), JsonConverter(typeof(ArrayJsonConverter<List<PermissionGrantedTo>, PermissionGrantedTo>), "PermissionsUserId"), JsonProperty("grantedTo")]
        public virtual ICollection<PermissionGrantedTo> PermissionGrantedTos { get; set; }

        [Display(Name = "Permission Sub Permissions", Order = 22), JsonConverter(typeof(ArrayJsonConverter<List<PermissionSubPermission>, PermissionSubPermission>), "Content"), JsonProperty("subPermissions")]
        public virtual ICollection<PermissionSubPermission> PermissionSubPermissions { get; set; }

        [Display(Name = "Permission Tags", Order = 23), JsonConverter(typeof(ArrayJsonConverter<List<PermissionTag>, PermissionTag>), "Content"), JsonProperty("tags")]
        public virtual ICollection<PermissionTag> PermissionTags { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Code)} = {Code}, {nameof(Name)} = {Name}, {nameof(Description)} = {Description}, {nameof(Editable)} = {Editable}, {nameof(Visible)} = {Visible}, {nameof(Dummy)} = {Dummy}, {nameof(Deprecated)} = {Deprecated}, {nameof(ModuleName)} = {ModuleName}, {nameof(ModuleVersion)} = {ModuleVersion}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(PermissionChildOfs)} = {(PermissionChildOfs != null ? $"{{ {string.Join(", ", PermissionChildOfs)} }}" : "")}, {nameof(PermissionGrantedTos)} = {(PermissionGrantedTos != null ? $"{{ {string.Join(", ", PermissionGrantedTos)} }}" : "")}, {nameof(PermissionSubPermissions)} = {(PermissionSubPermissions != null ? $"{{ {string.Join(", ", PermissionSubPermissions)} }}" : "")}, {nameof(PermissionTags)} = {(PermissionTags != null ? $"{{ {string.Join(", ", PermissionTags)} }}" : "")} }}";

        public static Permission2 FromJObject(JObject jObject) => jObject != null ? new Permission2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Code = (string)jObject.SelectToken("permissionName"),
            Name = (string)jObject.SelectToken("displayName"),
            Description = (string)jObject.SelectToken("description"),
            Editable = (bool?)jObject.SelectToken("mutable"),
            Visible = (bool?)jObject.SelectToken("visible"),
            Dummy = (bool?)jObject.SelectToken("dummy"),
            Deprecated = (bool?)jObject.SelectToken("deprecated"),
            ModuleName = (string)jObject.SelectToken("moduleName"),
            ModuleVersion = (string)jObject.SelectToken("moduleVersion"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            PermissionChildOfs = jObject.SelectToken("childOf")?.Select(jt => PermissionChildOf.FromJObject((JValue)jt)).ToArray(),
            PermissionGrantedTos = jObject.SelectToken("grantedTo")?.Select(jt => PermissionGrantedTo.FromJObject((JValue)jt)).ToArray(),
            PermissionSubPermissions = jObject.SelectToken("subPermissions")?.Select(jt => PermissionSubPermission.FromJObject((JValue)jt)).ToArray(),
            PermissionTags = jObject.SelectToken("tags")?.Select(jt => PermissionTag.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("permissionName", Code),
            new JProperty("displayName", Name),
            new JProperty("description", Description),
            new JProperty("mutable", Editable),
            new JProperty("visible", Visible),
            new JProperty("dummy", Dummy),
            new JProperty("deprecated", Deprecated),
            new JProperty("moduleName", ModuleName),
            new JProperty("moduleVersion", ModuleVersion),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("childOf", PermissionChildOfs?.Select(pco => pco.ToJObject())),
            new JProperty("grantedTo", PermissionGrantedTos?.Select(pgt => pgt.ToJObject())),
            new JProperty("subPermissions", PermissionSubPermissions?.Select(psp => psp.ToJObject())),
            new JProperty("tags", PermissionTags?.Select(pt => pt.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
