using FluentAssertions;
using Microsoft.EntityFrameworkCore;

public class ReparationServiceTest
{
    private readonly ReparationService _reparationService;
    private readonly ParcInformatiqueContext _context;

    // TP4 : même principe que OrdinateurServiceTest, une base en mémoire neuve pour chaque test
    public ReparationServiceTest()
    {
        var options = new DbContextOptionsBuilder<ParcInformatiqueContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        // Base remplie avec le parc de test
        _context = new ParcInformatiqueContext(options);
        _context.Ordinateurs.AddRange(ParcInitializer.InitialiserParc());
        _context.SaveChanges();

        // Le scopeFactory ne sert qu'à la simulation, on n'en a pas besoin ici
        _reparationService = new ReparationService(_context, null, new JournalService());
    }

    // TP4 : une réparation créée doit se retrouver dans la liste avec son technicien et son ordinateur
    [Fact]
    public async Task CreationReparation_ReparationValide_AjouteReparationALaListe()
    {
        // Arrange
        var reparation = new Reparation
        {
            OrdinateurId = 1,
            TypeReparation = "Diagnostic",
            DateDebut = DateTime.Now,
            Statut = "En cours",
            Technicien = "Karim"
        };

        // Act
        _context.Reparations.Add(reparation);
        await _context.SaveChangesAsync();

        // Assert
        var reparations = await _reparationService.GetAllAsync();
        reparations.Should().HaveCount(1);
        reparations[0].Technicien.Should().Be("Karim");
        reparations[0].UnOrdinateur.Nom.Should().Be("Ordinateur Fixe 1");
    }
}
