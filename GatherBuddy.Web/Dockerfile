FROM ubuntu:24.04

ENV WINEDEBUG="-all"
ARG BUILD_DIR=./bin/Release/net9.0-windows/win-x64/publish
ENV ASPNETCORE_URLS=http://0.0.0.0:5000

# Install Wine
RUN apt-get update && \
    DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends \
        wine64 \
        unzip \
        ca-certificates && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY ${BUILD_DIR} "/app/"

EXPOSE 5000

RUN useradd -m appuser
USER appuser
ENTRYPOINT ["/usr/lib/wine/wine64", "GatherBuddy.Web.exe"]