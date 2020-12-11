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
    public class StudentCoursesController : Controller
    {
        private readonly WPFW_week14Context _context;

        public StudentCoursesController(WPFW_week14Context context)
        {
            _context = context;
        }

        // GET: StudentCourses
        public async Task<IActionResult> Index()
        {
            var wPFW_week14Context = _context.StudentCourse.Include(s => s.Course).Include(s => s.Student);
            return View(await wPFW_week14Context.ToListAsync());
        }

        // GET: StudentCourses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentCourse = await _context.StudentCourse
                .Include(s => s.Course)
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (studentCourse == null)
            {
                return NotFound();
            }

            return View(studentCourse);
        }

        // GET: StudentCourses/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = _context.Course;
            ViewData["StudentId"] = _context.Student;
            return View();
        }

        // POST: StudentCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,CourseId")] StudentCourse studentCourse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", studentCourse.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Id", studentCourse.StudentId);
            return View(studentCourse);
        }

        // GET: StudentCourses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentCourse = await _context.StudentCourse.FindAsync(id);
            if (studentCourse == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", studentCourse.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Id", studentCourse.StudentId);
            return View(studentCourse);
        }

        // POST: StudentCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,CourseId")] StudentCourse studentCourse)
        {
            if (id != studentCourse.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentCourseExists(studentCourse.CourseId))
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
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", studentCourse.CourseId);
            ViewData["StudentId"] = new SelectList(_context.Student, "Id", "Id", studentCourse.StudentId);
            return View(studentCourse);
        }

        // GET: StudentCourses/Delete/5
        public async Task<IActionResult> Delete(int? studentId, int? courseId)
        {
            if (studentId == null || courseId == null)
            {
                return NotFound();
            }

            var studentCourse = await _context.StudentCourse
                .Include(s => s.Course)
                .Include(s => s.Student)
                .Where(m => m.StudentId == studentId)
                .FirstOrDefaultAsync(m => m.CourseId == courseId);
            if (studentCourse == null)
            {
                return NotFound();
            }

            return View(studentCourse);
        }

        // POST: StudentCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int studentId, int courseId)
        {
            var studentCourse = _context.StudentCourse.Where(m => m.StudentId == studentId).FirstOrDefault(m => m.CourseId == courseId);
            _context.StudentCourse.Remove(studentCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentCourseExists(int id)
        {
            return _context.StudentCourse.Any(e => e.CourseId == id);
        }
    }
}
