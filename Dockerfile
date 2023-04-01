FROM mcr.microsoft.com/dotnet/sdk:7.0 as base
WORKDIR /src
EXPOSE 80
COPY /NotesApi/*.csproj .
RUN dotnet restore NotesApi.csproj

FROM base as publish 
COPY . .
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NotesApi.dll"]
