FROM vendeq.azurecr.io/vendeq-aspnetcore-runtime:latest AS base
COPY . /Vendeq.Integrations.Api
WORKDIR /Vendeq.Integrations.Api
ENTRYPOINT ["dotnet", "Vendeq.Integrations.Api.dll"]
