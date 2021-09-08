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
    [Table("location", Schema = "diku_mod_inventory_storage")]
    public partial class Location
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Location.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(Location), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual string CreationUserId { get; set; }

        [Display(Order = 5)]
        public virtual Institution Institution { get; set; }

        [Column("institutionid"), Display(Name = "Institution", Order = 6), ForeignKey("Institution")]
        public virtual Guid? Institutionid { get; set; }

        [Display(Order = 7)]
        public virtual Campus Campus { get; set; }

        [Column("campusid"), Display(Name = "Campus", Order = 8), ForeignKey("Campus")]
        public virtual Guid? Campusid { get; set; }

        [Display(Order = 9)]
        public virtual Library Library { get; set; }

        [Column("libraryid"), Display(Name = "Library", Order = 10), ForeignKey("Library")]
        public virtual Guid? Libraryid { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Holding> Holdings { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Holding> Holdings1 { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Holding> Holdings2 { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Item> Items { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Item> Items1 { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Item> Items2 { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(Institutionid)} = {Institutionid}, {nameof(Campusid)} = {Campusid}, {nameof(Libraryid)} = {Libraryid} }}";

        public static Location FromJObject(JObject jObject) => jObject != null ? new Location
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToUniversalTime(),
            CreationUserId = (string)jObject.SelectToken("metadata.createdByUserId"),
            Institutionid = (Guid?)jObject.SelectToken("institutionId"),
            Campusid = (Guid?)jObject.SelectToken("campusId"),
            Libraryid = (Guid?)jObject.SelectToken("libraryId")
        } : null;

        public JObject ToJObject() => JsonConvert.DeserializeObject<JObject>(Content, FolioDapperContext.LocalTimeJsonSerializationSettings);
    }
}
