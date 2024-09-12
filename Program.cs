using Newtonsoft.Json;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using pdfCreator.Models;
using pdfCreator.Services;

class Program
{
    static void Main(string[] args)
    {
        // Définition de la license QuestPDF
        QuestPDF.Settings.License = LicenseType.Community;

        // Fichier Json en argument pour les données
        if (args.Length == 0)
        {
            Console.WriteLine("fichier JSON en paramètre manquant.");
            return;
        }

        string jsonFilePath = args[0];

        if (!File.Exists(jsonFilePath))
        {
            Console.WriteLine($"Le fichier {jsonFilePath} n'existe pas.");
            return;
        }

        // Lire le fichier JSON et le convertir en un objet
        string jsonData = File.ReadAllText(jsonFilePath);
        dataModel? content = JsonConvert.DeserializeObject<dataModel>(jsonData);

        if (content != null)
        {
            // Générer le document PDF
            pdfDocument document = new pdfDocument(content);
            document.GeneratePdf(@"exemple_CreationPDF.pdf");

            Console.WriteLine("PDF généré avec succès !");
            Console.WriteLine("Appuyez sur une touche pour fermer.");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine($"Le fichier Jsonest vide.");
            return;
        }
    }
}