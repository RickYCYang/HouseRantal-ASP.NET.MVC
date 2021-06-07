using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using prjHouseWebAp.Models;

namespace prjHouseWebAp.ViewModels
{
    public class MemberHouseViewModel
    {
        public 地點名稱 House { get; set; }
        public 會員 Member { get; set; }
        public List<評語> Message { get; set; }
    }
}