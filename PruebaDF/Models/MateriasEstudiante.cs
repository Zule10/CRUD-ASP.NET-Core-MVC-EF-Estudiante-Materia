using System;
using System.Collections.Generic;

namespace PruebaDF.Models;

public partial class MateriasEstudiante
{
    public int EstudianteId { get; set; }

    public int MateriaId { get; set; }

    public virtual Estudiante? Estudiante { get; set; }

    public virtual Materia? Materia { get; set; }
}
