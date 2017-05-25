FROM microsoft/dotnet:1.1-sdk
COPY . /app
WORKDIR /app
RUN dotnet restore
WORKDIR /app/src/Web.Application
RUN dotnet restore
WORKDIR /app/src/Web.Application