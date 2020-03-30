FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers (so that we don't rebuild the deps layer needlessly)
COPY *.sln .
COPY DiscordGitToolbox.App/*.csproj ./DiscordGitToolbox.App/
COPY DiscordGitToolbox.Core/*.csproj ./DiscordGitToolbox.Core/
COPY DiscordGitToolbox.Core.Tests/*.csproj ./DiscordGitToolbox.Core.Tests/
COPY DiscordGitToolbox.Discord/*.csproj ./DiscordGitToolbox.Discord/
COPY DiscordGitToolbox.GitHub/*.csproj ./DiscordGitToolbox.GitHub/
COPY DiscordGitToolbox.GitHub.Tests/*.csproj ./DiscordGitToolbox.GitHub.Tests/
RUN dotnet restore

# Copy everything else and build
COPY . ./
WORKDIR /app/DiscordGitToolbox.App
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:3.1
WORKDIR /app
COPY --from=build-env /app/DiscordGitToolbox.App/out .
ENTRYPOINT ["dotnet", "DiscordGitToolbox.App.dll"]
