using Microsoft.AspNetCore.Mvc;

namespace movie_api.Models
{
    public class QueryParams
    {
        public QueryParams() 
        {
            limit   = 10;
            page    = 1;
        }

        [FromQuery(Name = "page")]
        public int page { get; set; }

        [FromQuery(Name = "limit")]
        public int limit { get; set; }

    }
}
