﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurfProgressAPI.Data;
using SurfProgressAPI.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfProgressAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurfboardController : ControllerBase
    {
        private readonly SurfProgressDbContext _db;

        public SurfboardController(SurfProgressDbContext db)
        {
            _db = db;
        }

        // POST - CREATE: api/Surfboard
        [HttpPost]
        public async Task<ActionResult<Surfboard>> PostSurfboard(Surfboard surfboard)
        {
            _db.Surfboards.Add(surfboard);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetSurfboard", new { surfboardId = surfboard.SurfboardId }, surfboard);
        }

        // POST - CREATE: api/Surfboard/range
        [HttpPost("range")]
        public async Task<ActionResult<List<Surfboard>>> PostSurfboards(List<Surfboard> surfboards)
        {
            _db.Surfboards.AddRange(surfboards);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetSurfboard", surfboards);
        }

        // GET - READ: api/Surfboard
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Surfboard>>> GetSurfboard()
        {
            return await _db.Surfboards.ToListAsync();
        }

        // GET - READ: api/Surfboard/five-ten-fish
        [HttpGet("{id}")]
        public async Task<ActionResult<Surfboard>> GetSurfboard(int id)
        {
            Surfboard surfboard = await _db.Surfboards.FindAsync(id);

            if (surfboard == null)
            {
                return NotFound();
            }

            return surfboard;
        }

        // PUT - UPDATE: api/Surfboard/five-ten-fish
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSurfboard(int id, Surfboard surfboard)
        {
            if (id != surfboard.SurfboardId)
            {
                return BadRequest();
            }

            _db.Entry(surfboard).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
               if (!SurfboardExists(id))
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

        // DELETE: api/Surfboard/five-ten-fish
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSurfboard(int id)
        {
            Surfboard surfboard = await _db.Surfboards.FindAsync(id);
            if (surfboard == null)
            {
                return NotFound();
            }

            _db.Surfboards.Remove(surfboard);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        private bool SurfboardExists(int id)
        {
            return _db.Surfboards.Any(s => s.SurfboardId == id);
        }

    }
}
