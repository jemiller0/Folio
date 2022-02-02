using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Group2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Group2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Group2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var g2 = folioServiceContext.FindGroup2(id, true);
            if (g2 == null) Response.Redirect("Default.aspx");
            g2.Content = g2.Content != null ? JsonConvert.DeserializeObject<JToken>(g2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Group2FormView.DataSource = new[] { g2 };
            Title = $"Group {g2.Name}";
        }

        protected void BlockLimit2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["BlockLimit2sPermission"] == null) return;
            var id = (Guid?)Group2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "GroupId", "patronGroupId" }, { "ConditionId", "conditionId" }, { "Value", "value" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"patronGroupId == \"{id}\"",
                Global.GetCqlFilter(BlockLimit2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(BlockLimit2sRadGrid, "Condition.Name", "conditionId", "name", folioServiceContext.FolioServiceClient.BlockConditions),
                Global.GetCqlFilter(BlockLimit2sRadGrid, "Value", "value"),
                Global.GetCqlFilter(BlockLimit2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(BlockLimit2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(BlockLimit2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(BlockLimit2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            BlockLimit2sRadGrid.DataSource = folioServiceContext.BlockLimit2s(out var i, where, BlockLimit2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BlockLimit2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BlockLimit2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BlockLimit2sRadGrid.PageSize * BlockLimit2sRadGrid.CurrentPageIndex, BlockLimit2sRadGrid.PageSize, true);
            BlockLimit2sRadGrid.VirtualItemCount = i;
            if (BlockLimit2sRadGrid.MasterTableView.FilterExpression == "")
            {
                BlockLimit2sRadGrid.AllowFilteringByColumn = BlockLimit2sRadGrid.VirtualItemCount > 10;
                BlockLimit2sPanel.Visible = Group2FormView.DataKey.Value != null && Session["BlockLimit2sPermission"] != null && BlockLimit2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void Loan2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Loan2sPermission"] == null) return;
            var id = (Guid?)Group2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "ProxyUserId", "proxyUserId" }, { "ItemId", "itemId" }, { "ItemEffectiveLocationAtCheckOutId", "itemEffectiveLocationIdAtCheckOut" }, { "StatusName", "status.name" }, { "LoanTime", "loanDate" }, { "DueTime", "dueDate" }, { "ReturnTime", "returnDate" }, { "SystemReturnTime", "systemReturnDate" }, { "Action", "action" }, { "ActionComment", "actionComment" }, { "ItemStatus", "itemStatus" }, { "RenewalCount", "renewalCount" }, { "LoanPolicyId", "loanPolicyId" }, { "CheckoutServicePointId", "checkoutServicePointId" }, { "CheckinServicePointId", "checkinServicePointId" }, { "GroupId", "patronGroupIdAtCheckout" }, { "DueDateChangedByRecall", "dueDateChangedByRecall" }, { "DeclaredLostDate", "declaredLostDate" }, { "ClaimedReturnedDate", "claimedReturnedDate" }, { "OverdueFinePolicyId", "overdueFinePolicyId" }, { "LostItemPolicyId", "lostItemPolicyId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AgedToLostDelayedBillingLostItemHasBeenBilled", "agedToLostDelayedBilling.lostItemHasBeenBilled" }, { "AgedToLostDelayedBillingDateLostItemShouldBeBilled", "agedToLostDelayedBilling.dateLostItemShouldBeBilled" }, { "AgedToLostDelayedBillingAgedToLostDate", "agedToLostDelayedBilling.agedToLostDate" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"patronGroupIdAtCheckout == \"{id}\"",
                Global.GetCqlFilter(Loan2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(Loan2sRadGrid, "User.Username", "userId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Loan2sRadGrid, "ProxyUser.Username", "proxyUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Loan2sRadGrid, "Item.ShortId", "itemId", "hrid", folioServiceContext.FolioServiceClient.Items),
                Global.GetCqlFilter(Loan2sRadGrid, "ItemEffectiveLocationAtCheckOut.Name", "itemEffectiveLocationIdAtCheckOut", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(Loan2sRadGrid, "StatusName", "status.name"),
                Global.GetCqlFilter(Loan2sRadGrid, "LoanTime", "loanDate"),
                Global.GetCqlFilter(Loan2sRadGrid, "DueTime", "dueDate"),
                Global.GetCqlFilter(Loan2sRadGrid, "ReturnTime", "returnDate"),
                Global.GetCqlFilter(Loan2sRadGrid, "SystemReturnTime", "systemReturnDate"),
                Global.GetCqlFilter(Loan2sRadGrid, "Action", "action"),
                Global.GetCqlFilter(Loan2sRadGrid, "ActionComment", "actionComment"),
                Global.GetCqlFilter(Loan2sRadGrid, "ItemStatus", "itemStatus"),
                Global.GetCqlFilter(Loan2sRadGrid, "RenewalCount", "renewalCount"),
                Global.GetCqlFilter(Loan2sRadGrid, "LoanPolicy.Name", "loanPolicyId", "name", folioServiceContext.FolioServiceClient.LoanPolicies),
                Global.GetCqlFilter(Loan2sRadGrid, "CheckoutServicePoint.Name", "checkoutServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Loan2sRadGrid, "CheckinServicePoint.Name", "checkinServicePointId", "name", folioServiceContext.FolioServiceClient.ServicePoints),
                Global.GetCqlFilter(Loan2sRadGrid, "DueDateChangedByRecall", "dueDateChangedByRecall"),
                Global.GetCqlFilter(Loan2sRadGrid, "DeclaredLostDate", "declaredLostDate"),
                Global.GetCqlFilter(Loan2sRadGrid, "ClaimedReturnedDate", "claimedReturnedDate"),
                Global.GetCqlFilter(Loan2sRadGrid, "OverdueFinePolicy.Name", "overdueFinePolicyId", "name", folioServiceContext.FolioServiceClient.OverdueFinePolicies),
                Global.GetCqlFilter(Loan2sRadGrid, "LostItemPolicy.Name", "lostItemPolicyId", "name", folioServiceContext.FolioServiceClient.LostItemFeePolicies),
                Global.GetCqlFilter(Loan2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(Loan2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Loan2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(Loan2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(Loan2sRadGrid, "AgedToLostDelayedBillingLostItemHasBeenBilled", "agedToLostDelayedBilling.lostItemHasBeenBilled"),
                Global.GetCqlFilter(Loan2sRadGrid, "AgedToLostDelayedBillingDateLostItemShouldBeBilled", "agedToLostDelayedBilling.dateLostItemShouldBeBilled"),
                Global.GetCqlFilter(Loan2sRadGrid, "AgedToLostDelayedBillingAgedToLostDate", "agedToLostDelayedBilling.agedToLostDate")
            }.Where(s => s != null)));
            Loan2sRadGrid.DataSource = folioServiceContext.Loan2s(out var i, where, Loan2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Loan2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Loan2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Loan2sRadGrid.PageSize * Loan2sRadGrid.CurrentPageIndex, Loan2sRadGrid.PageSize, true);
            Loan2sRadGrid.VirtualItemCount = i;
            if (Loan2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Loan2sRadGrid.AllowFilteringByColumn = Loan2sRadGrid.VirtualItemCount > 10;
                Loan2sPanel.Visible = Group2FormView.DataKey.Value != null && Session["Loan2sPermission"] != null && Loan2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        protected void User2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["User2sPermission"] == null) return;
            var id = (Guid?)Group2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Username", "username" }, { "ExternalSystemId", "externalSystemId" }, { "Barcode", "barcode" }, { "Active", "active" }, { "GroupId", "patronGroup" }, { "LastName", "personal.lastName" }, { "FirstName", "personal.firstName" }, { "MiddleName", "personal.middleName" }, { "PreferredFirstName", "personal.preferredFirstName" }, { "EmailAddress", "personal.email" }, { "PhoneNumber", "personal.phone" }, { "MobilePhoneNumber", "personal.mobilePhone" }, { "BirthDate", "personal.dateOfBirth" }, { "PreferredContactTypeId", "personal.preferredContactTypeId" }, { "StartDate", "enrollmentDate" }, { "EndDate", "expirationDate" }, { "Source", "customFields.source" }, { "Category", "customFields.category" }, { "Status", "customFields.status" }, { "Statuses", "customFields.statuses" }, { "StaffStatus", "customFields.staffStatus" }, { "StaffPrivileges", "customFields.staffPrivileges" }, { "StaffDivision", "customFields.staffDivision" }, { "StaffDepartment", "customFields.staffDepartment" }, { "StudentId", "customFields.studentId" }, { "StudentStatus", "customFields.studentStatus" }, { "StudentRestriction", "customFields.studentRestriction" }, { "StudentDivision", "customFields.studentDivision" }, { "StudentDepartment", "customFields.studentDepartment" }, { "Deceased", "customFields.deceased" }, { "Collections", "customFields.collections" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"patronGroup == \"{id}\"",
                Global.GetCqlFilter(User2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(User2sRadGrid, "Username", "username"),
                Global.GetCqlFilter(User2sRadGrid, "ExternalSystemId", "externalSystemId"),
                Global.GetCqlFilter(User2sRadGrid, "Barcode", "barcode"),
                Global.GetCqlFilter(User2sRadGrid, "Active", "active"),
                Global.GetCqlFilter(User2sRadGrid, "Name", ""),
                Global.GetCqlFilter(User2sRadGrid, "LastName", "personal.lastName"),
                Global.GetCqlFilter(User2sRadGrid, "FirstName", "personal.firstName"),
                Global.GetCqlFilter(User2sRadGrid, "MiddleName", "personal.middleName"),
                Global.GetCqlFilter(User2sRadGrid, "PreferredFirstName", "personal.preferredFirstName"),
                Global.GetCqlFilter(User2sRadGrid, "EmailAddress", "personal.email"),
                Global.GetCqlFilter(User2sRadGrid, "PhoneNumber", "personal.phone"),
                Global.GetCqlFilter(User2sRadGrid, "MobilePhoneNumber", "personal.mobilePhone"),
                Global.GetCqlFilter(User2sRadGrid, "BirthDate", "personal.dateOfBirth"),
                Global.GetCqlFilter(User2sRadGrid, "StartDate", "enrollmentDate"),
                Global.GetCqlFilter(User2sRadGrid, "EndDate", "expirationDate"),
                Global.GetCqlFilter(User2sRadGrid, "Source", "customFields.source"),
                Global.GetCqlFilter(User2sRadGrid, "Category", "customFields.category"),
                Global.GetCqlFilter(User2sRadGrid, "Status", "customFields.status"),
                Global.GetCqlFilter(User2sRadGrid, "Statuses", "customFields.statuses"),
                Global.GetCqlFilter(User2sRadGrid, "StaffStatus", "customFields.staffStatus"),
                Global.GetCqlFilter(User2sRadGrid, "StaffPrivileges", "customFields.staffPrivileges"),
                Global.GetCqlFilter(User2sRadGrid, "StaffDivision", "customFields.staffDivision"),
                Global.GetCqlFilter(User2sRadGrid, "StaffDepartment", "customFields.staffDepartment"),
                Global.GetCqlFilter(User2sRadGrid, "StudentId", "customFields.studentId"),
                Global.GetCqlFilter(User2sRadGrid, "StudentStatus", "customFields.studentStatus"),
                Global.GetCqlFilter(User2sRadGrid, "StudentRestriction", "customFields.studentRestriction"),
                Global.GetCqlFilter(User2sRadGrid, "StudentDivision", "customFields.studentDivision"),
                Global.GetCqlFilter(User2sRadGrid, "StudentDepartment", "customFields.studentDepartment"),
                Global.GetCqlFilter(User2sRadGrid, "Deceased", "customFields.deceased"),
                Global.GetCqlFilter(User2sRadGrid, "Collections", "customFields.collections"),
                Global.GetCqlFilter(User2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(User2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(User2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(User2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            User2sRadGrid.DataSource = folioServiceContext.User2s(out var i, where, User2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[User2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(User2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, User2sRadGrid.PageSize * User2sRadGrid.CurrentPageIndex, User2sRadGrid.PageSize, true);
            User2sRadGrid.VirtualItemCount = i;
            if (User2sRadGrid.MasterTableView.FilterExpression == "")
            {
                User2sRadGrid.AllowFilteringByColumn = User2sRadGrid.VirtualItemCount > 10;
                User2sPanel.Visible = Group2FormView.DataKey.Value != null && ((string)Session["User2sPermission"] == "Edit" || Session["User2sPermission"] != null && User2sRadGrid.VirtualItemCount > 0);
            }
            traceSource.TraceEvent(TraceEventType.Information, 0, $"where = {where}");
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
