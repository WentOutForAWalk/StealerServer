FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["StealerServer/StealerServer.csproj", "StealerServer/"]
RUN dotnet restore "StealerServer/StealerServer.csproj"


COPY . .

WORKDIR "/src/StealerServer"
RUN dotnet publish "StealerServer.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "StealerServer.dll"]
