# Use the official ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# Set ASPNETCORE_HTTP_PORTS to 10000 to match Render's default PORT
ENV ASPNETCORE_HTTP_PORTS=10000
EXPOSE 10000

# Use the official SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# Copy the project file and restore dependencies
COPY ["EnhanzerAssessment.Api.csproj", "./"]
RUN dotnet restore "./EnhanzerAssessment.Api.csproj"
# Copy the remaining source code
COPY . .
WORKDIR "/src/."
# Build the application in Release mode
RUN dotnet build "EnhanzerAssessment.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "EnhanzerAssessment.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Start the application
ENTRYPOINT ["dotnet", "EnhanzerAssessmentAPI2.dll"]
