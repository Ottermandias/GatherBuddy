namespace GatherBuddy.KeyGenerator;

class Program
{
    static void Main(string[] args)
    {
        var key = Guid.NewGuid();
        Console.WriteLine(key);
    }
}
