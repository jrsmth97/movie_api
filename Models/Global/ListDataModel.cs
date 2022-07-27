using System;
using System.Collections.Generic;

namespace movie_api.Models
{
    public class MovieListData : QueryParams
    {
        public int total_data { get; set; }

        public Object search_query { get; set; }
        
        public IList<Movies> movies { get; set; }

    }
}
