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
    // uc.fund_types -> uchicago_mod_finance_storage.fund_type
    // FundType2 -> FundType
    [DisplayColumn(nameof(Name)), DisplayName("Fund Types"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("fund_types", Schema = "uc")]
    public partial class FundType2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.FundType.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("_version"), JsonProperty("_version"), ScaffoldColumn(false)]
        public virtual int? Version { get; set; }

        [Column("name"), Display(Order = 3), JsonProperty("name"), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("content"), CustomValidation(typeof(FundType), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 4), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Funds", Order = 5)]
        public virtual ICollection<Fund2> Fund2s { get; set; }

        [Display(Name = "Rollover Budgets", Order = 6)]
        public virtual ICollection<RolloverBudget2> RolloverBudget2s { get; set; }

        [Display(Name = "Rollover Budgets Rollovers", Order = 7)]
        public virtual ICollection<RolloverBudgetsRollover> RolloverBudgetsRollovers { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Version)} = {Version}, {nameof(Name)} = {Name}, {nameof(Content)} = {Content} }}";

        public static FundType2 FromJObject(JObject jObject) => jObject != null ? new FundType2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Version = (int?)jObject.SelectToken("_version"),
            Name = (string)jObject.SelectToken("name"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("_version", Version),
            new JProperty("name", Name)).RemoveNullAndEmptyProperties();
    }
}
