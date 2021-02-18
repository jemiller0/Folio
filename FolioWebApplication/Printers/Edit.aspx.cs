using FolioLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Printing;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace FolioWebApplication.Printers
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = new FolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["PrintersPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void PrinterFormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var p = id == null && (string)Session["PrintersPermission"] == "Edit" ? new Printer { Enabled = true } : folioServiceContext.FindPrinter(id, true);
            if (p == null) Response.Redirect("Default.aspx");
            PrinterFormView.DataSource = new[] { p };
            Title = $"Printer {p.ComputerName}";
        }

        protected void ComputerNameRadTextBox_TextChanged(object sender, EventArgs e)
        {
            var rtb = (RadTextBox)sender;
            var rcb = (RadComboBox)rtb.NamingContainer.FindControl("NameRadComboBox");
            try
            {
                rcb.Items.Clear();
                rcb.Items.Add("");
                using (var ps = new PrintServer($@"\\{rtb.Text}")) foreach (var s in ps.GetPrintQueues().Cast<PrintQueue>().Where(pq => pq.Name.StartsWith("ZDesigner") || pq.Name.StartsWith("Smart Label Printer") || pq.Name.StartsWith("EPSON")).Select(pq => pq.Name)) rcb.Items.Add(new RadComboBoxItem(s, s));
                rcb.SelectedIndex = rcb.Items.Count > 1 ? 1 : 0;
                rcb.Text = rcb.SelectedValue;
                NameRadComboBox_SelectedIndexChanged(rcb, null);
            }
            catch (Exception)
            {
                var cv = (CustomValidator)rtb.NamingContainer.FindControl("ComputerNameCustomValidator");
                cv.IsValid = false;
            }
        }

        protected void ComputerNameCustomValidator_ServerValidate(object sender, ServerValidateEventArgs args)
        {
            var vr = Printer.ValidateComputerName(args.Value);
            if (vr != null)
            {
                args.IsValid = false;
                var cv = (CustomValidator)sender;
                cv.ErrorMessage = vr.ErrorMessage;
            }
        }

        protected void NameRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            var p = (Printer)PrinterFormView.DataItem;
            using (var ps = new PrintServer($@"\\{p.ComputerName}"))
            {
                var l = ps.GetPrintQueues().Cast<PrintQueue>().Where(pq => pq.Name.StartsWith("ZDesigner") || pq.Name.StartsWith("Smart Label Printer") || pq.Name.StartsWith("EPSON")).Select(pq => pq.Name);
                rcb.DataSource = l;
                if (!l.Contains(p.Name)) p.Name = null;
            }
        }

        protected void NameRadComboBox_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            var rcb = (RadComboBox)sender;
            var leftRadNumericTextBox = (RadNumericTextBox)rcb.NamingContainer.FindControl("LeftRadNumericTextBox");
            var topRadNumericTextBox = (RadNumericTextBox)rcb.NamingContainer.FindControl("TopRadNumericTextBox");
            var widthRadNumericTextBox = (RadNumericTextBox)rcb.NamingContainer.FindControl("WidthRadNumericTextBox");
            var heightRadNumericTextBox = (RadNumericTextBox)rcb.NamingContainer.FindControl("HeightRadNumericTextBox");
            if (rcb.Text.StartsWith("ZDesigner"))
            {
                leftRadNumericTextBox.Value = 6;
                topRadNumericTextBox.Value = 15;
                widthRadNumericTextBox.Value = 125;
                heightRadNumericTextBox.Value = 162;
            }
            else if (rcb.Text.StartsWith("Smart Label Printer"))
            {
                leftRadNumericTextBox.Value = 0;
                topRadNumericTextBox.Value = 20;
                widthRadNumericTextBox.Value = 350;
                heightRadNumericTextBox.Value = 112;
            }
            else if (rcb.Text.StartsWith("EPSON"))
            {
                leftRadNumericTextBox.Value = 0;
                topRadNumericTextBox.Value = 0;
                widthRadNumericTextBox.Value = 325;
                heightRadNumericTextBox.Value = 500;
            }
            else
            {
                widthRadNumericTextBox.Value = heightRadNumericTextBox.Value = null;
            }
        }

        protected void PrinterFormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            var id = (Guid?)PrinterFormView.DataKey.Value;
            var p = id != null ? folioServiceContext.FindPrinter(id) : new Printer { Id = Guid.NewGuid(), CreationTime = DateTime.Now, CreationUserId = (Guid?)Session["UserId"] };
            p.ComputerName = Global.Trim((string)e.NewValues["ComputerName"]);
            p.Name = Global.Trim((string)e.NewValues["Name"]);
            p.Left = (int)e.NewValues["Left"];
            p.Top = (int)e.NewValues["Top"];
            p.Width = (int?)e.NewValues["Width"];
            p.Height = (int?)e.NewValues["Height"];
            p.Enabled = (bool)e.NewValues["Enabled"];
            p.LastWriteTime = DateTime.Now;
            p.LastWriteUserId = (Guid?)Session["UserId"];
            var vr = Printer.ValidatePrinter(p, new ValidationContext(folioServiceContext));
            if (vr != null)
            {
                var cv = (CustomValidator)PrinterFormView.FindControl("PrinterCustomValidator");
                cv.IsValid = false;
                cv.ErrorMessage = vr.ErrorMessage;
                e.Cancel = true;
                return;
            }
            if (id == null) folioServiceContext.Insert(p); else folioServiceContext.Update(p);
            if (id == null) Response.Redirect($"Edit.aspx?Id={p.Id}"); else Response.Redirect("Default.aspx");
        }

        protected void PrinterFormView_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel") Response.Redirect("Default.aspx");
        }

        protected void PrinterFormView_ItemDeleting(object sender, FormViewDeleteEventArgs e)
        {
            var id = (Guid?)PrinterFormView.DataKey.Value;
            try
            {
                folioServiceContext.DeletePrinter(id);
                Response.Redirect("Default.aspx");
            }
            catch (Exception)
            {
                var cv = (CustomValidator)((FormView)sender).FindControl("DeleteCustomValidator");
                cv.IsValid = false;
            }
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
