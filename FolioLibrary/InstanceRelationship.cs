using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    [Table("instance_relationship", Schema = "uchicago_mod_inventory_storage")]
    public partial class InstanceRelationship
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

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(InstanceRelationship), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual string CreationUserId { get; set; }

        [Display(Name = "Instance 1", Order = 5), InverseProperty("InstanceRelationships1")]
        public virtual Instance Instance1 { get; set; }

        [Column("superinstanceid"), Display(Name = "Instance 1", Order = 6), ForeignKey("Instance1")]
        public virtual Guid? Superinstanceid { get; set; }

        [Display(Order = 7), InverseProperty("InstanceRelationships")]
        public virtual Instance Instance { get; set; }

        [Column("subinstanceid"), Display(Name = "Instance", Order = 8), ForeignKey("Instance")]
        public virtual Guid? Subinstanceid { get; set; }

        [Display(Name = "Instance Relationship Type", Order = 9)]
        public virtual InstanceRelationshipType InstanceRelationshipType { get; set; }

        [Column("instancerelationshiptypeid"), Display(Name = "Instance Relationship Type", Order = 10), ForeignKey("InstanceRelationshipType")]
        public virtual Guid? Instancerelationshiptypeid { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(Superinstanceid)} = {Superinstanceid}, {nameof(Subinstanceid)} = {Subinstanceid}, {nameof(Instancerelationshiptypeid)} = {Instancerelationshiptypeid} }}";

        public static InstanceRelationship FromJObject(JObject jObject) => jObject != null ? new InstanceRelationship
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToUniversalTime(),
            CreationUserId = (string)jObject.SelectToken("metadata.createdByUserId"),
            Superinstanceid = (Guid?)jObject.SelectToken("superInstanceId"),
            Subinstanceid = (Guid?)jObject.SelectToken("subInstanceId"),
            Instancerelationshiptypeid = (Guid?)jObject.SelectToken("instanceRelationshipTypeId")
        } : null;

        public JObject ToJObject() => JsonConvert.DeserializeObject<JObject>(Content, FolioDapperContext.LocalTimeJsonSerializationSettings);
    }
}
