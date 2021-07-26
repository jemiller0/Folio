using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    // uc.order_templates -> diku_mod_orders_storage.order_templates
    // OrderTemplate2 -> OrderTemplate
    [DisplayColumn(nameof(Id)), DisplayName("Order Templates"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("order_templates", Schema = "uc")]
    public partial class OrderTemplate2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.OrderTemplate.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("template_name"), Display(Name = "Template Name", Order = 2), JsonProperty("templateName"), Required, StringLength(1024)]
        public virtual string TemplateName { get; set; }

        [Column("template_code"), Display(Name = "Template Code", Order = 3), JsonProperty("templateCode"), StringLength(1024)]
        public virtual string TemplateCode { get; set; }

        [Column("template_description"), Display(Name = "Template Description", Order = 4), JsonProperty("templateDescription"), StringLength(1024)]
        public virtual string TemplateDescription { get; set; }

        [Column("content"), CustomValidation(typeof(OrderTemplate), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 5), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Orders", Order = 6)]
        public virtual ICollection<Order2> Order2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(TemplateName)} = {TemplateName}, {nameof(TemplateCode)} = {TemplateCode}, {nameof(TemplateDescription)} = {TemplateDescription}, {nameof(Content)} = {Content} }}";

        public static OrderTemplate2 FromJObject(JObject jObject) => jObject != null ? new OrderTemplate2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            TemplateName = (string)jObject.SelectToken("templateName"),
            TemplateCode = (string)jObject.SelectToken("templateCode"),
            TemplateDescription = (string)jObject.SelectToken("templateDescription"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("templateName", TemplateName),
            new JProperty("templateCode", TemplateCode),
            new JProperty("templateDescription", TemplateDescription)).RemoveNullAndEmptyProperties();
    }
}
