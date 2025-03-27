using System.Reflection;

namespace Factory_method;

public abstract class Document
{
    public abstract void OutputName();
}

public class PdfDocument : Document
{
    public PdfDocument()
    {
        OutputName();
        Console.WriteLine(" документ сгенерирован");
    }

    public sealed override void OutputName()
    {
        Console.Write("Pdf");
    }
}

public class ExcelDocument : Document
{
    public ExcelDocument()
    {
        OutputName();
        Console.WriteLine(" документ сгенерирован");
    }

    public sealed override void OutputName()
    {
        Console.Write("Excel");
    }
}

public class WordDocument : Document
{
    public WordDocument()
    {
        OutputName();
        Console.WriteLine(" документ сгенерирован");
    }

    public sealed override void OutputName()
    {
        Console.Write("Word");
    }
}

public abstract class Creator
{
    public abstract Document CreateDocument();
}

public class PdfCreator : Creator
{
    public override Document CreateDocument()
    {
        return new PdfDocument();
    }
}

public class ExcelCreator : Creator
{
    public override Document CreateDocument()
    {
        return new ExcelDocument();
    }
}

public class WordCreator : Creator
{
    public override Document CreateDocument()
    {
        return new WordDocument();
    }
}

public static class DocumentSelector
{
    private static readonly Dictionary<string, Creator?> DocumentFactories = [];

    static DocumentSelector()
    {
        int index = 1;
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (type.IsSubclassOf(typeof(Creator)))
            {
                DocumentFactories.Add(index++.ToString(), Activator.CreateInstance(type) as Creator);
            }
        }
    }

    public static Creator? SelectDocument()
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
                Document document = factory.CreateDocument();
                document.OutputName();
            }
            else
                Console.WriteLine(LogOutput.InvalidChoose);

            var exit = AnalyzeToExit();
            if (exit)
                break;
        }
    }
}