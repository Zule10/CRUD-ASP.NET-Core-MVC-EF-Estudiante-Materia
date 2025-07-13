using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Moq;
using PruebaDF.Models;

namespace PruebaDF.Test
{
    public class MateriasEstudianteDummy
    {
        public string MensajeCreditosDummy() =>
         $"No se pueden asociar más de 3 materias que tienen más de 4 créditos. Por favor valide";

        public MateriasEstudiante materiaEstudianteTest = new()
        {
            EstudianteId = 1,
            Estudiante = new Estudiante
            {
                EstudianteId = 1,
                Nombre = "Test",
                Documento = 123,
                Correo = "test@test.com"
            },
            MateriaId = 1,
            Materia = new Materia
            {
                MateriaId = 1,
                Nombre = "Test",
                Codigo = 123,
                Creditos = 5
            }
        }; 

        public List<MateriasEstudiante> materiaEstudianteTrue =
        [
            new MateriasEstudiante{MateriaId=2,EstudianteId=1,Materia = new Materia{ MateriaId = 2, Creditos=2} },
            new MateriasEstudiante{MateriaId=3,EstudianteId=1,Materia = new Materia{ MateriaId = 3, Creditos=7} },
            new MateriasEstudiante{MateriaId=4,EstudianteId=1,Materia = new Materia{ MateriaId = 4, Creditos=8} },
        ];

        public List<MateriasEstudiante> materiaEstudianteFalse =
        [
            new MateriasEstudiante{MateriaId=2,EstudianteId=1,Materia = new Materia{ MateriaId = 2, Creditos=6} },
            new MateriasEstudiante{MateriaId=3,EstudianteId=1,Materia = new Materia{ MateriaId = 3, Creditos=7} },
            new MateriasEstudiante{MateriaId=4,EstudianteId=1,Materia = new Materia{ MateriaId = 4, Creditos=8} },
        ];

        public static ITempDataDictionary InicializarTempData()
        {
            ITempDataProvider tempDataProvider = Mock.Of<ITempDataProvider>();
            TempDataDictionaryFactory tempDataDictionaryFactory = new(tempDataProvider);
            ITempDataDictionary tempData = tempDataDictionaryFactory.GetTempData(new DefaultHttpContext());

            return tempData;
        }

        public static DbSet<T> MockDbSet<T>(List<T> data) where T : class
        {
            var _data = data.AsQueryable();
            var mockDbSet = new Mock<DbSet<T>>();

            mockDbSet.As<IQueryable<T>>().Setup(x => x.Provider).Returns(_data.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(_data.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(x => x.Expression).Returns(_data.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(_data.GetEnumerator());

            return mockDbSet.Object;
        }
    }
}
