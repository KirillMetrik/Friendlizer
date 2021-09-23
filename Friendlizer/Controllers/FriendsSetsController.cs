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

        // PUT: api/FriendsSets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFriendsSet(long id, FriendsSet friendsSet)
        {
            if (id != friendsSet.ID)
            {
                return BadRequest();
            }

            _context.Entry(friendsSet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendsSetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
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

            var friendsSet = new FriendsSet();
            friendsSet.Filename = file.FileName;
            var newEntity = _context.FriendsSetItems.Add(friendsSet);
            var relations = await _fileParser.Parse(file, newEntity.Entity.ID);
            _context.Relations.AddRange(relations);

            await _context.SaveChangesAsync();
            return CreatedAtAction("GetFriendsSet", 
                new { id = newEntity.Entity.ID }, 
                new { id = newEntity.Entity.ID, filename = newEntity.Entity.Filename, imported = relations.Count() });
        }

        // DELETE: api/FriendsSets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFriendsSet(long id)
        {
            var friendsSet = await _context.FriendsSetItems.FindAsync(id);
            if (friendsSet == null)
            {
                return NotFound();
            }

            _context.FriendsSetItems.Remove(friendsSet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FriendsSetExists(long id)
        {
            return _context.FriendsSetItems.Any(e => e.ID == id);
        }
    }
}
