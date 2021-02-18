using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.request_policy_request_types -> diku_mod_circulation_storage.request_policy
    // RequestPolicyRequestType -> RequestPolicy
    [DisplayColumn(nameof(Content)), DisplayName("Request Policy Request Types"), Table("request_policy_request_types", Schema = "uc")]
    public partial class RequestPolicyRequestType
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Request Policy", Order = 2)]
        public virtual RequestPolicy2 RequestPolicy { get; set; }

        [Column("request_policy_id"), Display(Name = "Request Policy", Order = 3), Required]
        public virtual Guid? RequestPolicyId { get; set; }

        [Column("content"), Display(Order = 4), Required, StringLength(1024)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(RequestPolicyId)} = {RequestPolicyId}, {nameof(Content)} = {Content} }}";

        public static RequestPolicyRequestType FromJObject(JValue jObject) => jObject != null ? new RequestPolicyRequestType
        {
            Content = (string)jObject
        } : null;

        public JValue ToJObject() => new JValue(Content);
    }
}
