using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    [Table("item_order_items", Schema = "local")]
    public partial class ItemOrderItem
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual int? Id { get; set; }

        [Column("order_item_id"), ScaffoldColumn(false)]
        public virtual int? OrderItemId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(OrderItemId)} = {OrderItemId} }}";
    }
}
