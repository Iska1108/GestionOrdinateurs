using Microsoft.EntityFrameworkCore;

// Contexte EF Core : fait le lien entre nos classes et la base SQLite (de ce que j'ai compris, c'est un peu comme un ORM)
public class ParcInformatiqueContext : DbContext
{
    public ParcInformatiqueContext(DbContextOptions options) : base(options)
    {
        
    }

    // Table des ordinateurs
    public DbSet<Ordinateur> Ordinateurs { get; set; }

    // Table des réparations
    public DbSet<Reparation> Reparations { get; set; }

    // TP5 : table des utilisateurs
    public DbSet<User> Users { get; set; }

    // Configuration de la base SQLite (fichier parcinfo.db)
    // Si une base est déjà fournie (ex : InMemory dans les tests), on ne met pas SQLite
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=parcinfo.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TP4 : les caractéristiques n'ont pas leur propre table, leurs colonnes sont dans la table Ordinateur
        modelBuilder.Entity<Ordinateur>()
            .OwnsOne(o => o.Caracteristiques);

        // TP4 : un ordinateur a plusieurs réparations, une réparation concerne un seul ordinateur
        modelBuilder.Entity<Ordinateur>()
            .HasMany(o => o.Reparations)
            .WithOne(r => r.UnOrdinateur);

        // TP5 : le login doit être unique en base (index unique)
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();
    }
}
