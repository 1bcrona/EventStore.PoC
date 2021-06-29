FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.8.0/wait /wait
RUN chmod +x /wait
FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
ADD https://github.com/ufoscout/docker-compose-wait/releases/download/2.8.0/wait /wait
RUN chmod +x /wait

EXPOSE 5000





WORKDIR /src

COPY ["EventStore.sln", "EventStore.sln"]
COPY ["EventStore.API/EventStore.API.csproj","EventStore.API/" ]
COPY ["EventStore.Domain/EventStore.Domain.csproj","EventStore.Domain/" ]
COPY ["EventStore.Store/EventStore.Store.csproj","EventStore.Store/" ]
COPY ["EventStore.StreamListener/EventStore.StreamListener.csproj","EventStore.StreamListener/" ]

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
CMD ["dotnet", "EventStore.StreamListener.dll"]

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "EventStore.API.dll"]

