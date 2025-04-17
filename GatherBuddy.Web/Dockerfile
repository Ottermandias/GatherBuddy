FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_TYPE=Debug

WORKDIR /build

COPY . ./

RUN dotnet restore GatherBuddy.Web/GatherBuddy.Web.csproj
RUN dotnet publish GatherBuddy.Web/GatherBuddy.Web.csproj -c ${BUILD_TYPE} -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
WORKDIR /app

COPY --from=build /build/out ./

EXPOSE 5000

CMD ["dotnet", "GatherBuddy.Web.dll"]
