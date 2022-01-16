using FolioLibrary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Telerik.Web.UI;

namespace FolioWebApplication.Contact2s
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Contact2sPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void Contact2FormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var c2 = folioServiceContext.FindContact2(id, true);
            if (c2 == null) Response.Redirect("Default.aspx");
            c2.Content = c2.Content != null ? JsonConvert.DeserializeObject<JToken>(c2.Content, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Local }).ToString() : null;
            Contact2FormView.DataSource = new[] { c2 };
            Title = $"Contact {c2.Name}";
        }

        protected void ContactAddressesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ContactAddressesPermission"] == null) return;
            var id = (Guid?)Contact2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindContact2(id, true).ContactAddresses ?? new ContactAddress[] { };
            ContactAddressesRadGrid.DataSource = l;
            ContactAddressesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ContactAddressesPanel.Visible = Contact2FormView.DataKey.Value != null && ((string)Session["ContactAddressesPermission"] == "Edit" || Session["ContactAddressesPermission"] != null && l.Any());
        }

        protected void ContactCategoriesRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ContactCategoriesPermission"] == null) return;
            var id = (Guid?)Contact2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindContact2(id, true).ContactCategories ?? new ContactCategory[] { };
            ContactCategoriesRadGrid.DataSource = l;
            ContactCategoriesRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ContactCategoriesPanel.Visible = Contact2FormView.DataKey.Value != null && ((string)Session["ContactCategoriesPermission"] == "Edit" || Session["ContactCategoriesPermission"] != null && l.Any());
        }

        protected void ContactEmailsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ContactEmailsPermission"] == null) return;
            var id = (Guid?)Contact2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindContact2(id, true).ContactEmails ?? new ContactEmail[] { };
            ContactEmailsRadGrid.DataSource = l;
            ContactEmailsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ContactEmailsPanel.Visible = Contact2FormView.DataKey.Value != null && ((string)Session["ContactEmailsPermission"] == "Edit" || Session["ContactEmailsPermission"] != null && l.Any());
        }

        protected void ContactPhoneNumbersRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ContactPhoneNumbersPermission"] == null) return;
            var id = (Guid?)Contact2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindContact2(id, true).ContactPhoneNumbers ?? new ContactPhoneNumber[] { };
            ContactPhoneNumbersRadGrid.DataSource = l;
            ContactPhoneNumbersRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ContactPhoneNumbersPanel.Visible = Contact2FormView.DataKey.Value != null && ((string)Session["ContactPhoneNumbersPermission"] == "Edit" || Session["ContactPhoneNumbersPermission"] != null && l.Any());
        }

        protected void ContactUrlsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["ContactUrlsPermission"] == null) return;
            var id = (Guid?)Contact2FormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.FindContact2(id, true).ContactUrls ?? new ContactUrl[] { };
            ContactUrlsRadGrid.DataSource = l;
            ContactUrlsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            ContactUrlsPanel.Visible = Contact2FormView.DataKey.Value != null && ((string)Session["ContactUrlsPermission"] == "Edit" || Session["ContactUrlsPermission"] != null && l.Any());
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
