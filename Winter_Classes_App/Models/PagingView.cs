using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Winter_Classes_App.Models
{
    namespace Paging.Models
    {
        public class PagingView
        {
            public IEnumerable<IPagingModel> PagingModel { get; set; }
            public int TotalPage { get; set; }
        }
    }
}
