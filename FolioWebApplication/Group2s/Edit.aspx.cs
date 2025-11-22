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
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
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

        protected void ActualCostRecord2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ActualCostRecord2sPermission"] == null) return;
            var id = (Guid?)Group2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "LossType", "lossType" }, { "LossDate", "lossDate" }, { "ExpirationDate", "expirationDate" }, { "UserBarcode", "user.barcode" }, { "UserFirstName", "user.firstName" }, { "UserLastName", "user.lastName" }, { "UserMiddleName", "user.middleName" }, { "UserGroupId", "user.patronGroupId" }, { "UserGroupName", "user.patronGroup" }, { "ItemBarcode", "item.barcode" }, { "ItemMaterialTypeId", "item.materialTypeId" }, { "ItemMaterialTypeName", "item.materialType" }, { "ItemPermanentLocationId", "item.permanentLocationId" }, { "ItemPermanentLocationName", "item.permanentLocation" }, { "ItemEffectiveLocationId", "item.effectiveLocationId" }, { "ItemEffectiveLocationName", "item.effectiveLocation" }, { "ItemLoanTypeId", "item.loanTypeId" }, { "ItemLoanTypeName", "item.loanType" }, { "ItemHoldingId", "item.holdingsRecordId" }, { "ItemEffectiveCallNumber", "item.effectiveCallNumberComponents.callNumber" }, { "ItemEffectiveCallNumberPrefix", "item.effectiveCallNumberComponents.prefix" }, { "ItemEffectiveCallNumberSuffix", "item.effectiveCallNumberComponents.suffix" }, { "ItemVolume", "item.volume" }, { "ItemEnumeration", "item.enumeration" }, { "ItemChronology", "item.chronology" }, { "ItemDisplaySummary", "item.displaySummary" }, { "ItemCopyNumber", "item.copyNumber" }, { "InstanceTitle", "instance.title" }, { "FeeId", "feeFine.accountId" }, { "FeeBilledAmount", "feeFine.billedAmount" }, { "OwnerId", "feeFine.ownerId" }, { "OwnerName", "feeFine.owner" }, { "FeeTypeId", "feeFine.typeId" }, { "FeeTypeName", "feeFine.type" }, { "Status", "status" }, { "AdditionalInfoForStaff", "additionalInfoForStaff" }, { "AdditionalInfoForPatron", "additionalInfoForPatron" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"user.patronGroupId == \"{id}\"",
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "LossType", "lossType"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "LossDate", "lossDate"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ExpirationDate", "expirationDate"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "UserBarcode", "user.barcode"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "UserFirstName", "user.firstName"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "UserLastName", "user.lastName"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "UserMiddleName", "user.middleName"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "UserGroupName", "user.patronGroup"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemBarcode", "item.barcode"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemMaterialType.Name", "item.materialTypeId", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemMaterialTypeName", "item.materialType"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemPermanentLocation.Name", "item.permanentLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemPermanentLocationName", "item.permanentLocation"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemEffectiveLocation.Name", "item.effectiveLocationId", "name", folioServiceContext.FolioServiceClient.Locations),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemEffectiveLocationName", "item.effectiveLocation"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemLoanType.Name", "item.loanTypeId", "name", folioServiceContext.FolioServiceClient.LoanTypes),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemLoanTypeName", "item.loanType"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemHolding.ShortId", "item.holdingsRecordId", "hrid", folioServiceContext.FolioServiceClient.Holdings),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemEffectiveCallNumber", "item.effectiveCallNumberComponents.callNumber"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemEffectiveCallNumberPrefix", "item.effectiveCallNumberComponents.prefix"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemEffectiveCallNumberSuffix", "item.effectiveCallNumberComponents.suffix"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemVolume", "item.volume"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemEnumeration", "item.enumeration"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemChronology", "item.chronology"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemDisplaySummary", "item.displaySummary"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "ItemCopyNumber", "item.copyNumber"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "InstanceTitle", "instance.title"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "Fee.Title", "feeFine.accountId", "title", folioServiceContext.FolioServiceClient.Fees),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "FeeBilledAmount", "feeFine.billedAmount"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "Owner.Name", "feeFine.ownerId", "owner", folioServiceContext.FolioServiceClient.Owners),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "OwnerName", "feeFine.owner"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "FeeType.Name", "feeFine.typeId", "feeFineType", folioServiceContext.FolioServiceClient.FeeTypes),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "FeeTypeName", "feeFine.type"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "Status", "status"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "AdditionalInfoForStaff", "additionalInfoForStaff"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "AdditionalInfoForPatron", "additionalInfoForPatron"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(ActualCostRecord2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            ActualCostRecord2sRadGrid.DataSource = folioServiceContext.ActualCostRecord2s(where, ActualCostRecord2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[ActualCostRecord2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(ActualCostRecord2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, ActualCostRecord2sRadGrid.PageSize * ActualCostRecord2sRadGrid.CurrentPageIndex, ActualCostRecord2sRadGrid.PageSize, true);
            ActualCostRecord2sRadGrid.VirtualItemCount = folioServiceContext.CountActualCostRecord2s(where);
            if (ActualCostRecord2sRadGrid.MasterTableView.FilterExpression == "")
            {
                ActualCostRecord2sRadGrid.AllowFilteringByColumn = ActualCostRecord2sRadGrid.VirtualItemCount > 10;
                ActualCostRecord2sPanel.Visible = Group2FormView.DataKey.Value != null && Session["ActualCostRecord2sPermission"] != null && ActualCostRecord2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
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
            BlockLimit2sRadGrid.DataSource = folioServiceContext.BlockLimit2s(where, BlockLimit2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[BlockLimit2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(BlockLimit2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, BlockLimit2sRadGrid.PageSize * BlockLimit2sRadGrid.CurrentPageIndex, BlockLimit2sRadGrid.PageSize, true);
            BlockLimit2sRadGrid.VirtualItemCount = folioServiceContext.CountBlockLimit2s(where);
            if (BlockLimit2sRadGrid.MasterTableView.FilterExpression == "")
            {
                BlockLimit2sRadGrid.AllowFilteringByColumn = BlockLimit2sRadGrid.VirtualItemCount > 10;
                BlockLimit2sPanel.Visible = Group2FormView.DataKey.Value != null && Session["BlockLimit2sPermission"] != null && BlockLimit2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void Loan2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["Loan2sPermission"] == null) return;
            var id = (Guid?)Group2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "UserId", "userId" }, { "ProxyUserId", "proxyUserId" }, { "ItemId", "itemId" }, { "ItemEffectiveLocationAtCheckOutId", "itemEffectiveLocationIdAtCheckOut" }, { "StatusName", "status.name" }, { "LoanTime", "loanDate" }, { "DueTime", "dueDate" }, { "ReturnTime", "returnDate" }, { "SystemReturnTime", "systemReturnDate" }, { "Action", "action" }, { "ActionComment", "actionComment" }, { "ItemStatus", "itemStatus" }, { "RenewalCount", "renewalCount" }, { "LoanPolicyId", "loanPolicyId" }, { "CheckoutServicePointId", "checkoutServicePointId" }, { "CheckinServicePointId", "checkinServicePointId" }, { "GroupId", "patronGroupIdAtCheckout" }, { "DueDateChangedByRecall", "dueDateChangedByRecall" }, { "IsDcb", "isDcb" }, { "DeclaredLostDate", "declaredLostDate" }, { "ClaimedReturnedDate", "claimedReturnedDate" }, { "OverdueFinePolicyId", "overdueFinePolicyId" }, { "LostItemPolicyId", "lostItemPolicyId" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" }, { "AgedToLostDelayedBillingLostItemHasBeenBilled", "agedToLostDelayedBilling.lostItemHasBeenBilled" }, { "AgedToLostDelayedBillingDateLostItemShouldBeBilled", "agedToLostDelayedBilling.dateLostItemShouldBeBilled" }, { "AgedToLostDelayedBillingAgedToLostDate", "agedToLostDelayedBilling.agedToLostDate" }, { "RemindersLastFeeBilledNumber", "reminders.lastFeeBilled.number" }, { "RemindersLastFeeBilledDate", "reminders.lastFeeBilled.date" } };
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
                Global.GetCqlFilter(Loan2sRadGrid, "IsDcb", "isDcb"),
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
                Global.GetCqlFilter(Loan2sRadGrid, "AgedToLostDelayedBillingAgedToLostDate", "agedToLostDelayedBilling.agedToLostDate"),
                Global.GetCqlFilter(Loan2sRadGrid, "RemindersLastFeeBilledNumber", "reminders.lastFeeBilled.number"),
                Global.GetCqlFilter(Loan2sRadGrid, "RemindersLastFeeBilledDate", "reminders.lastFeeBilled.date")
            }.Where(s => s != null)));
            Loan2sRadGrid.DataSource = folioServiceContext.Loan2s(where, Loan2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[Loan2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(Loan2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, Loan2sRadGrid.PageSize * Loan2sRadGrid.CurrentPageIndex, Loan2sRadGrid.PageSize, true);
            Loan2sRadGrid.VirtualItemCount = folioServiceContext.CountLoan2s(where);
            if (Loan2sRadGrid.MasterTableView.FilterExpression == "")
            {
                Loan2sRadGrid.AllowFilteringByColumn = Loan2sRadGrid.VirtualItemCount > 10;
                Loan2sPanel.Visible = Group2FormView.DataKey.Value != null && Session["Loan2sPermission"] != null && Loan2sRadGrid.VirtualItemCount > 0;
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        protected void User2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["User2sPermission"] == null) return;
            var id = (Guid?)Group2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Username", "username" }, { "ExternalSystemId", "externalSystemId" }, { "Barcode", "barcode" }, { "Active", "active" }, { "GroupId", "patronGroup" }, { "Pronouns", "personal.pronouns" }, { "LastName", "personal.lastName" }, { "FirstName", "personal.firstName" }, { "MiddleName", "personal.middleName" }, { "PreferredFirstName", "personal.preferredFirstName" }, { "EmailAddress", "personal.email" }, { "PhoneNumber", "personal.phone" }, { "MobilePhoneNumber", "personal.mobilePhone" }, { "BirthDate", "personal.dateOfBirth" }, { "PreferredContactTypeId", "personal.preferredContactTypeId" }, { "ProfilePictureLink", "personal.profilePictureLink" }, { "StartDate", "enrollmentDate" }, { "EndDate", "expirationDate" }, { "Source", "customFields.source" }, { "CategoryCode", "customFields.category" }, { "CategoryId", "customFields.category_2" }, { "Status", "customFields.status" }, { "Statuses", "customFields.statuses" }, { "StaffStatus", "customFields.staffStatus" }, { "StaffPrivileges", "customFields.staffPrivileges" }, { "StaffDivision", "customFields.staffDivision" }, { "StaffDepartment", "customFields.staffDepartment" }, { "StudentId", "customFields.studentId" }, { "StudentStatus", "customFields.studentStatus" }, { "StudentRestriction", "customFields.studentRestriction" }, { "StudentDivision", "customFields.studentDivision" }, { "StudentDepartment", "customFields.studentDepartment" }, { "Deceased", "customFields.deceased" }, { "Collections", "customFields.collections" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"patronGroup == \"{id}\"",
                Global.GetCqlFilter(User2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(User2sRadGrid, "Username", "username"),
                Global.GetCqlFilter(User2sRadGrid, "ExternalSystemId", "externalSystemId"),
                Global.GetCqlFilter(User2sRadGrid, "Barcode", "barcode"),
                Global.GetCqlFilter(User2sRadGrid, "Active", "active"),
                Global.GetCqlFilter(User2sRadGrid, "Pronouns", "personal.pronouns"),
                Global.GetCqlFilter(User2sRadGrid, "Name", ""),
                Global.GetCqlFilter(User2sRadGrid, "LastName", "personal.lastName"),
                Global.GetCqlFilter(User2sRadGrid, "FirstName", "personal.firstName"),
                Global.GetCqlFilter(User2sRadGrid, "MiddleName", "personal.middleName"),
                Global.GetCqlFilter(User2sRadGrid, "PreferredFirstName", "personal.preferredFirstName"),
                Global.GetCqlFilter(User2sRadGrid, "EmailAddress", "personal.email"),
                Global.GetCqlFilter(User2sRadGrid, "PhoneNumber", "personal.phone"),
                Global.GetCqlFilter(User2sRadGrid, "MobilePhoneNumber", "personal.mobilePhone"),
                Global.GetCqlFilter(User2sRadGrid, "BirthDate", "personal.dateOfBirth"),
                Global.GetCqlFilter(User2sRadGrid, "ProfilePictureLink", "personal.profilePictureLink"),
                Global.GetCqlFilter(User2sRadGrid, "StartDate", "enrollmentDate"),
                Global.GetCqlFilter(User2sRadGrid, "EndDate", "expirationDate"),
                Global.GetCqlFilter(User2sRadGrid, "Source", "customFields.source"),
                Global.GetCqlFilter(User2sRadGrid, "CategoryCode", "customFields.category"),
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
            User2sRadGrid.DataSource = folioServiceContext.User2s(where, User2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[User2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(User2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, User2sRadGrid.PageSize * User2sRadGrid.CurrentPageIndex, User2sRadGrid.PageSize, true);
            User2sRadGrid.VirtualItemCount = folioServiceContext.CountUser2s(where);
            if (User2sRadGrid.MasterTableView.FilterExpression == "")
            {
                User2sRadGrid.AllowFilteringByColumn = User2sRadGrid.VirtualItemCount > 10;
                User2sPanel.Visible = Group2FormView.DataKey.Value != null && ((string)Session["User2sPermission"] == "Edit" || Session["User2sPermission"] != null && User2sRadGrid.VirtualItemCount > 0);
            }
            traceSource.TraceEvent(TraceEventType.Verbose, 0, $"where = {where}");
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
