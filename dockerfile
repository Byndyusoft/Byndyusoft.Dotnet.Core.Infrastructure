FROM microsoft/dotnet:2.0-sdk
COPY . /app
WORKDIR /app
RUN dotnet restore
WORKDIR /app/src/Web.Application
RUN dotnet restore
WORKDIR /app/src/Web.Application