FROM microsoft/dotnet:1.1-runtime
#FROM microsoft/dotnet
# Set environment variables
ENV SERVICE_ENVIRONMENT="ENVIRNMNT"
COPY . /app
WORKDIR /app

EXPOSE 5000
ENTRYPOINT ["dotnet", "Byndyusoft.Dotnet.Core.Samples.Jobs.Consumer.dll"]