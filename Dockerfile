FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/*

EXPOSE 5001
EXPOSE 5000
EXPOSE 5002



WORKDIR /src

COPY ["EventStore.sln", "EventStore.sln"]

COPY src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done


RUN dotnet restore

COPY . .

WORKDIR /src
RUN dotnet build -c Release

FROM build AS runtime
WORKDIR "/src/EventStore.StreamListener"
RUN dotnet publish "EventStore.StreamListener.csproj" --no-build --no-restore -c Release -o /app/stream_listener

FROM build AS publish
WORKDIR "/src/EventStore.API"
RUN dotnet publish "EventStore.API.csproj" --no-build --no-restore -c Release -o /app/publish


FROM build AS stream_listener
WORKDIR /app
COPY --from=runtime /app/stream_listener .
CMD ["dotnet", "BetCR.Scheduled.dll"]

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "BetCR.Web.dll"]

