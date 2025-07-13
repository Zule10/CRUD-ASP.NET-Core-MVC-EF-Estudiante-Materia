using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaDF.Models;
using PruebaDF.Repository.Interfaces;

namespace PruebaDF.Controllers
{
    public class MateriasEstudiantesController : Controller
    {
        public  IMateriasEstudianteRepository _materiasEstudianteRepo { get; set; }

        public MateriasEstudiantesController( IMateriasEstudianteRepository materiasEstudianteRepo )
        {
            _materiasEstudianteRepo = materiasEstudianteRepo;
        }

        // GET: MateriasEstudiantes
        public IActionResult Index(int? id)
        {
            var materiasEstudiante = _materiasEstudianteRepo.ObtenerMateriasEstudianteId(id);

            ViewBag.IdEstudiante = id;
            ViewBag.Message = TempData["Message"];

            return View(materiasEstudiante);
        }

        // GET: MateriasEstudiantes/Details/5
        public IActionResult Details(int? id, int? idMat)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiasEstudiante =  _materiasEstudianteRepo.ObtenerMateriasEstudianteAsociadas(id, idMat);

            if (materiasEstudiante == null)
            {
                return NotFound();
            }

            return View(materiasEstudiante);
        }

        // GET: MateriasEstudiantes/Create
        public IActionResult Create(int? idEst)
        {            
            var materiasEstudiante = _materiasEstudianteRepo.ObtenerMateriasAsociar(idEst);

            ViewBag.IdEstudiante = idEst;

            if (materiasEstudiante != null && materiasEstudiante.Any())
            {
                var estudianteSeleccionado = _materiasEstudianteRepo.ObtenerEstudianteSeleccionado(idEst);

                ViewData["EstudianteId"] = new SelectList(estudianteSeleccionado, "EstudianteId", "Nombre");
                ViewData["MateriaId"] = new SelectList(materiasEstudiante, "MateriaId", "NombreCreditos");
            }
            else
            {
                TempData["Message"] = _materiasEstudianteRepo.ObtenerMensajeMateriasAsociadas();
                return RedirectToAction("Index", new { id = idEst });
            }

            return View();
        }

        // POST: MateriasEstudiantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EstudianteId,MateriaId")] MateriasEstudiante materiasEstudiante)
        {
            if (ModelState.IsValid)
            {
                if (_materiasEstudianteRepo.AsociarMateria(materiasEstudiante.EstudianteId, materiasEstudiante.MateriaId))
                {
                     _materiasEstudianteRepo.Crear(materiasEstudiante);
                    await _materiasEstudianteRepo.SaveAsync();
                    return RedirectToAction("Index",new { id = materiasEstudiante.EstudianteId });
                }
                else
                {
                    TempData["Message"] = _materiasEstudianteRepo.ObtenerMensajeMateriasCreditos(); 
                    return RedirectToAction("Index", new { id = materiasEstudiante.EstudianteId });            
                }
            }
            return View(materiasEstudiante);
        }

        // GET: MateriasEstudiantes/Edit/5
        public IActionResult Edit(int id, int idMat)
        {
            ViewBag.IdEstudiante = id;

            var materiasEstudiante =  _materiasEstudianteRepo.ObtenerMateriasEstudianteAsociadas(id, idMat);

            if (materiasEstudiante == null)
            {
                return NotFound();
            }
            var estudianteAsociado = new List<Estudiante?>{materiasEstudiante.Estudiante};
            var materiasPorAsociar = _materiasEstudianteRepo.ObtenerMateriasEditar(id, idMat);

            ViewData["EstudianteId"] =  new SelectList(estudianteAsociado, "EstudianteId", "Nombre");
            ViewData["MateriaId"] = new SelectList(materiasPorAsociar, "MateriaId", "NombreCreditos", idMat);

            return View(materiasEstudiante);
        }

        // POST: MateriasEstudiantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int idMat, [Bind("EstudianteId,MateriaId")] MateriasEstudiante materiasEstudiante)
        {
            if (id != materiasEstudiante.EstudianteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {          
                    if (_materiasEstudianteRepo.AsociarMateria(id, idMat))
                    {
                        _materiasEstudianteRepo.Actualizar(id, materiasEstudiante.MateriaId, idMat);
                        await _materiasEstudianteRepo.SaveAsync();
                        return RedirectToAction("Index", new { id });
                    }
                    else
                    {
                        TempData["Message"] = _materiasEstudianteRepo.ObtenerMensajeMateriasCreditos();
                        return RedirectToAction("Index", new { id });
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    var materiaEstudianteExiste =  _materiasEstudianteRepo.ObtenerMateriasEstudianteId(materiasEstudiante.EstudianteId);
                   
                    if (!materiaEstudianteExiste.Any())
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(materiasEstudiante);
        }

        // GET: MateriasEstudiantes/Delete/5
        public IActionResult Delete(int? id, int? idMat)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiasEstudiante =  _materiasEstudianteRepo.ObtenerMateriasEstudianteAsociadas(id, idMat);

            if (materiasEstudiante == null)
            {
                return NotFound();
            }

            return View(materiasEstudiante);
        }

        // POST: MateriasEstudiantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int idMat)
        {
            var materiasEstudiante = new MateriasEstudiante
            {
                EstudianteId = id,
                MateriaId = idMat
            };
            
            _materiasEstudianteRepo.Eliminar(materiasEstudiante);
            await _materiasEstudianteRepo.SaveAsync();
            return RedirectToAction("Index", new { id });
        }              
    }
}
