// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable SuggestVarOrType_Elsewhere


var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddSqlServer("db")
                .PublishAsContainer()
                .AddDatabase("YemekhaneDb");

var api = builder.AddProject<Projects.YemekhaneApp_Api>("api")
                 .WithReference(db)
                 .WaitFor(db);

builder.AddProject<Projects.YemekhaneApp_Frontend>("frontend")
       .WithReference(api)
       .WaitFor(api);


// var api = builder.AddContainer("api", "teknokentyemekhaneapp-api:latest")
//     .WithReference(db)
//     .WithEndpoint(5000, targetPort: 5000, isExternal:true)
//     .WaitFor(db);
//
// var ui = builder.AddContainer("webui", "teknokentyemekhaneapp-frontend:latest")
//     .WithEndpoint(5100, targetPort: 5100, isExternal: true)
//     .WithEnvironment("services__api__http__0", "http://api:5000")
//     .WaitFor(api);

builder.Build().Run();