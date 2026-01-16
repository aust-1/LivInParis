FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY *.sln ./
COPY src/LivInParis.Domain/*.csproj ./src/LivInParis.Domain/
COPY src/LivInParis.Infrastructure/*.csproj ./src/LivInParis.Infrastructure/
COPY src/LivInParis.Services/*.csproj ./src/LivInParis.Services/
COPY src/LivInParis.Api/*.csproj ./src/LivInParis.Api/
COPY src/LivInParis.Tests/*.csproj ./src/LivInParis.Tests/
RUN dotnet restore

COPY . .

RUN dotnet publish src/LivInParis.Api/LivInParis.Api.csproj -c Release -o /app

# Copy frontend assets into API output
RUN mkdir -p /app/frontend
RUN cp -r /src/frontend/* /app/frontend/


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "LivInParis.Api.dll"]
