FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ./src/BlackSheepFarms/BlackSheepFarms.csproj ./src/BlackSheepFarms/
RUN dotnet restore -r linux-musl-x64 src/BlackSheepFarms/BlackSheepFarms.csproj

COPY . .
WORKDIR /src/src/BlackSheepFarms
RUN dotnet publish -c Release --no-restore -o /app -r linux-musl-x64 --self-contained false

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine

ENV \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    LC_ALL=en_US.UTF-8 \
    LANG=en_US.UTF-8
RUN apk add --no-cache \
    icu-data-full \
    icu-libs

WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "BlackSheepFarms.dll"]
