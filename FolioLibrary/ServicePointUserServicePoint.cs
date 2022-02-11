using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.service_point_user_service_points -> uchicago_mod_inventory_storage.service_point_user
    // ServicePointUserServicePoint -> ServicePointUser
    [DisplayColumn(nameof(Id)), DisplayName("Service Point User Service Points"), Table("service_point_user_service_points", Schema = "uc")]
    public partial class ServicePointUserServicePoint
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Name = "Service Point User", Order = 2)]
        public virtual ServicePointUser2 ServicePointUser { get; set; }

        [Column("service_point_user_id"), Display(Name = "Service Point User", Order = 3), Required]
        public virtual Guid? ServicePointUserId { get; set; }

        [Display(Name = "Service Point", Order = 4)]
        public virtual ServicePoint2 ServicePoint { get; set; }

        [Column("service_point_id"), Display(Name = "Service Point", Order = 5), Required]
        public virtual Guid? ServicePointId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ServicePointUserId)} = {ServicePointUserId}, {nameof(ServicePointId)} = {ServicePointId} }}";

        public static ServicePointUserServicePoint FromJObject(JValue jObject) => jObject != null ? new ServicePointUserServicePoint
        {
            ServicePointId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(ServicePointId);
    }
}
