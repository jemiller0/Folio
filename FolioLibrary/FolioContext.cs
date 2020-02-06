using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace FolioLibrary
{
    public partial class FolioContext : DbContext
    {
        private string connectionString;
        private string databaseName;
        public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(configure => configure.SetMinimumLevel(LogLevel.Trace).AddProvider(new LoggingProvider()));
        private string providerName;
        public readonly static TraceSource traceSource = new TraceSource("FolioLibrary", SourceLevels.Information);

        public class LoggingProvider : ILoggerProvider
        {
            public ILogger CreateLogger(string categoryName) => new Logger();

            public void Dispose() { }

            private class Logger : ILogger
            {
                private static Dictionary<LogLevel, TraceEventType> levels = new Dictionary<LogLevel, TraceEventType> { { LogLevel.Trace, TraceEventType.Verbose }, { LogLevel.Debug, TraceEventType.Verbose }, { LogLevel.Information, TraceEventType.Verbose }, { LogLevel.Warning, TraceEventType.Warning }, { LogLevel.Error, TraceEventType.Error }, { LogLevel.Critical, TraceEventType.Critical } };

                public IDisposable BeginScope<TState>(TState state) => null;

                public bool IsEnabled(LogLevel logLevel) => true;

                public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
                {
                    if (eventId.Id == RelationalEventId.CommandExecuted) traceSource.TraceEvent(levels[logLevel], 0, formatter(state, exception));
                }
            }
        }

        public class MySqlCommandInterceptor : DbCommandInterceptor
        {
            public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
            {
                command.CommandText = Regex.Replace(command.CommandText, @"CONVERT\(('.*?') USING utf8mb4\) COLLATE utf8mb4_bin", "$1");
                return base.ReaderExecuting(command, eventData, result);
            }
        }

        public FolioContext() : this("FolioContext") { }

        public FolioContext(string name) : base()
        {
            providerName = ConfigurationManager.ConnectionStrings[name].ProviderName;
            connectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
            databaseName = Regex.Match(connectionString, @"(?i);?Database=(?<Database>\w+)").Groups["Database"].Value;
            Database.SetCommandTimeout(30);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory);
            if (IsSqlServer)
                throw new NotSupportedException();
            else if (IsMySql)
            {
                throw new NotSupportedException();
                optionsBuilder.AddInterceptors(new MySqlCommandInterceptor());
            }
            else if (IsPostgreSql)
                optionsBuilder.UseNpgsql(connectionString);
            else throw new NotSupportedException();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            if (IsPostgreSql) modelBuilder.Entity<Account>().Property(nameof(Account.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<AcquisitionsUnit>().Property(nameof(AcquisitionsUnit.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<AddressType>().Property(nameof(AddressType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Alert>().Property(nameof(Alert.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<AlternativeTitleType>().Property(nameof(AlternativeTitleType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<AuditLoan>().Property(nameof(AuditLoan.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<AuthAttempt>().Property(nameof(AuthAttempt.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<AuthCredentialsHistory>().Property(nameof(AuthCredentialsHistory.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<AuthPasswordAction>().Property(nameof(AuthPasswordAction.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Block>().Property(nameof(Block.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Budget>().Property(nameof(Budget.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<CallNumberType>().Property(nameof(CallNumberType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Campus>().Property(nameof(Campus.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<CancellationReason>().Property(nameof(CancellationReason.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Category>().Property(nameof(Category.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<CirculationRule>().Property(nameof(CirculationRule.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ClassificationType>().Property(nameof(ClassificationType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Comment>().Property(nameof(Comment.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Configuration>().Property(nameof(Configuration.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Contact>().Property(nameof(Contact.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ContributorNameType>().Property(nameof(ContributorNameType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ContributorType>().Property(nameof(ContributorType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Document>().Property(nameof(Document.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ElectronicAccessRelationship>().Property(nameof(ElectronicAccessRelationship.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ErrorRecord>().Property(nameof(ErrorRecord.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<EventLog>().Property(nameof(EventLog.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Fee>().Property(nameof(Fee.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<FeeAction>().Property(nameof(FeeAction.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<FinanceGroup>().Property(nameof(FinanceGroup.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<FiscalYear>().Property(nameof(FiscalYear.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<FixedDueDateSchedule>().Property(nameof(FixedDueDateSchedule.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Fund>().Property(nameof(Fund.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<FundDistribution>().Property(nameof(FundDistribution.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<FundType>().Property(nameof(FundType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Group>().Property(nameof(Group.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<GroupFundFiscalYear>().Property(nameof(GroupFundFiscalYear.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Holding>().Property(nameof(Holding.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<HoldingNoteType>().Property(nameof(HoldingNoteType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<HoldingType>().Property(nameof(HoldingType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<HridSetting>().Property(nameof(HridSetting.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<IdType>().Property(nameof(IdType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<IllPolicy>().Property(nameof(IllPolicy.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Instance>().Property(nameof(Instance.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<InstanceFormat>().Property(nameof(InstanceFormat.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<InstanceNoteType>().Property(nameof(InstanceNoteType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<InstanceRelationship>().Property(nameof(InstanceRelationship.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<InstanceRelationshipType>().Property(nameof(InstanceRelationshipType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<InstanceSourceMarc>().Property(nameof(InstanceSourceMarc.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<InstanceStatus>().Property(nameof(InstanceStatus.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<InstanceType>().Property(nameof(InstanceType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Institution>().Property(nameof(Institution.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Interface>().Property(nameof(Interface.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<InterfaceCredential>().Property(nameof(InterfaceCredential.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Invoice>().Property(nameof(Invoice.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<InvoiceItem>().Property(nameof(InvoiceItem.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Item>().Property(nameof(Item.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ItemDamagedStatus>().Property(nameof(ItemDamagedStatus.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ItemNoteType>().Property(nameof(ItemNoteType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<JobExecution>().Property(nameof(JobExecution.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<JobExecutionSourceChunk>().Property(nameof(JobExecutionSourceChunk.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Ledger>().Property(nameof(Ledger.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<LedgerFiscalYear>().Property(nameof(LedgerFiscalYear.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Library>().Property(nameof(Library.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Loan>().Property(nameof(Loan.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<LoanPolicy>().Property(nameof(LoanPolicy.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<LoanType>().Property(nameof(LoanType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Location>().Property(nameof(Location.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Login>().Property(nameof(Login.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<LostItemFeePolicy>().Property(nameof(LostItemFeePolicy.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<MappingRule>().Property(nameof(MappingRule.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<MarcRecord>().Property(nameof(MarcRecord.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<MaterialType>().Property(nameof(MaterialType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ModeOfIssuance>().Property(nameof(ModeOfIssuance.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<NatureOfContentTerm>().Property(nameof(NatureOfContentTerm.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Note>().Property(nameof(Note.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<NoteType>().Property(nameof(NoteType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Order>().Property(nameof(Order.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<OrderInvoice>().Property(nameof(OrderInvoice.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<OrderItem>().Property(nameof(OrderItem.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<OrderTemplate>().Property(nameof(OrderTemplate.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<OrderTransactionSummary>().Property(nameof(OrderTransactionSummary.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Organization>().Property(nameof(Organization.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<OverdueFinePolicy>().Property(nameof(OverdueFinePolicy.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Owner>().Property(nameof(Owner.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<PatronActionSession>().Property(nameof(PatronActionSession.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<PatronNoticePolicy>().Property(nameof(PatronNoticePolicy.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Payment>().Property(nameof(Payment.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Permission>().Property(nameof(Permission.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<PermissionsUser>().Property(nameof(PermissionsUser.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Piece>().Property(nameof(Piece.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Proxy>().Property(nameof(Proxy.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<RawRecord>().Property(nameof(RawRecord.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Record>().Property(nameof(Record.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Refund>().Property(nameof(Refund.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ReportingCode>().Property(nameof(ReportingCode.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Request>().Property(nameof(Request.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<RequestPolicy>().Property(nameof(RequestPolicy.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ScheduledNotice>().Property(nameof(ScheduledNotice.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ServicePoint>().Property(nameof(ServicePoint.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ServicePointUser>().Property(nameof(ServicePointUser.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Snapshot>().Property(nameof(Snapshot.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<StaffSlip>().Property(nameof(StaffSlip.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<StatisticalCode>().Property(nameof(StatisticalCode.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<StatisticalCodeType>().Property(nameof(StatisticalCodeType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Tag>().Property(nameof(Tag.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Template>().Property(nameof(Template.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Transaction>().Property(nameof(Transaction.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Transfer>().Property(nameof(Transfer.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<TransferCriteria>().Property(nameof(TransferCriteria.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<User>().Property(nameof(User.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<UserAcquisitionsUnit>().Property(nameof(UserAcquisitionsUnit.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<UserRequestPreference>().Property(nameof(UserRequestPreference.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Voucher>().Property(nameof(Voucher.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<VoucherItem>().Property(nameof(VoucherItem.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Waive>().Property(nameof(Waive.Content)).HasColumnType("jsonb");
            modelBuilder.Entity<Budget>().HasOne(typeof(Fund), nameof(Budget.Fund)).WithMany(nameof(Fund.Budgets)).HasForeignKey(nameof(Budget.FundId)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Budget>().HasOne(typeof(FiscalYear), nameof(Budget.FiscalYear)).WithMany(nameof(FiscalYear.Budgets)).HasForeignKey(nameof(Budget.FiscalYearId)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Campus>().HasOne(typeof(Institution), nameof(Campus.Institution)).WithMany(nameof(Institution.Campuses)).HasForeignKey(nameof(Campus.Institutionid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Document>().HasOne(typeof(Invoice), nameof(Document.Invoice)).WithMany(nameof(Invoice.Documents)).HasForeignKey(nameof(Document.Invoiceid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Fee>().HasOne(typeof(Owner), nameof(Fee.Owner)).WithMany(nameof(Owner.Fees)).HasForeignKey(nameof(Fee.Ownerid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Fund>().HasOne(typeof(Ledger), nameof(Fund.Ledger)).WithMany(nameof(Ledger.Funds)).HasForeignKey(nameof(Fund.LedgerId)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Fund>().HasOne(typeof(FundType), nameof(Fund.FundType)).WithMany(nameof(FundType.Funds)).HasForeignKey(nameof(Fund.Fundtypeid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<FundDistribution>().HasOne(typeof(Budget), nameof(FundDistribution.Budget)).WithMany(nameof(Budget.FundDistributions)).HasForeignKey(nameof(FundDistribution.Budgetid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<GroupFundFiscalYear>().HasOne(typeof(Budget), nameof(GroupFundFiscalYear.Budget)).WithMany(nameof(Budget.GroupFundFiscalYears)).HasForeignKey(nameof(GroupFundFiscalYear.Budgetid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<GroupFundFiscalYear>().HasOne(typeof(FinanceGroup), nameof(GroupFundFiscalYear.FinanceGroup)).WithMany(nameof(FinanceGroup.GroupFundFiscalYears)).HasForeignKey(nameof(GroupFundFiscalYear.Groupid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<GroupFundFiscalYear>().HasOne(typeof(Fund), nameof(GroupFundFiscalYear.Fund)).WithMany(nameof(Fund.GroupFundFiscalYears)).HasForeignKey(nameof(GroupFundFiscalYear.Fundid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<GroupFundFiscalYear>().HasOne(typeof(FiscalYear), nameof(GroupFundFiscalYear.FiscalYear)).WithMany(nameof(FiscalYear.GroupFundFiscalYears)).HasForeignKey(nameof(GroupFundFiscalYear.Fiscalyearid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Holding>().HasOne(typeof(Instance), nameof(Holding.Instance)).WithMany(nameof(Instance.Holdings)).HasForeignKey(nameof(Holding.Instanceid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Holding>().HasOne(typeof(Location), nameof(Holding.Location)).WithMany(nameof(Location.Holdings)).HasForeignKey(nameof(Holding.Permanentlocationid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Holding>().HasOne(typeof(Location), nameof(Holding.Location1)).WithMany(nameof(Location.Holdings1)).HasForeignKey(nameof(Holding.Temporarylocationid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Holding>().HasOne(typeof(HoldingType), nameof(Holding.HoldingType)).WithMany(nameof(HoldingType.Holdings)).HasForeignKey(nameof(Holding.Holdingstypeid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Holding>().HasOne(typeof(CallNumberType), nameof(Holding.CallNumberType)).WithMany(nameof(CallNumberType.Holdings)).HasForeignKey(nameof(Holding.Callnumbertypeid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Holding>().HasOne(typeof(IllPolicy), nameof(Holding.IllPolicy)).WithMany(nameof(IllPolicy.Holdings)).HasForeignKey(nameof(Holding.Illpolicyid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Instance>().HasOne(typeof(InstanceStatus), nameof(Instance.InstanceStatus)).WithMany(nameof(InstanceStatus.Instances)).HasForeignKey(nameof(Instance.Instancestatusid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Instance>().HasOne(typeof(ModeOfIssuance), nameof(Instance.ModeOfIssuance)).WithMany(nameof(ModeOfIssuance.Instances)).HasForeignKey(nameof(Instance.Modeofissuanceid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Instance>().HasOne(typeof(InstanceType), nameof(Instance.InstanceType)).WithMany(nameof(InstanceType.Instances)).HasForeignKey(nameof(Instance.Instancetypeid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InstanceRelationship>().HasOne(typeof(Instance), nameof(InstanceRelationship.Instance1)).WithMany(nameof(Instance.InstanceRelationships1)).HasForeignKey(nameof(InstanceRelationship.Superinstanceid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InstanceRelationship>().HasOne(typeof(Instance), nameof(InstanceRelationship.Instance)).WithMany(nameof(Instance.InstanceRelationships)).HasForeignKey(nameof(InstanceRelationship.Subinstanceid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InstanceRelationship>().HasOne(typeof(InstanceRelationshipType), nameof(InstanceRelationship.InstanceRelationshipType)).WithMany(nameof(InstanceRelationshipType.InstanceRelationships)).HasForeignKey(nameof(InstanceRelationship.Instancerelationshiptypeid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InstanceSourceMarc>().HasOne(typeof(Instance), nameof(InstanceSourceMarc.Instance)).WithOne(nameof(Instance.InstanceSourceMarc)).HasForeignKey(typeof(InstanceSourceMarc), nameof(InstanceSourceMarc.Id)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InterfaceCredential>().HasOne(typeof(Interface), nameof(InterfaceCredential.Interface)).WithMany(nameof(Interface.InterfaceCredentials)).HasForeignKey(nameof(InterfaceCredential.Interfaceid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InvoiceItem>().HasOne(typeof(Invoice), nameof(InvoiceItem.Invoice)).WithMany(nameof(Invoice.InvoiceItems)).HasForeignKey(nameof(InvoiceItem.Invoiceid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Item>().HasOne(typeof(Holding), nameof(Item.Holding)).WithMany(nameof(Holding.Items)).HasForeignKey(nameof(Item.Holdingsrecordid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Item>().HasOne(typeof(LoanType), nameof(Item.LoanType)).WithMany(nameof(LoanType.Items)).HasForeignKey(nameof(Item.Permanentloantypeid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Item>().HasOne(typeof(LoanType), nameof(Item.LoanType1)).WithMany(nameof(LoanType.Items1)).HasForeignKey(nameof(Item.Temporaryloantypeid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Item>().HasOne(typeof(MaterialType), nameof(Item.MaterialType)).WithMany(nameof(MaterialType.Items)).HasForeignKey(nameof(Item.Materialtypeid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Item>().HasOne(typeof(Location), nameof(Item.Location1)).WithMany(nameof(Location.Items1)).HasForeignKey(nameof(Item.Permanentlocationid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Item>().HasOne(typeof(Location), nameof(Item.Location2)).WithMany(nameof(Location.Items2)).HasForeignKey(nameof(Item.Temporarylocationid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Item>().HasOne(typeof(Location), nameof(Item.Location)).WithMany(nameof(Location.Items)).HasForeignKey(nameof(Item.Effectivelocationid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<JobExecutionSourceChunk>().HasOne(typeof(JobExecution), nameof(JobExecutionSourceChunk.JobExecution)).WithMany(nameof(JobExecution.JobExecutionSourceChunks)).HasForeignKey(nameof(JobExecutionSourceChunk.Jobexecutionid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<JournalRecord>().HasOne(typeof(JobExecution), nameof(JournalRecord.JobExecution)).WithMany(nameof(JobExecution.JournalRecords)).HasForeignKey(nameof(JournalRecord.JobExecutionId)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Ledger>().HasOne(typeof(FiscalYear), nameof(Ledger.FiscalYear)).WithMany(nameof(FiscalYear.Ledgers)).HasForeignKey(nameof(Ledger.Fiscalyearoneid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<LedgerFiscalYear>().HasOne(typeof(Ledger), nameof(LedgerFiscalYear.Ledger)).WithMany(nameof(Ledger.LedgerFiscalYears)).HasForeignKey(nameof(LedgerFiscalYear.Ledgerid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<LedgerFiscalYear>().HasOne(typeof(FiscalYear), nameof(LedgerFiscalYear.FiscalYear)).WithMany(nameof(FiscalYear.LedgerFiscalYears)).HasForeignKey(nameof(LedgerFiscalYear.Fiscalyearid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Library>().HasOne(typeof(Campus), nameof(Library.Campus)).WithMany(nameof(Campus.Libraries)).HasForeignKey(nameof(Library.Campusid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<LoanPolicy>().HasOne(typeof(FixedDueDateSchedule), nameof(LoanPolicy.FixedDueDateSchedule)).WithMany(nameof(FixedDueDateSchedule.LoanPolicies)).HasForeignKey(nameof(LoanPolicy.LoanspolicyFixedduedatescheduleid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<LoanPolicy>().HasOne(typeof(FixedDueDateSchedule), nameof(LoanPolicy.FixedDueDateSchedule1)).WithMany(nameof(FixedDueDateSchedule.LoanPolicies1)).HasForeignKey(nameof(LoanPolicy.RenewalspolicyAlternatefixedduedatescheduleid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Location>().HasOne(typeof(Institution), nameof(Location.Institution)).WithMany(nameof(Institution.Locations)).HasForeignKey(nameof(Location.Institutionid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Location>().HasOne(typeof(Campus), nameof(Location.Campus)).WithMany(nameof(Campus.Locations)).HasForeignKey(nameof(Location.Campusid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Location>().HasOne(typeof(Library), nameof(Location.Library)).WithMany(nameof(Library.Locations)).HasForeignKey(nameof(Location.Libraryid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Note>().HasOne(typeof(NoteType), nameof(Note.TemporaryType)).WithMany(nameof(NoteType.Notes)).HasForeignKey(nameof(Note.TemporaryTypeId)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrderInvoice>().HasOne(typeof(Order), nameof(OrderInvoice.Order)).WithMany(nameof(Order.OrderInvoices)).HasForeignKey(nameof(OrderInvoice.Purchaseorderid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrderItem>().HasOne(typeof(Order), nameof(OrderItem.Order)).WithMany(nameof(Order.OrderItems)).HasForeignKey(nameof(OrderItem.Purchaseorderid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Piece>().HasOne(typeof(OrderItem), nameof(Piece.OrderItem)).WithMany(nameof(OrderItem.Pieces)).HasForeignKey(nameof(Piece.Polineid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Record>().HasOne(typeof(Snapshot), nameof(Record.Snapshot)).WithMany(nameof(Snapshot.Records)).HasForeignKey(nameof(Record.Jobexecutionid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Request>().HasOne(typeof(CancellationReason), nameof(Request.CancellationReason)).WithMany(nameof(CancellationReason.Requests)).HasForeignKey(nameof(Request.Cancellationreasonid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ServicePointUser>().HasOne(typeof(ServicePoint), nameof(ServicePointUser.ServicePoint)).WithMany(nameof(ServicePoint.ServicePointUsers)).HasForeignKey(nameof(ServicePointUser.Defaultservicepointid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<StatisticalCode>().HasOne(typeof(StatisticalCodeType), nameof(StatisticalCode.StatisticalCodeType)).WithMany(nameof(StatisticalCodeType.StatisticalCodes)).HasForeignKey(nameof(StatisticalCode.Statisticalcodetypeid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Transaction>().HasOne(typeof(FiscalYear), nameof(Transaction.FiscalYear)).WithMany(nameof(FiscalYear.Transactions)).HasForeignKey(nameof(Transaction.Fiscalyearid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Transaction>().HasOne(typeof(Fund), nameof(Transaction.Fund)).WithMany(nameof(Fund.Transactions)).HasForeignKey(nameof(Transaction.Fromfundid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Transaction>().HasOne(typeof(FiscalYear), nameof(Transaction.FiscalYear1)).WithMany(nameof(FiscalYear.Transactions1)).HasForeignKey(nameof(Transaction.Sourcefiscalyearid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Transaction>().HasOne(typeof(Fund), nameof(Transaction.Fund1)).WithMany(nameof(Fund.Transactions1)).HasForeignKey(nameof(Transaction.Tofundid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<User>().HasOne(typeof(Group), nameof(User.Group)).WithMany(nameof(Group.Users)).HasForeignKey(nameof(User.Patrongroup)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<UserAcquisitionsUnit>().HasOne(typeof(AcquisitionsUnit), nameof(UserAcquisitionsUnit.AcquisitionsUnit)).WithMany(nameof(AcquisitionsUnit.UserAcquisitionsUnits)).HasForeignKey(nameof(UserAcquisitionsUnit.Acquisitionsunitid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Voucher>().HasOne(typeof(Invoice), nameof(Voucher.Invoice)).WithMany(nameof(Invoice.Vouchers)).HasForeignKey(nameof(Voucher.Invoiceid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<VoucherItem>().HasOne(typeof(Voucher), nameof(VoucherItem.Voucher)).WithMany(nameof(Voucher.VoucherItems)).HasForeignKey(nameof(VoucherItem.Voucherid)).OnDelete(DeleteBehavior.Restrict);
        }

        public bool IsMySql => providerName == "MySql.Data.MySqlClient";
        public bool IsPostgreSql => providerName == "Npgsql";
        public bool IsSqlServer => providerName == "System.Data.SqlClient";
        public string ProviderName => providerName;
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AcquisitionsUnit> AcquisitionsUnits { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<AlternativeTitleType> AlternativeTitleTypes { get; set; }
        public DbSet<AuditLoan> AuditLoans { get; set; }
        public DbSet<AuthAttempt> AuthAttempts { get; set; }
        public DbSet<AuthCredentialsHistory> AuthCredentialsHistories { get; set; }
        public DbSet<AuthPasswordAction> AuthPasswordActions { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<CallNumberType> CallNumberTypes { get; set; }
        public DbSet<Campus> Campuses { get; set; }
        public DbSet<CancellationReason> CancellationReasons { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CirculationRule> CirculationRules { get; set; }
        public DbSet<ClassificationType> ClassificationTypes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContributorNameType> ContributorNameTypes { get; set; }
        public DbSet<ContributorType> ContributorTypes { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<ElectronicAccessRelationship> ElectronicAccessRelationships { get; set; }
        public DbSet<ErrorRecord> ErrorRecords { get; set; }
        public DbSet<EventLog> EventLogs { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<FeeAction> FeeActions { get; set; }
        public DbSet<FinanceGroup> FinanceGroups { get; set; }
        public DbSet<FiscalYear> FiscalYears { get; set; }
        public DbSet<FixedDueDateSchedule> FixedDueDateSchedules { get; set; }
        public DbSet<Fund> Funds { get; set; }
        public DbSet<FundDistribution> FundDistributions { get; set; }
        public DbSet<FundType> FundTypes { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupFundFiscalYear> GroupFundFiscalYears { get; set; }
        public DbSet<Holding> Holdings { get; set; }
        public DbSet<HoldingNoteType> HoldingNoteTypes { get; set; }
        public DbSet<HoldingType> HoldingTypes { get; set; }
        public DbSet<HridSetting> HridSettings { get; set; }
        public DbSet<IdType> IdTypes { get; set; }
        public DbSet<IllPolicy> IllPolicies { get; set; }
        public DbSet<Instance> Instances { get; set; }
        public DbSet<InstanceFormat> InstanceFormats { get; set; }
        public DbSet<InstanceNoteType> InstanceNoteTypes { get; set; }
        public DbSet<InstanceRelationship> InstanceRelationships { get; set; }
        public DbSet<InstanceRelationshipType> InstanceRelationshipTypes { get; set; }
        public DbSet<InstanceSourceMarc> InstanceSourceMarcs { get; set; }
        public DbSet<InstanceStatus> InstanceStatuses { get; set; }
        public DbSet<InstanceType> InstanceTypes { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Interface> Interfaces { get; set; }
        public DbSet<InterfaceCredential> InterfaceCredentials { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemDamagedStatus> ItemDamagedStatuses { get; set; }
        public DbSet<ItemNoteType> ItemNoteTypes { get; set; }
        public DbSet<JobExecution> JobExecutions { get; set; }
        public DbSet<JobExecutionSourceChunk> JobExecutionSourceChunks { get; set; }
        public DbSet<JournalRecord> JournalRecords { get; set; }
        public DbSet<Ledger> Ledgers { get; set; }
        public DbSet<LedgerFiscalYear> LedgerFiscalYears { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanPolicy> LoanPolicies { get; set; }
        public DbSet<LoanType> LoanTypes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<LostItemFeePolicy> LostItemFeePolicies { get; set; }
        public DbSet<MappingRule> MappingRules { get; set; }
        public DbSet<MarcRecord> MarcRecords { get; set; }
        public DbSet<MaterialType> MaterialTypes { get; set; }
        public DbSet<ModeOfIssuance> ModeOfIssuances { get; set; }
        public DbSet<NatureOfContentTerm> NatureOfContentTerms { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<NoteType> NoteTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderInvoice> OrderInvoices { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderTemplate> OrderTemplates { get; set; }
        public DbSet<OrderTransactionSummary> OrderTransactionSummaries { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OverdueFinePolicy> OverdueFinePolicies { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<PatronActionSession> PatronActionSessions { get; set; }
        public DbSet<PatronNoticePolicy> PatronNoticePolicies { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionsUser> PermissionsUsers { get; set; }
        public DbSet<Piece> Pieces { get; set; }
        public DbSet<Proxy> Proxies { get; set; }
        public DbSet<RawRecord> RawRecords { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<ReportingCode> ReportingCodes { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestPolicy> RequestPolicies { get; set; }
        public DbSet<ScheduledNotice> ScheduledNotices { get; set; }
        public DbSet<ServicePoint> ServicePoints { get; set; }
        public DbSet<ServicePointUser> ServicePointUsers { get; set; }
        public DbSet<Snapshot> Snapshots { get; set; }
        public DbSet<StaffSlip> StaffSlips { get; set; }
        public DbSet<StatisticalCode> StatisticalCodes { get; set; }
        public DbSet<StatisticalCodeType> StatisticalCodeTypes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<TransferCriteria> TransferCriterias { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAcquisitionsUnit> UserAcquisitionsUnits { get; set; }
        public DbSet<UserRequestPreference> UserRequestPreferences { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherItem> VoucherItems { get; set; }
        public DbSet<Waive> Waives { get; set; }
    }
}
