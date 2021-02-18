using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.template_output_formats -> diku_mod_template_engine.template
    // TemplateOutputFormat -> Template
    [DisplayColumn(nameof(Content)), DisplayName("Template Output Formats"), Table("template_output_formats", Schema = "uc")]
    public partial class TemplateOutputFormat
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Template2 Template { get; set; }

        [Column("template_id"), Display(Name = "Template", Order = 3), Required]
        public virtual Guid? TemplateId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(TemplateId)} = {TemplateId}, {nameof(Content)} = {Content} }}";

        public static TemplateOutputFormat FromJObject(JValue jObject) => jObject != null ? new TemplateOutputFormat
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
