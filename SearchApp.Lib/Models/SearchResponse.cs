using System.Collections.Generic;

namespace SearchApp.Lib.Models
{
    public class SearchResponse
    {
        public int HighestAppearance { get; set; }
        public int NumberOfAppearance { get; set; }
        public List<SearchResultItem>? Results { get; set; } = null;
    }
}
