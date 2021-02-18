using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.addresses -> uc.configurations
    // Address -> Configuration2
    [DisplayColumn(nameof(Name)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("addresses", Schema = "uc")]
    public partial class Address
    {
        [Column("id"), JsonProperty("id"), ScaffoldColumn(false)]
        public virtual Guid? Id { get; set; }

        [Column("name"), Display(Order = 2), StringLength(1024)]
        public virtual string Name { get; set; }

        [Column("content"), Display(Order = 3), StringLength(128)]
        public virtual string Content { get; set; }

        [Column("enabled"), Display(Order = 4)]
        public virtual bool? Enabled { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 5), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 6), InverseProperty("Addresses")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 7), Editable(false)]
        public virtual Guid? CreationUserId { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 8), DisplayFormat(DataFormatString = "{0:g}"), Editable(false)]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 9), InverseProperty("Addresses1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 10), Editable(false)]
        public virtual Guid? LastWriteUserId { get; set; }

        [Display(Name = "Orders", Order = 11)]
        public virtual ICollection<Order2> Order2s { get; set; }

        [Display(Name = "Orders 1", Order = 12)]
        public virtual ICollection<Order2> Order2s1 { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Name)} = {Name}, {nameof(Content)} = {Content}, {nameof(Enabled)} = {Enabled}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId} }}";

        public static Address FromJObject(JObject jObject)
        {
            var jo = JObject.Parse((string)jObject["value"]);
            return new Address
            {
                Id = (Guid?)jObject.SelectToken("id"),
                Enabled = (bool?)jObject.SelectToken("enabled"),
                Name = (string)jo.SelectToken("name"),
                Content = (string)jo.SelectToken("address"),
                CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
                CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
                LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
                LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId")
            };
        }

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("module", "TENANT"),
            new JProperty("configName", "tenant.addresses"),
            new JProperty("code", Id),
            new JProperty("enabled", Enabled),
            new JProperty("value", new JObject(
                new JProperty("name", Name),
                new JProperty("address", Content)).ToString()),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("updatedDate", LastWriteTime),
                new JProperty("updatedByUserId", LastWriteUserId)))).RemoveNullAndEmptyProperties();
    }
}
