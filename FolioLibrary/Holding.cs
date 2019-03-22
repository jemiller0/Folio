using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Reflection;

namespace FolioLibrary
{
    [Table("holdings_record", Schema = "diku_mod_inventory_storage")]
    public partial class Holding
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StringReader(value))
            using (var jtr = new JsonTextReader(sr))
            using (var sr2 = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Holding.json")))
            using (var jtr2 = new JsonTextReader(sr2))
            using (var jsvr = new JSchemaValidatingReader(jtr))
            using (var sw = new StringWriter())
            {
                jsvr.Schema = JSchema.Load(jtr2);
                jsvr.ValidationEventHandler += (sender, e) => sw.WriteLine(e.Message);
                try
                {
                    while (jsvr.Read()) ;
                    var s = sw.ToString();
                    if (s.Length != 0) return new ValidationResult($"The Content field is invalid. {s}", new[] { "Content" });
                }
                catch (Exception e)
                {
                    return new ValidationResult($"The Content field is invalid. {e.Message}", new[] { "Content" });
                }
            }
            return ValidationResult.Success;
        }

        [Column("_id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(Holding), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual string CreationUserId { get; set; }

        [Display(Order = 5)]
        public virtual Instance Instance { get; set; }

        [Column("instanceid"), Display(Name = "Instance", Order = 6), Editable(false), ForeignKey("Instance")]
        public virtual Guid? Instanceid { get; set; }

        [Display(Order = 7), InverseProperty("Holdings")]
        public virtual Location Location { get; set; }

        [Column("permanentlocationid"), Display(Name = "Location", Order = 8), Editable(false), ForeignKey("Location")]
        public virtual Guid? Permanentlocationid { get; set; }

        [Display(Name = "Location 1", Order = 9), InverseProperty("Holdings1")]
        public virtual Location Location1 { get; set; }

        [Column("temporarylocationid"), Display(Name = "Location 1", Order = 10), Editable(false), ForeignKey("Location1")]
        public virtual Guid? Temporarylocationid { get; set; }

        [Column("holdingstypeid"), Display(Name = "Holding Type", Order = 11), Editable(false), ForeignKey("HoldingType")]
        public virtual Guid? Holdingstypeid { get; set; }

        [Display(Name = "Holding Type", Order = 12)]
        public virtual HoldingType HoldingType { get; set; }

        [Display(Name = "Call Number Type", Order = 13)]
        public virtual CallNumberType CallNumberType { get; set; }

        [Column("callnumbertypeid"), Display(Name = "Call Number Type", Order = 14), Editable(false), ForeignKey("CallNumberType")]
        public virtual Guid? Callnumbertypeid { get; set; }

        [Display(Name = "Ill Policy", Order = 15)]
        public virtual IllPolicy IllPolicy { get; set; }

        [Column("illpolicyid"), Display(Name = "Ill Policy", Order = 16), Editable(false), ForeignKey("IllPolicy")]
        public virtual Guid? Illpolicyid { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Item> Items { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(Instanceid)} = {Instanceid}, {nameof(Permanentlocationid)} = {Permanentlocationid}, {nameof(Temporarylocationid)} = {Temporarylocationid}, {nameof(Holdingstypeid)} = {Holdingstypeid}, {nameof(Callnumbertypeid)} = {Callnumbertypeid}, {nameof(Illpolicyid)} = {Illpolicyid} }}";
    }
}
