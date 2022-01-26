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
    // uc.custom_fields -> diku_mod_users.custom_fields
    // CustomField2 -> CustomField
    [DisplayColumn(nameof(Name)), DisplayName("Custom Fields"), JsonConverter(typeof(JsonPathJsonConverter<CustomField2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("custom_fields", Schema = "uc")]
    public partial class CustomField2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.CustomField.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("name"), Display(Order = 2), JsonProperty("name"), Required, StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("ref_id"), Display(Name = "Ref Id", Order = 3), Editable(false), JsonProperty("refId"), StringLength(128)]
        public virtual string RefId { get; set; }

        [Column("type"), Display(Order = 4), JsonProperty("type"), RegularExpression(@"^(RADIO_BUTTON|SINGLE_CHECKBOX|SINGLE_SELECT_DROPDOWN|MULTI_SELECT_DROPDOWN|TEXTBOX_SHORT|TEXTBOX_LONG)$"), Required, StringLength(1024)]
        public virtual string Type { get; set; }

        [Column("entity_type"), Display(Name = "Entity Type", Order = 5), JsonProperty("entityType"), Required, StringLength(1024)]
        public virtual string EntityType { get; set; }

        [Column("visible"), Display(Order = 6), JsonProperty("visible")]
        public virtual bool? Visible { get; set; }

        [Column("required"), Display(Order = 7), JsonProperty("required")]
        public virtual bool? Required { get; set; }

        [Column("is_repeatable"), Display(Name = "Is Repeatable", Order = 8), JsonProperty("isRepeatable")]
        public virtual bool? IsRepeatable { get; set; }

        [Column("order"), Display(Order = 9), Editable(false), JsonProperty("order")]
        public virtual int? Order { get; set; }

        [Column("help_text"), Display(Name = "Help Text", Order = 10), JsonProperty("helpText"), StringLength(1024)]
        public virtual string HelpText { get; set; }

        [Column("checkbox_field_default"), Display(Name = "Checkbox Field Default", Order = 11), JsonProperty("checkboxField.default")]
        public virtual bool? CheckboxFieldDefault { get; set; }

        [Column("select_field_multi_select"), Display(Name = "Select Field Multi Select", Order = 12), JsonProperty("selectField.multiSelect")]
        public virtual bool? SelectFieldMultiSelect { get; set; }

        [Column("select_field_options_sorting_order"), Display(Name = "Select Field Options Sorting Order", Order = 13), JsonProperty("selectField.options.sortingOrder"), RegularExpression(@"^(ASC|DESC|CUSTOM)$"), StringLength(1024)]
        public virtual string SelectFieldOptionsSortingOrder { get; set; }

        [Column("text_field_field_format"), Display(Name = "Text Field Field Format", Order = 14), JsonProperty("textField.fieldFormat"), RegularExpression(@"^(TEXT|EMAIL|URL|NUMBER)$"), Required, StringLength(1024)]
        public virtual string TextFieldFieldFormat { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 15), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 16), InverseProperty("CustomField2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 17), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 19), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 20), InverseProperty("CustomField2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 21), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(CustomField), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 23), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Custom Field Values", Order = 24), JsonProperty("selectField.options.values")]
        public virtual ICollection<CustomFieldValue> CustomFieldValues { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(RefId)} = {RefId}, {nameof(Type)} = {Type}, {nameof(EntityType)} = {EntityType}, {nameof(Visible)} = {Visible}, {nameof(Required)} = {Required}, {nameof(IsRepeatable)} = {IsRepeatable}, {nameof(Order)} = {Order}, {nameof(HelpText)} = {HelpText}, {nameof(CheckboxFieldDefault)} = {CheckboxFieldDefault}, {nameof(SelectFieldMultiSelect)} = {SelectFieldMultiSelect}, {nameof(SelectFieldOptionsSortingOrder)} = {SelectFieldOptionsSortingOrder}, {nameof(TextFieldFieldFormat)} = {TextFieldFieldFormat}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(CustomFieldValues)} = {(CustomFieldValues != null ? $"{{ {string.Join(", ", CustomFieldValues)} }}" : "")} }}";

        public static CustomField2 FromJObject(JObject jObject) => jObject != null ? new CustomField2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Name = (string)jObject.SelectToken("name"),
            RefId = (string)jObject.SelectToken("refId"),
            Type = (string)jObject.SelectToken("type"),
            EntityType = (string)jObject.SelectToken("entityType"),
            Visible = (bool?)jObject.SelectToken("visible"),
            Required = (bool?)jObject.SelectToken("required"),
            IsRepeatable = (bool?)jObject.SelectToken("isRepeatable"),
            Order = (int?)jObject.SelectToken("order"),
            HelpText = (string)jObject.SelectToken("helpText"),
            CheckboxFieldDefault = (bool?)jObject.SelectToken("checkboxField.default"),
            SelectFieldMultiSelect = (bool?)jObject.SelectToken("selectField.multiSelect"),
            SelectFieldOptionsSortingOrder = (string)jObject.SelectToken("selectField.options.sortingOrder"),
            TextFieldFieldFormat = (string)jObject.SelectToken("textField.fieldFormat"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            CustomFieldValues = jObject.SelectToken("selectField.options.values")?.Where(jt => jt.HasValues).Select(jt => CustomFieldValue.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("name", Name),
            new JProperty("refId", RefId),
            new JProperty("type", Type),
            new JProperty("entityType", EntityType),
            new JProperty("visible", Visible),
            new JProperty("required", Required),
            new JProperty("isRepeatable", IsRepeatable),
            new JProperty("order", Order),
            new JProperty("helpText", HelpText),
            new JProperty("checkboxField", new JObject(
                new JProperty("default", CheckboxFieldDefault))),
            new JProperty("selectField", new JObject(
                new JProperty("multiSelect", SelectFieldMultiSelect),
                new JProperty("options", new JObject(
                    new JProperty("sortingOrder", SelectFieldOptionsSortingOrder),
                    new JProperty("values", CustomFieldValues?.Select(cfv => cfv.ToJObject())))))),
            new JProperty("textField", new JObject(
                new JProperty("fieldFormat", TextFieldFieldFormat))),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername)))).RemoveNullAndEmptyProperties();
    }
}
