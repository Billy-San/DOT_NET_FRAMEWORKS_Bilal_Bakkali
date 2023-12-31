﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DOT_NET_Examenproject.Data;
using DOT_NET_Examenproject.Models;
using System.Numerics;
using System.Security.Claims;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity;
using DOT_NET_Examenproject.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;



namespace DOT_NET_Examenproject.Controllers
{

    public class BedrijfsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;



        public BedrijfsController(AppDbContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Bedrijfs
        [Authorize]
        public async Task<IActionResult> Index(string OpzoekVeld)
        {

            if (HttpContext.User.IsInRole("SystemAdministrator"))
            {
                var bedrijven = from g in _context.Bedrijf
                                where g.IsDeleted == false
                                orderby g.Name
                                select g;


                if (!string.IsNullOrEmpty(OpzoekVeld))
                    bedrijven = from g in bedrijven
                                where g.Name.Contains(OpzoekVeld) && g.IsDeleted == false
                                orderby g.Name
                                select g;




                return View(await bedrijven.ToListAsync());
            }
            else if(HttpContext.User.IsInRole("User"))
            {
                string userIdd = _userManager.GetUserId(HttpContext.User);

                var FilBedrijven = from g in _context.Bedrijf
                                   where g.user_id == userIdd
                                   orderby g.Name
                                   select g;


                var bedrijven = from g in FilBedrijven
                                where g.IsDeleted == false
                                orderby g.Name
                                select g;


                if (!string.IsNullOrEmpty(OpzoekVeld))
                    bedrijven = from g in bedrijven
                                where g.Name.Contains(OpzoekVeld) && g.IsDeleted == false
                                orderby g.Name
                                select g;
                return View(await bedrijven.ToListAsync());
            }
            else
            {
                return View(null);
            }


            

            //  var Bedrijfs = _context.Bedrijf.Where(b => b.CreatedBy == userId);

            /*


            var bedrijven = from x in _context.Bedrijf
                            where x.UserId == HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value
                            select x;
            */

            
        }

        // GET: Bedrijfs/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bedrijf == null)
            {
                return NotFound();
            }

            var bedrijf = await _context.Bedrijf
                .FirstOrDefaultAsync(m => m.BedrijfId == id);
            if (bedrijf == null)
            {
                return NotFound();
            }

            return View(bedrijf);
        }

        // GET: Bedrijfs/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bedrijfs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BedrijfId,Name,NrTva,Adres,Email,NrTel")] Bedrijf bedrijf)
        {
            string userIdd = _userManager.GetUserId(HttpContext.User);

            // bedrijf.UserId = User.Identity.GetUserId();
            /* foreach (ModelState modelState in ViewData.ModelState.Values)
             {
                 foreach (ModelError error in modelState.Errors)
                 {
                     DoSomethingWith(error);
                 }
             }
            */
            if (ModelState.IsValid)
            {
                bedrijf.user_id = userIdd;
                _context.Add(bedrijf);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bedrijf);
        }

        // GET: Bedrijfs/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bedrijf == null)
            {
                return NotFound();
            }

            var bedrijf = await _context.Bedrijf.FindAsync(id);
            if (bedrijf == null)
            {
                return NotFound();
            }
            return View(bedrijf);
        }

        // POST: Bedrijfs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BedrijfId,Name,NrTva,Adres,Email,NrTel")] Bedrijf bedrijf)
        {
            string userIdd = _userManager.GetUserId(HttpContext.User);

            if (id != bedrijf.BedrijfId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bedrijf.user_id = userIdd;
                    _context.Update(bedrijf);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BedrijfExists(bedrijf.BedrijfId))
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
            return View(bedrijf);
        }

        // GET: Bedrijfs/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bedrijf == null)
            {
                return NotFound();
            }

            var bedrijf = await _context.Bedrijf
                .FirstOrDefaultAsync(m => m.BedrijfId == id);
            if (bedrijf == null)
            {
                return NotFound();
            }

            return View(bedrijf);
        }

        // POST: Bedrijfs/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bedrijf == null)
            {
                return Problem("Entity set 'DOT_NET_ExamenprojectContext.Bedrijf'  is null.");
            }
            var bedrijf = await _context.Bedrijf.FindAsync(id);
            if (bedrijf != null)
            {
                bedrijf.IsDeleted = true;
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BedrijfExists(int id)
        {
          return _context.Bedrijf.Any(e => e.BedrijfId == id);
        }
    }
}
