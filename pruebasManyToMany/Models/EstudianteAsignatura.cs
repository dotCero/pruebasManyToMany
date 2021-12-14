using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace pruebasManyToMany.Models
{
    public class EstudianteAsignatura
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Estudiante")]
        public int EstudianteId { get; set; }
        [ForeignKey("Asignatura")]
        public int AsignaturaId { get; set; }

        public virtual Asignatura Asignatura { get; set; }
        public virtual Estudiante Estudiante { get; set; }
    }
}
