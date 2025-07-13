using System;
using System.Collections.Generic;

namespace PruebaDF.Models;

public partial class Estudiante
{
    public int EstudianteId { get; set; }

    public string Nombre { get; set; } = null!;

    public int? Documento { get; set; }

    public string Correo { get; set; } = null!;

    public virtual ICollection<MateriasEstudiante> MateriasEstudiantes { get; set; } = new List<MateriasEstudiante>();

}
