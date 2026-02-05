FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Install Chromium dependencies for PuppeteerSharp PDF generation
RUN apt-get update && apt-get install -y \
    wget \
    gnupg \
    ca-certificates \
    fonts-liberation \
    libasound2 \
    libatk-bridge2.0-0 \
    libatk1.0-0 \
    libcups2 \
    libdbus-1-3 \
    libdrm2 \
    libgbm1 \
    libgtk-3-0 \
    libnspr4 \
    libnss3 \
    libxcomposite1 \
    libxdamage1 \
    libxfixes3 \
    libxkbcommon0 \
    libxrandr2 \
    xdg-utils \
    libu2f-udev \
    libvulkan1 \
    && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["AutoDokas.csproj", "./"]
RUN dotnet restore "AutoDokas.csproj"
COPY . .
RUN dotnet build "AutoDokas.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AutoDokas.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

# Create directory for SQLite database persistence
RUN mkdir -p /app/data

COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV PUPPETEER_EXECUTABLE_PATH=/root/.cache/puppeteer/chrome/linux-*/chrome-linux64/chrome

ENTRYPOINT ["dotnet", "AutoDokas.dll"]