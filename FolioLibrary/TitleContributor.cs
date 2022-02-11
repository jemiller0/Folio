using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FolioLibrary
{
    // uc.title_contributors -> uchicago_mod_orders_storage.titles
    // TitleContributor -> Title
    [DisplayColumn(nameof(Id)), DisplayName("Title Contributors"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("title_contributors", Schema = "uc")]
    public partial class TitleContributor
    {
        [Column("id"), ScaffoldColumn(false)]
        public virtual string Id { get; set; }

        [Display(Order = 2)]
        public virtual Title2 Title { get; set; }

        [Column("title_id"), Display(Name = "Title", Order = 3), Required]
        public virtual Guid? TitleId { get; set; }

        [Column("contributor"), Display(Order = 4), JsonProperty("contributor"), StringLength(1024)]
        public virtual string Contributor { get; set; }

        [Display(Name = "Contributor Name Type", Order = 5)]
        public virtual ContributorNameType2 ContributorNameType { get; set; }

        [Column("contributor_name_type_id"), Display(Name = "Contributor Name Type", Order = 6), JsonProperty("contributorNameTypeId"), Required]
        public virtual Guid? ContributorNameTypeId { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(TitleId)} = {TitleId}, {nameof(Contributor)} = {Contributor}, {nameof(ContributorNameTypeId)} = {ContributorNameTypeId} }}";

        public static TitleContributor FromJObject(JObject jObject) => jObject != null ? new TitleContributor
        {
            Contributor = (string)jObject.SelectToken("contributor"),
            ContributorNameTypeId = (Guid?)jObject.SelectToken("contributorNameTypeId")
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("contributor", Contributor),
            new JProperty("contributorNameTypeId", ContributorNameTypeId)).RemoveNullAndEmptyProperties();
    }
}
