var builder = DistributedApplication.CreateBuilder(args);

var keycloak = builder.AddKeycloak("keycloak", 8080)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithRealmImport("../realms");

var webApi = builder.AddProject<Projects.Keycloak_Api>("webapi")
    .WithReference(keycloak).WaitFor(keycloak)
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.Keycloak_Web>("webfrontend")
    .WithReference(keycloak).WaitFor(keycloak)
    .WithReference(webApi)
    .WithExternalHttpEndpoints();

builder.Build().Run();
