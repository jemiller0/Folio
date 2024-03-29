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
    [Table("holdings_record", Schema = "uchicago_mod_inventory_storage")]
    public partial class Holding
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Holding.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(Holding), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual string CreationUserId { get; set; }

        [Display(Order = 5)]
        public virtual Instance Instance { get; set; }

        [Column("instanceid"), Display(Name = "Instance", Order = 6), ForeignKey("Instance")]
        public virtual Guid? Instanceid { get; set; }

        [Display(Name = "Location 1", Order = 7), InverseProperty("Holdings1")]
        public virtual Location Location1 { get; set; }

        [Column("permanentlocationid"), Display(Name = "Location 1", Order = 8), ForeignKey("Location1")]
        public virtual Guid? Permanentlocationid { get; set; }

        [Display(Name = "Location 2", Order = 9), InverseProperty("Holdings2")]
        public virtual Location Location2 { get; set; }

        [Column("temporarylocationid"), Display(Name = "Location 2", Order = 10), ForeignKey("Location2")]
        public virtual Guid? Temporarylocationid { get; set; }

        [Column("effectivelocationid"), Display(Name = "Location", Order = 11), ForeignKey("Location")]
        public virtual Guid? Effectivelocationid { get; set; }

        [Display(Order = 12), InverseProperty("Holdings")]
        public virtual Location Location { get; set; }

        [Column("holdingstypeid"), Display(Name = "Holding Type", Order = 13), ForeignKey("HoldingType")]
        public virtual Guid? Holdingstypeid { get; set; }

        [Display(Name = "Holding Type", Order = 14)]
        public virtual HoldingType HoldingType { get; set; }

        [Display(Name = "Call Number Type", Order = 15)]
        public virtual CallNumberType CallNumberType { get; set; }

        [Column("callnumbertypeid"), Display(Name = "Call Number Type", Order = 16), ForeignKey("CallNumberType")]
        public virtual Guid? Callnumbertypeid { get; set; }

        [Display(Name = "Ill Policy", Order = 17)]
        public virtual IllPolicy IllPolicy { get; set; }

        [Column("illpolicyid"), Display(Name = "Ill Policy", Order = 18), ForeignKey("IllPolicy")]
        public virtual Guid? Illpolicyid { get; set; }

        [Display(Order = 19)]
        public virtual Source Source { get; set; }

        [Column("sourceid"), Display(Name = "Source", Order = 20), ForeignKey("Source")]
        public virtual Guid? Sourceid { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<BoundWithPart> BoundWithParts { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Item> Items { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(Instanceid)} = {Instanceid}, {nameof(Permanentlocationid)} = {Permanentlocationid}, {nameof(Temporarylocationid)} = {Temporarylocationid}, {nameof(Effectivelocationid)} = {Effectivelocationid}, {nameof(Holdingstypeid)} = {Holdingstypeid}, {nameof(Callnumbertypeid)} = {Callnumbertypeid}, {nameof(Illpolicyid)} = {Illpolicyid}, {nameof(Sourceid)} = {Sourceid} }}";

        public static Holding FromJObject(JObject jObject) => jObject != null ? new Holding
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToUniversalTime(),
            CreationUserId = (string)jObject.SelectToken("metadata.createdByUserId"),
            Instanceid = (Guid?)jObject.SelectToken("instanceId"),
            Permanentlocationid = (Guid?)jObject.SelectToken("permanentLocationId"),
            Temporarylocationid = (Guid?)jObject.SelectToken("temporaryLocationId"),
            Effectivelocationid = (Guid?)jObject.SelectToken("effectiveLocationId"),
            Holdingstypeid = (Guid?)jObject.SelectToken("holdingsTypeId"),
            Callnumbertypeid = (Guid?)jObject.SelectToken("callNumberTypeId"),
            Illpolicyid = (Guid?)jObject.SelectToken("illPolicyId"),
            Sourceid = (Guid?)jObject.SelectToken("sourceId")
        } : null;

        public JObject ToJObject() => JsonConvert.DeserializeObject<JObject>(Content, FolioDapperContext.LocalTimeJsonSerializationSettings);
    }
}
