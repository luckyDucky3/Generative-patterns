namespace Generative_patterns;

public interface IMailing
{
    public object Clone();
}

public class EmailMailing : IMailing
{
    public string Subject { get; set; }
    public string Body { get; set; }
    public string Footer { get; set; }
    public List<string> Attachments { get; set; } = [];
    
    public void AddAttachment(string file) => Attachments.Add(file);

    public EmailMailing DeepCopy()
    {
        var clone = (EmailMailing)this.MemberwiseClone();
        clone.Attachments = [..this.Attachments];
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
    
    public object Clone() => this.MemberwiseClone();
}

public class SmsMailing : IMailing
{
    public string SenderName { get; set; }
    public string Message { get; set; }
    
    public void Send(string phoneNumber, string recipientName)
    {
        var personalizedMessage = Message.Replace("{{Name}}", recipientName);
        Console.WriteLine($"Отправлено SMS на номер {phoneNumber} от {SenderName}:\n" +
                         $"Текст: {personalizedMessage}\n");
    }
    
    public SmsMailing DeepCopy()
    {
        return (SmsMailing)this.MemberwiseClone();
    }
    
    public object Clone() => this.MemberwiseClone();
}

internal static class Program
{
    private static void Main(string[] args)
    {
        // Email рассылка
        var baseTemplate = new EmailMailing
        {
            Subject = "Важная информация",
            Body = "Уважаемый {{Name}}, это персональное предложение для вас!",
            Footer = "С уважением, Команда поддержки"
        };

        baseTemplate.AddAttachment("Презентация.pdf");

        var emailForClientA = (EmailMailing)baseTemplate.Clone();
        emailForClientA.Subject = "Эксклюзив для вас!";
        emailForClientA.Send("client_a@mail.com", "Иван");
        
        var emailForClientB = baseTemplate.DeepCopy();
        emailForClientB.AddAttachment("Договор.docx");
        emailForClientB.Send("client_b@mail.com", "Мария");

        Console.WriteLine("Оригинальный шаблон email:");
        Console.WriteLine($"Вложения: {string.Join(", ", baseTemplate.Attachments)}\n");

        // SMS рассылка
        var baseSmsTemplate = new SmsMailing
        {
            SenderName = "Банк",
            Message = "Уважаемый {{Name}}, ваш счет был пополнен на 1000 руб."
        };

        var smsForClientA = (SmsMailing)baseSmsTemplate.Clone();
        smsForClientA.Message = "{{Name}}, ваш персональный кредит одобрен!";
        smsForClientA.Send("+79123456789", "Алексей");
        
        var smsForClientB = baseSmsTemplate.DeepCopy();
        smsForClientB.Send("+79876543210", "Ольга");

        Console.WriteLine("Оригинальный шаблон SMS:");
        Console.WriteLine($"Сообщение: {baseSmsTemplate.Message}");
    }
}