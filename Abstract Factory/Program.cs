using System.Reflection;

namespace Abstract_factory;

public interface IReport
{
    public void Generate();
}

public interface IInvoice
{
    public void Create();
}

public class PdfReport : IReport
{
    public void Generate()
    {
        Console.WriteLine("Создан отчет в формате PDF");
    }
}

public class PdfInvoice : IInvoice
{
    public void Create()
    {
        Console.WriteLine("Создана счет-фактура в формате PDF");
    }
}

public class ExcelReport : IReport
{
    public void Generate()
    {
        Console.WriteLine("Создан отчет в формате Excel");
    }
}

public class ExcelInvoice : IInvoice
{
    public void Create()
    {
        Console.WriteLine("Создана счет-фактура в формате Excel");
    }
}

public class WordReport : IReport
{
    public void Generate()
    {
        Console.WriteLine("Cоздан отчет в формате Word");
    }
}

public class WordInvoice : IInvoice
{
    public void Create()
    {
        Console.WriteLine("Создана счет-фактура в формате Word");
    }
}

public interface IDocumentFactory
{
    IReport CreateReport();
    IInvoice CreateInvoice();
}

public class PdfDocumentFactory : IDocumentFactory
{
    public IReport CreateReport() => new PdfReport();

    public IInvoice CreateInvoice() => new PdfInvoice();
}

public class ExcelDocumentFactory : IDocumentFactory
{
    public IReport CreateReport() => new ExcelReport();

    public IInvoice CreateInvoice() => new ExcelInvoice();
}

public class WordDocumentFactory : IDocumentFactory
{
    public IReport CreateReport() => new WordReport();
    

    public IInvoice CreateInvoice() => new WordInvoice();
}

public class DocumentGenerator
{
    private readonly IReport _report;
    private readonly IInvoice _invoice;

    public
    DocumentGenerator(IDocumentFactory documentFactory)
    {
        _report = documentFactory.CreateReport();
        _invoice = documentFactory.CreateInvoice();
    }

    public void GenerateDocument()
    {
        _report.Generate();
        _invoice.Create();
    }
}

public static class DocumentSelector
{
    private static readonly Dictionary<string, IDocumentFactory?> DocumentFactories = [];

    static DocumentSelector()
    {
        int index = 1;
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (typeof(IDocumentFactory).IsAssignableFrom(type) && !type.IsInterface)
            {
                DocumentFactories.Add(index++.ToString(), Activator.CreateInstance(type) as IDocumentFactory);
            }
        }
    }

    public static IDocumentFactory? SelectDocument()
    {
        Console.WriteLine(LogOutput.ChooseDocument);
        foreach (var entry in DocumentFactories)
        {
            Console.WriteLine($"{entry.Key}: {entry.Value?.GetType().Name.Replace("Creator", "")}");
        }
        Console.Write(LogOutput.Choose);
        string choice = Console.ReadLine()!;
        return (DocumentFactories.GetValueOrDefault(choice));
    }
}

public static class LogOutput
{
    public const string InvalidChoose = "Неверный выбор. Попробуйте снова";
    public const string Choose = "Ваш выбор: ";
    public const string ChooseDocument = "Выберите документ:";
    public const string Continue = "Продолжить? Y(y)/N(n)";
}

internal static class Program
{
    private static bool AnalyzeToExit()
    {
        Console.WriteLine(LogOutput.Continue);
        var input = Console.ReadLine();
        if (input != "y" && input != "Y" && input != "yes" && input != "Yes")
        {
            return true;
        }
        return false;
    }
    
    static void Main()
    {
        while (true)
        {
            var factory = DocumentSelector.SelectDocument();
            if (factory != null)
            {
                DocumentGenerator documentGenerator = new DocumentGenerator(factory);
                documentGenerator.GenerateDocument();
            }
            else
                Console.WriteLine(LogOutput.InvalidChoose);

            var exit = AnalyzeToExit();
            if (exit)
                break;
        }
    }
}