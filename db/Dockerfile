FROM mysql:8.0
EXPOSE 3306
COPY ./RestAPI/db/migrations/ /home/database
COPY ./RestAPI/db/dataset/ /home/database
COPY ./RestAPI/ci/init_database.sh/ /docker-entrypoint-initdb.d/init_database.sh