namespace SearchApp.Lib.Models
{
    public class SearchRequest
    {
        public string Keywords { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public int MaxNumber { get; set; } = 100;
    }
}
