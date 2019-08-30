using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace FalunDagisMenyParser
{
    public static class MenuItemExtensions {
        public static MenuItem FromEntity(this MenuItemEntity item) =>
            new MenuItem
            {
                 DayOfWeek = item.DayOfWeek,
                 Menu = item.Menu,
                 Week = item.Week
            };

        public static MenuItemEntity ToEntity(this MenuItem item) =>
            new MenuItemEntity(item.Week.ToString(),item.DayOfWeek.ToString())
            {
                DayOfWeek = item.DayOfWeek,
                Menu = item.Menu,
                Week = item.Week
            };
    }

    public class MenuItemEntity : TableEntity
    {
        public MenuItemEntity(string key, string row)
        {
            this.PartitionKey = key;
            this.RowKey = row;
        }

        public MenuItemEntity() { }

        public string Menu { get; set; }
        public int Week { get; set; }
        public int DayOfWeek { get; set; }
    }

    public class MenuItem
    {
        public string Menu { get; set; }
        public int Week { get; set; }
        public int DayOfWeek { get; set; }
    }
}

