FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build-env
# @sha256:35792ea4ad1db051981f62b313f1be3b46b1f45cadbaa3c288cd0d3056eefb83 

WORKDIR /App

COPY . ./

RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy
# @sha256:6c4df091e4e531bb93bdbfe7e7f0998e7ced344f54426b7e874116a3dc3233ff
RUN apt-get update 
RUN apt-get install --upgrade -y libldap-2.5.0
RUN ln -s /usr/lib/x86_64-linux-gnu/libldap.so.2 /usr/lib/x86_64-linux-gnu/{libldap-2.5.so.0,libldap-2.4.so.2}
RUN apt-get update && \
    apt-get install -yq tzdata && \
    ln -fs /usr/share/zoneinfo/America/La_Paz /etc/localtime && \
    dpkg-reconfigure -f noninteractive tzdata

WORKDIR /App
RUN mv /etc/ssl/openssl.cnf /etc/ssl/openssl.cnf.bak
COPY --from=build-env /App/openssl.cnf /etc/ssl/
COPY --from=build-env /App/Assets ./Assets
COPY --from=build-env /App/Templates ./Templates
COPY --from=build-env /App/Fonts ./Fonts
COPY --from=build-env /App/out .

ENTRYPOINT ["dotnet", "siapv_backend.dll"]