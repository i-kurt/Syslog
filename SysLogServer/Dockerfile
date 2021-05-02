FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SysLogServer.csproj", "."]
RUN dotnet restore "./SysLogServer.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SysLogServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SysLogServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SysLogServer.dll"]