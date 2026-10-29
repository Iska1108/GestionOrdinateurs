// Service qui gère le stock de pièces (SSD) de l'atelier
public class StockService
{
    public int StockSSD { get; set; } = 10;
    private readonly object _verrou = new object();
    // TP3 : méthode pour consommer un SSD du stock
    // Sans le lock : plusieurs threads modifient StockSSD en même temps, certains retraits
    // sont perdus et le stock final n'est pas forcément 0 (incohérent).
    // Avec le lock : un seul thread à la fois modifie le stock, le résultat est bien 0.
    public async void ConsommerSSD()
    {
        Task.Delay(10).Wait();

        lock (_verrou)
        {
            StockSSD--;
        }
    }
}
