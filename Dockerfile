FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore ReleaseNotes-WebApi.csproj
COPY . ./
RUN dotnet publish ReleaseNotes-WebApi.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 5000