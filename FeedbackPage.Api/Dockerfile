#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["FeedbackPage.Api/FeedbackPage.Api.csproj", "FeedbackPage.Api/"]
COPY ["FeedbackPage.Dal/FeedbackPage.Dal.csproj", "FeedbackPage.Dal/"]
RUN dotnet restore "FeedbackPage.Api/FeedbackPage.Api.csproj"
COPY . .
WORKDIR "/src/FeedbackPage.Api"
RUN dotnet build "FeedbackPage.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FeedbackPage.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FeedbackPage.Api.dll"]