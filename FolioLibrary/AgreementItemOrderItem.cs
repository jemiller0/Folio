using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.agreement_item_order_items -> uc_agreements.agreement_items
    // AgreementItemOrderItem -> AgreementItem
    [DisplayColumn(nameof(Id)), DisplayName("Agreement Item Order Items"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("agreement_item_order_items", Schema = "uc")]
    public partial class AgreementItemOrderItem
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Agreement Item", Order = 2)]
        public virtual AgreementItem2 AgreementItem { get; set; }

        [Column("agreement_item_id"), Display(Name = "Agreement Item", Order = 3)]
        public virtual Guid? AgreementItemId { get; set; }

        [Display(Name = "Order Item", Order = 4)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("order_item_id"), Display(Name = "Order Item", Order = 5), JsonProperty("poLineId")]
        public virtual Guid? OrderItemId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(AgreementItemId)} = {AgreementItemId}, {nameof(OrderItemId)} = {OrderItemId} }}";

        public static AgreementItemOrderItem FromJObject(JObject jObject) => jObject != null ? new AgreementItemOrderItem
        {
            OrderItemId = (Guid?)jObject.SelectToken("poLineId")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("poLineId", OrderItemId)).RemoveNullAndEmptyProperties();
    }
}
