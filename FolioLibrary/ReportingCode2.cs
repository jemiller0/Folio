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
    // uc.reporting_codes -> diku_mod_orders_storage.reporting_code
    // ReportingCode2 -> ReportingCode
    [DisplayColumn(nameof(Id)), DisplayName("Reporting Codes"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("reporting_codes", Schema = "uc")]
    public partial class ReportingCode2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.ReportingCode.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("code"), Display(Order = 2), JsonProperty("code"), Required, StringLength(1024)]
        public virtual string Code { get; set; }

        [Column("description"), Display(Order = 3), JsonProperty("description"), StringLength(1024)]
        public virtual string Description { get; set; }

        [Column("content"), CustomValidation(typeof(ReportingCode), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 4), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Order Item Reporting Codes", Order = 5)]
        public virtual ICollection<OrderItemReportingCode> OrderItemReportingCodes { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Code)} = {Code}, {nameof(Description)} = {Description}, {nameof(Content)} = {Content} }}";

        public static ReportingCode2 FromJObject(JObject jObject) => jObject != null ? new ReportingCode2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Code = (string)jObject.SelectToken("code"),
            Description = (string)jObject.SelectToken("description"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("code", Code),
            new JProperty("description", Description)).RemoveNullAndEmptyProperties();
    }
}
