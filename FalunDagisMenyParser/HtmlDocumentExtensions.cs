using HtmlAgilityPack;
using System.Collections.Generic;

namespace FalunDagisMenyParser
{
    public static class HtmlDocumentExtensions
    {
        public static int GetWeekNumber(this HtmlDocument doc) =>
            doc.GetElementbyId("WeekPageWeekNo").GetAttributeValue("value", 0);

        public static IEnumerable<HtmlNode> GetMenuItemNodes (this HtmlDocument doc) =>
            doc.DocumentNode.GetElementWithClass("div", "menu-text");

        public static HtmlDocument ToHtmlDocument(this string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            return doc;
        }
    }
}
