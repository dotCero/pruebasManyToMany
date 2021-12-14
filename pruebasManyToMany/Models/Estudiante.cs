using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pruebasManyToMany.Models
{
    public class Estudiante
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Nombres { get; set; }
        [Required]
        [StringLength(255)]
        public string Apellido1 { get; set; }
        [Required]
        [StringLength(255)]
        public string Apellido2 { get; set; }
        public virtual ICollection<EstudianteAsignatura> EstudianteAsignatura { get; set; }
    }
}
