using SearchApp.Lib.Models;
using System.Threading.Tasks;

namespace SearchApp.Lib.Utilities
{
    public interface ISearch
    {
        Task<SearchResponse> SearchAsync(SearchRequest request);
    }
}
