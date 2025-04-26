using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ProductSpecificationsParameters
    {
        public int? BrandId { get; set; }

        public int? TypeId { get; set; }

        public string? Sort { get; set; }

        private int _pageIndex=1;

        private int _pageSize=5;

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize= value; }
        }


        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }

    }
}
