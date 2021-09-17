using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace FolioWebApplication.Group2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
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
            var g2 = id == null && (string)Session["Group2sPermission"] == "Edit" ? new Group2() : folioServiceContext.FindGroup2(id, true);
            if (g2 == null) Response.Redirect("Default.aspx");
            g2.Content = g2.Content != null ? JsonConvert.DeserializeObject<JToken>(g2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Group2FormView.DataSource = new[] { g2 };
            Title = $"Group {g2.Name}";
        }

        protected void Group2FormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            var id = (Guid?)Group2FormView.DataKey.Value;
            var g2 = id != null ? folioServiceContext.FindGroup2(id) : new Group2 { Id = Guid.NewGuid(), CreationTime = DateTime.Now, CreationUserId = (Guid?)Session["UserId"] };
            g2.Name = Global.Trim((string)e.NewValues["Name"]);
            g2.Description = Global.Trim((string)e.NewValues["Description"]);
            g2.ExpirationOffsetInDays = (int?)e.NewValues["ExpirationOffsetInDays"];
            g2.LastWriteTime = DateTime.Now;
            g2.LastWriteUserId = (Guid?)Session["UserId"];
            var vr = Group2.ValidateGroup2(g2, new ValidationContext(folioServiceContext));
            if (vr != null)
            {
                var cv = (CustomValidator)Group2FormView.FindControl("Group2CustomValidator");
                cv.IsValid = false;
                cv.ErrorMessage = vr.ErrorMessage;
                e.Cancel = true;
                return;
            }
            if (id == null) folioServiceContext.Insert(g2); else folioServiceContext.Update(g2);
            if (id == null) Response.Redirect($"Edit.aspx?Id={g2.Id}"); else Response.Redirect("Default.aspx");
        }

        protected void Group2FormView_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel") Response.Redirect("Default.aspx");
        }

        protected void Group2FormView_ItemDeleting(object sender, FormViewDeleteEventArgs e)
        {
            var id = (Guid?)Group2FormView.DataKey.Value;
            try
            {
                if (folioServiceContext.AnyBlockLimit2s($"patronGroupId == \"{id}\"")) throw new Exception("Group cannot be deleted because it is being referenced by a block limit");
                if (folioServiceContext.AnyLoan2s($"patronGroupIdAtCheckout == \"{id}\"")) throw new Exception("Group cannot be deleted because it is being referenced by a loan");
                if (folioServiceContext.AnyUser2s($"patronGroup == \"{id}\"")) throw new Exception("Group cannot be deleted because it is being referenced by a user");
                folioServiceContext.DeleteGroup2(id);
                Response.Redirect("Default.aspx");
            }
            catch (Exception)
            {
                var cv = (CustomValidator)((FormView)sender).FindControl("DeleteCustomValidator");
                cv.IsValid = false;
            }
        }

        protected void BlockLimit2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["BlockLimit2sPermission"] == null) return;
            var id = (Guid?)Group2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "GroupId", "patronGroupId" }, { "ConditionId", "conditionId" }, { "Value", "value" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            BlockLimit2sRadGrid.DataSource = folioServiceContext.BlockLimit2s(out var i, Global.GetCqlFilter(BlockLimit2sRadGrid, d, $"patronGroupId == \"{id}\""), BlockLimit2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BlockLimit2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BlockLimit2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BlockLimit2sRadGrid.PageSize * BlockLimit2sRadGrid.CurrentPageIndex, BlockLimit2sRadGrid.PageSize, true);
            BlockLimit2sRadGrid.VirtualItemCount = i;
            if (BlockLimit2sRadGrid.MasterTableView.FilterExpression == "")
            {
                BlockLimit2sRadGrid.AllowFilteringByColumn = BlockLimit2sRadGrid.VirtualItemCount > 10;
                BlockLimit2sPanel.Visible = Group2FormView.DataKey.Value != null && Session["BlockLimit2sPermission"] != null && BlockLimit2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void Loan2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Loan2sPermission"] == null) return;
            var id = (Guid?)Group2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "ProxyUserId", "proxyUserId" }, { "ItemId", "itemId" }, { "ItemEffectiveLocationAtCheckOutId", "itemEffectiveLocationIdAtCheckOut" }, { "StatusName", "status.name" }, { "LoanTime", "loanDate" }, { "DueTime", "dueDate" }, { "ReturnTime", "returnDate" }, { "SystemReturnTime", "systemReturnDate" }, { "Action", "action" }, { "ActionComment", "actionComment" }, { "ItemStatus", "itemStatus" }, { "RenewalCount", "renewalCount" }, { "LoanPolicyId", "loanPolicyId" }, { "CheckoutServicePointId", "checkoutServicePointId" }, { "CheckinServicePointId", "checkinServicePointId" }, { "GroupId", "patronGroupIdAtCheckout" }, { "DueDateChangedByRecall", "dueDateChangedByRecall" }, { "DeclaredLostDate", "declaredLostDate" }, { "ClaimedReturnedDate", "claimedReturnedDate" }, { "OverdueFinePolicyId", "overdueFinePolicyId" }, { "LostItemPolicyId", "lostItemPolicyId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AgedToLostDelayedBillingLostItemHasBeenBilled", "agedToLostDelayedBilling.lostItemHasBeenBilled" }, { "AgedToLostDelayedBillingDateLostItemShouldBeBilled", "agedToLostDelayedBilling.dateLostItemShouldBeBilled" }, { "AgedToLostDelayedBillingAgedToLostDate", "agedToLostDelayedBilling.agedToLostDate" } };
            Loan2sRadGrid.DataSource = folioServiceContext.Loan2s(out var i, Global.GetCqlFilter(Loan2sRadGrid, d, $"patronGroupIdAtCheckout == \"{id}\""), Loan2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Loan2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Loan2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Loan2sRadGrid.PageSize * Loan2sRadGrid.CurrentPageIndex, Loan2sRadGrid.PageSize, true);
            Loan2sRadGrid.VirtualItemCount = i;
            if (Loan2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Loan2sRadGrid.AllowFilteringByColumn = Loan2sRadGrid.VirtualItemCount > 10;
                Loan2sPanel.Visible = Group2FormView.DataKey.Value != null && Session["Loan2sPermission"] != null && Loan2sRadGrid.VirtualItemCount > 0;
            }
        }

        protected void User2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["User2sPermission"] == null) return;
            var id = (Guid?)Group2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Username", "username" }, { "ExternalSystemId", "externalSystemId" }, { "Barcode", "barcode" }, { "Active", "active" }, { "GroupId", "patronGroup" }, { "LastName", "personal.lastName" }, { "FirstName", "personal.firstName" }, { "MiddleName", "personal.middleName" }, { "PreferredFirstName", "personal.preferredFirstName" }, { "EmailAddress", "personal.email" }, { "PhoneNumber", "personal.phone" }, { "MobilePhoneNumber", "personal.mobilePhone" }, { "BirthDate", "personal.dateOfBirth" }, { "PreferredContactTypeId", "personal.preferredContactTypeId" }, { "StartDate", "enrollmentDate" }, { "EndDate", "expirationDate" }, { "Source", "customFields.source" }, { "Category", "customFields.category" }, { "Status", "customFields.status" }, { "Statuses", "customFields.statuses" }, { "StaffStatus", "customFields.staffStatus" }, { "StaffPrivileges", "customFields.staffPrivileges" }, { "StaffDivision", "customFields.staffDivision" }, { "StaffDepartment", "customFields.staffDepartment" }, { "StudentId", "customFields.studentId" }, { "StudentStatus", "customFields.studentStatus" }, { "StudentRestriction", "customFields.studentRestriction" }, { "StudentDivision", "customFields.studentDivision" }, { "StudentDepartment", "customFields.studentDepartment" }, { "Deceased", "customFields.deceased" }, { "Collections", "customFields.collections" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            User2sRadGrid.DataSource = folioServiceContext.User2s(out var i, Global.GetCqlFilter(User2sRadGrid, d, $"patronGroup == \"{id}\""), User2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[User2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(User2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, User2sRadGrid.PageSize * User2sRadGrid.CurrentPageIndex, User2sRadGrid.PageSize, true);
            User2sRadGrid.VirtualItemCount = i;
            if (User2sRadGrid.MasterTableView.FilterExpression == "")
            {
                User2sRadGrid.AllowFilteringByColumn = User2sRadGrid.VirtualItemCount > 10;
                User2sPanel.Visible = Group2FormView.DataKey.Value != null && ((string)Session["User2sPermission"] == "Edit" || Session["User2sPermission"] != null && User2sRadGrid.VirtualItemCount > 0);
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
