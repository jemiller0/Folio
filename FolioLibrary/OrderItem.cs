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
    [Table("po_line", Schema = "diku_mod_orders_storage")]
    public partial class OrderItem
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.OrderItem.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false)]
        public virtual Guid? Id { get; set; }

        [Column("jsonb"), CustomValidation(typeof(OrderItem), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 2), Required]
        public virtual string Content { get; set; }

        [Column("creation_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 3), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Column("created_by"), Display(Name = "Creation User Id", Order = 4), Editable(false)]
        public virtual string CreationUserId { get; set; }

        [Display(Order = 5)]
        public virtual Order Order { get; set; }

        [Column("purchaseorderid"), Display(Name = "Order", Order = 6), ForeignKey("Order")]
        public virtual Guid? Purchaseorderid { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Piece> Pieces { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Title> Titles { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Content)} = {Content}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(Purchaseorderid)} = {Purchaseorderid} }}";

        public static OrderItem FromJObject(JObject jObject) => new OrderItem
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Content = jObject.ToString(),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (string)jObject.SelectToken("metadata.createdByUserId"),
            Purchaseorderid = (Guid?)jObject.SelectToken("purchaseOrderId")
        };

        public JObject ToJObject() => JObject.Parse(Content);
    }
}
