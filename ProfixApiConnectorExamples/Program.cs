
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
        config.CallbackUrl= "http://profix-it.nl/callback";
    });

}).Build();

var authorizer = x.Services.GetRequiredService<IProfixAuthorizer>();

//you should invoke the url to go to the login screen for yourself
var result = authorizer.Connect("authorizationcode");


if (result == ConnectionStatus.Ok)
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
