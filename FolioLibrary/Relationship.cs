using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    // uc.relationships -> diku_mod_inventory_storage.instance_relationship
    // Relationship -> InstanceRelationship
    [DisplayColumn(nameof(Id)), JsonConverter(typeof(JsonPathJsonConverter<Relationship>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("relationships", Schema = "uc")]
    public partial class Relationship
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.InstanceRelationship.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Name = "Super Instance", Order = 2), InverseProperty("Relationships1")]
        public virtual Instance2 SuperInstance { get; set; }

        [Column("super_instance_id"), Display(Name = "Super Instance", Order = 3), JsonProperty("superInstanceId"), Required]
        public virtual Guid? SuperInstanceId { get; set; }

        [Display(Name = "Sub Instance", Order = 4), InverseProperty("Relationships")]
        public virtual Instance2 SubInstance { get; set; }

        [Column("sub_instance_id"), Display(Name = "Sub Instance", Order = 5), JsonProperty("subInstanceId"), Required]
        public virtual Guid? SubInstanceId { get; set; }

        [Display(Name = "Instance Relationship Type", Order = 6)]
        public virtual RelationshipType InstanceRelationshipType { get; set; }

        [Column("instance_relationship_type_id"), Display(Name = "Instance Relationship Type", Order = 7), JsonProperty("instanceRelationshipTypeId"), Required]
        public virtual Guid? InstanceRelationshipTypeId { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 8), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 9), InverseProperty("Relationships")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 10), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 12), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 13), InverseProperty("Relationships1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 14), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(InstanceRelationship), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 16), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Electronic Accesses", Order = 17)]
        public virtual ICollection<ElectronicAccess> ElectronicAccesses { get; set; }

        [Display(Name = "Holding Electronic Accesses", Order = 18)]
        public virtual ICollection<HoldingElectronicAccess> HoldingElectronicAccesses { get; set; }

        [Display(Name = "Item Electronic Accesses", Order = 19)]
        public virtual ICollection<ItemElectronicAccess> ItemElectronicAccesses { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(SuperInstanceId)} = {SuperInstanceId}, {nameof(SubInstanceId)} = {SubInstanceId}, {nameof(InstanceRelationshipTypeId)} = {InstanceRelationshipTypeId}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content} }}";

        public static Relationship FromJObject(JObject jObject) => jObject != null ? new Relationship
        {
            Id = (Guid?)jObject.SelectToken("id"),
            SuperInstanceId = (Guid?)jObject.SelectToken("superInstanceId"),
            SubInstanceId = (Guid?)jObject.SelectToken("subInstanceId"),
            InstanceRelationshipTypeId = (Guid?)jObject.SelectToken("instanceRelationshipTypeId"),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("superInstanceId", SuperInstanceId),
            new JProperty("subInstanceId", SubInstanceId),
            new JProperty("instanceRelationshipTypeId", InstanceRelationshipTypeId),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
