// See https://aka.ms/new-console-template for more information



using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperInject.Console;
using SuperInject.Extensions;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, service) =>
    {
        service.AddSuperInject();
    }).Build();


var myService = host.Services.GetRequiredService<IMyService>();

// Print result to console
myService.DoSomething();
Console.ReadLine();
