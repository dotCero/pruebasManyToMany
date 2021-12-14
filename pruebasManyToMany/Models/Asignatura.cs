using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pruebasManyToMany.Models
{
    public class Asignatura
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Nombre { get; set; }
        public virtual ICollection<EstudianteAsignatura> EstudianteAsignatura { get; set; }
    }
}
