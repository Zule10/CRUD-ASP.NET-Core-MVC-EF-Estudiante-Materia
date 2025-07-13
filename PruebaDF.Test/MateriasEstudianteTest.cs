using Moq;
using PruebaDF.Controllers;
using PruebaDF.Models;
using PruebaDF.Repository;
using PruebaDF.Repository.Interfaces;

namespace PruebaDF.Test
{
    [TestClass]
    public class MateriasEstudianteTest
    {
        public required MateriasEstudiantesController _materiasEstController;

        public required MateriasEstudianteRepository _materiasEstRepository;

        public required MateriasEstudianteDummy _materiasEstDummy;

        [TestInitialize]
        public void Inicializar()
        {
            var mockMateriasEstRepo = new Mock<IMateriasEstudianteRepository>();
            var mockPruebaContext = new Mock<PruebaDfContext>();

            _materiasEstController = new MateriasEstudiantesController(mockMateriasEstRepo.Object);
            _materiasEstRepository = new MateriasEstudianteRepository(mockPruebaContext.Object);
            _materiasEstDummy = new MateriasEstudianteDummy();
        }

        
        [TestMethod]
        public void AsociarMateriaTest()
        {
            var mockPruebaContext = new Mock<PruebaDfContext>();
            var materiaEstudiante = _materiasEstDummy.materiaEstudianteTest;
            var mockDbMateria = MateriasEstudianteDummy.MockDbSet(new List<Materia> { materiaEstudiante.Materia });
            var mockDbMateriasEst = MateriasEstudianteDummy.MockDbSet(_materiasEstDummy.materiaEstudianteTrue);
            //var mockDbMateriasEst = _materiasEstDummy.MockDbSet<MateriasEstudiante>(_materiasEstDummy.materiaEstudianteFalse);

            mockPruebaContext.Setup(x => x.Materias).Returns(mockDbMateria);
            mockPruebaContext.Setup(x => x.MateriasEstudiantes).Returns(mockDbMateriasEst);

            _materiasEstRepository._context = mockPruebaContext.Object;           

            var resultado = _materiasEstRepository.AsociarMateria(materiaEstudiante.EstudianteId, materiaEstudiante.MateriaId);
            
            Assert.IsTrue(resultado);
            //Assert.IsFalse(resultado);
        }

        
        [TestMethod]
        public async Task CreateMateriasEstudianteTest()
        {
            var tempData = MateriasEstudianteDummy.InicializarTempData();
            var mockMateriasEstRepo = new Mock<IMateriasEstudianteRepository>();
            var mensajeDummy = _materiasEstDummy.MensajeCreditosDummy();
            var materiaEstudiante = _materiasEstDummy.materiaEstudianteTest;

            mockMateriasEstRepo.Setup(x => x.ObtenerMensajeMateriasCreditos()).Returns(mensajeDummy);
            //mockMateriasEstRepo.Setup(x => x.AsociarMateria(materiaEstudiante.EstudianteId, materiaEstudiante.MateriaId)).Returns(false);
            mockMateriasEstRepo.Setup(x => x.AsociarMateria(materiaEstudiante.EstudianteId, materiaEstudiante.MateriaId)).Returns(true);

            _materiasEstController._materiasEstudianteRepo = mockMateriasEstRepo.Object;
            _materiasEstController.TempData = tempData;

            var resultado = await _materiasEstController.Create(materiaEstudiante);

            //Assert.IsTrue(_materiasEstController.TempData.ContainsKey("Message"));
            //Assert.AreEqual(mensajeDummy, _materiasEstController.TempData["Message"]);
            Assert.IsFalse(_materiasEstController.TempData.Count > 0);
        }
        
    }
}
