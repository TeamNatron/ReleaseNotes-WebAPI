# ReleaseNotes-WebAPI

This is the the repository for the application server for the bachelor
project Release Notes System.
---
Setup:

Create a new file `.env` to host all environment variables.

Include all fields that are represented in the example below

`HOST=postgres_image`

`PORT=5432`

`DB_NAME=releasenotedb`

`DB_USER=dbuser`

`DB_PASSW=dbpassword`

---
To run the application in developer-mode use:

`docker-compose -f docker-compose.dev.yml  up --build`

To run the application in production-mode use:

`docker-compose -f docker-compose.prod.yml  up --build`

To run the integration tests for web-api

`docker-compose -f docker-compose.test.yml up --build`

`docker-compose -f docker-compose.test.yml down --volumes`
