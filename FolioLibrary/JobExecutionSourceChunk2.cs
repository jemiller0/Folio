using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FolioLibrary
{
    // uc.job_execution_source_chunks -> uchicago_mod_source_record_manager.job_execution_source_chunks
    // JobExecutionSourceChunk2 -> JobExecutionSourceChunk
    [DisplayColumn(nameof(Id)), DisplayName("Job Execution Source Chunks"), JsonObject(MemberSerialization = MemberSerialization.OptIn), Table("job_execution_source_chunks", Schema = "uc")]
    public partial class JobExecutionSourceChunk2
    {
        public static ValidationResult ValidateContent(string value)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("FolioLibrary.JobExecutionSourceChunk.json")))
            {
                var js = JsonSchema.FromJsonAsync(sr.ReadToEndAsync().Result).Result;
                var l = js.Validate(value);
                if (l.Any()) return new ValidationResult($"The Content field is invalid. {string.Join(" ", l.Select(ve => ve.ToString()))}", new[] { "Content" });
            }
            return ValidationResult.Success;
        }

        [Column("id"), Display(Order = 1), Editable(false), JsonProperty("id")]
        public virtual Guid? Id { get; set; }

        [Display(Name = "Job Execution", Order = 2)]
        public virtual JobExecution2 JobExecution { get; set; }

        [Column("job_execution_id"), Display(Name = "Job Execution", Order = 3), JsonProperty("jobExecutionId"), Required]
        public virtual Guid? JobExecutionId { get; set; }

        [Column("last"), Display(Order = 4), JsonProperty("last")]
        public virtual bool? Last { get; set; }

        [Column("state"), Display(Order = 5), JsonProperty("state"), RegularExpression(@"^(IN_PROGRESS|COMPLETED|ERROR)$"), Required, StringLength(1024)]
        public virtual string State { get; set; }

        [Column("chunk_size"), Display(Name = "Chunk Size", Order = 6), JsonProperty("chunkSize")]
        public virtual int? ChunkSize { get; set; }

        [Column("processed_amount"), Display(Name = "Processed Amount", Order = 7), JsonProperty("processedAmount")]
        public virtual int? ProcessedAmount { get; set; }

        [Column("completed_date"), DataType(DataType.Date), Display(Name = "Completed Date", Order = 8), DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true), JsonProperty("completedDate")]
        public virtual DateTime? CompletedDate { get; set; }

        [Column("error"), Display(Order = 9), JsonProperty("error"), StringLength(1024)]
        public virtual string Error { get; set; }

        [Column("content"), CustomValidation(typeof(JobExecutionSourceChunk), nameof(ValidateContent)), DataType(DataType.MultilineText), Display(Order = 10), Editable(false)]
        public virtual string Content { get; set; }

        public override string ToString() => $"{{ {nameof(Id)} = {Id}, {nameof(JobExecutionId)} = {JobExecutionId}, {nameof(Last)} = {Last}, {nameof(State)} = {State}, {nameof(ChunkSize)} = {ChunkSize}, {nameof(ProcessedAmount)} = {ProcessedAmount}, {nameof(CompletedDate)} = {CompletedDate}, {nameof(Error)} = {Error}, {nameof(Content)} = {Content} }}";

        public static JobExecutionSourceChunk2 FromJObject(JObject jObject) => jObject != null ? new JobExecutionSourceChunk2
        {
            Id = (Guid?)jObject.SelectToken("id"),
            JobExecutionId = (Guid?)jObject.SelectToken("jobExecutionId"),
            Last = (bool?)jObject.SelectToken("last"),
            State = (string)jObject.SelectToken("state"),
            ChunkSize = (int?)jObject.SelectToken("chunkSize"),
            ProcessedAmount = (int?)jObject.SelectToken("processedAmount"),
            CompletedDate = (DateTime?)jObject.SelectToken("completedDate"),
            Error = (string)jObject.SelectToken("error"),
            Content = JsonConvert.SerializeObject(jObject, FolioDapperContext.UniversalTimeJsonSerializationSettings)
        } : null;

        public JObject ToJObject() => new JObject(
            new JProperty("id", Id),
            new JProperty("jobExecutionId", JobExecutionId),
            new JProperty("last", Last),
            new JProperty("state", State),
            new JProperty("chunkSize", ChunkSize),
            new JProperty("processedAmount", ProcessedAmount),
            new JProperty("completedDate", CompletedDate?.ToLocalTime()),
            new JProperty("error", Error)).RemoveNullAndEmptyProperties();
    }
}
