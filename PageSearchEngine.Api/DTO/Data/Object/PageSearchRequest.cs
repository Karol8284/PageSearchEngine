namespace PageSearchEngine.Api.DTO.Data.Object
{
    public class PageSearchRequest
    {
        public required string searchText { get; set; }
        public bool searchInTitle { get; set; } = true;
        public bool searchInDescription { get; set; } =  false;
        public bool searchInPage { get; set; } = false;
    }
}
