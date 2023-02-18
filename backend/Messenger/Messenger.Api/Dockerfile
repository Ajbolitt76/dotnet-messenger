
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS prepare-restore-files
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install --global --no-cache dotnet-subset --version 0.3.1
WORKDIR /src
COPY . .
RUN dotnet subset restore "Messenger.Api/Messenger.Api.csproj" --root-directory /src --output restore_subset/


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG CI_JOB_TOKEN
WORKDIR /src
RUN echo "Token: $CI_JOB_TOKEN"
RUN dotnet nuget add source "https://gitlab.com/api/v4/groups/57111509/-/packages/nuget/index.json" --store-password-in-clear-text --name repet-gitlab --username gitlab-ci-token --password $CI_JOB_TOKEN
COPY --from=prepare-restore-files /src/restore_subset .
RUN dotnet restore "Messenger.Api/Messenger.Api.csproj"

COPY . .
WORKDIR "/src/Messenger.Api"
RUN dotnet build "Messenger.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Messenger.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish ./
ENTRYPOINT ["dotnet", "Messenger.Api.dll"]
