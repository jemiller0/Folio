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
    [Table("preceding_succeeding_title", Schema = "diku_mod_inventory_storage")]
    public partial class PrecedingSucceedingTitle
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.PrecedingSucceedingTitle.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(PrecedingSucceedingTitle), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual string CreationUserId { get; set; }

        [Display(Order = 5), InverseProperty("PrecedingSucceedingTitles")]
        public virtual Instance Instance { get; set; }

        [Column("precedinginstanceid"), Display(Name = "Instance", Order = 6), ForeignKey("Instance")]
        public virtual Guid? Precedinginstanceid { get; set; }

        [Display(Name = "Instance 1", Order = 7), InverseProperty("PrecedingSucceedingTitles1")]
        public virtual Instance Instance1 { get; set; }

        [Column("succeedinginstanceid"), Display(Name = "Instance 1", Order = 8), ForeignKey("Instance1")]
        public virtual Guid? Succeedinginstanceid { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(Precedinginstanceid)} = {Precedinginstanceid}, {nameof(Succeedinginstanceid)} = {Succeedinginstanceid} }}";

        public static PrecedingSucceedingTitle FromJObject(JObject jObject) => jObject != null ? new PrecedingSucceedingTitle
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = jObject.ToString(),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (string)jObject.SelectToken("metadata.createdByUserId"),
            Precedinginstanceid = (Guid?)jObject.SelectToken("precedingInstanceId"),
            Succeedinginstanceid = (Guid?)jObject.SelectToken("succeedingInstanceId")
        } : null;

        public JObject ToJObject() => JObject.Parse(Content);
    }
}
