using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.order_item_contributors -> diku_mod_orders_storage.po_line
    // OrderItemContributor -> OrderItem
    [DisplayColumn(nameof(Id)), DisplayName("Order Item Contributors"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("order_item_contributors", Schema = "uc")]
    public partial class OrderItemContributor
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Order Item", Order = 2)]
        public virtual OrderItem2 OrderItem { get; set; }

        [Column("order_item_id"), Display(Name = "Order Item", Order = 3), Required]
        public virtual Guid? OrderItemId { get; set; }

        [Column("contributor"), Display(Order = 4), JsonProperty("contributor"), StringLength(1024)]
        public virtual string Contributor { get; set; }

        [Display(Name = "Contributor Name Type", Order = 5)]
        public virtual ContributorNameType2 ContributorNameType { get; set; }

        [Column("contributor_name_type_id"), Display(Name = "Contributor Name Type", Order = 6), JsonProperty("contributorNameTypeId"), Required]
        public virtual Guid? ContributorNameTypeId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrderItemId)} = {OrderItemId}, {nameof(Contributor)} = {Contributor}, {nameof(ContributorNameTypeId)} = {ContributorNameTypeId} }}";

        public static OrderItemContributor FromJObject(JObject jObject) => jObject != null ? new OrderItemContributor
        {
            Contributor = (string)jObject.SelectToken("contributor"),
            ContributorNameTypeId = (Guid?)jObject.SelectToken("contributorNameTypeId")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("contributor", Contributor),
            new JProperty("contributorNameTypeId", ContributorNameTypeId)).RemoveNullAndEmptyProperties();
    }
}
