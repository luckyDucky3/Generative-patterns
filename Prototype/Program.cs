namespace Generative_patterns;

public class EmailTemplate : ICloneable
{
    public string Subject { get; set; }
    public string Body { get; set; }
    public string Footer { get; set; }
    public List<string> Attachments { get; set; } = [];

    // Метод для добавления вложения
    public void AddAttachment(string file) => Attachments.Add(file);

    // Поверхностное копирование (не копирует вложения!)
    public object Clone() => this.MemberwiseClone();

    // Глубокое копирование
    public EmailTemplate DeepCopy()
    {
        var clone = (EmailTemplate)this.MemberwiseClone();
        clone.Attachments = new List<string>(this.Attachments); // Копируем список
        return clone;
    }

    public void Send(string recipientEmail, string recipientName)
    {
        var personalizedBody = Body.Replace("{{Name}}", recipientName);
        Console.WriteLine($"Отправлено письмо для {recipientEmail}:\n" +
                          $"Тема: {Subject}\n" +
                          $"Текст: {personalizedBody}\n" +
                          $"Подпись: {Footer}\n" +
                          $"Вложения: {string.Join(", ", Attachments)}\n");
    }
}

internal static class Program
{
    private static void Main(string[] args)
    {
        var baseTemplate = new EmailTemplate
        {
            Subject = "Важная информация",
            Body = "Уважаемый {{Name}}, это персональное предложение для вас!",
            Footer = "С уважением, Команда поддержки"
        };

        baseTemplate.AddAttachment("Презентация.pdf");
        
        // Для клиента A (поверхностная копия)
        var emailForClientA = (EmailTemplate)baseTemplate.Clone();
        emailForClientA.Subject = "Эксклюзив для вас!";
        emailForClientA.Send("client_a@mail.com", "Иван");

        // Для клиента B (глубокая копия + новое вложение)
        var emailForClientB = baseTemplate.DeepCopy();
        emailForClientB.AddAttachment("Договор.docx");
        emailForClientB.Send("client_b@mail.com", "Мария");

        // Оригинальный шаблон не изменился
        Console.WriteLine("Оригинальный шаблон:");
        Console.WriteLine($"Вложения: {string.Join(", ", baseTemplate.Attachments)}");
    }
}