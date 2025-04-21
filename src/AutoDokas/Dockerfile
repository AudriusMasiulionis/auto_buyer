FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["AutoDokas.csproj", "./"]
RUN dotnet restore "AutoDokas.csproj"
COPY . .
RUN dotnet build "AutoDokas.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AutoDokas.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AutoDokas.dll"]