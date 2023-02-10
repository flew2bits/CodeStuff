FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CodeStuff/CodeStuff.csproj", "CodeStuff/"]
RUN dotnet restore "CodeStuff/CodeStuff.csproj"
COPY . .
WORKDIR "/src/CodeStuff"
RUN dotnet build "CodeStuff.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CodeStuff.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeStuff.dll"]
