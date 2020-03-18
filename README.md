# ReleaseNotes-WebAPI

This is the the repository for the application server for the bachelor
project Release Notes System.

###To run the application in developer-mode use:

`docker-compose -f docker-compose.dev.yml  up --build`

###To run the application in production-mode use:

`docker-compose -f docker-compose.prod.yml  up --build`

###To run the integration tests for web-api

`docker-compose -f docker-compose.test.yml up --build`

`docker-compose down --volumes`