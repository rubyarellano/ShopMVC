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
    public class RentalHeadersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RentalHeadersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RentalHeaders
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RentalHeader
                .Include(rh => rh.Customer)
                .Include(rh => rh.RentalDetails)
                    .ThenInclude(rd => rd.Movie);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RentalHeaders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalHeader = await _context.RentalHeader
                .Include(rh => rh.Customer)
                .Include(rh => rh.RentalDetails)
                    .ThenInclude(rd => rd.Movie)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (rentalHeader == null)
            {
                return NotFound();
            }

            return View(rentalHeader);
        }

        // GET: RentalHeaders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId");
            ViewData["Movies"] = new SelectList(_context.Movie, "MovieId", "Title");
            return View();
        }

        // POST: RentalHeaders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RentalHeaderId,CustomerId,RentalDate,ReturnDate")] RentalHeader rentalHeader, List<int>? MovieIds)
        {
            if (ModelState.IsValid)
            {
                _context.RentalHeader.Add(rentalHeader);
                await _context.SaveChangesAsync();

                if (MovieIds != null && MovieIds.Count > 0)
                {
                    foreach (var movieId in MovieIds)
                    {
                        var rentalDetail = new RentalDetail
                        {
                            RentalHeaderId = rentalHeader.CustomerId,
                            MovieId = movieId,
                            Status = "Rented"
                        };
                        _context.RentalDetail.Add(rentalDetail);
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", rentalHeader.CustomerId);
            ViewData["Movies"] = new SelectList(_context.Movie, "MovieId", "Title");
            return View(rentalHeader);
        }

        // GET: RentalHeaders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalHeader = await _context.RentalHeader
                .Include(r => r.RentalDetails)
                .ThenInclude(d => d.Movie)
                .FirstOrDefaultAsync(r => r.CustomerId == id);

            if (rentalHeader == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", rentalHeader.CustomerId);
            var movieList = _context.Movie.ToList();
            var selectedMovies = rentalHeader.RentalDetails?.Select(d => d.MovieId).ToList() ?? new List<int>();

            ViewData["MovieList"] = new SelectList(movieList, "MovieId", "Title");
            ViewBag.SelectedMovies = rentalHeader.RentalDetails?.Select(d => new { d.MovieId, d.Movie.Title, d.Status }).ToList();
            return View(rentalHeader);
        }

        // POST: RentalHeaders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RentalHeaderId,CustomerId,RentalDate,ReturnDate")] RentalHeader rentalHeader, List<int> MovieIds, List<string> Statuses)
        {
            if (id != rentalHeader.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingRental = await _context.RentalHeader
                        .Include(r => r.RentalDetails)
                        .FirstOrDefaultAsync(r => r.CustomerId == id);

                    if (existingRental == null)
                    {
                        return NotFound();
                    }

                    // Update RentalHeader details
                    existingRental.CustomerId = rentalHeader.CustomerId;
                    existingRental.RentalDate = rentalHeader.RentalDate;
                    existingRental.ReturnDate = rentalHeader.ReturnDate;

                    // Remove old movie selections
                    _context.RentalDetail.RemoveRange(existingRental.RentalDetails);

                    // Add new movie selections with statuses
                    if (MovieIds != null && Statuses != null && MovieIds.Count == Statuses.Count)
                    {
                        for (int i = 0; i < MovieIds.Count; i++)
                        {
                            _context.RentalDetail.Add(new RentalDetail
                            {
                                RentalHeaderId = rentalHeader.CustomerId,
                                MovieId = MovieIds[i],
                                Status = Statuses[i]
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalHeaderExists(rentalHeader.CustomerId))
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

            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "CustomerId", rentalHeader.CustomerId);
            return View(rentalHeader);
        }


        // GET: RentalHeaders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rentalHeader = await _context.RentalHeader
                .Include(r => r.CustomerId)
                .FirstOrDefaultAsync(m => m.RentalHeaderId == id);
            if (rentalHeader == null)
            {
                return NotFound();
            }

            return View(rentalHeader);
        }

        // POST: RentalHeaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rentalHeader = await _context.RentalHeader.FindAsync(id);
            if (rentalHeader != null)
            {
                _context.RentalHeader.Remove(rentalHeader);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentalHeaderExists(int id)
        {
            return _context.RentalHeader.Any(e => e.CustomerId == id);
        }
    }
}
