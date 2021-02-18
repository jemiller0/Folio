using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.request_tags -> diku_mod_circulation_storage.request
    // RequestTag -> Request
    [DisplayColumn(nameof(Content)), DisplayName("Request Tags"), Table("request_tags", Schema = "uc")]
    public partial class RequestTag
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Request2 Request { get; set; }

        [Column("request_id"), Display(Name = "Request", Order = 3), Required]
        public virtual Guid? RequestId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(RequestId)} = {RequestId}, {nameof(Content)} = {Content} }}";

        public static RequestTag FromJObject(JValue jObject) => jObject != null ? new RequestTag
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
