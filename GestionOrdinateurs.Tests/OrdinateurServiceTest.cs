using FluentAssertions;
using Microsoft.EntityFrameworkCore;

public class OrdinateurServiceTest
{
    private readonly OrdinateurService _ordinateurService;

    // xUnit recrée la classe pour chaque test, donc chaque test repart d'un parc neuf (bonne technologie ca) 
    public OrdinateurServiceTest()
    {
        // Base en mémoire neuve pour chaque test, remplie avec le parc de test
        var context = CreerContexte();
        context.Ordinateurs.AddRange(ParcInitializer.InitialiserParc());
        context.SaveChanges();

        // TP4 : le service a aussi besoin du journal pour écrire les logs
        _ordinateurService = new OrdinateurService(context, new JournalService());
    }

    // Crée un contexte sur une base InMemory au nom unique (les tests ne se partagent pas les données)
    private ParcInformatiqueContext CreerContexte()
    {
        var options = new DbContextOptionsBuilder<ParcInformatiqueContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ParcInformatiqueContext(options);
    }

     // Prof : Les tests doivent avoir une nomenclature claire avec une légère phrase explicative pour comprendre ce qu'on teste
    [Fact]
    public async Task AddAsync_OrdinateurValide_AjouteOrdinateurAuParc()
    {
        // Arrange
        var nombreInitial = (await _ordinateurService.GetAllAsync()).Count;
        // TP4 : plus de classe OrdinateurPortable, le type est dans les caractéristiques
        var nouvelOrdinateur = new Ordinateur
        {
            Id = 6,
            Nom = "Ordinateur Portable 4",
            Etat = EtatOrdinateur.EnService,
            Caracteristiques = new Caracteristiques(16, 512, TypeOrdinateur.Portable, "Apple", "Apple M3")
        };

        // Act
        await _ordinateurService.AddAsync(nouvelOrdinateur);

        // Assert
        (await _ordinateurService.GetAllAsync()).Should().HaveCount(nombreInitial + 1);
        (await _ordinateurService.GetByIdAsync(6)).Should().Be(nouvelOrdinateur);
    }

    [Fact]
    public async Task DeleteAsync_OrdinateurExistant_RetourneTrueEtSupprimeOrdinateur()
    {
        // Arrange
        var nombreInitial = (await _ordinateurService.GetAllAsync()).Count;

        // Act
        var resultat = await _ordinateurService.DeleteAsync(1);

        // Assert
        resultat.Should().BeTrue();
        (await _ordinateurService.GetAllAsync()).Should().HaveCount(nombreInitial - 1);
        (await _ordinateurService.GetByIdAsync(1)).Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_OrdinateurInexistant_RetourneFalseEtParcInchange()
    {
        // Arrange
        var nombreInitial = (await _ordinateurService.GetAllAsync()).Count;

        // Act : l'id 999 n'existe pas dans le parc 
        var resultat = await _ordinateurService.DeleteAsync(999);

        // Assert
        resultat.Should().BeFalse();
        (await _ordinateurService.GetAllAsync()).Should().HaveCount(nombreInitial);
    }
}
