
using System.Text.Json.Serialization;
namespace codegen.Helpers
{
    public class Pagination<T>
    {
        public Pagination(List<T> queriedList, int currentPage, int pageSize)
        {
            QueriedList = queriedList;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalItems = queriedList.Count;
        }

        public List<T> List => QueriedList.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        public int CurrentPage { get; set; }
        public IEnumerable<int> PageIndices => TotalItems == 0 ? Enumerable.Empty<int>() : GetPageRange();
        public string PageSummary => TotalItems == 0 ? "No results found" : $"Showing {FirstShownEntry} to {LastShownEntry} of {TotalItems} {EntryLabel}";

        [JsonIgnore]
        public List<T> QueriedList { get; set; }
        [JsonIgnore]
        public string EntryLabel => TotalItems > 1 ? "entries" : "entry";
        [JsonIgnore]
        public int PageSize { get; set; }
        [JsonIgnore]
        public int TotalItems { get; set; }
        [JsonIgnore]
        public int PageCount => (int)Math.Ceiling((decimal)TotalItems / PageSize);
        [JsonIgnore]
        public int FirstShownEntry => (CurrentPage * PageSize) - PageSize + 1;
        [JsonIgnore]
        public int LastShownEntry => CurrentPage * PageSize > TotalItems ? TotalItems : CurrentPage * PageSize;

        private IEnumerable<int> GetPageRange()
        {
            List<int> range = new List<int>();
            int maxRangeSize = 10;

            // Start of List
            if (CurrentPage <= (int)Math.Ceiling((decimal)maxRangeSize / 2) || PageCount < maxRangeSize)
            {
                for (int i = 1; i <= maxRangeSize; i++)
                {
                    range.Add(i);
                    if (i == PageCount) break;
                }
            }
            // End of List
            else if (CurrentPage + (int)Math.Ceiling((decimal)maxRangeSize / 2) > PageCount)
            {
                for (int i = PageCount - maxRangeSize + 1; i <= PageCount; i++)
                {
                    range.Add(i);
                }
            }
            // Middle of list
            else
            {
                int startIndex = (int)Math.Floor((decimal)maxRangeSize / 2);
                if (maxRangeSize % 2 == 0) startIndex--;
                for (int i = CurrentPage - startIndex; i <= CurrentPage + (int)Math.Floor((decimal)maxRangeSize / 2); i++)
                {
                    range.Add(i);
                }
            }

            return range.ToArray();
        }
    }
}