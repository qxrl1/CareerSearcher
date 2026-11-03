FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/playwright/dotnet:v1.64.1-noble AS runtime
WORKDIR /runtime

COPY --from=build /app/out ./
EXPOSE 8080

ENTRYPOINT ["dotnet", "CareerSearcher.API.dll"]
