# Friendlizer

Code challenge task for LinkSoft.

## Prerequisites

The following software must be installed:
* Visual Studio 2019, version 16.11.3
* NodeJS at least 14.17.0
* NPM at least 6.14.13

## How to run

1. Open Friendlizer.sln in Visual Studio.
1. Click "Run IIS Express". Visual Studio will install required dependencies, build the project and open Swagger showing available API.
1. Navigate to `Frontend/friendlizer` folder.
1. Run `npm install`.
1. Run `ng serve`.
1. Open http://localhost:4200/

## Implementation Remarks

Since this is only a testing software (not intended for production uses), the following simplifications were done:
* Frontend uses hardcoded endpoint URLs (real app would probably use some config).
* Frontend is small and doesn't really separate data retrieval logic into separate components/services. Real-life app would certainly do it.
* Backend uses in-memory database (not bound to a real SQL database).
* Backend contains SQL/LINQ queries directly in controller. In real-life app this logic would be probably moved to separate classes but for a small testing app this is still maintanable.
* Relations for graph a returned by a single call. For testing purposes of 88 thousand records this is still ok but in real-life app it would be good to read in pages.