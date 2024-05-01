FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Gourmet.WebApi/Gourmet.WebApi.csproj", "Gourmet.WebApi/"]
COPY ["Gourmet.Core/Gourmet.Core.csproj", "Gourmet.Core/"]
RUN dotnet restore "Gourmet.WebApi/Gourmet.WebApi.csproj"
COPY . .
WORKDIR "/src/Gourmet.WebApi"
RUN dotnet build "Gourmet.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gourmet.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gourmet.WebApi.dll"]
