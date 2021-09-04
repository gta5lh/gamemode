FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim

# Move to working directory /build
RUN mkdir -p /server
WORKDIR /server

# Copy the code into the container
COPY . /server
COPY ./production/conf.json /server

EXPOSE 22005/tcp
EXPOSE 22005/udp
EXPOSE 22006
ENTRYPOINT ["/server/ragemp-server"]
