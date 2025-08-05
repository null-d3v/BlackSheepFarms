ARG VERSION=1.0

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG VERSION
WORKDIR /src

COPY ./Directory.Build.props ./
COPY ./Directory.Packages.props ./
COPY ./src/BlackSheepFarms/BlackSheepFarms.csproj ./src/BlackSheepFarms/
RUN --mount=type=cache,target=/root/.nuget/packages \
    --mount=type=cache,target=/root/.local/share/NuGet/v3-cache \
    dotnet restore -p:Version=$VERSION -r linux-musl-x64 src/BlackSheepFarms/BlackSheepFarms.csproj

COPY . .
WORKDIR /src/src/BlackSheepFarms
RUN --mount=type=cache,target=/root/.nuget/packages \
    --mount=type=cache,target=/root/.local/share/NuGet/v3-cache \
    dotnet publish -c Release --no-restore -o /app -p:Version=$VERSION -r linux-musl-x64 --self-contained false

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
