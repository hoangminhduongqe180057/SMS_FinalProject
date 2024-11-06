# Use the official ASP.NET Core image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj files and restore dependencies
COPY ["StudentsManagement/StudentsManagement.csproj", "StudentsManagement/"]
COPY ["StudentsManagement.Client/StudentsManagement.Client.csproj", "StudentsManagement.Client/"]
COPY ["StudentsManagement.Shared/StudentsManagement.Shared.csproj", "StudentsManagement.Shared/"]

RUN dotnet restore "StudentsManagement/StudentsManagement.csproj"

# Copy the rest of the application source code
COPY . .

# Build the application
WORKDIR "/src/StudentsManagement"
RUN dotnet build "StudentsManagement.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "StudentsManagement.csproj" -c Release -o /app/publish

# Create the final image for running the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudentsManagement.dll"]
