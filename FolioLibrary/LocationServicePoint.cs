using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.location_service_points -> diku_mod_inventory_storage.location
    // LocationServicePoint -> Location
    [DisplayColumn(nameof(Id)), DisplayName("Location Service Points"), Table("location_service_points", Schema = "uc")]
    public partial class LocationServicePoint
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Location2 Location { get; set; }

        [Column("location_id"), Display(Name = "Location", Order = 3), Required]
        public virtual Guid? LocationId { get; set; }

        [Display(Name = "Service Point", Order = 4)]
        public virtual ServicePoint2 ServicePoint { get; set; }

        [Column("service_point_id"), Display(Name = "Service Point", Order = 5), Required]
        public virtual Guid? ServicePointId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(LocationId)} = {LocationId}, {nameof(ServicePointId)} = {ServicePointId} }}";

        public static LocationServicePoint FromJObject(JValue jObject) => jObject != null ? new LocationServicePoint
        {
            ServicePointId = (Guid?)jObject
        } : null;

        public JValue ToJObject() => new JValue(ServicePointId);
    }
}
