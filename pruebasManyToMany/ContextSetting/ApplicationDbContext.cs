using Microsoft.EntityFrameworkCore;
using pruebasManyToMany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pruebasManyToMany.ContextSetting
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Asignatura> Asignatura { get; set; }
        public DbSet<EstudianteAsignatura> EstudianteAsignaturas { get; set; }
    }
}
