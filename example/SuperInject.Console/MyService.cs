namespace SuperInject.Console;

[Service(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped)]
public class MyService : IMyService
{
    public void DoSomething()
    {
        System.Console.WriteLine("##########################################");
        System.Console.WriteLine();
        System.Console.WriteLine(">>>> Resolve the { IMyService } interface and print this string value to console.");
        System.Console.WriteLine();
        System.Console.WriteLine("##########################################");
    }
}


public interface IMyService
{
    void DoSomething();
}
