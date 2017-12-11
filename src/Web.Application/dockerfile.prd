FROM microsoft/dotnet:2.0-runtime
WORKDIR /app
COPY . /app
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT="ENVIRNMNT"
EXPOSE 5000
ENTRYPOINT ["dotnet", "Byndyusoft.Dotnet.Core.Samples.Web.Application.dll","--xml_docs","Byndyusoft.Dotnet.Core.Samples.Web.Application.xml"]