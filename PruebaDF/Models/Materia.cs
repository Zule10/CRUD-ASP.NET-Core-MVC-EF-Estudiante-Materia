using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaDF.Models;

public partial class Materia
{
    public int MateriaId { get; set; }

    public string Nombre { get; set; } = null!;

    public int? Codigo { get; set; }
    
    public int Creditos { get; set; }

    [NotMapped]
    public string NombreCreditos
    {
        get
        {
            return string.Format("{0} - {1} Créditos", Nombre, Creditos);
        }
     }
    public virtual ICollection<MateriasEstudiante> MateriasEstudiantes { get; set; } = new List<MateriasEstudiante>();
}
