FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore ReleaseNotes-WebAPI.csproj
COPY . ./
RUN dotnet publish ReleaseNotes-WebAPI.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "ReleaseNotes-WebAPI.dll"]
EXPOSE 5000