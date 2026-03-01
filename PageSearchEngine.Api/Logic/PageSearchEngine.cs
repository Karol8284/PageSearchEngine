namespace PageSearchEngine.Api.Logic
{
    public class PageSearchEngine
    {
        public Task<PageSearchResponse> Search(PageSearchRequest request)
        {
            var answer = new PageSearchResponse();
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.searchInPage)
            {
                foreach (var  in )
                {
                    
                }
            }
            if (request.searchInDescription)
            {

            }
            if (request.searchInTitle)
            {

            }

            //silnik wyboru V1.0
            return Task<new PageSearchResponse()>; 
        }
    }
}
