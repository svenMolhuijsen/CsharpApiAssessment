# CsharpApiAssessment
An API showcasing my current abilities in C#
A simple API for managing addresses, with advanced searching and sorting and calculating distances between them

## Getting Started

To get started with the Addresses API, follow these steps:

## Prerequisites

Make sure you have the following software installed:

- .NET 6.0 SDK
- SQLite (for local development)

## Installation

1. Clone the repository:

2. navigate to CsharpApiAssessment/API or open the project in visual studio

make sure all paths in Program.cs point to the right place in your environment

run commands:

dotnet restore
dotnet build
dotnet run

Or run through visual studio (IIS express is tested)

## Not implemented
* Unit tests 
* Logging

## Problems encountered / Choices made
* At first i tried working with my own databse design. but later is switched to using Entity framework to accomplish the dynamic query in advanced search since SQLite doesnt support stored procedures.
* The advanced sorting was hard to implement because of limited knowledge on LINQ and Entity Framework
* The distances are calculated using https://nominatim.org/ this has a rate limit of one request per second, I use this service to get coordinates for adddresses without needing an API key being published online. I'm using https://www.nuget.org/packages/GeoCoordinate.NetCore to calculate the distance between these coordinate 'as the crow flies'.


