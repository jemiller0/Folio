using FolioLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace FolioWebApplication.Settings
{
    public partial class Edit : System.Web.UI.Page
    {
        private readonly FolioServiceContext folioServiceContext = FolioServiceContextPool.GetFolioServiceContext();
        private readonly static TraceSource traceSource = new TraceSource("FolioWebApplication", SourceLevels.All);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["SettingsPermission"] == null)
            {
                Response.StatusCode = 401;
                Response.End();
            }
            if (!IsPostBack) DataBind();
        }

        protected void SettingFormView_DataBinding(object sender, EventArgs e)
        {
            var id = Request.QueryString["Id"] != null ? (Guid?)Guid.Parse(Request.QueryString["Id"]) : null;
            var s = id == null && (string)Session["SettingsPermission"] == "Edit" ? new Setting { Enabled = true } : folioServiceContext.FindSetting(id, true);
            if (s == null) Response.Redirect("Default.aspx");
            SettingFormView.DataSource = new[] { s };
            Title = $"Setting {s.Name}";
        }

        protected void OrientationRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = Enum.GetNames(typeof(FolioLibrary.Orientation)).OrderBy(s => s).ToArray();
        }

        protected void FontFamilyRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = new string[] { "Agency FB", "Algerian", "Arial", "Arial Black", "Arial Narrow", "Arial Rounded MT Bold", "Bahnschrift", "Bahnschrift Condensed", "Bahnschrift Light", "Bahnschrift Light Condensed", "Bahnschrift Light SemiCondensed", "Bahnschrift SemiBold", "Bahnschrift SemiBold Condensed", "Bahnschrift SemiBold SemiConden", "Bahnschrift SemiCondensed", "Bahnschrift SemiLight", "Bahnschrift SemiLight Condensed", "Bahnschrift SemiLight SemiConde", "Baskerville Old Face", "Bauhaus 93", "Bell MT", "Berlin Sans FB", "Berlin Sans FB Demi", "Bernard MT Condensed", "Blackadder ITC", "Bodoni MT", "Bodoni MT Black", "Bodoni MT Condensed", "Bodoni MT Poster Compressed", "Book Antiqua", "Bookman Old Style", "Bookshelf Symbol 7", "Bradley Hand ITC", "Britannic Bold", "Broadway", "Brush Script MT", "Calibri", "Calibri Light", "Californian FB", "Calisto MT", "Cambria", "Cambria Math", "Candara", "Candara Light", "Cascadia Code", "Cascadia Code ExtraLight", "Cascadia Code Light", "Cascadia Code SemiBold", "Cascadia Code SemiLight", "Cascadia Mono", "Cascadia Mono ExtraLight", "Cascadia Mono Light", "Cascadia Mono SemiBold", "Cascadia Mono SemiLight", "Castellar", "Centaur", "Century", "Century Gothic", "Century Schoolbook", "Chiller", "Colonna MT", "Comic Sans MS", "Consolas", "Constantia", "Cooper Black", "Copperplate Gothic Bold", "Copperplate Gothic Light", "Corbel", "Corbel Light", "Courier New", "Curlz MT", "Dubai", "Dubai Light", "Dubai Medium", "Ebrima", "Edwardian Script ITC", "Elephant", "Engravers MT", "Eras Bold ITC", "Eras Demi ITC", "Eras Light ITC", "Eras Medium ITC", "Felix Titling", "Footlight MT Light", "Forte", "Franklin Gothic Book", "Franklin Gothic Demi", "Franklin Gothic Demi Cond", "Franklin Gothic Heavy", "Franklin Gothic Medium", "Franklin Gothic Medium Cond", "Freestyle Script", "French Script MT", "Gabriola", "Gadugi", "Garamond", "Georgia", "Gigi", "Gill Sans MT", "Gill Sans MT Condensed", "Gill Sans MT Ext Condensed Bold", "Gill Sans Ultra Bold", "Gill Sans Ultra Bold Condensed", "Gloucester MT Extra Condensed", "Goudy Old Style", "Goudy Stout", "Haettenschweiler", "Harlow Solid Italic", "Harrington", "High Tower Text", "Impact", "Imprint MT Shadow", "Informal Roman", "Ink Free", "Javanese Text", "Jokerman", "Juice ITC", "Kristen ITC", "Kunstler Script", "Leelawadee", "Leelawadee UI", "Leelawadee UI Semilight", "Lucida Bright", "Lucida Calligraphy", "Lucida Console", "Lucida Fax", "Lucida Handwriting", "Lucida Sans", "Lucida Sans Typewriter", "Lucida Sans Unicode", "Magneto", "Maiandra GD", "Malgun Gothic", "Malgun Gothic Semilight", "Marlett", "Matura MT Script Capitals", "Microsoft Himalaya", "Microsoft JhengHei", "Microsoft JhengHei Light", "Microsoft JhengHei UI", "Microsoft JhengHei UI Light", "Microsoft New Tai Lue", "Microsoft PhagsPa", "Microsoft Sans Serif", "Microsoft Tai Le", "Microsoft Uighur", "Microsoft YaHei", "Microsoft YaHei Light", "Microsoft YaHei UI", "Microsoft YaHei UI Light", "Microsoft Yi Baiti", "MingLiU-ExtB", "MingLiU_HKSCS-ExtB", "MingLiU_MSCS-ExtB", "Mistral", "Modern No. 20", "Mongolian Baiti", "Monotype Corsiva", "MS Gothic", "MS Outlook", "MS PGothic", "MS Reference Sans Serif", "MS Reference Specialty", "MS UI Gothic", "MT Extra", "MV Boli", "Myanmar Text", "Niagara Engraved", "Niagara Solid", "Nirmala Text", "Nirmala Text Semilight", "Nirmala UI", "Nirmala UI Semilight", "NSimSun", "OCR A Extended", "Old English Text MT", "Onyx", "Palace Script MT", "Palatino Linotype", "Papyrus", "Parchment", "Perpetua", "Perpetua Titling MT", "Playbill", "PMingLiU-ExtB", "Poor Richard", "Pristina", "Rage Italic", "Ravie", "Rockwell", "Rockwell Condensed", "Rockwell Extra Bold", "Sans Serif Collection", "Script MT Bold", "Segoe Fluent Icons", "Segoe MDL2 Assets", "Segoe Print", "Segoe Script", "Segoe UI", "Segoe UI Black", "Segoe UI Emoji", "Segoe UI Historic", "Segoe UI Light", "Segoe UI Semibold", "Segoe UI Semilight", "Segoe UI Symbol", "Segoe UI Variable Display", "Segoe UI Variable Display Light", "Segoe UI Variable Display Semib", "Segoe UI Variable Display Semil", "Segoe UI Variable Small", "Segoe UI Variable Small Light", "Segoe UI Variable Small Semibol", "Segoe UI Variable Small Semilig", "Segoe UI Variable Text", "Segoe UI Variable Text Light", "Segoe UI Variable Text Semibold", "Segoe UI Variable Text Semiligh", "Showcard Gothic", "SimSun", "SimSun-ExtB", "SimSun-ExtG", "Sitka Banner", "Sitka Banner Semibold", "Sitka Display", "Sitka Display Semibold", "Sitka Heading", "Sitka Heading Semibold", "Sitka Small", "Sitka Small Semibold", "Sitka Subheading", "Sitka Subheading Semibold", "Sitka Text", "Sitka Text Semibold", "Snap ITC", "Stencil", "Sylfaen", "Symbol", "Tahoma", "Tempus Sans ITC", "Times New Roman", "Trebuchet MS", "Tw Cen MT", "Tw Cen MT Condensed", "Tw Cen MT Condensed Extra Bold", "Ubuntu Mono", "Ubuntu Mono Medium", "Verdana", "Viner Hand ITC", "Vivaldi", "Vladimir Script", "Webdings", "Wide Latin", "Wingdings", "Wingdings 2", "Wingdings 3", "Yu Gothic", "Yu Gothic Light", "Yu Gothic Medium", "Yu Gothic UI", "Yu Gothic UI Light", "Yu Gothic UI Semibold", "Yu Gothic UI Semilight" };
        }

        protected void FontWeightRadComboBox_DataBinding(object sender, EventArgs e)
        {
            var rcb = (RadComboBox)sender;
            rcb.DataSource = Enum.GetNames(typeof(FontWeight)).OrderBy(s => s).ToArray();
        }

        protected void SettingFormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            var id = (Guid?)SettingFormView.DataKey.Value;
            var s = id != null ? folioServiceContext.FindSetting(id) : new Setting { Id = Guid.NewGuid(), CreationTime = DateTime.Now, CreationUserId = (Guid?)Session["UserId"] };
            s.Name = Global.Trim((string)e.NewValues["Name"]);
            s.Orientation = (FolioLibrary.Orientation)Enum.Parse(typeof(FolioLibrary.Orientation), (string)e.NewValues["Orientation"]);
            s.FontFamily = Global.Trim((string)e.NewValues["FontFamily"]);
            s.FontSize = (int)e.NewValues["FontSize"];
            s.FontWeight = (FontWeight)Enum.Parse(typeof(FontWeight), (string)e.NewValues["FontWeight"]);
            s.Enabled = (bool?)e.NewValues["Enabled"];
            s.LastWriteTime = DateTime.Now;
            s.LastWriteUserId = (Guid?)Session["UserId"];
            var vr = Setting.ValidateSetting(s, new ValidationContext(folioServiceContext));
            if (vr != null)
            {
                var cv = (CustomValidator)SettingFormView.FindControl("SettingCustomValidator");
                cv.IsValid = false;
                cv.ErrorMessage = vr.ErrorMessage;
                e.Cancel = true;
                return;
            }
            if (id == null) folioServiceContext.Insert(s); else folioServiceContext.Update(s);
            if (id == null) Response.Redirect($"Edit.aspx?Id={s.Id}"); else Response.Redirect("Default.aspx");
        }

        protected void SettingFormView_ItemCommand(object sender, FormViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel") Response.Redirect("Default.aspx");
        }

        protected void SettingFormView_ItemDeleting(object sender, FormViewDeleteEventArgs e)
        {
            var id = (Guid?)SettingFormView.DataKey.Value;
            try
            {
                if (folioServiceContext.LocationSettings().Any(s => s.SettingsId == id)) throw new Exception("Setting cannot be deleted because it is being referenced by a location setting");
                folioServiceContext.DeleteSetting(id);
                Response.Redirect("Default.aspx");
            }
            catch (Exception)
            {
                var cv = (CustomValidator)((FormView)sender).FindControl("DeleteCustomValidator");
                cv.IsValid = false;
            }
        }

        protected void LocationSettingsRadGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (Session["LocationSettingsPermission"] == null) return;
            var id = (Guid?)SettingFormView.DataKey.Value;
            if (id == null) return;
            var l = folioServiceContext.LocationSettings(load: true).Where(ls => ls.SettingsId == id).ToArray();
            LocationSettingsRadGrid.DataSource = l;
            LocationSettingsRadGrid.AllowFilteringByColumn = l.Count() > 10;
            LocationSettingsPanel.Visible = SettingFormView.DataKey.Value != null && ((string)Session["LocationSettingsPermission"] == "Edit" || Session["LocationSettingsPermission"] != null && l.Any());
        }

        public override void Dispose()
        {
            folioServiceContext.Dispose();
            base.Dispose();
        }
    }
}
