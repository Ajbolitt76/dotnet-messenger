FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Messenger.FilesService/Messenger.FilesService.csproj", "Messenger.FilesService/"]
RUN dotnet restore "Messenger.FilesService/Messenger.FilesService.csproj"
COPY . .
WORKDIR "/src/Messenger.FilesService"
RUN dotnet build "Messenger.FilesService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Messenger.FilesService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Messenger.FilesService.dll"]
