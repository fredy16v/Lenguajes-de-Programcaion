using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TareasMVC.Entities;

namespace TareasMVC.Database;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Tarea> Tareas { get; set; }// es como una tabla de la base de datos
    public DbSet<Paso> Pasos { get; set; }// es como una tabla de la base de datos
    public DbSet<ArchivoAdjunto> ArchivosAdjuntos { get; set; }// es como una tabla de la base de datos
    
}