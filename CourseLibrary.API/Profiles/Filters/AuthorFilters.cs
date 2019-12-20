namespace CourseLibrary.API.Profiles.ResourceParameters
{
    public class AuthorFilters
    {
        public string MainCategory { get; set; }
        public string SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
