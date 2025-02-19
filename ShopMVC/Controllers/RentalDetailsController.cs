using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopMVC.Data;
using ShopMVC.Models;

namespace ShopMVC.Controllers
{
    public class RentalDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RentalDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RentalDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RentalDetail.Include(r => r.Movie).Include(r => r.RentalHeader);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RentalDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalDetail = await _context.RentalDetail
                .Include(r => r.Movie)
                .Include(r => r.RentalHeader)
                .FirstOrDefaultAsync(m => m.RentalHeaderDetailId == id);
            if (rentalDetail == null)
            {
                return NotFound();
            }

            return View(rentalDetail);
        }

        // GET: RentalDetails/Create
        public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(_context.Movie, "MovieId", "Director");
            ViewData["RentalHeaderId"] = new SelectList(_context.RentalHeader, "RentalHeaderId", "RentalHeaderId");
            return View();
        }

        // POST: RentalDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RentalHeaderDetailId,RentalHeaderId,MovieId")] RentalDetail rentalDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rentalDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movie, "MovieId", "Director", rentalDetail.MovieId);
            ViewData["RentalHeaderId"] = new SelectList(_context.RentalHeader, "RentalHeaderId", "RentalHeaderId", rentalDetail.RentalHeaderId);
            return View(rentalDetail);
        }

        // GET: RentalDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalDetail = await _context.RentalDetail.FindAsync(id);
            if (rentalDetail == null)
            {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_context.Movie, "MovieId", "Director", rentalDetail.MovieId);
            ViewData["RentalHeaderId"] = new SelectList(_context.RentalHeader, "RentalHeaderId", "RentalHeaderId", rentalDetail.RentalHeaderId);
            return View(rentalDetail);
        }

        // POST: RentalDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RentalHeaderDetailId,RentalHeaderId,MovieId")] RentalDetail rentalDetail)
        {
            if (id != rentalDetail.RentalHeaderDetailId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rentalDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalDetailExists(rentalDetail.RentalHeaderDetailId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movie, "MovieId", "Director", rentalDetail.MovieId);
            ViewData["RentalHeaderId"] = new SelectList(_context.RentalHeader, "RentalHeaderId", "RentalHeaderId", rentalDetail.RentalHeaderId);
            return View(rentalDetail);
        }

        // GET: RentalDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalDetail = await _context.RentalDetail
                .Include(r => r.Movie)
                .Include(r => r.RentalHeader)
                .FirstOrDefaultAsync(m => m.RentalHeaderDetailId == id);
            if (rentalDetail == null)
            {
                return NotFound();
            }

            return View(rentalDetail);
        }

        // POST: RentalDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rentalDetail = await _context.RentalDetail.FindAsync(id);
            if (rentalDetail != null)
            {
                _context.RentalDetail.Remove(rentalDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalDetailExists(int id)
        {
            return _context.RentalDetail.Any(e => e.RentalHeaderDetailId == id);
        }
    }
}
