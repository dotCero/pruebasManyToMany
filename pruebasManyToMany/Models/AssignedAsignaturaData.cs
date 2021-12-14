using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pruebasManyToMany.Models
{
    public class AssignedAsignaturaData
    {
        public int AsignaturaId { get; set; }
        public string Nombre { get; set; }
        public bool Asignado { get; set; }
    }
}
