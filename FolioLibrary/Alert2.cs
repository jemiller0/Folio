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
    // uc.alerts -> diku_mod_orders_storage.alert
    // Alert2 -> Alert
    [DisplayColumn(nameof(Id)), DisplayName("Alerts"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("alerts", Schema = "uc")]
    public partial class Alert2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Alert.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("alert"), Display(Order = 2), JsonProperty("alert"), Required, StringLength(1024)]
        public virtual string Alert { get; set; }

        [Column("content"), CustomValidation(typeof(Alert), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 3), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Order Item Alerts", Order = 4)]
        public virtual ICollection<OrderItemAlert> OrderItemAlerts { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Alert)} = {Alert}, {nameof(Content)} = {Content} }}";

        public static Alert2 FromJObject(JObject jObject) => jObject != null ? new Alert2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Alert = (string)jObject.SelectToken("alert"),
            Content = jObject.ToString()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("alert", Alert)).RemoveNullAndEmptyProperties();
    }
}
