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
    // uc.instances -> uchicago_mod_inventory_storage.instance
    // Instance2 -> Instance
    [CustomValidation(typeof(Instance2), nameof(ValidateInstance2)), DisplayColumn(nameof(Title)), DisplayName("Instances"), JsonConverter(typeof(JsonPathJsonConverter<Instance2>)), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("instances", Schema = "uc")]
    public partial class Instance2
    {
        public static ValidationResult ValidateInstance2(Instance2 instance2, ValidationContext context)
        {
            var fsc = (FolioServiceContext)context.ObjectInstance;
            if (instance2.ShortId != null && fsc.AnyInstance2s($"id <> \"{instance2.Id}\" and hrid == \"{instance2.ShortId}\"")) return new ValidationResult("Short Id already exists");
            return ValidationResult.Success;
        }

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

        [Column("_version"), JsonProperty("_version"), ScaffoldColumn(false)]
        public virtual int? Version { get; set; }

        [Column("hrid"), Display(Name = "Short Id", Order = 3), Editable(false), JsonConverter(typeof(StringJsonConverter<int?>)), JsonProperty("hrid")]
        public virtual int? ShortId { get; set; }

        [Column("match_key"), Display(Name = "Match Key", Order = 4), JsonProperty("matchKey"), StringLength(1024)]
        public virtual string MatchKey { get; set; }

        [Column("source"), Display(Order = 5), Editable(false), JsonProperty("source"), RegularExpression(@"^(FOLIO|MARC|EPKB)$"), StringLength(1024)]
        public virtual string Source { get; set; }

        [Column("title"), Display(Order = 6), JsonProperty("title"), Required, StringLength(1024)]
        public virtual string Title { get; set; }

        [Column("author"), Display(Order = 7), Editable(false), JsonProperty("contributors[0].name"), StringLength(1024)]
        public virtual string Author { get; set; }

        [Column("index_title"), JsonProperty("indexTitle"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string IndexTitle { get; set; }

        [Column("publication_period_start"), Display(Name = "Publication Start Year", Order = 9), JsonProperty("publicationPeriod.start")]
        public virtual int? PublicationStartYear { get; set; }

        [Column("publication_period_end"), Display(Name = "Publication End Year", Order = 10), JsonProperty("publicationPeriod.end")]
        public virtual int? PublicationEndYear { get; set; }

        [Column("dates_date_type_id"), Display(Name = "Dates Date Type Id", Order = 11), JsonProperty("dates.dateTypeId")]
        public virtual Guid? DatesDateTypeId { get; set; }

        [Column("dates_date1"), Display(Name = "Dates Date 1", Order = 12), JsonProperty("dates.date1"), StringLength(4)]
        public virtual string DatesDate1 { get; set; }

        [Column("dates_date2"), Display(Name = "Dates Date 2", Order = 13), JsonProperty("dates.date2"), StringLength(4)]
        public virtual string DatesDate2 { get; set; }

        [Display(Name = "Instance Type", Order = 14)]
        public virtual InstanceType2 InstanceType { get; set; }

        [Column("instance_type_id"), Display(Name = "Instance Type", Order = 15), JsonProperty("instanceTypeId"), Required]
        public virtual Guid? InstanceTypeId { get; set; }

        [Display(Name = "Issuance Mode", Order = 16)]
        public virtual IssuanceMode IssuanceMode { get; set; }

        [Column("mode_of_issuance_id"), Display(Name = "Issuance Mode", Order = 17), JsonProperty("modeOfIssuanceId")]
        public virtual Guid? IssuanceModeId { get; set; }

        [Column("cataloged_date"), DataType(DataType.Date), Display(Name = "Cataloged Date", Order = 18), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("catalogedDate")]
        public virtual DateTime? CatalogedDate { get; set; }

        [Column("previously_held"), Display(Name = "Previously Held", Order = 19), JsonProperty("previouslyHeld")]
        public virtual bool? PreviouslyHeld { get; set; }

        [Column("staff_suppress"), Display(Name = "Staff Suppress", Order = 20), JsonProperty("staffSuppress")]
        public virtual bool? StaffSuppress { get; set; }

        [Column("discovery_suppress"), Display(Name = "Discovery Suppress", Order = 21), JsonProperty("discoverySuppress")]
        public virtual bool? DiscoverySuppress { get; set; }

        [Column("source_record_format"), Display(Name = "Source Record Format", Order = 22), Editable(false), JsonProperty("sourceRecordFormat"), RegularExpression(@"^(MARC-JSON)$"), StringLength(1024)]
        public virtual string SourceRecordFormat { get; set; }

        [Display(Order = 23)]
        public virtual Status Status { get; set; }

        [Column("status_id"), Display(Name = "Status", Order = 24), JsonProperty("statusId")]
        public virtual Guid? StatusId { get; set; }

        [Column("status_updated_date"), DataType(DataType.DateTime), Display(Name = "Status Last Write Time", Order = 25), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("statusUpdatedDate")]
        public virtual DateTime? StatusLastWriteTime { get; set; }

        [Column("created_date"), DataType(DataType.DateTime), Display(Name = "Creation Time", Order = 26), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.createdDate")]
        public virtual DateTime? CreationTime { get; set; }

        [Display(Name = "Creation User", Order = 27), InverseProperty("Instance2s")]
        public virtual User2 CreationUser { get; set; }

        [Column("created_by_user_id"), Display(Name = "Creation User", Order = 28), Editable(false), JsonProperty("metadata.createdByUserId")]
        public virtual Guid? CreationUserId { get; set; }

        [Column("created_by_username"), JsonProperty("metadata.createdByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string CreationUserUsername { get; set; }

        [Column("updated_date"), DataType(DataType.DateTime), Display(Name = "Last Write Time", Order = 30), DisplayFormat(DataFormatString = "{0:g}"), Editable(false), JsonProperty("metadata.updatedDate")]
        public virtual DateTime? LastWriteTime { get; set; }

        [Display(Name = "Last Write User", Order = 31), InverseProperty("Instance2s1")]
        public virtual User2 LastWriteUser { get; set; }

        [Column("updated_by_user_id"), Display(Name = "Last Write User", Order = 32), Editable(false), JsonProperty("metadata.updatedByUserId")]
        public virtual Guid? LastWriteUserId { get; set; }

        [Column("updated_by_username"), JsonProperty("metadata.updatedByUsername"), ScaffoldColumn(false), StringLength(1024)]
        public virtual string LastWriteUserUsername { get; set; }

        [Column("content"), CustomValidation(typeof(Instance), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 34), Editable(false)]
        public virtual string Content { get; set; }

        [Column("completion_time"), DataType(DataType.DateTime), Display(Name = "Completion Time", Order = 35), DisplayFormat(DataFormatString = "{0:g}", ApplyFormatInEditMode = true)]
        public virtual DateTime? CompletionTime { get; set; }

        [Display(Name = "Administrative Notes", Order = 36), JsonConverter(typeof(ArrayJsonConverter<List<AdministrativeNote>, AdministrativeNote>), "Content"), JsonProperty("administrativeNotes")]
        public virtual ICollection<AdministrativeNote> AdministrativeNotes { get; set; }

        [Display(Name = "Alternative Titles", Order = 37), JsonProperty("alternativeTitles")]
        public virtual ICollection<AlternativeTitle> AlternativeTitles { get; set; }

        [Display(Order = 38), JsonProperty("classifications")]
        public virtual ICollection<Classification> Classifications { get; set; }

        [Display(Order = 39), JsonProperty("contributors")]
        public virtual ICollection<Contributor> Contributors { get; set; }

        [Display(Order = 40), JsonConverter(typeof(ArrayJsonConverter<List<Edition>, Edition>), "Content"), JsonProperty("editions")]
        public virtual ICollection<Edition> Editions { get; set; }

        [Display(Name = "Electronic Accesses", Order = 41), JsonProperty("electronicAccess")]
        public virtual ICollection<ElectronicAccess> ElectronicAccesses { get; set; }

        [Display(Name = "Fees", Order = 42)]
        public virtual ICollection<Fee2> Fee2s { get; set; }

        [Display(Name = "Holdings", Order = 43)]
        public virtual ICollection<Holding2> Holding2s { get; set; }

        [Display(Order = 44), JsonProperty("identifiers")]
        public virtual ICollection<Identifier> Identifiers { get; set; }

        [Display(Name = "Instance Formats", Order = 45), JsonConverter(typeof(ArrayJsonConverter<List<InstanceFormat2>, InstanceFormat2>), "FormatId"), JsonProperty("instanceFormatIds")]
        public virtual ICollection<InstanceFormat2> InstanceFormat2s { get; set; }

        [Display(Name = "Instance Nature Of Content Terms", Order = 46), JsonConverter(typeof(ArrayJsonConverter<List<InstanceNatureOfContentTerm>, InstanceNatureOfContentTerm>), "NatureOfContentTermId"), JsonProperty("natureOfContentTermIds")]
        public virtual ICollection<InstanceNatureOfContentTerm> InstanceNatureOfContentTerms { get; set; }

        [Display(Name = "Instance Notes", Order = 47), JsonProperty("notes")]
        public virtual ICollection<InstanceNote> InstanceNotes { get; set; }

        [Display(Name = "Instance Statistical Codes", Order = 48), JsonConverter(typeof(ArrayJsonConverter<List<InstanceStatisticalCode>, InstanceStatisticalCode>), "StatisticalCodeId"), JsonProperty("statisticalCodeIds")]
        public virtual ICollection<InstanceStatisticalCode> InstanceStatisticalCodes { get; set; }

        [Display(Name = "Instance Tags", Order = 49), JsonConverter(typeof(ArrayJsonConverter<List<InstanceTag>, InstanceTag>), "Content"), JsonProperty("tags.tagList")]
        public virtual ICollection<InstanceTag> InstanceTags { get; set; }

        [Display(Name = "ISBNs", Order = 50)]
        public virtual ICollection<Isbn> Isbns { get; set; }

        [Display(Name = "ISSNs", Order = 51)]
        public virtual ICollection<Issn> Issns { get; set; }

        [Display(Order = 52), JsonConverter(typeof(ArrayJsonConverter<List<Language>, Language>), "Content"), JsonProperty("languages")]
        public virtual ICollection<Language> Languages { get; set; }

        [Display(Name = "OCLC Numbers", Order = 53)]
        public virtual ICollection<OclcNumber> OclcNumbers { get; set; }

        [Display(Name = "Order Items", Order = 54)]
        public virtual ICollection<OrderItem2> OrderItem2s { get; set; }

        [Display(Name = "Physical Descriptions", Order = 55), JsonConverter(typeof(ArrayJsonConverter<List<PhysicalDescription>, PhysicalDescription>), "Content"), JsonProperty("physicalDescriptions")]
        public virtual ICollection<PhysicalDescription> PhysicalDescriptions { get; set; }

        [Display(Name = "Preceding Succeeding Titles", Order = 56)]
        public virtual ICollection<PrecedingSucceedingTitle2> PrecedingSucceedingTitle2s { get; set; }

        [Display(Name = "Preceding Succeeding Titles 1", Order = 57)]
        public virtual ICollection<PrecedingSucceedingTitle2> PrecedingSucceedingTitle2s1 { get; set; }

        [Display(Name = "Publication Frequencies", Order = 58), JsonConverter(typeof(ArrayJsonConverter<List<PublicationFrequency>, PublicationFrequency>), "Content"), JsonProperty("publicationFrequency")]
        public virtual ICollection<PublicationFrequency> PublicationFrequencies { get; set; }

        [Display(Name = "Publication Ranges", Order = 59), JsonConverter(typeof(ArrayJsonConverter<List<PublicationRange>, PublicationRange>), "Content"), JsonProperty("publicationRange")]
        public virtual ICollection<PublicationRange> PublicationRanges { get; set; }

        [Display(Order = 60), JsonProperty("publication")]
        public virtual ICollection<Publication> Publications { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Record2> Record2s { get; set; }

        [Display(Order = 62)]
        public virtual ICollection<Relationship> Relationships { get; set; }

        [Display(Name = "Relationships 1", Order = 63)]
        public virtual ICollection<Relationship> Relationships1 { get; set; }

        [Display(Name = "Requests", Order = 64)]
        public virtual ICollection<Request2> Request2s { get; set; }

        [Display(Order = 65), JsonProperty("series")]
        public virtual ICollection<Series> Series { get; set; }

        [Display(Order = 66), JsonProperty("subjects")]
        public virtual ICollection<Subject> Subjects { get; set; }

        [Display(Name = "Titles", Order = 67)]
        public virtual ICollection<Title2> Title2s { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(Version)} = {Version}, {nameof(ShortId)} = {ShortId}, {nameof(MatchKey)} = {MatchKey}, {nameof(Source)} = {Source}, {nameof(Title)} = {Title}, {nameof(Author)} = {Author}, {nameof(IndexTitle)} = {IndexTitle}, {nameof(PublicationStartYear)} = {PublicationStartYear}, {nameof(PublicationEndYear)} = {PublicationEndYear}, {nameof(DatesDateTypeId)} = {DatesDateTypeId}, {nameof(DatesDate1)} = {DatesDate1}, {nameof(DatesDate2)} = {DatesDate2}, {nameof(InstanceTypeId)} = {InstanceTypeId}, {nameof(IssuanceModeId)} = {IssuanceModeId}, {nameof(CatalogedDate)} = {CatalogedDate}, {nameof(PreviouslyHeld)} = {PreviouslyHeld}, {nameof(StaffSuppress)} = {StaffSuppress}, {nameof(DiscoverySuppress)} = {DiscoverySuppress}, {nameof(SourceRecordFormat)} = {SourceRecordFormat}, {nameof(StatusId)} = {StatusId}, {nameof(StatusLastWriteTime)} = {StatusLastWriteTime}, {nameof(CreationTime)} = {CreationTime}, {nameof(CreationUserId)} = {CreationUserId}, {nameof(CreationUserUsername)} = {CreationUserUsername}, {nameof(LastWriteTime)} = {LastWriteTime}, {nameof(LastWriteUserId)} = {LastWriteUserId}, {nameof(LastWriteUserUsername)} = {LastWriteUserUsername}, {nameof(Content)} = {Content}, {nameof(CompletionTime)} = {CompletionTime}, {nameof(AdministrativeNotes)} = {(AdministrativeNotes != null ? $"{{ {string.Join(", ", AdministrativeNotes)} }}" : "")}, {nameof(AlternativeTitles)} = {(AlternativeTitles != null ? $"{{ {string.Join(", ", AlternativeTitles)} }}" : "")}, {nameof(Classifications)} = {(Classifications != null ? $"{{ {string.Join(", ", Classifications)} }}" : "")}, {nameof(Contributors)} = {(Contributors != null ? $"{{ {string.Join(", ", Contributors)} }}" : "")}, {nameof(Editions)} = {(Editions != null ? $"{{ {string.Join(", ", Editions)} }}" : "")}, {nameof(ElectronicAccesses)} = {(ElectronicAccesses != null ? $"{{ {string.Join(", ", ElectronicAccesses)} }}" : "")}, {nameof(Identifiers)} = {(Identifiers != null ? $"{{ {string.Join(", ", Identifiers)} }}" : "")}, {nameof(InstanceFormat2s)} = {(InstanceFormat2s != null ? $"{{ {string.Join(", ", InstanceFormat2s)} }}" : "")}, {nameof(InstanceNatureOfContentTerms)} = {(InstanceNatureOfContentTerms != null ? $"{{ {string.Join(", ", InstanceNatureOfContentTerms)} }}" : "")}, {nameof(InstanceNotes)} = {(InstanceNotes != null ? $"{{ {string.Join(", ", InstanceNotes)} }}" : "")}, {nameof(InstanceStatisticalCodes)} = {(InstanceStatisticalCodes != null ? $"{{ {string.Join(", ", InstanceStatisticalCodes)} }}" : "")}, {nameof(InstanceTags)} = {(InstanceTags != null ? $"{{ {string.Join(", ", InstanceTags)} }}" : "")}, {nameof(Languages)} = {(Languages != null ? $"{{ {string.Join(", ", Languages)} }}" : "")}, {nameof(PhysicalDescriptions)} = {(PhysicalDescriptions != null ? $"{{ {string.Join(", ", PhysicalDescriptions)} }}" : "")}, {nameof(PublicationFrequencies)} = {(PublicationFrequencies != null ? $"{{ {string.Join(", ", PublicationFrequencies)} }}" : "")}, {nameof(PublicationRanges)} = {(PublicationRanges != null ? $"{{ {string.Join(", ", PublicationRanges)} }}" : "")}, {nameof(Publications)} = {(Publications != null ? $"{{ {string.Join(", ", Publications)} }}" : "")}, {nameof(Series)} = {(Series != null ? $"{{ {string.Join(", ", Series)} }}" : "")}, {nameof(Subjects)} = {(Subjects != null ? $"{{ {string.Join(", ", Subjects)} }}" : "")} }}";

        public static Instance2 FromJObject(JObject jObject) => jObject != null ? new Instance2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            Version = (int?)jObject.SelectToken("_version"),
            ShortId = (int?)jObject.SelectToken("hrid"),
            MatchKey = (string)jObject.SelectToken("matchKey"),
            Source = (string)jObject.SelectToken("source"),
            Title = (string)jObject.SelectToken("title"),
            Author = (string)jObject.SelectToken("contributors[0].name"),
            IndexTitle = (string)jObject.SelectToken("indexTitle"),
            PublicationStartYear = (int?)jObject.SelectToken("publicationPeriod.start"),
            PublicationEndYear = (int?)jObject.SelectToken("publicationPeriod.end"),
            DatesDateTypeId = (Guid?)jObject.SelectToken("dates.dateTypeId"),
            DatesDate1 = (string)jObject.SelectToken("dates.date1"),
            DatesDate2 = (string)jObject.SelectToken("dates.date2"),
            InstanceTypeId = (Guid?)jObject.SelectToken("instanceTypeId"),
            IssuanceModeId = (Guid?)jObject.SelectToken("modeOfIssuanceId"),
            CatalogedDate = ((DateTime?)jObject.SelectToken("catalogedDate"))?.ToUniversalTime(),
            PreviouslyHeld = (bool?)jObject.SelectToken("previouslyHeld"),
            StaffSuppress = (bool?)jObject.SelectToken("staffSuppress"),
            DiscoverySuppress = (bool?)jObject.SelectToken("discoverySuppress"),
            SourceRecordFormat = (string)jObject.SelectToken("sourceRecordFormat"),
            StatusId = (Guid?)jObject.SelectToken("statusId"),
            StatusLastWriteTime = (DateTime?)jObject.SelectToken("statusUpdatedDate"),
            CreationTime = (DateTime?)jObject.SelectToken("metadata.createdDate"),
            CreationUserId = (Guid?)jObject.SelectToken("metadata.createdByUserId"),
            CreationUserUsername = (string)jObject.SelectToken("metadata.createdByUsername"),
            LastWriteTime = (DateTime?)jObject.SelectToken("metadata.updatedDate"),
            LastWriteUserId = (Guid?)jObject.SelectToken("metadata.updatedByUserId"),
            LastWriteUserUsername = (string)jObject.SelectToken("metadata.updatedByUsername"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings),
            AdministrativeNotes = jObject.SelectToken("administrativeNotes")?.Select(jt => AdministrativeNote.FromJObject((JValue)jt)).ToArray(),
            AlternativeTitles = jObject.SelectToken("alternativeTitles")?.Where(jt => jt.HasValues).Select(jt => AlternativeTitle.FromJObject((JObject)jt)).ToArray(),
            Classifications = jObject.SelectToken("classifications")?.Where(jt => jt.HasValues).Select(jt => Classification.FromJObject((JObject)jt)).ToArray(),
            Contributors = jObject.SelectToken("contributors")?.Where(jt => jt.HasValues).Select(jt => Contributor.FromJObject((JObject)jt)).ToArray(),
            Editions = jObject.SelectToken("editions")?.Select(jt => Edition.FromJObject((JValue)jt)).ToArray(),
            ElectronicAccesses = jObject.SelectToken("electronicAccess")?.Where(jt => jt.HasValues).Select(jt => ElectronicAccess.FromJObject((JObject)jt)).ToArray(),
            Identifiers = jObject.SelectToken("identifiers")?.Where(jt => jt.HasValues).Select(jt => Identifier.FromJObject((JObject)jt)).ToArray(),
            InstanceFormat2s = jObject.SelectToken("instanceFormatIds")?.Select(jt => InstanceFormat2.FromJObject((JValue)jt)).ToArray(),
            InstanceNatureOfContentTerms = jObject.SelectToken("natureOfContentTermIds")?.Select(jt => InstanceNatureOfContentTerm.FromJObject((JValue)jt)).ToArray(),
            InstanceNotes = jObject.SelectToken("notes")?.Where(jt => jt.HasValues).Select(jt => InstanceNote.FromJObject((JObject)jt)).ToArray(),
            InstanceStatisticalCodes = jObject.SelectToken("statisticalCodeIds")?.Select(jt => InstanceStatisticalCode.FromJObject((JValue)jt)).ToArray(),
            InstanceTags = jObject.SelectToken("tags.tagList")?.Select(jt => InstanceTag.FromJObject((JValue)jt)).ToArray(),
            Languages = jObject.SelectToken("languages")?.Select(jt => Language.FromJObject((JValue)jt)).ToArray(),
            PhysicalDescriptions = jObject.SelectToken("physicalDescriptions")?.Select(jt => PhysicalDescription.FromJObject((JValue)jt)).ToArray(),
            PublicationFrequencies = jObject.SelectToken("publicationFrequency")?.Select(jt => PublicationFrequency.FromJObject((JValue)jt)).ToArray(),
            PublicationRanges = jObject.SelectToken("publicationRange")?.Select(jt => PublicationRange.FromJObject((JValue)jt)).ToArray(),
            Publications = jObject.SelectToken("publication")?.Where(jt => jt.HasValues).Select(jt => Publication.FromJObject((JObject)jt)).ToArray(),
            Series = jObject.SelectToken("series")?.Where(jt => jt.HasValues).Select(jt => FolioLibrary.Series.FromJObject((JObject)jt)).ToArray(),
            Subjects = jObject.SelectToken("subjects")?.Where(jt => jt.HasValues).Select(jt => Subject.FromJObject((JObject)jt)).ToArray()
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("_version", Version),
            new JProperty("hrid", ShortId?.ToString()),
            new JProperty("matchKey", MatchKey),
            new JProperty("source", Source),
            new JProperty("title", Title),
            new JProperty("indexTitle", IndexTitle),
            new JProperty("publicationPeriod", new JObject(
                new JProperty("start", PublicationStartYear),
                new JProperty("end", PublicationEndYear))),
            new JProperty("dates", new JObject(
                new JProperty("dateTypeId", DatesDateTypeId),
                new JProperty("date1", DatesDate1),
                new JProperty("date2", DatesDate2))),
            new JProperty("instanceTypeId", InstanceTypeId),
            new JProperty("modeOfIssuanceId", IssuanceModeId),
            new JProperty("catalogedDate", CatalogedDate?.ToLocalTime()),
            new JProperty("previouslyHeld", PreviouslyHeld),
            new JProperty("staffSuppress", StaffSuppress),
            new JProperty("discoverySuppress", DiscoverySuppress),
            new JProperty("sourceRecordFormat", SourceRecordFormat),
            new JProperty("statusId", StatusId),
            new JProperty("statusUpdatedDate", StatusLastWriteTime?.ToLocalTime()),
            new JProperty("metadata", new JObject(
                new JProperty("createdDate", CreationTime?.ToLocalTime()),
                new JProperty("createdByUserId", CreationUserId),
                new JProperty("createdByUsername", CreationUserUsername),
                new JProperty("updatedDate", LastWriteTime?.ToLocalTime()),
                new JProperty("updatedByUserId", LastWriteUserId),
                new JProperty("updatedByUsername", LastWriteUserUsername))),
            new JProperty("administrativeNotes", AdministrativeNotes?.Select(an => an.ToJObject())),
            new JProperty("alternativeTitles", AlternativeTitles?.Select(at => at.ToJObject())),
            new JProperty("classifications", Classifications?.Select(c => c.ToJObject())),
            new JProperty("contributors", Contributors?.Select(c => c.ToJObject())),
            new JProperty("editions", Editions?.Select(e2 => e2.ToJObject())),
            new JProperty("electronicAccess", ElectronicAccesses?.Select(ea => ea.ToJObject())),
            new JProperty("identifiers", Identifiers?.Select(i => i.ToJObject())),
            new JProperty("instanceFormatIds", InstanceFormat2s?.Select(if2 => if2.ToJObject())),
            new JProperty("natureOfContentTermIds", InstanceNatureOfContentTerms?.Select(inoct => inoct.ToJObject())),
            new JProperty("notes", InstanceNotes?.Select(@in => @in.ToJObject())),
            new JProperty("statisticalCodeIds", InstanceStatisticalCodes?.Select(isc => isc.ToJObject())),
            new JProperty("tags", new JObject(
                new JProperty("tagList", InstanceTags?.Select(it => it.ToJObject())))),
            new JProperty("languages", Languages?.Select(l => l.ToJObject())),
            new JProperty("physicalDescriptions", PhysicalDescriptions?.Select(pd => pd.ToJObject())),
            new JProperty("publicationFrequency", PublicationFrequencies?.Select(pf => pf.ToJObject())),
            new JProperty("publicationRange", PublicationRanges?.Select(pr => pr.ToJObject())),
            new JProperty("publication", Publications?.Select(p => p.ToJObject())),
            new JProperty("series", Series?.Select(s => s.ToJObject())),
            new JProperty("subjects", Subjects?.Select(s => s.ToJObject()))).RemoveNullAndEmptyProperties();
    }
}
