using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.order_item_product_ids -> uchicago_mod_orders_storage.po_line
    // OrderItemProductId -> OrderItem
    [DisplayColumn(nameof(Id)), DisplayName("Order Item Product Ids"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("order_item_product_ids", Schema = "uc")]
    public partial class OrderItemProductId
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Order Item", Order = 2)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("order_item_id"), Display(Name = "Order Item", Order = 3), Required]
        public virtual Guid? OrderItemId { get; set; }

        [Column("product_id"), Display(Name = "Product Id", Order = 4), JsonProperty("productId"), StringLength(1024)]
        public virtual string ProductId { get; set; }

        [Display(Name = "Product Id Type", Order = 5)]
        public virtual IdType2 ProductIdType { get; set; }

        [Column("product_id_type_id"), Display(Name = "Product Id Type", Order = 6), JsonProperty("productIdType")]
        public virtual Guid? ProductIdTypeId { get; set; }

        [Column("qualifier"), Display(Order = 7), JsonProperty("qualifier"), StringLength(1024)]
        public virtual string Qualifier { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(ProductId)} = {ProductId}, {nameof(ProductIdTypeId)} = {ProductIdTypeId}, {nameof(Qualifier)} = {Qualifier} }}";

        public static OrderItemProductId FromJObject(JObject jObject) => jObject != null ? new OrderItemProductId
        {
            ProductId = (string)jObject.SelectToken("productId"),
            ProductIdTypeId = (Guid?)jObject.SelectToken("productIdType"),
            Qualifier = (string)jObject.SelectToken("qualifier")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("productId", ProductId),
            new JProperty("productIdType", ProductIdTypeId),
            new JProperty("qualifier", Qualifier)).RemoveNullAndEmptyProperties();
    }
}
