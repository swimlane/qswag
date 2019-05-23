using System.Collections.Generic;

namespace QSwagWebApi.Models
{
    public enum ChartSortDirection
    {
        LabelAscending = 1,
        LabelDescending = 2,
        Custom = 3,
        ValueAscending = 4,
        ValueDescending = 5,
    }

    public class ChartSort
    {
        public ChartSortDirection DirectionD0 { get; set; }

        public List<ChartSortOption> EntriesD0 { get; set; }

        public ChartSortDirection DirectionD1 { get; set; }

        public List<ChartSortOption> EntriesD1 { get; set; }
    }

    public class ChartSortOption
    {
        public dynamic Name { get; set; }
    }
}
