FROM microsoft/dotnet:2.1-sdk as builder

WORKDIR /app
ADD . /app

RUN dotnet build -c Release

RUN cat ./scripts/.travis.sdk2.txt | xargs -I {} dotnet test -c Release -f {} --no-build ./tests/msgpack.spec.tests/msgpack.spec.tests.csproj -- -parallel assemblies

FROM microsoft/dotnet:1.1-sdk

WORKDIR /app

COPY --from=builder /app .

RUN dotnet restore

RUN cat ./scripts/.travis.sdk1.txt | xargs -I {} dotnet test -c Release -f {} --no-build ./tests/msgpack.spec.tests/msgpack.spec.tests.csproj -- -parallel assemblies
