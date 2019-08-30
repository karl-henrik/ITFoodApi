using HtmlAgilityPack;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using System.Collections;

namespace FalunDagisMenyParser
{
    public static class HtmlNodeExtensions
    {
        public static IEnumerable<HtmlNode> GetElementWithClass(this HtmlNode node, string elementType, string className) =>
                node.Descendants(elementType).Where(x => x.Attributes.Contains("class") && x.Attributes["class"].Value.Contains(className));

        public static string GetJoinedInnerText(this HtmlNode node, string elementName, string className, string separator = "\r\n") =>
            string.Join(separator, node.GetElementWithClass(elementName, className)
                .Select(line => WebUtility.HtmlDecode(line.InnerText).Trim()));

        public static IList<MenuItem> ToMenuItems(this IEnumerable<HtmlNode> nodes,int weekNr, int dayOfWeek = 0) =>
            nodes.Select(item => new MenuItem
            {
                DayOfWeek = dayOfWeek++,
                Menu = item.GetJoinedInnerText("div", "meal-text"),
                Week = weekNr,
            }).ToList();
    }
}
