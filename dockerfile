FROM microsoft/dotnet:1.1-sdk
COPY . /app
WORKDIR /app
RUN dotnet restore
WORKDIR /app/src/Web.Application
RUN dotnet restore
WORKDIR /app/tests/Tests
RUN dotnet restore
RUN dotnet test --logger "trx;LogFileName=unit_tests.xml"