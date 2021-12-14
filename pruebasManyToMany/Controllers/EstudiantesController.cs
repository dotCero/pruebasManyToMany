using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pruebasManyToMany.ContextSetting;
using pruebasManyToMany.Models;

namespace pruebasManyToMany.Controllers
{
    public class EstudiantesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstudiantesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Estudiantes
        public async Task<IActionResult> Index()
        {

            var viewModel = new EstudiantesIndexData();
            viewModel.Estudiantes = await _context.Estudiantes.Include(i => i.EstudianteAsignatura).ThenInclude(a => a.Asignatura).AsNoTracking().ToListAsync();
            //var estudiantes = _context.Estudiantes.Include(ea => ea.EstudianteAsignatura).AsNoTracking();
            return View(viewModel);
        }

        // GET: Estudiantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var viewModel = new EstudiantesIndexData();
            viewModel.Estudiantes = await _context.Estudiantes.Include(i => i.EstudianteAsignatura).ThenInclude(a => a.Asignatura).AsNoTracking().ToListAsync();
            viewModel.Estudiante = viewModel.Estudiantes.Where(e => e.Id == id.Value).Single();
            viewModel.Asignaturas = viewModel.Estudiante.EstudianteAsignatura.Select(a => a.Asignatura);
            /*
            var estudiante = await _context.Estudiantes
                .FirstOrDefaultAsync(m => m.Id == id);
            */
            if (viewModel.Estudiante == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // GET: Estudiantes/Create
        public IActionResult Create()
        {
            var estudiante = new Estudiante();
            estudiante.EstudianteAsignatura = new List<EstudianteAsignatura>();
            PopulateListAssignedAsignatura(estudiante);
            return View();
        }

        private void PopulateListAssignedAsignatura(Estudiante estudiante)
        {
            var allAsignaturas = _context.Asignatura;
            var estudianteAsignaturas = new HashSet<int>(estudiante.EstudianteAsignatura.Select(a => a.AsignaturaId));
            var viewModel = new List<AssignedAsignaturaData>();
            foreach(var asignatura in allAsignaturas)
            {
                viewModel.Add(new AssignedAsignaturaData { AsignaturaId = asignatura.Id, Nombre = asignatura.Nombre, Asignado = estudianteAsignaturas.Contains(asignatura.Id) });
            }
            ViewData["Asignaturas"] = viewModel;
        }

        // POST: Estudiantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombres,Apellido1,Apellido2")] Estudiante estudiante, string[] selectedAsignaturas)
        {
            if(selectedAsignaturas != null)
            {
                estudiante.EstudianteAsignatura = new List<EstudianteAsignatura>();
                foreach(var asignatura in selectedAsignaturas)
                {
                    var asignaturaAAgregar = new EstudianteAsignatura { EstudianteId = estudiante.Id, AsignaturaId = int.Parse(asignatura) };
                    estudiante.EstudianteAsignatura.Add(asignaturaAAgregar);
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(estudiante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateListAssignedAsignatura(estudiante);
            return View(estudiante);
        }

        // GET: Estudiantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var estudiante = await _context.Estudiantes
                .Include(ea => ea.EstudianteAsignatura)
                .ThenInclude(a => a.Asignatura)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id.Value);
            //var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null)
            {
                return NotFound();
            }
            PopulateListAssignedAsignatura(estudiante);
            return View(estudiante);
        }

        // POST: Estudiantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedAsignaturas)
        {
            /*
            if (id != estudiante.Id)
            {
                return NotFound();
            }
            */
            if(id == null)
            {
                return NotFound();
            }

            var estudianteAEditar = await _context.Estudiantes
                .Include(e => e.EstudianteAsignatura)
                .ThenInclude(ea => ea.Asignatura)
                .FirstOrDefaultAsync(e => e.Id == id);

            if(await TryUpdateModelAsync<Estudiante>(estudianteAEditar,"", e => e.Nombres, e => e.Apellido1, e => e.Apellido2))
            {
                UpdateEstudianteAsignaturas(selectedAsignaturas, estudianteAEditar);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("error", "desc error");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateEstudianteAsignaturas(selectedAsignaturas, estudianteAEditar);
            PopulateListAssignedAsignatura(estudianteAEditar);
            return View(estudianteAEditar);



            /*
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estudiante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstudianteExists(estudiante.Id))
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
            return View(estudiante);
            */
        }

        private void UpdateEstudianteAsignaturas(string[] selectedAsignatura, Estudiante estudianteAEditar)
        {
            if (selectedAsignatura == null)
            {
                estudianteAEditar.EstudianteAsignatura = new List<EstudianteAsignatura>();
                return;
            }

            var selectedAsignaturasHS = new HashSet<string>(selectedAsignatura);//tiene las asignaturas que vienen del formulario
            var estudianteAsignaturas = new HashSet<int>(estudianteAEditar.EstudianteAsignatura.Select(e => e.Asignatura.Id)); //tiene las asignaturas del estudiante

            foreach(var asignatura in _context.Asignatura)
            {
                if (selectedAsignaturasHS.Contains(asignatura.Id.ToString()))
                {
                    if (!estudianteAsignaturas.Contains(asignatura.Id))
                    {
                        estudianteAEditar.EstudianteAsignatura.Add
                            (
                            new EstudianteAsignatura { AsignaturaId=asignatura.Id, EstudianteId = estudianteAEditar.Id}
                            );
                    }
                }
                else
                {
                    if (estudianteAsignaturas.Contains(asignatura.Id))
                    {
                        EstudianteAsignatura asignaturaParaRemover = estudianteAEditar.EstudianteAsignatura.FirstOrDefault(a => a.AsignaturaId == asignatura.Id);
                        _context.Remove(asignaturaParaRemover);
                    }
                }
            }
        }

        // GET: Estudiantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estudiante = await _context.Estudiantes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (estudiante == null)
            {
                return NotFound();
            }

            return View(estudiante);
        }

        // POST: Estudiantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstudianteExists(int id)
        {
            return _context.Estudiantes.Any(e => e.Id == id);
        }
    }
}
