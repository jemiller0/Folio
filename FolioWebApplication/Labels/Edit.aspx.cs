using FolioLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Net;
using System.Web.ModelBinding;
using System.Web.UI;
using Telerik.Web.UI;
using Label = FolioLibrary.Label;
using Orientation = FolioLibrary.Orientation;

namespace FolioWebApplication.Labels
{
    public partial class Edit : Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LabelsPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack)
            {
                DataBind();
                var c = LabelFormView.FindControl("ViewRadButton");
                if (c != null) c.Focus(); else BarcodeRadTextBox.Focus();
                var computerName = Request.IsLocal ? Dns.GetHostName() : Request.UserHostAddress.StartsWith("128.135.") ? Dns.GetHostEntry(Request.UserHostAddress)?.HostName?.Split('.')?.FirstOrDefault() : null;
                if (!folioServiceContext.Printers().Any(p2 => p2.ComputerName == computerName && p2.Enabled.Value)) RadAjaxManager1.EnableAJAX = false;
            }
        }

        protected void FindRadButton_Click(object sender, EventArgs e)
        {
            Session.Remove("Label");
            if (IsValid)
            {
                Response.Redirect("Edit.aspx?Barcode=" + BarcodeRadTextBox.Text);
            }
        }

        public Label LabelFormView_GetItem([QueryString]string barcode)
        {
            if (barcode == null) return null;
            BarcodeRadTextBox.Text = barcode;
            if (Session["Label"] != null) return (Label)Session["Label"];
            var l = FolioService.Instance.GetLabel(barcode);
            if (l == null) FindCustomValidator.IsValid = false;
            return l;
        }

        public void LabelFormView_UpdateItem(Label label)
        {
            if (ModelState.IsValid)
            {
                Session["Label"] = label;
                Global.Print(label, this, folioServiceContext);
            }
        }

        public IEnumerable<string> OrientationRadComboBox_GetItems()
        {
            return Enum.GetNames(typeof(Orientation));
        }

        protected void OrientationRadComboBox_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var rcb = (RadComboBox)sender;
            var rtb = (RadTextBox)rcb.NamingContainer.FindControl("TextRadTextBox");
            rtb.Text = FolioService.Instance.GetLabel((string)LabelFormView.DataKey.Value, (Orientation)Enum.Parse(typeof(Orientation), rcb.Text)).Text;
        }

        public IEnumerable<string> FontFamilyRadComboBox_GetItems()
        {
            return from ff in new InstalledFontCollection().Families
                   select ff.Name;
        }

        public IEnumerable<int> FontSizeRadComboBox_GetItems()
        {
            return new int[] { 6, 7, 8, 9, 10, 11, 12, 14, 16, 18 };
        }

        public IEnumerable<string> FontWeightRadComboBox_GetItems()
        {
            return Enum.GetNames(typeof(FontWeight));
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
