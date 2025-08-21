# (!!!) Файл используется для локальной разработки Visual Studio.
# Изменять только ознакомившись с документацией по ссылке ниже.
# Стадия base должна обязательно присутствовать, с неё запускается локальная отладка в Docker.

# See https://aka.ms/customizecontainer to learn how to customize your debug container
# and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Docker build запукается по ум. из папки с файлом sln.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Install Node & Yarn.
WORKDIR /
RUN apt-get update && apt-get install -y ca-certificates curl gnupg
RUN mkdir -p /etc/apt/keyrings
ENV NODE_MAJOR=20
RUN curl -fsSL https://deb.nodesource.com/gpgkey/nodesource-repo.gpg.key\
	| gpg --dearmor -o /etc/apt/keyrings/nodesource.gpg
RUN echo "deb [signed-by=/etc/apt/keyrings/nodesource.gpg] https://deb.nodesource.com/node_$NODE_MAJOR.x nodistro main"\
	| tee /etc/apt/sources.list.d/nodesource.list
RUN apt-get update && apt-get install -y nodejs
RUN rm -rf /var/lib/apt/lists/*
RUN corepack enable && yarn set version stable
RUN node --version && yarn --version

# Copy project files & restore NuGet.
WORKDIR /src
COPY . .
RUN dotnet restore "Demo.DbRefreshManager.WebApi/Demo.DbRefreshManager.WebApi.csproj"

# Restore Yarn packages.
WORKDIR "/src/Demo.DbRefreshManager.WebUI"
RUN yarn

# Build project.
WORKDIR "/src/Demo.DbRefreshManager.WebApi"
RUN dotnet build "Demo.DbRefreshManager.WebApi.csproj" -c Release -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
WORKDIR "/src/Demo.DbRefreshManager.WebApi"
RUN dotnet publish "Demo.DbRefreshManager.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Demo.DbRefreshManager.WebApi.dll"]