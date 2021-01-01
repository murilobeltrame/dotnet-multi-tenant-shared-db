FROM mcr.microsoft.com/mssql/server:2019-latest

COPY entrypoint.sh .
COPY create-db.sh .
COPY setup.sql .

USER root

RUN chmod +x entrypoint.sh
RUN chmod +x create-db.sh

USER mssql

ENTRYPOINT /bin/bash entrypoint.sh