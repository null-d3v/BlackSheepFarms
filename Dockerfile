FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . ./
RUN dotnet restore
RUN dotnet publish -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /src
COPY --from=build /app .
ENTRYPOINT ["./BlackSheepFarms"]
