// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperInject.Console;
using SuperInject.Extensions;
using System.Reflection;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, service) =>
    {
        service.AddSuperInject(x=>
        {
            x.Assemblies = [Assembly.GetExecutingAssembly()];
        });
    }).Build();


var myService = host.Services.GetRequiredService<IMyService>();

// Print result to console
myService.DoSomething();
Console.ReadLine();
