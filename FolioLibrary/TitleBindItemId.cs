using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.title_bind_item_ids -> uchicago_mod_orders_storage.titles
    // TitleBindItemId -> Title
    [DisplayColumn(nameof(Content)), DisplayName("Title Bind Item Ids"), Table("title_bind_item_ids", Schema = "uc")]
    public partial class TitleBindItemId
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Title2 Title { get; set; }

        [Column("title_id"), Display(Name = "Title", Order = 3)]
        public virtual Guid? TitleId { get; set; }

        [Column("content"), Display(Order = 4), StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(TitleId)} = {TitleId}, {nameof(Content)} = {Content} }}";

        public static TitleBindItemId FromJObject(JValue jObject) => jObject != null ? new TitleBindItemId
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
