FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Supperxin.WechatMessager/Supperxin.WechatMessager.csproj", "Supperxin.WechatMessager/"]
RUN dotnet restore "Supperxin.WechatMessager/Supperxin.WechatMessager.csproj"
COPY . .
WORKDIR "/src/Supperxin.WechatMessager"
RUN dotnet build "Supperxin.WechatMessager.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Supperxin.WechatMessager.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Supperxin.WechatMessager.dll"]
