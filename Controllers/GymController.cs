using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonuUygulamasi.Data;
using SporSalonuUygulamasi.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SporSalonuUygulamasi.Utility;

namespace SporSalonuUygulamasi.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class GymController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GymController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. LİSTELEME - [ActionName("List")] SİLİNDİ!
        public async Task<IActionResult> Index()
        {
            var gyms = await _context.Gyms.ToListAsync();
            ViewBag.Count = gyms.Count;
            return View(gyms);
        }

        // ... Diğer metotlar (Details, Create, Edit, Delete) aynı kalacak ...

        // 2. DETAYLAR
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var gym = await _context.Gyms.FirstOrDefaultAsync(m => m.Id == id);
            if (gym == null) return NotFound();
            return View(gym);
        }

        // 3. OLUŞTURMA (GET)
        public IActionResult Create()
        {
            return View();
        }

        // 4. OLUŞTURMA (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,WorkingHours")] Gym gym)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gym);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gym);
        }

        // 5. DÜZENLEME (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var gym = await _context.Gyms.FindAsync(id);
            if (gym == null) return NotFound();
            return View(gym);
        }

        // 6. DÜZENLEME (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,WorkingHours")] Gym gym)
        {
            if (id != gym.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gym);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymExists(gym.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gym);
        }

        // 7. SİLME (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var gym = await _context.Gyms.FirstOrDefaultAsync(m => m.Id == id);
            if (gym == null) return NotFound();
            return View(gym);
        }

        // 8. SİLME (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gym = await _context.Gyms.FindAsync(id);
            if (gym != null)
            {
                _context.Gyms.Remove(gym);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool GymExists(int id)
        {
            return _context.Gyms.Any(e => e.Id == id);
        }
    }
}