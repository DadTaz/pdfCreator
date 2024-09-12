using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using pdfCreator.Models;
using pdfCreator.Interfaces;

namespace pdfCreator.Services
{
    internal class pdfDocument : IPdfDocument, IDocument
    {
        private dataModel Content { get; }

        public pdfDocument(dataModel content)
        {
            Content = content;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50);
                    page.Size(PageSizes.A4);

                    page.Header().Element(ComposeHeader);

                    page.Content().Element(ComposeContent);

                    page.Footer().Element(ComposeFooter);
                });
        }

        public void ComposeHeader(IContainer container)
        {
            TextStyle titleStyle = TextStyle.Default.FontSize(14).SemiBold().FontColor(Colors.Blue.Medium);
            string logoPath = @"Images\logo.png";

            container.Row(row =>
            {
                row.ConstantItem(180).Height(50).Image(logoPath).FitArea();
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("Pdf creation using C#").Style(titleStyle).AlignRight();
                });
            });
        }

        public void ComposeContent(IContainer container)
        {
            TextStyle titleStyle = TextStyle.Default.FontSize(14).SemiBold().FontColor(Colors.Black);

            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    // Titre
                    column.Item().PaddingVertical(20);
                    column.Item().Text(Content.Title).FontSize(20)
                    .Bold()
                    .Underline()
                    .AlignCenter();
                    column.Item().PaddingVertical(20);

                    // Texte
                    column.Item().Text(x =>
                    {
                        x.DefaultTextStyle(titleStyle);
                        x.Span("Ceci est du texte en style normal, suivi de ");
                        x.Span("'texte en style gras'").Bold();
                        x.Span(", texte normal ");
                        x.Span(", 'texte en style italic' ").Italic();
                        x.Span("et pour finir du text en style normale.");
                    });
                    column.Item().PaddingVertical(20);

                    // Création du tableau statique de 5 lignes et 3 colonnes avec fond vert
                    if (Content.TableDataFull != null)
                    {
                        if (Content.TableDataFull.Length > 0)
                        {
                            column.Item().Text("Tableau statique").FontSize(16).Bold().AlignCenter();
                            CreateTable(column.Item(), Content.TableDataFull, true, 150);
                        }
                    }

                    column.Item().PaddingVertical(20);

                    // Création du tableau dynamique
                    if (Content.TableDataPartial != null)
                    {
                        if (Content.TableDataPartial.Length > 0)
                        {
                            column.Item().Text("Tableau dynamique").FontSize(16).Bold().AlignCenter();
                            CreateTable(column.Item(), Content.TableDataPartial, true);
                        }
                    }


                });
            });
        }

        public void ComposeFooter(IContainer container)
        {
            TextStyle titleStyle = TextStyle.Default.FontSize(12).Bold().FontColor(Colors.Red.Darken4);
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span("/");
                        x.TotalPages();
                        x.DefaultTextStyle(titleStyle);
                        x.AlignRight();
                    });
                });
            });
        }

        // Fonction pour créer un tableau, parametre optionnel: la taille des colonnes
        public void CreateTable(IContainer container, string[][] data, bool hasHeaders, int constantColumn = 0)
        {

            TextStyle headerStyle = TextStyle.Default.FontSize(14).Bold().FontColor(Colors.White);
            TextStyle cellStyle = TextStyle.Default.FontSize(12).FontColor(Colors.Black);

            container
                .AlignCenter()
                .Table(table =>
                {
                    // Définir le nombre de colonnes basé sur la ligne avec le plus de colonnes
                    int maxColumns = 0;

                    foreach (string[] row in data)
                    {
                        if (row.Length > maxColumns)
                            maxColumns = row.Length;
                    }

                    table.ColumnsDefinition(columns =>
                    {
                        for (int i = 0; i < maxColumns; i++)
                        {
                            if (constantColumn > 0)
                            {
                                columns.ConstantColumn(constantColumn);
                            }
                            else
                            {
                                columns.RelativeColumn();
                            }
                        }
                    });

                    if (hasHeaders)
                    {
                        // Ajouter la ligne d'en-têtes de colonnes
                        table.Header(header =>
                        {
                            for (int i = 0; i < maxColumns; i++)
                            {
                                header.Cell()
                                .Border(1)
                                .BorderColor(Colors.Black)
                                .Background(Colors.Green.Darken1)
                                .Padding(5)
                                .AlignCenter()
                                .Text($"En-tête {i + 1}")
                                .Style(headerStyle);
                            }
                        });
                    }

                    // Ajouter les lignes de données avec fond vert
                    foreach (string[] row in data)
                    {
                        for (int i = 0; i < maxColumns; i++)
                        {
                            if (i < row.Length)
                            {
                                table.Cell()
                                .Border(1)
                                .BorderColor(Colors.Black)
                                .Background(Colors.Green.Lighten3)
                                .Padding(5)
                                .AlignCenter()
                                .Text(row[i])
                                .Style(cellStyle);
                            }
                            else
                            {
                                table.Cell()
                                .Border(1)
                                .BorderColor(Colors.Black)
                                .Background(Colors.Green.Lighten3)
                                .Padding(5);
                            }
                        }
                    }
                });
        }
    }
}
