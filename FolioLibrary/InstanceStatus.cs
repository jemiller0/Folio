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
    [Table("instance_status", Schema = "diku_mod_inventory_storage")]
    public partial class InstanceStatus
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StringReader(value))
            using (var jtr = new JsonTextReader(sr))
            using (var sr2 = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.InstanceStatus.json")))
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

        [Column("jsonb"), CustomValidation(typeof(InstanceStatus), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual string CreationUserId { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Instance> Instances { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId} }}";
    }
}
