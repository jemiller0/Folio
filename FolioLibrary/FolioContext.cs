using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace FolioLibrary
{
    public partial class FolioContext : DbContext
    {
        private string connectionString;
        private string databaseName;
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
                    if (eventId.Id == 20100) traceSource.TraceEvent(levels[logLevel], 0, formatter(state, exception));
                }
            }
        }

        public FolioContext() : this("FolioContext") { }

        public FolioContext(string name) : base()
        {
            providerName = ConfigurationManager.ConnectionStrings[name].ProviderName;
            connectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
            databaseName = Regex.Match(connectionString, @"(?i);?Database=(?<Database>\w+)").Groups["Database"].Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (IsSqlServer)
                throw new NotSupportedException();
            else if (IsMySql)
                throw new NotSupportedException();
            else if (IsPostgreSql)
                optionsBuilder.UseNpgsql(connectionString);
            else throw new NotSupportedException();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            this.GetService<ILoggerFactory>().AddProvider(new LoggingProvider());
            if (IsMySql) modelBuilder.Entity<Account>().ToTable("diku_mod_feesfines_accounts", null);
            if (IsMySql) modelBuilder.Entity<Account2>().ToTable("diku_mod_vendors_account", null);
            if (IsMySql) modelBuilder.Entity<Address>().ToTable("diku_mod_vendors_address", null);
            if (IsMySql) modelBuilder.Entity<AddressType>().ToTable("diku_mod_users_addresstype", null);
            if (IsMySql) modelBuilder.Entity<Adjustment>().ToTable("diku_mod_orders_storage_adjustment", null);
            if (IsMySql) modelBuilder.Entity<Agreement>().ToTable("diku_mod_vendors_agreement", null);
            if (IsMySql) modelBuilder.Entity<Alert>().ToTable("diku_mod_orders_storage_alert", null);
            if (IsMySql) modelBuilder.Entity<Alias>().ToTable("diku_mod_vendors_alias", null);
            if (IsMySql) modelBuilder.Entity<AlternativeTitleType>().ToTable("diku_mod_inventory_storage_alternative_title_type", null);
            if (IsMySql) modelBuilder.Entity<AuditLoan>().ToTable("diku_mod_circulation_storage_audit_loan", null);
            if (IsMySql) modelBuilder.Entity<AuthAttempt>().ToTable("diku_mod_login_auth_attempts", null);
            if (IsMySql) modelBuilder.Entity<AuthCredentialsHistory>().ToTable("diku_mod_login_auth_credentials_history", null);
            if (IsMySql) modelBuilder.Entity<AuthPasswordAction>().ToTable("diku_mod_login_auth_password_action", null);
            if (IsMySql) modelBuilder.Entity<Block>().ToTable("diku_mod_feesfines_manualblocks", null);
            if (IsMySql) modelBuilder.Entity<Budget>().ToTable("diku_mod_finance_storage_budget", null);
            if (IsMySql) modelBuilder.Entity<CallNumberType>().ToTable("diku_mod_inventory_storage_call_number_type", null);
            if (IsMySql) modelBuilder.Entity<Campus>().ToTable("diku_mod_inventory_storage_loccampus", null);
            if (IsMySql) modelBuilder.Entity<CancellationReason>().ToTable("diku_mod_circulation_storage_cancellation_reason", null);
            if (IsMySql) modelBuilder.Entity<Category>().ToTable("diku_mod_vendors_category", null);
            if (IsMySql) modelBuilder.Entity<CirculationRule>().ToTable("diku_mod_circulation_storage_circulation_rules", null);
            if (IsMySql) modelBuilder.Entity<Claim>().ToTable("diku_mod_orders_storage_claim", null);
            if (IsMySql) modelBuilder.Entity<ClassificationType>().ToTable("diku_mod_inventory_storage_classification_type", null);
            if (IsMySql) modelBuilder.Entity<Comment>().ToTable("diku_mod_feesfines_comments", null);
            if (IsMySql) modelBuilder.Entity<Contact>().ToTable("diku_mod_vendors_contact", null);
            if (IsMySql) modelBuilder.Entity<ContactCategory>().ToTable("diku_mod_vendors_contact_category", null);
            if (IsMySql) modelBuilder.Entity<ContributorNameType>().ToTable("diku_mod_inventory_storage_contributor_name_type", null);
            if (IsMySql) modelBuilder.Entity<ContributorType>().ToTable("diku_mod_inventory_storage_contributor_type", null);
            if (IsMySql) modelBuilder.Entity<Cost>().ToTable("diku_mod_orders_storage_cost", null);
            if (IsMySql) modelBuilder.Entity<Detail>().ToTable("diku_mod_orders_storage_details", null);
            if (IsMySql) modelBuilder.Entity<ElectronicAccessRelationship>().ToTable("diku_mod_inventory_storage_electronic_access_relationship", null);
            if (IsMySql) modelBuilder.Entity<Email>().ToTable("diku_mod_vendors_email", null);
            if (IsMySql) modelBuilder.Entity<Eresource>().ToTable("diku_mod_orders_storage_eresource", null);
            if (IsMySql) modelBuilder.Entity<EventLog>().ToTable("diku_mod_login_event_logs", null);
            if (IsMySql) modelBuilder.Entity<Fee>().ToTable("diku_mod_feesfines_feefines", null);
            if (IsMySql) modelBuilder.Entity<FeeAction>().ToTable("diku_mod_feesfines_feefineactions", null);
            if (IsMySql) modelBuilder.Entity<FiscalYear>().ToTable("diku_mod_finance_storage_fiscal_year", null);
            if (IsMySql) modelBuilder.Entity<FixedDueDateSchedule>().ToTable("diku_mod_circulation_storage_fixed_due_date_schedule", null);
            if (IsMySql) modelBuilder.Entity<Fund>().ToTable("diku_mod_finance_storage_fund", null);
            if (IsMySql) modelBuilder.Entity<FundDistribution>().ToTable("diku_mod_finance_storage_fund_distribution", null);
            if (IsMySql) modelBuilder.Entity<FundDistribution2>().ToTable("diku_mod_orders_storage_fund_distribution", null);
            if (IsMySql) modelBuilder.Entity<Group>().ToTable("diku_mod_users_groups", null);
            if (IsMySql) modelBuilder.Entity<Holding>().ToTable("diku_mod_inventory_storage_holdings_record", null);
            if (IsMySql) modelBuilder.Entity<HoldingNoteType>().ToTable("diku_mod_inventory_storage_holdings_note_type", null);
            if (IsMySql) modelBuilder.Entity<HoldingType>().ToTable("diku_mod_inventory_storage_holdings_type", null);
            if (IsMySql) modelBuilder.Entity<IdType>().ToTable("diku_mod_inventory_storage_identifier_type", null);
            if (IsMySql) modelBuilder.Entity<IllPolicy>().ToTable("diku_mod_inventory_storage_ill_policy", null);
            if (IsMySql) modelBuilder.Entity<Instance>().ToTable("diku_mod_inventory_storage_instance", null);
            if (IsMySql) modelBuilder.Entity<InstanceFormat>().ToTable("diku_mod_inventory_storage_instance_format", null);
            if (IsMySql) modelBuilder.Entity<InstanceRelationship>().ToTable("diku_mod_inventory_storage_instance_relationship", null);
            if (IsMySql) modelBuilder.Entity<InstanceRelationshipType>().ToTable("diku_mod_inventory_storage_instance_relationship_type", null);
            if (IsMySql) modelBuilder.Entity<InstanceSourceMarc>().ToTable("diku_mod_inventory_storage_instance_source_marc", null);
            if (IsMySql) modelBuilder.Entity<InstanceStatus>().ToTable("diku_mod_inventory_storage_instance_status", null);
            if (IsMySql) modelBuilder.Entity<InstanceType>().ToTable("diku_mod_inventory_storage_instance_type", null);
            if (IsMySql) modelBuilder.Entity<Institution>().ToTable("diku_mod_inventory_storage_locinstitution", null);
            if (IsMySql) modelBuilder.Entity<Interface>().ToTable("diku_mod_vendors_interface", null);
            if (IsMySql) modelBuilder.Entity<Item>().ToTable("diku_mod_inventory_storage_item", null);
            if (IsMySql) modelBuilder.Entity<ItemNoteType>().ToTable("diku_mod_inventory_storage_item_note_type", null);
            if (IsMySql) modelBuilder.Entity<Ledger>().ToTable("diku_mod_finance_storage_ledger", null);
            if (IsMySql) modelBuilder.Entity<Library>().ToTable("diku_mod_inventory_storage_loclibrary", null);
            if (IsMySql) modelBuilder.Entity<Loan>().ToTable("diku_mod_circulation_storage_loan", null);
            if (IsMySql) modelBuilder.Entity<LoanPolicy>().ToTable("diku_mod_circulation_storage_loan_policy", null);
            if (IsMySql) modelBuilder.Entity<LoanType>().ToTable("diku_mod_inventory_storage_loan_type", null);
            if (IsMySql) modelBuilder.Entity<Location>().ToTable("diku_mod_inventory_storage_location", null);
            if (IsMySql) modelBuilder.Entity<Login>().ToTable("diku_mod_login_auth_credentials", null);
            if (IsMySql) modelBuilder.Entity<MaterialType>().ToTable("diku_mod_inventory_storage_material_type", null);
            if (IsMySql) modelBuilder.Entity<ModeOfIssuance>().ToTable("diku_mod_inventory_storage_mode_of_issuance", null);
            if (IsMySql) modelBuilder.Entity<Note>().ToTable("diku_mod_notes_note_data", null);
            if (IsMySql) modelBuilder.Entity<Order>().ToTable("diku_mod_orders_storage_purchase_order", null);
            if (IsMySql) modelBuilder.Entity<OrderItem>().ToTable("diku_mod_orders_storage_po_line", null);
            if (IsMySql) modelBuilder.Entity<OrderItemLocation>().ToTable("diku_mod_orders_storage_location", null);
            if (IsMySql) modelBuilder.Entity<Owner>().ToTable("diku_mod_feesfines_owners", null);
            if (IsMySql) modelBuilder.Entity<PatronNoticePolicy>().ToTable("diku_mod_circulation_storage_patron_notice_policy", null);
            if (IsMySql) modelBuilder.Entity<Payment>().ToTable("diku_mod_feesfines_payments", null);
            if (IsMySql) modelBuilder.Entity<Permission>().ToTable("diku_mod_permissions_permissions", null);
            if (IsMySql) modelBuilder.Entity<PermissionsUser>().ToTable("diku_mod_permissions_permissions_users", null);
            if (IsMySql) modelBuilder.Entity<PhoneNumber>().ToTable("diku_mod_vendors_phone_number", null);
            if (IsMySql) modelBuilder.Entity<Physical>().ToTable("diku_mod_orders_storage_physical", null);
            if (IsMySql) modelBuilder.Entity<Piece>().ToTable("diku_mod_orders_storage_pieces", null);
            if (IsMySql) modelBuilder.Entity<Proxy>().ToTable("diku_mod_users_proxyfor", null);
            if (IsMySql) modelBuilder.Entity<Refund>().ToTable("diku_mod_feesfines_refunds", null);
            if (IsMySql) modelBuilder.Entity<ReportingCode>().ToTable("diku_mod_orders_storage_reporting_code", null);
            if (IsMySql) modelBuilder.Entity<Request>().ToTable("diku_mod_circulation_storage_request", null);
            if (IsMySql) modelBuilder.Entity<RequestPolicy>().ToTable("diku_mod_circulation_storage_request_policy", null);
            if (IsMySql) modelBuilder.Entity<ServicePoint>().ToTable("diku_mod_inventory_storage_service_point", null);
            if (IsMySql) modelBuilder.Entity<ServicePointUser>().ToTable("diku_mod_inventory_storage_service_point_user", null);
            if (IsMySql) modelBuilder.Entity<Source>().ToTable("diku_mod_orders_storage_source", null);
            if (IsMySql) modelBuilder.Entity<StaffSlip>().ToTable("diku_mod_circulation_storage_staff_slips", null);
            if (IsMySql) modelBuilder.Entity<StatisticalCode>().ToTable("diku_mod_inventory_storage_statistical_code", null);
            if (IsMySql) modelBuilder.Entity<StatisticalCodeType>().ToTable("diku_mod_inventory_storage_statistical_code_type", null);
            if (IsMySql) modelBuilder.Entity<Tag>().ToTable("diku_mod_tags_tags", null);
            if (IsMySql) modelBuilder.Entity<Transaction>().ToTable("diku_mod_finance_storage_transaction", null);
            if (IsMySql) modelBuilder.Entity<Transfer>().ToTable("diku_mod_feesfines_transfers", null);
            if (IsMySql) modelBuilder.Entity<TransferCriteria>().ToTable("diku_mod_feesfines_transfer_criteria", null);
            if (IsMySql) modelBuilder.Entity<Url>().ToTable("diku_mod_vendors_url", null);
            if (IsMySql) modelBuilder.Entity<User>().ToTable("diku_mod_users_users", null);
            if (IsMySql) modelBuilder.Entity<Vendor>().ToTable("diku_mod_vendors_vendor", null);
            if (IsMySql) modelBuilder.Entity<VendorCategory>().ToTable("diku_mod_vendors_vendor_category", null);
            if (IsMySql) modelBuilder.Entity<VendorDetail>().ToTable("diku_mod_orders_storage_vendor_detail", null);
            if (IsMySql) modelBuilder.Entity<VendorType>().ToTable("diku_mod_vendors_vendor_type", null);
            if (IsMySql) modelBuilder.Entity<Waive>().ToTable("diku_mod_feesfines_waives", null);
            if (IsPostgreSql) modelBuilder.Entity<Account>().Property(nameof(Account.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Account2>().Property(nameof(Account2.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Address>().Property(nameof(Address.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<AddressType>().Property(nameof(AddressType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Adjustment>().Property(nameof(Adjustment.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Agreement>().Property(nameof(Agreement.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Alert>().Property(nameof(Alert.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Alias>().Property(nameof(Alias.Content)).HasColumnType("jsonb");
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
            if (IsPostgreSql) modelBuilder.Entity<Claim>().Property(nameof(Claim.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ClassificationType>().Property(nameof(ClassificationType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Comment>().Property(nameof(Comment.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Contact>().Property(nameof(Contact.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ContactCategory>().Property(nameof(ContactCategory.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ContributorNameType>().Property(nameof(ContributorNameType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ContributorType>().Property(nameof(ContributorType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Cost>().Property(nameof(Cost.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Detail>().Property(nameof(Detail.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ElectronicAccessRelationship>().Property(nameof(ElectronicAccessRelationship.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Email>().Property(nameof(Email.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Eresource>().Property(nameof(Eresource.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<EventLog>().Property(nameof(EventLog.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Fee>().Property(nameof(Fee.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<FeeAction>().Property(nameof(FeeAction.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<FiscalYear>().Property(nameof(FiscalYear.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<FixedDueDateSchedule>().Property(nameof(FixedDueDateSchedule.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Fund>().Property(nameof(Fund.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<FundDistribution>().Property(nameof(FundDistribution.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<FundDistribution2>().Property(nameof(FundDistribution2.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Group>().Property(nameof(Group.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Holding>().Property(nameof(Holding.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<HoldingNoteType>().Property(nameof(HoldingNoteType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<HoldingType>().Property(nameof(HoldingType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<IdType>().Property(nameof(IdType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<IllPolicy>().Property(nameof(IllPolicy.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Instance>().Property(nameof(Instance.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<InstanceFormat>().Property(nameof(InstanceFormat.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<InstanceRelationship>().Property(nameof(InstanceRelationship.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<InstanceRelationshipType>().Property(nameof(InstanceRelationshipType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<InstanceSourceMarc>().Property(nameof(InstanceSourceMarc.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<InstanceStatus>().Property(nameof(InstanceStatus.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<InstanceType>().Property(nameof(InstanceType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Institution>().Property(nameof(Institution.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Interface>().Property(nameof(Interface.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Item>().Property(nameof(Item.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ItemNoteType>().Property(nameof(ItemNoteType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Ledger>().Property(nameof(Ledger.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Library>().Property(nameof(Library.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Loan>().Property(nameof(Loan.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<LoanPolicy>().Property(nameof(LoanPolicy.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<LoanType>().Property(nameof(LoanType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Location>().Property(nameof(Location.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Login>().Property(nameof(Login.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<MaterialType>().Property(nameof(MaterialType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ModeOfIssuance>().Property(nameof(ModeOfIssuance.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Note>().Property(nameof(Note.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Order>().Property(nameof(Order.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<OrderItem>().Property(nameof(OrderItem.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<OrderItemLocation>().Property(nameof(OrderItemLocation.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Owner>().Property(nameof(Owner.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<PatronNoticePolicy>().Property(nameof(PatronNoticePolicy.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Payment>().Property(nameof(Payment.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Permission>().Property(nameof(Permission.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<PermissionsUser>().Property(nameof(PermissionsUser.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<PhoneNumber>().Property(nameof(PhoneNumber.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Physical>().Property(nameof(Physical.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Piece>().Property(nameof(Piece.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Proxy>().Property(nameof(Proxy.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Refund>().Property(nameof(Refund.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ReportingCode>().Property(nameof(ReportingCode.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Request>().Property(nameof(Request.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<RequestPolicy>().Property(nameof(RequestPolicy.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ServicePoint>().Property(nameof(ServicePoint.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<ServicePointUser>().Property(nameof(ServicePointUser.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Source>().Property(nameof(Source.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<StaffSlip>().Property(nameof(StaffSlip.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<StatisticalCode>().Property(nameof(StatisticalCode.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<StatisticalCodeType>().Property(nameof(StatisticalCodeType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Tag>().Property(nameof(Tag.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Transaction>().Property(nameof(Transaction.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Transfer>().Property(nameof(Transfer.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<TransferCriteria>().Property(nameof(TransferCriteria.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Url>().Property(nameof(Url.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<User>().Property(nameof(User.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Vendor>().Property(nameof(Vendor.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<VendorCategory>().Property(nameof(VendorCategory.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<VendorDetail>().Property(nameof(VendorDetail.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<VendorType>().Property(nameof(VendorType.Content)).HasColumnType("jsonb");
            if (IsPostgreSql) modelBuilder.Entity<Waive>().Property(nameof(Waive.Content)).HasColumnType("jsonb");
            modelBuilder.Entity<Budget>().HasOne(typeof(Fund), nameof(Budget.Fund)).WithMany(nameof(Fund.Budgets)).HasForeignKey(nameof(Budget.FundId)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Budget>().HasOne(typeof(FiscalYear), nameof(Budget.FiscalYear)).WithMany(nameof(FiscalYear.Budgets)).HasForeignKey(nameof(Budget.FiscalYearId)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Campus>().HasOne(typeof(Institution), nameof(Campus.Institution)).WithMany(nameof(Institution.Campuses)).HasForeignKey(nameof(Campus.Institutionid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Fee>().HasOne(typeof(Owner), nameof(Fee.Owner)).WithMany(nameof(Owner.Fees)).HasForeignKey(nameof(Fee.Ownerid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Fund>().HasOne(typeof(Ledger), nameof(Fund.Ledger)).WithMany(nameof(Ledger.Funds)).HasForeignKey(nameof(Fund.LedgerId)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<FundDistribution>().HasOne(typeof(Budget), nameof(FundDistribution.Budget)).WithMany(nameof(Budget.FundDistributions)).HasForeignKey(nameof(FundDistribution.BudgetId)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Holding>().HasOne(typeof(Instance), nameof(Holding.Instance)).WithMany(nameof(Instance.Holdings)).HasForeignKey(nameof(Holding.Instanceid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Holding>().HasOne(typeof(Location), nameof(Holding.Location)).WithMany(nameof(Location.Holdings)).HasForeignKey(nameof(Holding.Permanentlocationid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Holding>().HasOne(typeof(Location), nameof(Holding.Location1)).WithMany(nameof(Location.Holdings1)).HasForeignKey(nameof(Holding.Temporarylocationid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Holding>().HasOne(typeof(HoldingType), nameof(Holding.HoldingType)).WithMany(nameof(HoldingType.Holdings)).HasForeignKey(nameof(Holding.Holdingstypeid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Holding>().HasOne(typeof(CallNumberType), nameof(Holding.CallNumberType)).WithMany(nameof(CallNumberType.Holdings)).HasForeignKey(nameof(Holding.Callnumbertypeid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Holding>().HasOne(typeof(IllPolicy), nameof(Holding.IllPolicy)).WithMany(nameof(IllPolicy.Holdings)).HasForeignKey(nameof(Holding.Illpolicyid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Instance>().HasOne(typeof(InstanceStatus), nameof(Instance.InstanceStatus)).WithMany(nameof(InstanceStatus.Instances)).HasForeignKey(nameof(Instance.Instancestatusid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Instance>().HasOne(typeof(ModeOfIssuance), nameof(Instance.ModeOfIssuance)).WithMany(nameof(ModeOfIssuance.Instances)).HasForeignKey(nameof(Instance.Modeofissuanceid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InstanceRelationship>().HasOne(typeof(Instance), nameof(InstanceRelationship.Instance1)).WithMany(nameof(Instance.InstanceRelationships1)).HasForeignKey(nameof(InstanceRelationship.Superinstanceid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InstanceRelationship>().HasOne(typeof(Instance), nameof(InstanceRelationship.Instance)).WithMany(nameof(Instance.InstanceRelationships)).HasForeignKey(nameof(InstanceRelationship.Subinstanceid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InstanceRelationship>().HasOne(typeof(InstanceRelationshipType), nameof(InstanceRelationship.InstanceRelationshipType)).WithMany(nameof(InstanceRelationshipType.InstanceRelationships)).HasForeignKey(nameof(InstanceRelationship.Instancerelationshiptypeid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InstanceSourceMarc>().HasOne(typeof(Instance), nameof(InstanceSourceMarc.Instance)).WithOne(nameof(Instance.InstanceSourceMarc)).HasForeignKey(typeof(InstanceSourceMarc), nameof(InstanceSourceMarc.Id)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Item>().HasOne(typeof(Holding), nameof(Item.Holding)).WithMany(nameof(Holding.Items)).HasForeignKey(nameof(Item.Holdingsrecordid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Item>().HasOne(typeof(LoanType), nameof(Item.LoanType)).WithMany(nameof(LoanType.Items)).HasForeignKey(nameof(Item.Permanentloantypeid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Item>().HasOne(typeof(LoanType), nameof(Item.LoanType1)).WithMany(nameof(LoanType.Items1)).HasForeignKey(nameof(Item.Temporaryloantypeid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Item>().HasOne(typeof(MaterialType), nameof(Item.MaterialType)).WithMany(nameof(MaterialType.Items)).HasForeignKey(nameof(Item.Materialtypeid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Item>().HasOne(typeof(Location), nameof(Item.Location)).WithMany(nameof(Location.Items)).HasForeignKey(nameof(Item.Permanentlocationid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Item>().HasOne(typeof(Location), nameof(Item.Location1)).WithMany(nameof(Location.Items1)).HasForeignKey(nameof(Item.Temporarylocationid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Library>().HasOne(typeof(Campus), nameof(Library.Campus)).WithMany(nameof(Campus.Libraries)).HasForeignKey(nameof(Library.Campusid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<LoanPolicy>().HasOne(typeof(FixedDueDateSchedule), nameof(LoanPolicy.FixedDueDateSchedule)).WithMany(nameof(FixedDueDateSchedule.LoanPolicies)).HasForeignKey(nameof(LoanPolicy.LoanspolicyFixedduedatescheduleid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<LoanPolicy>().HasOne(typeof(FixedDueDateSchedule), nameof(LoanPolicy.FixedDueDateSchedule1)).WithMany(nameof(FixedDueDateSchedule.LoanPolicies1)).HasForeignKey(nameof(LoanPolicy.RenewalspolicyAlternatefixedduedatescheduleid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Location>().HasOne(typeof(Institution), nameof(Location.Institution)).WithMany(nameof(Institution.Locations)).HasForeignKey(nameof(Location.Institutionid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Location>().HasOne(typeof(Campus), nameof(Location.Campus)).WithMany(nameof(Campus.Locations)).HasForeignKey(nameof(Location.Campusid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Location>().HasOne(typeof(Library), nameof(Location.Library)).WithMany(nameof(Library.Locations)).HasForeignKey(nameof(Location.Libraryid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Request>().HasOne(typeof(CancellationReason), nameof(Request.CancellationReason)).WithMany(nameof(CancellationReason.Requests)).HasForeignKey(nameof(Request.Cancellationreasonid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ServicePointUser>().HasOne(typeof(ServicePoint), nameof(ServicePointUser.ServicePoint)).WithMany(nameof(ServicePoint.ServicePointUsers)).HasForeignKey(nameof(ServicePointUser.Defaultservicepointid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<StatisticalCode>().HasOne(typeof(StatisticalCodeType), nameof(StatisticalCode.StatisticalCodeType)).WithMany(nameof(StatisticalCodeType.StatisticalCodes)).HasForeignKey(nameof(StatisticalCode.Statisticalcodetypeid)).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Transaction>().HasOne(typeof(Budget), nameof(Transaction.Budget)).WithMany(nameof(Budget.Transactions)).HasForeignKey(nameof(Transaction.BudgetId)).OnDelete(DeleteBehavior.Restrict);
            Database.SetCommandTimeout(90);
        }

        public bool IsMySql => providerName == "MySql.Data.MySqlClient";
        public bool IsPostgreSql => providerName == "Npgsql";
        public bool IsSqlServer => providerName == "System.Data.SqlClient";
        public string ProviderName => providerName;
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Account2> Account2s { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<Adjustment> Adjustments { get; set; }
        public DbSet<Agreement> Agreements { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Alias> Aliases { get; set; }
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
        public DbSet<Claim> Claims { get; set; }
        public DbSet<ClassificationType> ClassificationTypes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactCategory> ContactCategories { get; set; }
        public DbSet<ContributorNameType> ContributorNameTypes { get; set; }
        public DbSet<ContributorType> ContributorTypes { get; set; }
        public DbSet<Cost> Costs { get; set; }
        public DbSet<Detail> Details { get; set; }
        public DbSet<ElectronicAccessRelationship> ElectronicAccessRelationships { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Eresource> Eresources { get; set; }
        public DbSet<EventLog> EventLogs { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<FeeAction> FeeActions { get; set; }
        public DbSet<FiscalYear> FiscalYears { get; set; }
        public DbSet<FixedDueDateSchedule> FixedDueDateSchedules { get; set; }
        public DbSet<Fund> Funds { get; set; }
        public DbSet<FundDistribution> FundDistributions { get; set; }
        public DbSet<FundDistribution2> FundDistribution2s { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Holding> Holdings { get; set; }
        public DbSet<HoldingNoteType> HoldingNoteTypes { get; set; }
        public DbSet<HoldingType> HoldingTypes { get; set; }
        public DbSet<IdType> IdTypes { get; set; }
        public DbSet<IllPolicy> IllPolicies { get; set; }
        public DbSet<Instance> Instances { get; set; }
        public DbSet<InstanceFormat> InstanceFormats { get; set; }
        public DbSet<InstanceRelationship> InstanceRelationships { get; set; }
        public DbSet<InstanceRelationshipType> InstanceRelationshipTypes { get; set; }
        public DbSet<InstanceSourceMarc> InstanceSourceMarcs { get; set; }
        public DbSet<InstanceStatus> InstanceStatuses { get; set; }
        public DbSet<InstanceType> InstanceTypes { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Interface> Interfaces { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemNoteType> ItemNoteTypes { get; set; }
        public DbSet<Ledger> Ledgers { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanPolicy> LoanPolicies { get; set; }
        public DbSet<LoanType> LoanTypes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<MaterialType> MaterialTypes { get; set; }
        public DbSet<ModeOfIssuance> ModeOfIssuances { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderItemLocation> OrderItemLocations { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<PatronNoticePolicy> PatronNoticePolicies { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionsUser> PermissionsUsers { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<Physical> Physicals { get; set; }
        public DbSet<Piece> Pieces { get; set; }
        public DbSet<Proxy> Proxies { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<ReportingCode> ReportingCodes { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestPolicy> RequestPolicies { get; set; }
        public DbSet<ServicePoint> ServicePoints { get; set; }
        public DbSet<ServicePointUser> ServicePointUsers { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<StaffSlip> StaffSlips { get; set; }
        public DbSet<StatisticalCode> StatisticalCodes { get; set; }
        public DbSet<StatisticalCodeType> StatisticalCodeTypes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<TransferCriteria> TransferCriterias { get; set; }
        public DbSet<Url> Urls { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<VendorCategory> VendorCategories { get; set; }
        public DbSet<VendorDetail> VendorDetails { get; set; }
        public DbSet<VendorType> VendorTypes { get; set; }
        public DbSet<Waive> Waives { get; set; }
    }
}
