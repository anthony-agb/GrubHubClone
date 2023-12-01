#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GrubHubClone.Payment/GrubHubClone.Payment.csproj", "GrubHubClone.Payment/"]
COPY ["GrubHubClone.Common.Dtos/GrubHubClone.Common.Dtos.csproj", "GrubHubClone.Common.Dtos/"]
COPY ["GrubHubClone.Common.Models/GrubHubClone.Common.Models.csproj", "GrubHubClone.Common.Models/"]
COPY ["GrubHubClone.Common.Exceptions/GrubHubClone.Common.Exceptions.csproj", "GrubHubClone.Common.Exceptions/"]
COPY ["GrubHubClone.Common/GrubHubClone.Common.csproj", "GrubHubClone.Common/"]
RUN dotnet restore "GrubHubClone.Payment/GrubHubClone.Payment.csproj"
COPY . .
WORKDIR "/src/GrubHubClone.Payment"
RUN dotnet build "GrubHubClone.Payment.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GrubHubClone.Payment.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrubHubClone.Payment.dll"]