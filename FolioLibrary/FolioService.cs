//using MARC4J.Net;
//using MARC4J.Net.MARC;
//using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
//using DataField = MARC4J.Net.MARC.DataField;

namespace FolioLibrary
{
    public static class StringExtensions
    {
        public static string ToAscii(this string value)
        {
            var e = Encoding.GetEncoding("us-ascii", new EncoderReplacementFallback(""), new DecoderReplacementFallback(""));
            return e.GetString(e.GetBytes(value));
        }
    }

    public class FolioService
    {
        private static Lazy<FolioService> instance = new Lazy<FolioService>(() => new FolioService());
        private readonly static TraceSource traceSource = new TraceSource("FolioLibrary", SourceLevels.All);

        public static FolioService Instance
        {
            get { return instance.Value; }
        }

        private FolioService() { }

        public static string Clean(string s)
        {
            if (s == null) return null;
            s = s.Trim();
            s = Regex.Replace(s, @"^\[", "", RegexOptions.Compiled);
            s = Regex.Replace(s, @"[/,\]\.:;=]$", "", RegexOptions.Compiled);
            s = s.Trim();
            if (s.Length == 0) s = null;
            return s;
        }

        public static string FormatCallNumber(string type, string value)
        {
            value = Trim(value);
            if (value == null) return null;
            if (type == /*"DDC"*/"Dewey Decimal classification")
            {
                var m = Regex.Match(value, @"(?<ClassificationDigits>\d+(\.\d+)?) (?<Cutters>[A-Z]*\d+)");
                var s2 = m.Groups["ClassificationDigits"].Value;
                value = m.Success ? $"{(s2.Length <= 6 ? s2 : s2.Replace(".", "\n."))}\n{m.Groups["Cutters"].Value}" : value;
            }
            else if (type == /*"8"*/"Other scheme") // Harvard/Yenching
            {
                var m = Regex.Match(value, @"(?<ClassificationLetters>[A-Z]*)(?<ClassificationDigits>\d+(\.\d+)?) (?<ClassificationDigitsAndLetters2>\d+(\.\d+)?[A-Z]?)");
                var s2 = m.Groups["ClassificationLetters"].Value;
                value = m.Success ? $"{(s2 != "" ? s2 + "\n" : null)}{m.Groups["ClassificationDigits"].Value}\n{m.Groups["ClassificationDigitsAndLetters2"].Value}" : value;
            }
            else if (type == /*"LCC"*/"Library of Congress classification" || type == null)
            {
                ////PK1730.16.A39519Z466 2012
                ////var m = Regex.Match(value, @"(?<ClassificationLetters>[A-Z]+) ?(?<ClassificationDigits>\d+(\.\d+)?) ?\.(?<Cutters>( ?[A-Z]+\d+[a-zA-Z]*)+)( (?<Year>.+))?");
                ////var m = Regex.Match(value, @"(?<ClassificationLetters>[A-Z]+) ?(?<ClassificationDigits>\d+(\.\d+)?) ?\.(?<Cutters>( ?[A-Z]+\d+[A-Z]*?)+)( (?<Year>.+))?", RegexOptions.IgnoreCase);
                //var m = Regex.Match(value, @"^(?<ClassificationLetters>[A-Z]+) ?(?<ClassificationDigits>\d+(\.\d+)?) ?\.(?<Cutters>( ?.*?))( (?<Year>\d+))?$", RegexOptions.IgnoreCase);
                //var cutters = m.Groups["Cutters"].Value.Replace(" ", "\n");
                //cutters = Regex.Replace(cutters, @"(\d)([A-Z])", "$1\n$2", RegexOptions.IgnoreCase);
                //var classificationLetters = Regex.Replace(m.Groups["ClassificationLetters"].Value, @"^XX", "XX\n");
                //var classificationDigits = Regex.Replace(m.Groups["ClassificationDigits"].Value, @"(\d)\.(\d)", "$1\n.$2");
                //value = m.Success ? $"{classificationLetters}\n{classificationDigits}\n.{cutters}{(m.Groups["Year"].Value != "" ? "\n" + m.Groups["Year"].Value.Replace(" ", "") : null)}" : value;

                //value = Regex.Replace(value, @"(\d) *\.(?!\d)", "$1\n.");
                value = Regex.Replace(value, @"(\d) *\.", "$1\n.");
                value = Regex.Replace(value, @"(?<!\.) +", "\n");
                value = Regex.Replace(value, @"(\d+)([A-Z]+)", "$1\n$2");
                value = Regex.Replace(value, @"^([A-Z]+)(\d+)", "$1\n$2");
                value = Regex.Replace(value, @"^XX", "XX\n");
            }
            return value;
        }

        public static string FormatCopyNumber(string value)
        {
            return value != null ? Regex.Replace(Regex.Replace(value.Trim(), @"c\. +", "c."), @"(.*)(c\.\d+[a-z]*)(.*)", "$1$3 $2").Trim().Replace(" ", "\n").Replace("-", "-\n") : null;
        }

        public static string FormatEnumeration(string value)
        {
            return value?.Trim().Replace(" ", "\n").Replace("-", "-\n");
        }

        public static string GetCollectionCode(string locationCode)
        {
            if (locationCode == null) return null;
            if (locationCode == "RecASR") return "Rec";
            if (locationCode == "GameASR") return "Games";
            if (locationCode == "GenHY") return "Gen";
            if (locationCode == "EckX") return "Eck";
            if (locationCode == "JRLASR") return "Gen";
            if (locationCode == "SciASR") return "Sci";
            if (locationCode == "SSAdX") return "SSAd";
            if (locationCode == "XClosedGen") return "Gen";
            if (locationCode == "XClosedCJK") return "CJK";
            return locationCode;
        }

        public Label GetBindingSlip(string barcode)
        {
            throw new NotImplementedException();

            //using (var fsc = new FolioServiceClient())
            //{
            //    //var i = fsc.Item1s.FirstOrDefault/*SingleOrDefault*/(i2 => i2.Barcode == barcode);
            //    dynamic i = null;

            //    if (i == null) return null;
            //    //var b = fsc.Bibs.Find(i.BibId);
            //    dynamic b = null;

            //    string title = null;
            //    string author = null;
            //    if (b != null)
            //    {
            //        XNamespace xn = "http://www.loc.gov/MARC21/slim";
            //        var xe = XDocument.Parse(b.Content).Element(xn + "collection").Element(xn + "record");
            //        title = Clean(Trim(string.Join(" ", xe.Descendants(xn + "subfield").Where(xe2 => (xe2.Attribute("code").Value == "a" || xe2.Attribute("code").Value == "n" || xe2.Attribute("code").Value == "p") && xe2.Parent.Attribute("tag").Value == "245").OrderBy(xe2 => xe2.Attribute("code").Value).Select(xe2 => /*Clean(*/xe2.Value/*)*/))));
            //        author = Trim(string.Join("", xe.Descendants(xn + "subfield").Where(xe2 => xe2.Attribute("code").Value == "a" && xe2.Parent.Attribute("tag").Value == "100").Select(xe2 => Clean(xe2.Value))));
            //        if (author != null) author = Regex.Replace(author, @",.*", "", RegexOptions.Compiled);
            //    }
            //    var l = new Label
            //    {
            //        Id = i.Barcode,
            //        Font = new Font
            //        {
            //            Family = "Verdana",
            //            Size = 16,
            //            Weight = FontWeight.Normal
            //        },
            //        Item = i,
            //        Orientation = Orientation.Portrait,
            //        Text = $"{(title != null ? title + "\n\n\n" : null)}{(author != null ? author + "\n\n\n" : null)}{(i.CallNumberPrefix != null ? i.CallNumberPrefix + "\n" : null)}{(i.CallNumber != null ? FormatCallNumber(i.CallNumberType, i.CallNumber) + "\n" : null)}{(i.Enumeration != null ? FormatEnumeration(i.Enumeration) + "\n" : null)}{(i.Chronology != null ? i.Chronology + "\n" : null)}{(i.CopyNumber != null ? FormatCopyNumber(i.CopyNumber) + "\n" : null)}{(i.LocationCode != null ? GetCollectionCode(i.LocationCode) + "\n" : null)}\n\n\n--- {(i.Barcode)} ---",
            //        Width = 200,
            //        Title = title,
            //        Author = author,
            //        CallNumber = Trim($"{(i.CallNumberPrefix != null ? i.CallNumberPrefix + "\n" : null)}{(i.CallNumber != null ? FormatCallNumber(i.CallNumberType, i.CallNumber) + "\n" : null)}{(i.Enumeration != null ? FormatEnumeration(i.Enumeration) + "\n" : null)}{(i.Chronology != null ? i.Chronology + "\n" : null)}{(i.CopyNumber != null ? FormatCopyNumber(i.CopyNumber) + "\n" : null)}"),
            //        CollectionCode = i.LocationCode != null ? GetCollectionCode(i.LocationCode) : null
            //    };
            //    return l;
            //}
        }

        public Label GetLabel(string barcode, Orientation? orientation = null)
        {
            using (var fsc = new FolioServiceContext())
            {
                var i = fsc.Item2s($"barcode == \"{barcode}\""/*, load: true*/).SingleOrDefault();
                if (i.EffectiveCallNumberTypeId != null) i.EffectiveCallNumberType = fsc.FindCallNumberType2(i.EffectiveCallNumberTypeId);
                if (i.EffectiveLocationId != null) i.EffectiveLocation = fsc.FindLocation2(i.EffectiveLocationId);
                if (i == null) return null;
                var ls = fsc.LocationSettings().SingleOrDefault(ls2 => ls2.LocationId == i.EffectiveLocationId);
                var s = fsc.FindSetting(ls?.SettingsId) ?? new Setting { FontFamily = "Courier New", FontSize = 11, FontWeight = FontWeight.Bold, Orientation = orientation ?? Orientation.Portrait };
                if (orientation != null) s.Orientation = orientation;
                var locationCode = i.EffectiveLocation?.Code != null ? GetCollectionCode(Regex.Replace(i.EffectiveLocation?.Code, ".+/", "")) : null;
                var l = new Label
                {
                    Id = i.Barcode,
                    Font = new Font
                    {
                        Family = s.FontFamily,
                        Size = s.FontSize,
                        Weight = s.FontWeight
                    },
                    Item = i,
                    Orientation = s.Orientation,
                    Text = $"{(i.EffectiveCallNumberPrefix != null ? i.EffectiveCallNumberPrefix + "\n" : null)}{(i.EffectiveCallNumber != null ? FormatCallNumber(i.EffectiveCallNumberType.Name, i.EffectiveCallNumber) + "\n" : null)}{(i.Enumeration != null ? FormatEnumeration(i.Enumeration) + "\n" : null)}{(i.Chronology != null ? i.Chronology + "\n" : null)}{(i.CopyNumber != null ? FormatCopyNumber(i.CopyNumber) + "\n" : null)}{(locationCode != null ? locationCode + "\n" : null)}{(false && locationCode == "Rec" ? string.Join(" ", i.ItemNotes.Select(@in => @in.Note)) + "\n" : null)}"
                };
                if (l.Orientation == Orientation.Landscape)
                {
                    l.Text = l.Text.Replace("\n", " ");
                }
                return l;
            }
        }

        public static string Trim(string s)
        {
            if (s == null) return null;
            s = s.Trim();
            if (s.Length == 0) s = null;
            return s;
        }

        public static string GetBibTitle(string content)
        {
            XNamespace xn = "http://www.loc.gov/MARC21/slim";
            var xe = XDocument.Parse(content).Element(xn + "collection").Element(xn + "record");
            return Trim(string.Join(" ", xe.Descendants(xn + "subfield").Where(xe2 => xe2.Attribute("code").Value == "a" && xe2.Parent.Attribute("tag").Value == "245").Select(xe2 => Clean(xe2.Value))));
        }

        public static string GetBibAuthor(string content)
        {
            XNamespace xn = "http://www.loc.gov/MARC21/slim";
            var xe = XDocument.Parse(content).Element(xn + "collection").Element(xn + "record");
            return Trim(string.Join("", xe.Descendants(xn + "subfield").Where(xe2 => xe2.Attribute("code").Value == "a" && (xe2.Parent.Attribute("tag").Value == "100" || xe2.Parent.Attribute("tag").Value == "110" || xe2.Parent.Attribute("tag").Value == "111")).Select(xe2 => Clean(xe2.Value))));
        }
    }
}
