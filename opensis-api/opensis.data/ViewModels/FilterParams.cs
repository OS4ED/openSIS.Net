using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.ViewModels
{
    public class FilterParams
    {
        public string ColumnName { get; set; } = string.Empty;
        public string FilterValue { get; set; } = string.Empty;
        public string JoinCondition { get; set; } 
        public FilterOptions FilterOption { get; set; } = FilterOptions.Contains;
    }
}
