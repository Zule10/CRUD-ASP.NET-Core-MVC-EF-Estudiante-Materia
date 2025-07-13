using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaDF.Models;
using PruebaDF.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaDF.Repository
{
    public class MateriasEstudianteRepository : IMateriasEstudianteRepository
    {
        public PruebaDfContext _context { get; set; }

        private readonly int _cantidadMaximaMaterias = 3;

        private readonly int _cantidadCreditos = 4;

        public MateriasEstudianteRepository(PruebaDfContext context)
        {
            _context = context;
        }       

        public string ObtenerMensajeMateriasCreditos() =>
            $"No se pueden asociar más de {_cantidadMaximaMaterias} materias que tienen más de {_cantidadCreditos} créditos. Por favor valide";

        public string ObtenerMensajeMateriasAsociadas() =>
           "No hay materias para asociar. Por favor valide";

        public void Actualizar(int id, int idMateriaActual, int idMateriaAsociar)
        {
            var materiaEstudiante = new MateriasEstudiante
            {
                EstudianteId = id,
                MateriaId = idMateriaActual
            };
            var materiaPorAsociar = new MateriasEstudiante
            {
                EstudianteId = id,
                MateriaId = idMateriaAsociar
            };
            _context.Remove(materiaEstudiante);
            _context.Add(materiaPorAsociar);
        }

        public void Crear(MateriasEstudiante materiasEstudiante)
        {
            _context.Add(materiasEstudiante);
        }

        public void Eliminar(MateriasEstudiante materiasEstudiante)
        {
            _context.Remove(materiasEstudiante);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IEnumerable<MateriasEstudiante> ObtenerMateriasEstudiante()
        {
            return  _context.MateriasEstudiantes.AsNoTracking();
        }
        
        public IEnumerable<MateriasEstudiante> ObtenerMateriasEstudianteId(int? id)
        {
            return _context.MateriasEstudiantes
              .Include(m => m.Estudiante)
              .Include(m => m.Materia)
              .Where(m => m.EstudianteId == id).AsNoTracking();
        }

        public MateriasEstudiante? ObtenerMateriasEstudianteAsociadas(int? id, int? idMat)
        {
            return _context.MateriasEstudiantes
           .Include(m => m.Estudiante)
           .Include(m => m.Materia)
           .Where(m => m.EstudianteId == id && m.MateriaId == idMat).AsNoTracking().FirstOrDefault();
        }
       
        public IEnumerable<Materia> ObtenerMateriasAsociar(int? idEst)
        {
            return  _context.Materias.Where(
                item => (_context.MateriasEstudiantes
                .Include(m => m.Estudiante)
                .Include(m => m.Materia)
                .Where(m => m.EstudianteId == idEst))
                .All(id => id.Materia != item));
        }

        public IEnumerable<Estudiante> ObtenerEstudianteSeleccionado(int? id)
        {
            return _context.Estudiantes.Where(m => m.EstudianteId == id);
        }

        public Materia? ObtenerMateriaAsociada(int? mat)
        {
            return _context.Materias.Where(m => m.MateriaId == mat).FirstOrDefault();
        }

        public IEnumerable<Materia?> ObtenerMateriasEditar(int? idEst, int? idMat)
        {
            var materiaAsociada = ObtenerMateriaAsociada(idMat);
            var materiasPorAsociar = ObtenerMateriasAsociar(idEst);

            if (materiasPorAsociar != null && materiasPorAsociar.Any())
            {
                return materiasPorAsociar.Append(materiaAsociada);
            }
            else
            {
                return new List<Materia?> { materiaAsociada };
            }
        }

        public bool AsociarMateria(int id, int idMat)
        {
            var materiaAsociar = ObtenerMateriaAsociada(idMat);
            int? creditosMateria = materiaAsociar.Creditos;

            var materiasCreditos = ObtenerMateriasEstudianteId(id);
            int cantidadMateriasCreditos = materiasCreditos.Count(m => m.Materia.Creditos > _cantidadCreditos);
            int cantidadMateriasEstudiante = materiasCreditos.Count();

            if (creditosMateria > _cantidadCreditos && (cantidadMateriasCreditos == _cantidadMaximaMaterias
                || cantidadMateriasEstudiante > _cantidadMaximaMaterias ))
            {
                return false;
            }

            return true;
        }      
    }
}
