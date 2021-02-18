using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.title_product_ids -> diku_mod_orders_storage.titles
    // TitleProductId -> Title
    [DisplayColumn(nameof(Id)), DisplayName("Title Product Ids"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("title_product_ids", Schema = "uc")]
    public partial class TitleProductId
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Title2 Title { get; set; }

        [Column("title_id"), Display(Name = "Title", Order = 3), Required]
        public virtual Guid? TitleId { get; set; }

        [Column("product_id"), Display(Name = "Product Id", Order = 4), JsonProperty("productId"), StringLength(1024)]
        public virtual string ProductId { get; set; }

        [Display(Name = "Product Id Type", Order = 5)]
        public virtual IdType2 ProductIdType { get; set; }

        [Column("product_id_type_id"), Display(Name = "Product Id Type", Order = 6), JsonProperty("productIdType")]
        public virtual Guid? ProductIdTypeId { get; set; }

        [Column("qualifier"), Display(Order = 7), JsonProperty("qualifier"), StringLength(1024)]
        public virtual string Qualifier { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(TitleId)} = {TitleId}, {nameof(ProductId)} = {ProductId}, {nameof(ProductIdTypeId)} = {ProductIdTypeId}, {nameof(Qualifier)} = {Qualifier} }}";

        public static TitleProductId FromJObject(JObject jObject) => jObject != null ? new TitleProductId
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
