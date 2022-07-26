using Microsoft.AspNetCore.Mvc;

namespace movie_api.Models
{
    public class QueryParamsMovies : QueryParams
    {
        public QueryParamsMovies() 
        {
            title = "";
        }

        [FromQuery(Name = "title")]
        public string title { get; set; }

    }
}
