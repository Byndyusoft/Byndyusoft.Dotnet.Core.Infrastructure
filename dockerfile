FROM microsoft/dotnet:2.0-sdk
COPY . /app
WORKDIR /app
RUN dotnet restore
WORKDIR /app/src/Web.Application
RUN dotnet restore
WORKDIR /app/src/Jobs/Consumer
RUN dotnet restore
WORKDIR /app/tests/Tests
RUN dotnet restore
RUN dotnet test --logger "trx;LogFileName=unit_tests.xml"
