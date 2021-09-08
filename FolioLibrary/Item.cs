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
    [Table("item", Schema = "diku_mod_inventory_storage")]
    public partial class Item
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Item.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(Item), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual string CreationUserId { get; set; }

        [Display(Order = 5)]
        public virtual Holding Holding { get; set; }

        [Column("holdingsrecordid"), Display(Name = "Holding", Order = 6), ForeignKey("Holding")]
        public virtual Guid? Holdingsrecordid { get; set; }

        [Display(Name = "Loan Type", Order = 7), InverseProperty("Items")]
        public virtual LoanType LoanType { get; set; }

        [Column("permanentloantypeid"), Display(Name = "Loan Type", Order = 8), ForeignKey("LoanType")]
        public virtual Guid? Permanentloantypeid { get; set; }

        [Display(Name = "Loan Type 1", Order = 9), InverseProperty("Items1")]
        public virtual LoanType LoanType1 { get; set; }

        [Column("temporaryloantypeid"), Display(Name = "Loan Type 1", Order = 10), ForeignKey("LoanType1")]
        public virtual Guid? Temporaryloantypeid { get; set; }

        [Display(Name = "Material Type", Order = 11)]
        public virtual MaterialType MaterialType { get; set; }

        [Column("materialtypeid"), Display(Name = "Material Type", Order = 12), ForeignKey("MaterialType")]
        public virtual Guid? Materialtypeid { get; set; }

        [Display(Name = "Location 1", Order = 13), InverseProperty("Items1")]
        public virtual Location Location1 { get; set; }

        [Column("permanentlocationid"), Display(Name = "Location 1", Order = 14), ForeignKey("Location1")]
        public virtual Guid? Permanentlocationid { get; set; }

        [Display(Name = "Location 2", Order = 15), InverseProperty("Items2")]
        public virtual Location Location2 { get; set; }

        [Column("temporarylocationid"), Display(Name = "Location 2", Order = 16), ForeignKey("Location2")]
        public virtual Guid? Temporarylocationid { get; set; }

        [Column("effectivelocationid"), Display(Name = "Location", Order = 17), ForeignKey("Location")]
        public virtual Guid? Effectivelocationid { get; set; }

        [Display(Order = 18), InverseProperty("Items")]
        public virtual Location Location { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<BoundWithPart> BoundWithParts { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(Holdingsrecordid)} = {Holdingsrecordid}, {nameof(Permanentloantypeid)} = {Permanentloantypeid}, {nameof(Temporaryloantypeid)} = {Temporaryloantypeid}, {nameof(Materialtypeid)} = {Materialtypeid}, {nameof(Permanentlocationid)} = {Permanentlocationid}, {nameof(Temporarylocationid)} = {Temporarylocationid}, {nameof(Effectivelocationid)} = {Effectivelocationid} }}";

        public static Item FromJObject(JObject jObject) => jObject != null ? new Item
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToUniversalTime(),
            CreationUserId = (string)jObject.SelectToken("metadata.createdByUserId"),
            Holdingsrecordid = (Guid?)jObject.SelectToken("holdingsRecordId"),
            Permanentloantypeid = (Guid?)jObject.SelectToken("permanentLoanTypeId"),
            Temporaryloantypeid = (Guid?)jObject.SelectToken("temporaryLoanTypeId"),
            Materialtypeid = (Guid?)jObject.SelectToken("materialTypeId"),
            Permanentlocationid = (Guid?)jObject.SelectToken("permanentLocationId"),
            Temporarylocationid = (Guid?)jObject.SelectToken("temporaryLocationId"),
            Effectivelocationid = (Guid?)jObject.SelectToken("effectiveLocationId")
        } : null;

        public JObject ToJObject() => JsonConvert.DeserializeObject<JObject>(Content, FolioDapperContext.LocalTimeJsonSerializationSettings);
    }
}
