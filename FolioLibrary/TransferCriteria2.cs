using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    // uc.transfer_criterias -> uchicago_mod_feesfines.transfer_criteria
    // TransferCriteria2 -> TransferCriteria
    [DisplayColumn(nameof(Id)), DisplayName("Transfer Criterias"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("transfer_criterias", Schema = "uc")]
    public partial class TransferCriteria2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.TransferCriteria.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("criteria"), Display(Order = 2), JsonProperty("criteria"), StringLength(1024)]
        public virtual string Criteria { get; set; }

        [Column("type"), Display(Order = 3), JsonProperty("type"), StringLength(1024)]
        public virtual string Type { get; set; }

        [Column("value"), Display(Order = 4), JsonProperty("value")]
        public virtual decimal? Value { get; set; }

        [Column("interval"), Display(Order = 5), JsonProperty("interval"), StringLength(1024)]
        public virtual string Interval { get; set; }

        [Column("content"), CustomValidation(typeof(TransferCriteria), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 6), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Criteria)} = {Criteria}, {nameof(Type)} = {Type}, {nameof(Value)} = {Value}, {nameof(Interval)} = {Interval}, {nameof(Content)} = {Content} }}";

        public static TransferCriteria2 FromJObject(JObject jObject) => jObject != null ? new TransferCriteria2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Criteria = (string)jObject.SelectToken("criteria"),
            Type = (string)jObject.SelectToken("type"),
            Value = (decimal?)jObject.SelectToken("value"),
            Interval = (string)jObject.SelectToken("interval"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("criteria", Criteria),
            new JProperty("type", Type),
            new JProperty("value", Value),
            new JProperty("interval", Interval)).RemoveNullAndEmptyProperties();
    }
}
