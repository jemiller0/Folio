using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    // uc.instances -> diku_mod_inventory_storage.instance
    // Instance2 -> Instance
    [DisplayColumn(nameof(Title)), DisplayName("Instances"), JsonConverter(typeof(JsonPathJsonConverter<Instance2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("instances", Schema = "uc")]
    public partial class Instance2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.Instance.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Column("hrid"), Display(Name = "Short Id", Order = 2), Editable(false), JsonConverter(typeof(StringJsonConverter<int?>)), JsonProperty("hrid")]
        public virtual int? ShortId { get; set; }

        [Column("match_key"), Display(Name = "Match Key", Order = 3), JsonProperty("matchKey"), StringLength(1024)]
        public virtual string MatchKey { get; set; }

        [Column("source"), Display(Order = 4), Editable(false), JsonProperty("source"), RegularExpression(@"^(FOLIO|MARC|EPKB)$"), StringLength(1024)]
        public virtual string Source { get; set; }

        [Column("title"), Display(Order = 5), JsonProperty("title"), Required, StringLength(1024)]
        public virtual string Title { get; set; }

        [Column("author"), Display(Order = 6), Editable(false), JsonProperty("contributors.0.name"), StringLength(1024)]
        public virtual string Author { get; set; }

        [Column("publication_year"), Display(Name = "Publication Year", Order = 7), Editable(false), JsonProperty("publication.0.dateOfPublication"), StringLength(1024)]
        public virtual string PublicationYear { get; set; }

        [Column("index_title"), JsonProperty("indexTitle"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string IndexTitle { get; set; }

        [Display(Name = "Instance Type", Order = 9)]
        public virtual InstanceType2 InstanceType { get; set; }

        [Column("instance_type_id"), Display(Name = "Instance Type", Order = 10), JsonProperty("instanceTypeId"), Required]
        public virtual Guid? InstanceTypeId { get; set; }

        [Display(Name = "Issuance Mode", Order = 11)]
        public virtual IssuanceMode IssuanceMode { get; set; }

        [Column("mode_of_issuance_id"), Display(Name = "Issuance Mode", Order = 12), JsonProperty("modeOfIssuanceId")]
        public virtual Guid? IssuanceModeId { get; set; }

        [Column("cataloged_date"), DataType(DataType.Date), Display(Name = "Cataloged Date", Order = 13), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("catalogedDate")]
        public virtual DateTime? CatalogedDate { get; set; }

        [Column("previously_held"), Display(Name = "Previously Held", Order = 14), JsonProperty("previouslyHeld")]
        public virtual bool? PreviouslyHeld { get; set; }

        [Column("staff_suppress"), Display(Name = "Staff Suppress", Order = 15), JsonProperty("staffSuppress")]
        public virtual bool? StaffSuppress { get; set; }

        [Column("discovery_suppress"), Display(Name = "Discovery Suppress", Order = 16), JsonProperty("discoverySuppress")]
        public virtual bool? DiscoverySuppress { get; set; }

        [Column("source_record_format"), Display(Name = "Source Record Format", Order = 17), Editable(false), JsonProperty("sourceRecordFormat"), RegularExpression(@"^(MARC-JSON)$"), StringLength(1024)]
        public virtual string SourceRecordFormat { get; set; }

        [Display(Order = 18)]
        public virtual Status Status { get; set; }

        [Column("status_id"), Display(Name = "Status", Order = 19), JsonProperty("statusId")]
        public virtual Guid? StatusId { get; set; }

        [Column("status_updated_date"), DataType(DataType.DateTime), Display(Name = "Status Last Write Time", Order = 20), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("statusUpdatedDate")]
        public virtual DateTime? StatusLastWriteTime { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 21), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 22), InverseProperty("Instance2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 23), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 25), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 26), InverseProperty("Instance2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 27), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Instance), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 29), Editable(false)]
        public virtual string Content { get; set; }

        [Display(Name = "Alternative Titles", Order = 30), JsonProperty("alternativeTitles")]
        public virtual ICollection<AlternativeTitle> AlternativeTitles { get; set; }

        [Display(Order = 31), JsonProperty("classifications")]
        public virtual ICollection<Classification> Classifications { get; set; }

        [Display(Order = 32), JsonProperty("contributors")]
        public virtual ICollection<Contributor> Contributors { get; set; }

        [Display(Order = 33), JsonConverter(typeof(ArrayJsonConverter<List<Edition>, Edition>), "Content"), JsonProperty("editions")]
        public virtual ICollection<Edition> Editions { get; set; }

        [Display(Name = "Electronic Accesses", Order = 34), JsonProperty("electronicAccess")]
        public virtual ICollection<ElectronicAccess> ElectronicAccesses { get; set; }

        [Display(Name = "Fees", Order = 35)]
        public virtual ICollection<Fee2> Fee2s { get; set; }

        [Display(Name = "Holdings", Order = 36)]
        public virtual ICollection<Holding2> Holding2s { get; set; }

        [Display(Order = 37), JsonProperty("identifiers")]
        public virtual ICollection<Identifier> Identifiers { get; set; }

        [Display(Name = "Instance Formats", Order = 38), JsonConverter(typeof(ArrayJsonConverter<List<InstanceFormat2>, InstanceFormat2>), "FormatId"), JsonProperty("instanceFormatIds")]
        public virtual ICollection<InstanceFormat2> InstanceFormat2s { get; set; }

        [Display(Name = "Instance Nature Of Content Terms", Order = 39), JsonConverter(typeof(ArrayJsonConverter<List<InstanceNatureOfContentTerm>, InstanceNatureOfContentTerm>), "NatureOfContentTermId"), JsonProperty("natureOfContentTermIds")]
        public virtual ICollection<InstanceNatureOfContentTerm> InstanceNatureOfContentTerms { get; set; }

        [Display(Name = "Instance Statistical Codes", Order = 40), JsonConverter(typeof(ArrayJsonConverter<List<InstanceStatisticalCode>, InstanceStatisticalCode>), "StatisticalCodeId"), JsonProperty("statisticalCodeIds")]
        public virtual ICollection<InstanceStatisticalCode> InstanceStatisticalCodes { get; set; }

        [Display(Name = "Instance Tags", Order = 41), JsonConverter(typeof(ArrayJsonConverter<List<InstanceTag>, InstanceTag>), "Content"), JsonProperty("tags.tagList")]
        public virtual ICollection<InstanceTag> InstanceTags { get; set; }

        [Display(Order = 42), JsonConverter(typeof(ArrayJsonConverter<List<Language>, Language>), "Content"), JsonProperty("languages")]
        public virtual ICollection<Language> Languages { get; set; }

        [Display(Name = "Notes", Order = 43), JsonProperty("notes")]
        public virtual ICollection<Note2> Note2s { get; set; }

        [Display(Name = "Order Items", Order = 44)]
        public virtual ICollection<OrderItem2> OrderItem2s { get; set; }

        [Display(Name = "Physical Descriptions", Order = 45), JsonConverter(typeof(ArrayJsonConverter<List<PhysicalDescription>, PhysicalDescription>), "Content"), JsonProperty("physicalDescriptions")]
        public virtual ICollection<PhysicalDescription> PhysicalDescriptions { get; set; }

        [Display(Name = "Preceding Succeeding Titles", Order = 46)]
        public virtual ICollection<PrecedingSucceedingTitle2> PrecedingSucceedingTitle2s { get; set; }

        [Display(Name = "Preceding Succeeding Titles 1", Order = 47)]
        public virtual ICollection<PrecedingSucceedingTitle2> PrecedingSucceedingTitle2s1 { get; set; }

        [Display(Name = "Publication Frequencies", Order = 48), JsonConverter(typeof(ArrayJsonConverter<List<PublicationFrequency>, PublicationFrequency>), "Content"), JsonProperty("publicationFrequency")]
        public virtual ICollection<PublicationFrequency> PublicationFrequencies { get; set; }

        [Display(Name = "Publication Ranges", Order = 49), JsonConverter(typeof(ArrayJsonConverter<List<PublicationRange>, PublicationRange>), "Content"), JsonProperty("publicationRange")]
        public virtual ICollection<PublicationRange> PublicationRanges { get; set; }

        [Display(Order = 50), JsonProperty("publication")]
        public virtual ICollection<Publication> Publications { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Record2> Record2s { get; set; }

        [Display(Order = 52)]
        public virtual ICollection<Relationship> Relationships { get; set; }

        [Display(Name = "Relationships 1", Order = 53)]
        public virtual ICollection<Relationship> Relationships1 { get; set; }

        [Display(Order = 54), JsonConverter(typeof(ArrayJsonConverter<List<Series>, Series>), "Content"), JsonProperty("series")]
        public virtual ICollection<Series> Series { get; set; }

        [Display(Order = 55), JsonConverter(typeof(ArrayJsonConverter<List<Subject>, Subject>), "Content"), JsonProperty("subjects")]
        public virtual ICollection<Subject> Subjects { get; set; }

        [Display(Name = "Titles", Order = 56)]
        public virtual ICollection<Title2> Title2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(ShortId)} = {ShortId}, {nameof(MatchKey)} = {MatchKey}, {nameof(Source)} = {Source}, {nameof(Title)} = {Title}, {nameof(Author)} = {Author}, {nameof(PublicationYear)} = {PublicationYear}, {nameof(IndexTitle)} = {IndexTitle}, {nameof(InstanceTypeId)} = {InstanceTypeId}, {nameof(IssuanceModeId)} = {IssuanceModeId}, {nameof(CatalogedDate)} = {CatalogedDate}, {nameof(PreviouslyHeld)} = {PreviouslyHeld}, {nameof(StaffSuppress)} = {StaffSuppress}, {nameof(DiscoverySuppress)} = {DiscoverySuppress}, {nameof(SourceRecordFormat)} = {SourceRecordFormat}, {nameof(StatusId)} = {StatusId}, {nameof(StatusLastWriteTime)} = {StatusLastWriteTime}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(AlternativeTitles)} = {(AlternativeTitles != null ? $"{{ {string.Join(", ", AlternativeTitles)} }}" : "")}, {nameof(Classifications)} = {(Classifications != null ? $"{{ {string.Join(", ", Classifications)} }}" : "")}, {nameof(Contributors)} = {(Contributors != null ? $"{{ {string.Join(", ", Contributors)} }}" : "")}, {nameof(Editions)} = {(Editions != null ? $"{{ {string.Join(", ", Editions)} }}" : "")}, {nameof(ElectronicAccesses)} = {(ElectronicAccesses != null ? $"{{ {string.Join(", ", ElectronicAccesses)} }}" : "")}, {nameof(Identifiers)} = {(Identifiers != null ? $"{{ {string.Join(", ", Identifiers)} }}" : "")}, {nameof(InstanceFormat2s)} = {(InstanceFormat2s != null ? $"{{ {string.Join(", ", InstanceFormat2s)} }}" : "")}, {nameof(InstanceNatureOfContentTerms)} = {(InstanceNatureOfContentTerms != null ? $"{{ {string.Join(", ", InstanceNatureOfContentTerms)} }}" : "")}, {nameof(InstanceStatisticalCodes)} = {(InstanceStatisticalCodes != null ? $"{{ {string.Join(", ", InstanceStatisticalCodes)} }}" : "")}, {nameof(InstanceTags)} = {(InstanceTags != null ? $"{{ {string.Join(", ", InstanceTags)} }}" : "")}, {nameof(Languages)} = {(Languages != null ? $"{{ {string.Join(", ", Languages)} }}" : "")}, {nameof(Note2s)} = {(Note2s != null ? $"{{ {string.Join(", ", Note2s)} }}" : "")}, {nameof(PhysicalDescriptions)} = {(PhysicalDescriptions != null ? $"{{ {string.Join(", ", PhysicalDescriptions)} }}" : "")}, {nameof(PublicationFrequencies)} = {(PublicationFrequencies != null ? $"{{ {string.Join(", ", PublicationFrequencies)} }}" : "")}, {nameof(PublicationRanges)} = {(PublicationRanges != null ? $"{{ {string.Join(", ", PublicationRanges)} }}" : "")}, {nameof(Publications)} = {(Publications != null ? $"{{ {string.Join(", ", Publications)} }}" : "")}, {nameof(Series)} = {(Series != null ? $"{{ {string.Join(", ", Series)} }}" : "")}, {nameof(Subjects)} = {(Subjects != null ? $"{{ {string.Join(", ", Subjects)} }}" : "")} }}";

        public static Instance2 FromJObject(JObject jObject) => jObject != null ? new Instance2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            ShortId = (int?)jObject.SelectToken("hrid"),
            MatchKey = (string)jObject.SelectToken("matchKey"),
            Source = (string)jObject.SelectToken("source"),
            Title = (string)jObject.SelectToken("title"),
            Author = (string)jObject.SelectToken("contributors.0.name"),
            PublicationYear = (string)jObject.SelectToken("publication.0.dateOfPublication"),
            IndexTitle = (string)jObject.SelectToken("indexTitle"),
            InstanceTypeId = (Guid?)jObject.SelectToken("instanceTypeId"),
            IssuanceModeId = (Guid?)jObject.SelectToken("modeOfIssuanceId"),
            CatalogedDate = ((DateTime?)jObject.SelectToken("catalogedDate"))?.ToLocalTime(),
            PreviouslyHeld = (bool?)jObject.SelectToken("previouslyHeld"),
            StaffSuppress = (bool?)jObject.SelectToken("staffSuppress"),
            DiscoverySuppress = (bool?)jObject.SelectToken("discoverySuppress"),
            SourceRecordFormat = (string)jObject.SelectToken("sourceRecordFormat"),
            StatusId = (Guid?)jObject.SelectToken("statusId"),
            StatusLastWriteTime = ((DateTime?)jObject.SelectToken("statusUpdatedDate"))?.ToLocalTime(),
            CreationTime = ((DateTime?)jObject.SelectToken("metadata.createdDate"))?.ToLocalTime(),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = ((DateTime?)jObject.SelectToken("metadata.updatedDate"))?.ToLocalTime(),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = jObject.ToString(),
            AlternativeTitles = jObject.SelectToken("alternativeTitles")?.Where(jt => jt.HasValues).Select(jt => AlternativeTitle.FromJObject((JObject)jt)).ToArray(),
            Classifications = jObject.SelectToken("classifications")?.Where(jt => jt.HasValues).Select(jt => Classification.FromJObject((JObject)jt)).ToArray(),
            Contributors = jObject.SelectToken("contributors")?.Where(jt => jt.HasValues).Select(jt => Contributor.FromJObject((JObject)jt)).ToArray(),
            Editions = jObject.SelectToken("editions")?.Where(jt => jt.HasValues).Select(jt => Edition.FromJObject((JValue)jt)).ToArray(),
            ElectronicAccesses = jObject.SelectToken("electronicAccess")?.Where(jt => jt.HasValues).Select(jt => ElectronicAccess.FromJObject((JObject)jt)).ToArray(),
            Identifiers = jObject.SelectToken("identifiers")?.Where(jt => jt.HasValues).Select(jt => Identifier.FromJObject((JObject)jt)).ToArray(),
            InstanceFormat2s = jObject.SelectToken("instanceFormatIds")?.Where(jt => jt.HasValues).Select(jt => InstanceFormat2.FromJObject((JValue)jt)).ToArray(),
            InstanceNatureOfContentTerms = jObject.SelectToken("natureOfContentTermIds")?.Where(jt => jt.HasValues).Select(jt => InstanceNatureOfContentTerm.FromJObject((JValue)jt)).ToArray(),
            InstanceStatisticalCodes = jObject.SelectToken("statisticalCodeIds")?.Where(jt => jt.HasValues).Select(jt => InstanceStatisticalCode.FromJObject((JValue)jt)).ToArray(),
            InstanceTags = jObject.SelectToken("tags.tagList")?.Where(jt => jt.HasValues).Select(jt => InstanceTag.FromJObject((JValue)jt)).ToArray(),
            Languages = jObject.SelectToken("languages")?.Where(jt => jt.HasValues).Select(jt => Language.FromJObject((JValue)jt)).ToArray(),
            Note2s = jObject.SelectToken("notes")?.Where(jt => jt.HasValues).Select(jt => Note2.FromJObject((JObject)jt)).ToArray(),
            PhysicalDescriptions = jObject.SelectToken("physicalDescriptions")?.Where(jt => jt.HasValues).Select(jt => PhysicalDescription.FromJObject((JValue)jt)).ToArray(),
            PublicationFrequencies = jObject.SelectToken("publicationFrequency")?.Where(jt => jt.HasValues).Select(jt => PublicationFrequency.FromJObject((JValue)jt)).ToArray(),
            PublicationRanges = jObject.SelectToken("publicationRange")?.Where(jt => jt.HasValues).Select(jt => PublicationRange.FromJObject((JValue)jt)).ToArray(),
            Publications = jObject.SelectToken("publication")?.Where(jt => jt.HasValues).Select(jt => Publication.FromJObject((JObject)jt)).ToArray(),
            Series = jObject.SelectToken("series")?.Where(jt => jt.HasValues).Select(jt => FolioLibrary.Series.FromJObject((JValue)jt)).ToArray(),
            Subjects = jObject.SelectToken("subjects")?.Where(jt => jt.HasValues).Select(jt => Subject.FromJObject((JValue)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("hrid", ShortId?.ToString()),
            new JProperty("matchKey", MatchKey),
            new JProperty("source", Source),
            new JProperty("title", Title),
            new JProperty("contributors", Contributors?.Select(c => c.ToJObject())),
            new JProperty("publication", Publications?.Select(p => p.ToJObject())),
            new JProperty("indexTitle", IndexTitle),
            new JProperty("instanceTypeId", InstanceTypeId),
            new JProperty("modeOfIssuanceId", IssuanceModeId),
            new JProperty("catalogedDate", CatalogedDate?.ToUniversalTime()),
            new JProperty("previouslyHeld", PreviouslyHeld),
            new JProperty("staffSuppress", StaffSuppress),
            new JProperty("discoverySuppress", DiscoverySuppress),
            new JProperty("sourceRecordFormat", SourceRecordFormat),
            new JProperty("statusId", StatusId),
            new JProperty("statusUpdatedDate", StatusLastWriteTime?.ToUniversalTime()),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToUniversalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToUniversalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("alternativeTitles", AlternativeTitles?.Select(at => at.ToJObject())),
            new JProperty("classifications", Classifications?.Select(c => c.ToJObject())),
            new JProperty("editions", Editions?.Select(e2 => e2.ToJObject())),
            new JProperty("electronicAccess", ElectronicAccesses?.Select(ea => ea.ToJObject())),
            new JProperty("identifiers", Identifiers?.Select(i => i.ToJObject())),
            new JProperty("instanceFormatIds", InstanceFormat2s?.Select(if2 => if2.ToJObject())),
            new JProperty("natureOfContentTermIds", InstanceNatureOfContentTerms?.Select(inoct => inoct.ToJObject())),
            new JProperty("statisticalCodeIds", InstanceStatisticalCodes?.Select(isc => isc.ToJObject())),
            new JProperty("tags", new JObject(
                new JProperty("tagList", InstanceTags?.Select(it => it.ToJObject())))),
            new JProperty("languages", Languages?.Select(l => l.ToJObject())),
            new JProperty("notes", Note2s?.Select(n2 => n2.ToJObject())),
            new JProperty("physicalDescriptions", PhysicalDescriptions?.Select(pd => pd.ToJObject())),
            new JProperty("publicationFrequency", PublicationFrequencies?.Select(pf => pf.ToJObject())),
            new JProperty("publicationRange", PublicationRanges?.Select(pr => pr.ToJObject())),
            new JProperty("series", Series?.Select(s => s.ToJObject())),
            new JProperty("subjects", Subjects?.Select(s => s.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
