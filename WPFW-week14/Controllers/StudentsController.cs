using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WPFW_week14.Data;
using WPFW_week14.Models;

namespace WPFW_week14.Controllers
{
    public class StudentsController : Controller
    {
        private readonly WPFW_week14Context _context;

        public StudentsController(WPFW_week14Context context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string sortType, string sortByOrder, string filter)
        {
            IQueryable<Student> students = Sort(sortType, sortByOrder);
            students = FilterByUserInput(students, filter);
            return View(await students.ToListAsync());
        }

        private IQueryable<Student> FilterByUserInput(IQueryable<Student> students, string filter)
        {
            ViewData["Filter"] = filter;

            if (!String.IsNullOrEmpty(filter)) return students.Where(s => s.Name.Contains(filter));
            return students;
        }

        private IQueryable<Student> Sort(string sortType, string sortByOrder)
        {
            IQueryable<Student> students = _context.Student;

            ViewData["SortType"] = sortType;
            ViewData["SortByOrder"] = sortByOrder;

            switch (sortType)
            {
                case "Name": students = SortByName(students, sortByOrder); break;
                case "Length": students = SortByLength(students, sortByOrder); break;
                default: students = SortById(students, sortByOrder); break;
            }

            return students;
        }

        private IQueryable<Student> SortById(IQueryable<Student> students, string sortByOrder)
        {
            if (sortByOrder == "Descending") return students.OrderByDescending(s => s.Id);
            return students.OrderBy(s => s.Id);
        }

        private IQueryable<Student> SortByName(IQueryable<Student> students, string sortByOrder)
        {
            if (sortByOrder == "Descending") return students.OrderByDescending(s => s.Name);
            return students.OrderBy(s => s.Name);
        }

        private IQueryable<Student> SortByLength(IQueryable<Student> students, string sortByOrder)
        {
            if (sortByOrder == "Descending") return students.OrderByDescending(s => s.Length);
            return students.OrderBy(s => s.Length);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Length")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Length")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }
    }
}
