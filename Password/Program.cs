using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Profix.Api.Connector;
using Profix.Api.Connector.ApiObjects;

var host = Host.CreateDefaultBuilder(args);
var x = host.ConfigureServices(services => {
    services.AddProfixConnection(config =>
    {
        config.ClientID = "client-id";
        config.Secret = "client-secret";
    });

}).Build();

var authorizer = x.Services.GetRequiredService<IProfixAuthorizer>();

var loginResult = authorizer.Login("development@profix-it.nl", "your_supersecret_password");
if (loginResult == ConnectionStatus.PasswordMustChange)
{
    Console.WriteLine("change password:");
    var newPW = Console.ReadLine();
    if (authorizer.ChangePassword("your_supersecret_password", newPW))
    {
        loginResult = authorizer.Login("development@profix-it.nl", newPW);
    }
    else
    {
        Console.WriteLine("Password change failed");  
        Console.ReadKey();
        return;
    }
}
if (loginResult == ConnectionStatus.Ok)
{
    IProfixConnection conn = authorizer.GetConnection();
    ApiResult<ICollection<Application>> applist = conn.Applications.Get();
    if (applist.Success)
    {
        authorizer.CurrentApplication = applist.Result.First().Id;
    }
    else
    {
        Console.Write(applist.ErrorMessage);
        Console.ReadKey();
        return;
    }
    ApiResult<Me> me = conn.Me.Get();
}