using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Friendlizer.Models
{
    public class Relation
    {
        public long FriendsSetId { get; set; }

        public long FirstPersonId { get; set; }

        public long SecondPersonId { get; set; }
    }
}
