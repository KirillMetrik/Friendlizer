using Friendlizer.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Friendlizer.Services
{
    public class FileParser : IFileParser
    {
        public async Task<IEnumerable<Relation>> Parse(IFormFile file, long setId)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            using var reader = new StreamReader(file.OpenReadStream());
            var line = await reader.ReadLineAsync();
            var result = new List<Relation>();

            while (line != null)
            {
                var pair = line.Split(' ').ToArray();
                if (pair.Length != 2)
                {
                    throw new ArgumentException($"File contents are invalid, cannot parse line '{line}'!", "file");
                }

                result.Add(new Relation { FriendsSetId = setId, FirstPersonId = long.Parse(pair[0]), SecondPersonId = long.Parse(pair[1]) });
                line = await reader.ReadLineAsync();
            }
            return result;
        }
    }
}
