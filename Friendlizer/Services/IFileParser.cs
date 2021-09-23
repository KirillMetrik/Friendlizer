using Friendlizer.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Friendlizer.Services
{
    public interface IFileParser
    {
        Task<IEnumerable<Relation>> Parse(IFormFile file, long setId);
    }
}
