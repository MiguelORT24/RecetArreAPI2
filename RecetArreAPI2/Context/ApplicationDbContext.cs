using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecetArreAPI2.Models;

namespace RecetArreAPI2.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Ingrediente> Ingredientes { get; set; }
        public DbSet<Receta> Recetas { get; set; }
        public DbSet<Rec_Tiem> Rec_Tiems { get; set; }
        public DbSet<Tiempo> Tiempos { get; set; }
        public DbSet<Ing_Rec> Ing_Recs { get; set; }
        public DbSet<Cat_Rec> Cat_Recs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuración de Categoria
            builder.Entity<Categoria>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(500)
                    .IsRequired(false);

                entity.Property(e => e.CreadoUtc)
                    .IsRequired()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Relación con ApplicationUser
                entity.HasOne(e => e.CreadoPorUsuario)
                    .WithMany()
                    .HasForeignKey(e => e.CreadoPorUsuarioId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);

                // Índices
                entity.HasIndex(e => e.Nombre).IsUnique();
                entity.HasIndex(e => e.CreadoPorUsuarioId);
            });

            // Configuración de CAT_REC (tabla intermedia entre Receta y Categoria)
            builder.Entity<Cat_Rec>(entity =>
            {
                entity.HasKey(e => new { e.RecetaId, e.CategoriaId });

                entity.HasOne(e => e.Receta)
                      .WithMany(r => r.Cat_Recs)
                      .HasForeignKey(e => e.RecetaId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Categoria)
                      .WithMany(c => c.Cat_Recs)
                      .HasForeignKey(e => e.CategoriaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de ING_REC (tabla intermedia entre Receta e Ingrediente)
            builder.Entity<Ing_Rec>(entity =>
            {
                entity.HasKey(e => new { e.RecetaId, e.IngredienteId });

                entity.HasOne(e => e.Receta)
                      .WithMany(r => r.Ing_Recs)
                      .HasForeignKey(e => e.RecetaId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Ingrediente)
                      .WithMany(i => i.Ing_Recs)
                      .HasForeignKey(e => e.IngredienteId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de TIEMPO
            builder.Entity<Tiempo>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(200)
                    .IsRequired(false);
            });

            // Configuración de RECETA
            builder.Entity<Receta>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Instrucciones)
                    .HasMaxLength(2000)
                    .IsRequired(false);

                entity.Property(e => e.CreadoUtc)
                    .IsRequired()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Relación con ApplicationUser
                entity.HasOne(e => e.CreadoPorUsuario)
                    .WithMany()
                    .HasForeignKey(e => e.CreadoPorUsuarioId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);

                // Relación muchos-a-muchos con Tiempo a través de Rec_Tiem
                entity.HasMany(e => e.Rec_Tiems)
                      .WithOne(rt => rt.Receta)
                      .HasForeignKey(rt => rt.RecetaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de REC_TIEM (tabla intermedia)
            builder.Entity<Rec_Tiem>(entity =>
            {
                // Clave compuesta por las dos FKs
                entity.HasKey(e => new { e.RecetaId, e.TiempoId });

                entity.HasOne(e => e.Receta)
                      .WithMany(r => r.Rec_Tiems)
                      .HasForeignKey(e => e.RecetaId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Tiempo)
                      .WithMany()
                      .HasForeignKey(e => e.TiempoId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de INGREDIENTE
            builder.Entity<Ingrediente>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UnidadMedida).HasConversion //...............
                    (v => v.ToString(), v => v)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(500)
                    .IsRequired(false);

                entity.Property(e => e.CreadoUtc)
                    .IsRequired()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Relación con ApplicationUser
                entity.HasOne(e => e.CreadoPorUsuario)
                    .WithMany()
                    .HasForeignKey(e => e.CreadoPorUsuarioId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);

                // Índices
                entity.HasIndex(e => e.Nombre).IsUnique();
                entity.HasIndex(e => e.CreadoPorUsuarioId);
            });
        }

        
        
        


    }
}
