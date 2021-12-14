using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pruebasManyToMany.Models
{
    public class EstudiantesIndexData
    {
        public Estudiante Estudiante { get; set; }
        public IEnumerable<Estudiante> Estudiantes { get; set; }
        public IEnumerable<Asignatura> Asignaturas { get; set; }
    }
}
