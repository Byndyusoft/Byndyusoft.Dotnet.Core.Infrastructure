FROM microsoft/dotnet:2.1-sdk as build-box
COPY . /app
WORKDIR /app
RUN dotnet restore 

# Tests
ARG testspath
ENV testspath=$testspath
WORKDIR  $testspath
RUN dotnet test --logger "trx;LogFileName=/app/TestResults/unit_tests.xml"

# Build
FROM build-box as publish
ARG projectpath
ENV projectpath=$projectpath
WORKDIR $projectpath
RUN dotnet  publish -o /app/out

# Build migrations 
# WORKDIR migrator
# RUN dotnet  publish -o /app/migrations

FROM microsoft/dotnet:2.1.1-aspnetcore-runtime as runtime
ARG projectdll
ENV projectdll=$projectdll
ARG arg1
ENV arg1=$arg1
ARG arg2
ENV arg2=$arg2
WORKDIR /app
COPY --from=publish /app/out ./
# COPY --from=publish /app/migrations /migrations/
COPY --from=publish /app/TestResults ./
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000
ENTRYPOINT dotnet $projectdll $arg1 $arg2