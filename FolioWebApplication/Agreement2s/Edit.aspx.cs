using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Agreement2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext(timeout: TimeSpan.FromSeconds(30));
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Agreement2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Agreement2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var a2 = folioServiceContext.FindAgreement2(id, true);
            if (a2 == null) Response.Redirect("Default.aspx");
            a2.Content = a2.Content != null ? JsonConvert.DeserializeObject<JToken>(a2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Agreement2FormView.DataSource = new[] { a2 };
            Title = $"Agreement {a2.Name}";
        }

        protected void OrderItem2sRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["OrderItem2sPermission"] == null) return;
            var id = (Guid?)Agreement2FormView.DataKey.Value;
            if (id == null) return;
            var d = new Dictionary<string, string>() { { "Id", "id" }, { "Edition", "edition" }, { "CheckinItems", "checkinItems" }, { "AgreementId", "agreementId" }, { "AcquisitionMethodId", "acquisitionMethod" }, { "AutomaticExport", "automaticExport" }, { "CancellationRestriction", "cancellationRestriction" }, { "CancellationRestrictionNote", "cancellationRestrictionNote" }, { "Collection", "collection" }, { "PhysicalUnitListPrice", "cost.listUnitPrice" }, { "ElectronicUnitListPrice", "cost.listUnitPriceElectronic" }, { "Currency", "cost.currency" }, { "AdditionalCost", "cost.additionalCost" }, { "Discount", "cost.discount" }, { "DiscountType", "cost.discountType" }, { "ExchangeRate", "cost.exchangeRate" }, { "PhysicalQuantity", "cost.quantityPhysical" }, { "ElectronicQuantity", "cost.quantityElectronic" }, { "EstimatedPrice", "cost.poLineEstimatedPrice" }, { "FiscalYearRolloverAdjustmentAmount", "cost.fyroAdjustmentAmount" }, { "InternalNote", "description" }, { "ReceivingNote", "details.receivingNote" }, { "SubscriptionFrom", "details.subscriptionFrom" }, { "SubscriptionInterval", "details.subscriptionInterval" }, { "SubscriptionTo", "details.subscriptionTo" }, { "Donor", "donor" }, { "EresourceActivated", "eresource.activated" }, { "EresourceActivationDue", "eresource.activationDue" }, { "EresourceCreateInventory", "eresource.createInventory" }, { "EresourceTrial", "eresource.trial" }, { "EresourceExpectedActivationDate", "eresource.expectedActivation" }, { "EresourceUserLimit", "eresource.userLimit" }, { "EresourceAccessProviderId", "eresource.accessProvider" }, { "EresourceLicenseCode", "eresource.license.code" }, { "EresourceLicenseDescription", "eresource.license.description" }, { "EresourceLicenseReference", "eresource.license.reference" }, { "EresourceMaterialTypeId", "eresource.materialType" }, { "EresourceResourceUrl", "eresource.resourceUrl" }, { "InstanceId", "instanceId" }, { "IsPackage", "isPackage" }, { "LastEdiExportDate", "lastEDIExportDate" }, { "OrderFormat", "orderFormat" }, { "PackageOrderItemId", "packagePoLineId" }, { "PaymentStatus", "paymentStatus" }, { "PhysicalCreateInventory", "physical.createInventory" }, { "PhysicalMaterialTypeId", "physical.materialType" }, { "PhysicalMaterialSupplierId", "physical.materialSupplier" }, { "PhysicalExpectedReceiptDate", "physical.expectedReceiptDate" }, { "PhysicalReceiptDue", "physical.receiptDue" }, { "Description", "poLineDescription" }, { "Number", "poLineNumber" }, { "PublicationYear", "publicationDate" }, { "Publisher", "publisher" }, { "OrderId", "purchaseOrderId" }, { "ReceiptDate", "receiptDate" }, { "ReceiptStatus", "receiptStatus" }, { "RenewalNote", "renewalNote" }, { "Requester", "requester" }, { "Rush", "rush" }, { "Selector", "selector" }, { "Source", "source" }, { "TitleOrPackage", "titleOrPackage" }, { "VendorInstructions", "vendorDetail.instructions" }, { "VendorNote", "vendorDetail.noteFromVendor" }, { "VendorCustomerId", "vendorDetail.vendorAccount" }, { "CreationTime", "metadata.createdDate" }, { "CreationUserId", "metadata.createdByUserId" }, { "LastWriteTime", "metadata.updatedDate" }, { "LastWriteUserId", "metadata.updatedByUserId" } };
            var where = Global.Trim(string.Join(" and ", new string[]
            {
                $"agreementId == \"{id}\"",
                Global.GetCqlFilter(OrderItem2sRadGrid, "Id", "id"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Edition", "edition"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "CheckinItems", "checkinItems"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "AcquisitionMethod.Name", "acquisitionMethod", "value", folioServiceContext.FolioServiceClient.AcquisitionMethods),
                Global.GetCqlFilter(OrderItem2sRadGrid, "AutomaticExport", "automaticExport"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "CancellationRestriction", "cancellationRestriction"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "CancellationRestrictionNote", "cancellationRestrictionNote"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Collection", "collection"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PhysicalUnitListPrice", "cost.listUnitPrice"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ElectronicUnitListPrice", "cost.listUnitPriceElectronic"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Currency", "cost.currency"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "AdditionalCost", "cost.additionalCost"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Discount", "cost.discount"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "DiscountType", "cost.discountType"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ExchangeRate", "cost.exchangeRate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PhysicalQuantity", "cost.quantityPhysical"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ElectronicQuantity", "cost.quantityElectronic"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EstimatedPrice", "cost.poLineEstimatedPrice"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "FiscalYearRolloverAdjustmentAmount", "cost.fyroAdjustmentAmount"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "InternalNote", "description"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ReceivingNote", "details.receivingNote"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "SubscriptionFrom", "details.subscriptionFrom"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "SubscriptionInterval", "details.subscriptionInterval"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "SubscriptionTo", "details.subscriptionTo"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Donor", "donor"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceActivated", "eresource.activated"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceActivationDue", "eresource.activationDue"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceCreateInventory", "eresource.createInventory"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceTrial", "eresource.trial"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceExpectedActivationDate", "eresource.expectedActivation"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceUserLimit", "eresource.userLimit"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceAccessProvider.Name", "eresource.accessProvider", "name", folioServiceContext.FolioServiceClient.Organizations),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceLicenseCode", "eresource.license.code"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceLicenseDescription", "eresource.license.description"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceLicenseReference", "eresource.license.reference"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceMaterialType.Name", "eresource.materialType", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
                Global.GetCqlFilter(OrderItem2sRadGrid, "EresourceResourceUrl", "eresource.resourceUrl"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Instance.Title", "instanceId", "title", folioServiceContext.FolioServiceClient.Instances),
                Global.GetCqlFilter(OrderItem2sRadGrid, "IsPackage", "isPackage"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "LastEdiExportDate", "lastEDIExportDate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "OrderFormat", "orderFormat"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PackageOrderItem.Number", "packagePoLineId", "poLineNumber", folioServiceContext.FolioServiceClient.OrderItems),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PaymentStatus", "paymentStatus"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PhysicalCreateInventory", "physical.createInventory"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PhysicalMaterialType.Name", "physical.materialType", "name", folioServiceContext.FolioServiceClient.MaterialTypes),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PhysicalMaterialSupplier.Name", "physical.materialSupplier", "name", folioServiceContext.FolioServiceClient.Organizations),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PhysicalExpectedReceiptDate", "physical.expectedReceiptDate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PhysicalReceiptDue", "physical.receiptDue"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Description", "poLineDescription"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Number", "poLineNumber"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "PublicationYear", "publicationDate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Publisher", "publisher"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Order.Number", "purchaseOrderId", "poNumber", folioServiceContext.FolioServiceClient.Orders),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ReceiptDate", "receiptDate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "ReceiptStatus", "receiptStatus"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "RenewalNote", "renewalNote"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Requester", "requester"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Rush", "rush"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Selector", "selector"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "Source", "source"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "TitleOrPackage", "titleOrPackage"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "VendorInstructions", "vendorDetail.instructions"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "VendorNote", "vendorDetail.noteFromVendor"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "VendorCustomerId", "vendorDetail.vendorAccount"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "CreationTime", "metadata.createdDate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "CreationUser.Username", "metadata.createdByUserId", "username", folioServiceContext.FolioServiceClient.Users),
                Global.GetCqlFilter(OrderItem2sRadGrid, "LastWriteTime", "metadata.updatedDate"),
                Global.GetCqlFilter(OrderItem2sRadGrid, "LastWriteUser.Username", "metadata.updatedByUserId", "username", folioServiceContext.FolioServiceClient.Users)
            }.Where(s => s != null)));
            OrderItem2sRadGrid.DataSource = folioServiceContext.OrderItem2s(out var i, where, OrderItem2sRadGrid.MasterTableView.SortExpressions.Count > 0 ? $"{d[OrderItem2sRadGrid.MasterTableView.SortExpressions[0].FieldName]}{(OrderItem2sRadGrid.MasterTableView.SortExpressions[0].SortOrder == GridSortOrder.Descending ? "/sort.descending" : "")}" : null, OrderItem2sRadGrid.PageSize * OrderItem2sRadGrid.CurrentPageIndex, OrderItem2sRadGrid.PageSize, true);
            OrderItem2sRadGrid.VirtualItemCount = i;
            if (OrderItem2sRadGrid.MasterTableView.FilterExpression == "")
            {
                OrderItem2sRadGrid.AllowFilteringByColumn = OrderItem2sRadGrid.VirtualItemCount > 10;
                OrderItem2sPanel.Visible = Agreement2FormView.DataKey.Value != null && Session["OrderItem2sPermission"] != null && OrderItem2sRadGrid.VirtualItemCount > 0;
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
