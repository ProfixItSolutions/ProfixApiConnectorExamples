# ProfixApiConnectorExamples
Examples for the Profix Api Connector

## Step 1:
[Downnload Nuget Package](https://www.nuget.org/packages/Profix.Api.Connector/)

## Step 2:
Add Profix to the service:
`services.AddProfixConnection();`
Mostlikely you need to add your app credentials in here:

```C#
	services.AddProfixConnection(config =>{
        config.ClientID = "client-id";
        config.Secret = "client-secret";
        config.CallbackUrl= "http://profix-it.nl/callback";
	});
```
Default all services included with be added as Scoped. 
If your application preferres a SingleTon instantiation instead (like a WinForms application) you can do that by including this in the configuration

```C#
	services.AddProfixConnection(config =>{
        config.ClientID = "client-id";
        config.Secret = "client-secret";
        config.CallbackUrl= "http://profix-it.nl/callback";
		config.DeclarationType = DeclarationType.SingleTon;
	});
```

## Step 3:
Getting a connector can on two ways.
When you are logged in (see examples based on how to that) you can create an Connection from the Authorizer

```C#
	IProfixAuthorizer authorizer = _serviceProvider.GetRequiredService<IProfixAuthorizer>();
	ConnectionStatus result = authorizer.Connect(); //ClientCredentials
	if (result == ConnectionStatus.Ok)
	{
		IProfixConnection conn = authorizer.GetConnection();
	}
```

If you have already established connection in another class you can also directly call IProfixConnection in your class or by the ServiceProvider

```C#
	IProfixConnection conn = _serviceProvider.GetRequiredService<IProfixConnection>();
```

## Step 4
You always have to set your current application (Except for subscriptions)
```C#
	IProfixAuthorizer authorizer = _serviceProvider.GetRequiredService<IProfixAuthorizer>();
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
```


## Step 5
Reuse tokens:
Retreive the current token:
```C#
IProfixAuthorizer authorizer = _serviceProvider.GetRequiredService<IProfixAuthorizer>();
var token = authorizer.GetToken();
SaveToDatabase(token.AccessToken, token.RefreshToken);
```

Setting an pre-existing token:
```C#
IProfixAuthorizer authorizer = _serviceProvider.GetRequiredService<IProfixAuthorizer>();
authorizer.LoadAccessToken("access_token", "refresh_token");
IProfixConnection conn = authorizer.GetConnection();
```

## Step 6
Retreive Me Information, as Me returns some basic Information about the current user.
```C#

    ApiResult<Me> me = conn.Me.Get();
    if (me.Success)
    {
        var actualResult = me.Result;
    }
    else
    {
        throw new Exception(me.ErrorMessage);
    }
```
