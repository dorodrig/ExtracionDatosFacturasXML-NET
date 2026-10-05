# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ExtraerDatosFacturasXML.csproj", "./"]
RUN dotnet restore "ExtraerDatosFacturasXML.csproj"
COPY . .
RUN dotnet build "ExtraerDatosFacturasXML.csproj" -c Release -o /app/build
RUN dotnet publish "ExtraerDatosFacturasXML.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa de ejecución (Runtime/Final)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ExtraerDatosFacturasXML.dll"]
