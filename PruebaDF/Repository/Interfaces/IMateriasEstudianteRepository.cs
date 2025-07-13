using PruebaDF.Models;

namespace PruebaDF.Repository.Interfaces
{
    public interface IMateriasEstudianteRepository
    {
        string ObtenerMensajeMateriasCreditos();
        string ObtenerMensajeMateriasAsociadas();
        void Actualizar(int id, int idMateriaActual, int idMateriaAsociar);
        void Crear(MateriasEstudiante materiasEstudiante);
        void Eliminar(MateriasEstudiante materiasEstudiante);
        Task SaveAsync();
        IEnumerable<MateriasEstudiante> ObtenerMateriasEstudiante();
        IEnumerable<MateriasEstudiante> ObtenerMateriasEstudianteId(int? id);
        MateriasEstudiante? ObtenerMateriasEstudianteAsociadas(int? id, int? idMat);
        IEnumerable<Materia> ObtenerMateriasAsociar(int? idEst);
        IEnumerable<Estudiante> ObtenerEstudianteSeleccionado(int? id);
        Materia? ObtenerMateriaAsociada(int? mat);
        bool AsociarMateria(int id, int idMat);
        IEnumerable<Materia?> ObtenerMateriasEditar(int? idEst, int? idMat);
    }    
}
