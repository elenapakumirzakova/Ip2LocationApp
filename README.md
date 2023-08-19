# Ip2Location Service

## What is Ip2Location?

The IP-to-Location service is a simple RESTful API that provides geographical location information for a given IP address.

The app is using an in-memory database and in-memory cache for a Minimum Viable Product (MVP). 
In order to have a quick and efficient way to prototype and test the functionality of an application without the overhead of setting up and managing a persistent database. 

  - [Features](#Features)
  - [Getting started](#Getting-started)
  - [Usage](#Usage)
  - [Caching](#Caching)
  - [ToDo List](#ToDo-List)
  - [Contributing](#Contributing)
  - [Links](#Links)
  - [License](#License)

### Features
Resolve IP addresses to a geographical location (latitude, longitude).
Supports both IPv4 and IPv6 addresses.
Built-in caching for faster responses.

### Getting started

#### Prerequisites
  - .NET7

#### Installation
Clone the repository:
```
git clone https://github.com/yourusername/ip-to-location.git
```

Navigate to the project directory:
```
cd ip-to-location
```

Restore packages and build the project:
```
dotnet restore
dotnet build
```

#### Configuration
Update the appsettings.json with the required configurations:

  - ApiKey: API key for the third-party geolocation service (update if required).

#### Running the Service
From the project directory, run:

```
dotnet run
The service will start, and you can access it at http://localhost:5000.
```

### Usage
##### Get Location for an IP
Endpoint: /location

Parameter: ip

Method: GET

Example Request:
```
GET http://localhost:5000/location?ip=8.8.8.8
```

Example Response:
```
{
    "ip": "8.8.8.8",
    "type": "ipv4",
    "country": "Germany",
    "region": "Berlin",
    "city": "Berlin",
    "zip": "12099",
    "latitude": 52.45899963378906,
    "longitude": 13.399999618530273
}
```

## Caching
The service caches IP location results in In-Memory cache to speed up consecutive requests for the same IP address. The default cache duration is 24 hours, but you can adjust this in appsettings.json.

## ToDo List
What supposed to be done next:
  - Extending of 3d party subscription plan(for now used a free version with 1000 requests per month)
  - Errorhandler
  - Add additional test cases
  - CI Deploy (Cloud preffered)
  - Replace In-Memory DB with persistent DB
  - Replace In-Memory Cache with Redis or any similar

## Contributing
If you'd like to contribute, please fork the repository and use a feature branch.
Pull requests are warmly welcome :-)

## Links
Author: https://www.linkedin.com/in/yelena-pak-umirzakova

## License
Its use is governed by MIT License (X11 License).