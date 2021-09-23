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
                throw new ArgumentException("File cannot be null!");
            }

            using var reader = new StreamReader(file.OpenReadStream());
            var line = await reader.ReadLineAsync();
            var result = new List<Relation>();

            while (line != null)
            {
                var pair = line.Split(' ').Select(id => long.Parse(id)).ToArray();
                if (pair.Length != 2)
                {
                    throw new ArgumentException($"File contents are invalid, cannot parse line '{line}'!", "file");
                }

                result.Add(new Relation { FriendsSetId = setId, FirstPersonId = pair[0], SecondPersonId = pair[1] });
                line = await reader.ReadLineAsync();
            }
            return result;
        }
    }
}
