using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.order_item_claims -> diku_mod_orders_storage.po_line
    // OrderItemClaim -> OrderItem
    [DisplayColumn(nameof(Id)), DisplayName("Order Item Claims"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("order_item_claims", Schema = "uc")]
    public partial class OrderItemClaim
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Order Item", Order = 2)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("order_item_id"), Display(Name = "Order Item", Order = 3), Required]
        public virtual Guid? OrderItemId { get; set; }

        [Column("claimed"), Display(Order = 4), JsonProperty("claimed")]
        public virtual bool? Claimed { get; set; }

        [Column("sent"), Display(Order = 5), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true), JsonProperty("sent")]
        public virtual DateTime? Sent { get; set; }

        [Column("grace"), Display(Order = 6), JsonProperty("grace")]
        public virtual int? Grace { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(Claimed)} = {Claimed}, {nameof(Sent)} = {Sent}, {nameof(Grace)} = {Grace} }}";

        public static OrderItemClaim FromJObject(JObject jObject) => jObject != null ? new OrderItemClaim
        {
            Claimed = (bool?)jObject.SelectToken("claimed"),
            Sent = (DateTime?)jObject.SelectToken("sent"),
            Grace = (int?)jObject.SelectToken("grace")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("claimed", Claimed),
            new JProperty("sent", Sent?.ToLocalTime()),
            new JProperty("grace", Grace)).RemoveNullAndEmptyProperties();
    }
}
