using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Profix.Api.Connector;

var host = Host.CreateDefaultBuilder(args);
var x = host.ConfigureServices(services => {
    services.AddProfixConnection();

}).Build();

var authorizer = x.Services.GetRequiredService<IProfixAuthorizer>();
IProfixConnection conn = authorizer.GetConnection();
conn.SubScriptions.Post("webhookname", "Some payload");
