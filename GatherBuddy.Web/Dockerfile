FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG SECRET_KEY=ghp_7bf8162c0dda421885645d6f51c4ce6bd7fa
#THIS IS NOT A REAL KEY

WORKDIR /build

COPY . ./

RUN dotnet restore GatherBuddy.Web/GatherBuddy.Web.csproj
RUN dotnet publish GatherBuddy.Web/GatherBuddy.Web.csproj -c Release -o out /p:SecretKey=${SECRET_KEY}
RUN cat /build/GatherBuddy.SecretKeys/SecretKeys.generated.cs

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
WORKDIR /app

COPY --from=build /build/out ./

EXPOSE 5000

CMD ["dotnet", "GatherBuddy.Web.dll"]
