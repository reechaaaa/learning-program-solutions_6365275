using System;

namespace FactoryMethodPatternExample
{
    /// <summary>
    /// Step 2: Define Document Interface
    /// Defines the common interface for all document types.
    /// All concrete document classes must implement this interface.
    /// </summary>
    public interface IDocument
    {
        string GetDocumentType();
        void Open();
        void Save();
        void Print();
    }

    /// <summary>
    /// Step 3: Create Concrete Document Classes
    /// Implements the IDocument interface for a Word document.
    /// </summary>
    public class WordDocument : IDocument
    {
        public string GetDocumentType() => "Word Document";

        public void Open()
        {
            Console.WriteLine("Opening Word Document...");
        }

        public void Save()
        {
            Console.WriteLine("Saving Word Document...");
        }

        public void Print()
        {
            Console.WriteLine("Printing Word Document...");
        }
    }

    /// <summary>
    /// Step 3: Create Concrete Document Classes
    /// Implements the IDocument interface for a PDF document.
    /// </summary>
    public class PdfDocument : IDocument
    {
        public string GetDocumentType() => "PDF Document";

        public void Open()
        {
            Console.WriteLine("Opening PDF Document...");
        }

        public void Save()
        {
            Console.WriteLine("Saving PDF Document...");
        }

        public void Print()
        {
            Console.WriteLine("Printing PDF Document...");
        }
    }

    /// <summary>
    /// Step 3: Create Concrete Document Classes
    /// Implements the IDocument interface for an Excel document.
    /// </summary>
    public class ExcelDocument : IDocument
    {
        public string GetDocumentType() => "Excel Document";

        public void Open()
        {
            Console.WriteLine("Opening Excel Document...");
        }

        public void Save()
        {
            Console.WriteLine("Saving Excel Document...");
        }

        public void Print()
        {
            Console.WriteLine("Printing Excel Document...");
        }
    }

    /// <summary>
    /// Step 4: Implement the Abstract Factory Method
    /// Defines the abstract factory class with the factory method CreateDocument().
    /// This method is responsible for creating document objects.
    /// </summary>
    public abstract class DocumentFactory
    {
        /// <summary>
        /// The abstract factory method that concrete factories will implement
        /// to create a specific type of document.
        /// </summary>
        /// <returns>An instance of an IDocument.</returns>
        public abstract IDocument CreateDocument();

        /// <summary>
        /// A common operation that can be performed by the factory before or after creating a document.
        /// (This is often a part of the factory method pattern, showing how the factory "manages" creation).
        /// </summary>
        public void PerformDocumentOperations()
        {
            // The factory can contain logic common to all document types or
            // interact with the created document.
            Console.WriteLine($"\n--- Using factory {this.GetType().Name} ---");
            IDocument document = CreateDocument(); // Calls the concrete factory's implementation
            Console.WriteLine($"Created: {document.GetDocumentType()}");
            document.Open();
            document.Save();
            document.Print();
            Console.WriteLine("--- Document operations completed ---\n");
        }
    }

    /// <summary>
    /// Step 4: Create Concrete Factory Classes
    /// Concrete factory for creating WordDocument instances.
    /// </summary>
    public class WordDocumentFactory : DocumentFactory
    {
        public override IDocument CreateDocument()
        {
            return new WordDocument();
        }
    }

    /// <summary>
    /// Step 4: Create Concrete Factory Classes
    /// Concrete factory for creating PdfDocument instances.
    /// </summary>
    public class PdfDocumentFactory : DocumentFactory
    {
        public override IDocument CreateDocument()
        {
            return new PdfDocument();
        }
    }

    /// <summary>
    /// Step 4: Create Concrete Factory Classes
    /// Concrete factory for creating ExcelDocument instances.
    /// </summary>
    public class ExcelDocumentFactory : DocumentFactory
    {
        public override IDocument CreateDocument()
        {
            return new ExcelDocument();
        }
    }

    /// <summary>
    /// Step 5: Test the Factory Method Implementation
    /// Main program to demonstrate the use of the Factory Method Pattern.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Factory Method Pattern Example: Document Management System ---\n");

            // Create a Word document using its factory
            Console.WriteLine("Creating Word Document...");
            DocumentFactory wordFactory = new WordDocumentFactory();
            wordFactory.PerformDocumentOperations(); // The factory orchestrates creation and basic operations

            // Create a PDF document using its factory
            Console.WriteLine("Creating PDF Document...");
            DocumentFactory pdfFactory = new PdfDocumentFactory();
            pdfFactory.PerformDocumentOperations();

            // Create an Excel document using its factory
            Console.WriteLine("Creating Excel Document...");
            DocumentFactory excelFactory = new ExcelDocumentFactory();
            excelFactory.PerformDocumentOperations();

            // Demonstrating direct creation using the factory method
            Console.WriteLine("\n--- Direct Creation via Factory Method ---");
            IDocument newWordDoc = new WordDocumentFactory().CreateDocument();
            Console.WriteLine($"Directly created: {newWordDoc.GetDocumentType()}");
            newWordDoc.Open();

            IDocument newPdfDoc = new PdfDocumentFactory().CreateDocument();
            Console.WriteLine($"Directly created: {newPdfDoc.GetDocumentType()}");
            newPdfDoc.Print();

            Console.WriteLine("\n--- Factory Method Pattern Example Finished ---");
            Console.ReadKey(); // Keep console open until a key is pressed
        }
    }
}
