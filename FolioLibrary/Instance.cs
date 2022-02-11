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
    [Table("instance", Schema = "uchicago_mod_inventory_storage")]
    public partial class Instance
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Instance.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(Instance), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual string CreationUserId { get; set; }

        [Display(Name = "Instance Status", Order = 5)]
        public virtual InstanceStatus InstanceStatus { get; set; }

        [Column("instancestatusid"), Display(Name = "Instance Status", Order = 6), ForeignKey("InstanceStatus")]
        public virtual Guid? Instancestatusid { get; set; }

        [Display(Name = "Mode Of Issuance", Order = 7)]
        public virtual ModeOfIssuance ModeOfIssuance { get; set; }

        [Column("modeofissuanceid"), Display(Name = "Mode Of Issuance", Order = 8), ForeignKey("ModeOfIssuance")]
        public virtual Guid? Modeofissuanceid { get; set; }

        [Display(Name = "Instance Type", Order = 9)]
        public virtual InstanceType InstanceType { get; set; }

        [Column("instancetypeid"), Display(Name = "Instance Type", Order = 10), ForeignKey("InstanceType")]
        public virtual Guid? Instancetypeid { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Holding> Holdings { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<InstanceRelationship> InstanceRelationships { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<InstanceRelationship> InstanceRelationships1 { get; set; }

        [Display(Name = "Instance Source Marc", Order = 14)]
        public virtual InstanceSourceMarc InstanceSourceMarc { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<PrecedingSucceedingTitle> PrecedingSucceedingTitles { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<PrecedingSucceedingTitle> PrecedingSucceedingTitles1 { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(Instancestatusid)} = {Instancestatusid}, {nameof(Modeofissuanceid)} = {Modeofissuanceid}, {nameof(Instancetypeid)} = {Instancetypeid} }}";

        public static Instance FromJObject(JObject jObject) => jObject != null ? new Instance
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToUniversalTime(),
            CreationUserId = (string)jObject.SelectToken("metadata.createdByUserId"),
            Instancestatusid = (Guid?)jObject.SelectToken("statusId"),
            Modeofissuanceid = (Guid?)jObject.SelectToken("modeOfIssuanceId"),
            Instancetypeid = (Guid?)jObject.SelectToken("instanceTypeId")
        } : null;

        public JObject ToJObject() => JsonConvert.DeserializeObject<JObject>(Content, FolioDapperContext.LocalTimeJsonSerializationSettings);
    }
}
