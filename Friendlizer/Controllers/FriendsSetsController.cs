using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Friendlizer;
using Friendlizer.Models;
using System.IO;
using Friendlizer.Services;
using Friendlizer.Controllers.ResultModels;

namespace Friendlizer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsSetsController : ControllerBase
    {
        private readonly FriendsDbContext _context;
        private readonly IFileParser _fileParser;

        public FriendsSetsController(FriendsDbContext context, IFileParser fileParser)
        {
            _context = context;
            _fileParser = fileParser;
        }

        // GET: api/FriendsSets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FriendsSet>>> GetFriendsSetItems()
        {
            return await _context.FriendsSetItems.ToListAsync();
        }

        // GET: api/FriendsSets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FriendsSet>> GetFriendsSet(long id)
        {
            var friendsSet = await _context.FriendsSetItems.FindAsync(id);

            if (friendsSet == null)
            {
                return NotFound();
            }

            return friendsSet;
        }

        [HttpGet("{id}/stats")]
        public async Task<ActionResult<Stats>> GetStats(long id)
        {
            if (!FriendsSetExists(id))
            {
                return NotFound();
            }

            var res = new Stats();
            res.TotalUsers = await (from r in _context.Relations where r.FriendsSetId == id select r.FirstPersonId).Union(
                from r in _context.Relations where r.FriendsSetId==id select r.SecondPersonId).CountAsync();

            var counts = from r in _context.Relations
                        where r.FriendsSetId == id
                        group r by r.FirstPersonId into grp
                        select grp.Count();
            res.AvgFriendsCount = (long)counts.Average();
            return res;
        }

        [HttpGet("{setId}/relations")]
        public async Task<ActionResult<Relation[]>> GetRelations(long setId)
        {
            if (!FriendsSetExists(setId))
            {
                return NotFound();
            }

            return await (from r in _context.Relations where r.FriendsSetId == setId select r).ToArrayAsync();
        }

        // POST: api/FriendsSets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult<FriendsSet>> PostFriendsSet(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("File is required!");
            }
            if ((from f in _context.FriendsSetItems
                 where f.Filename == file.FileName
                 select f).Count() != 0)
            {
                return BadRequest("This file was already imported!");
            }

            var friendsSet = new FriendsSet();
            friendsSet.Filename = file.FileName;
            var newEntity = _context.FriendsSetItems.Add(friendsSet);
            var relations = await _fileParser.Parse(file, newEntity.Entity.ID);
            _context.Relations.AddRange(relations);

            await _context.SaveChangesAsync();
            return CreatedAtAction("GetFriendsSet", 
                new { id = newEntity.Entity.ID }, 
                new FriendsSetImportResult { id = newEntity.Entity.ID, filename = newEntity.Entity.Filename, imported = relations.Count() });
        }

        private bool FriendsSetExists(long id)
        {
            return _context.FriendsSetItems.Any(e => e.ID == id);
        }
    }
}
