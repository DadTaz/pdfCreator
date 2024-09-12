using QuestPDF.Infrastructure;

namespace pdfCreator.Interfaces
{
    public interface IPdfDocument
    {
        DocumentMetadata GetMetadata();

        // Méthodes
        public void Compose(IDocumentContainer container);
        public void ComposeHeader(IContainer container);

        public void ComposeContent(IContainer container);

        public void ComposeFooter(IContainer container);

        public void CreateTable(IContainer container, string[][] data, bool hasHeaders, int constantColumn = 0);

    }
}
