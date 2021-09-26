using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Friendlizer.Controllers
{
    public class FriendsSetImportResult
    {
        public long id { get; set; }
        public string filename { get; set; }
        public long imported { get; set; }
    }
}
