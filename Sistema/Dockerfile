FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY Sistema.csproj ./
RUN dotnet restore Sistema.csproj
COPY . ./
RUN dotnet build Sistema.csproj --no-restore -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
COPY --from=build /app/build ./
USER $APP_UID
ENTRYPOINT ["dotnet", "Sistema.dll"]
