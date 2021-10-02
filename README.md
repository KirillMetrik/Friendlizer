# Friendlizer

Code challenge task for LinkSoft.

![Friendlizer screenshot](https://i.ibb.co/sjBbz43/friendlizer.png "Screenshot")

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

For "senior developer" task **I've chosen to implement graph visualization** (since I prefer to develop something a user can "see"), i.e. additional task number 2.
Implementing task number 1 would involve 2 parts:

 1. building an in-memory linked graph to provide calculations of N-depth connections. Simple algorithm of graph traverse with breadth-first search would give complexity of O(N^2). Saving paths which we already visited would improve the whole thing.
 1. Determining a number of groups where everybody knows each other is basically a task of "clicques" detection, there is a Bron-Kerbosch algorithm for that. The solution would involve implementing the algorithm (I've heard about the problem and the algorithm but didn't spend much time understanding it).

Because this is only a testing software (not intended for production uses), the following simplifications were done:
* Frontend uses hardcoded endpoint URLs (real app would probably use some config).
* Frontend is small and doesn't really separate data retrieval logic into separate components/services. Real-life app would certainly do it.
* Backend uses in-memory database (not bound to a real SQL database).
* Backend contains SQL/LINQ queries directly in controller. In real-life app this logic would be probably moved to separate classes but for a small testing app this is still maintanable.
* Relations for graph are returned by a single call. For testing purposes of 88 thousand records this is still ok but in real-life app it would be good to read in pages.
* It is impossible to visualize all 88 thousands of relations at once (on any normal PC). Plus this kind of visualization would represent a mess of lines and circles. I've chosen to visualize pages of data. For now this is a simple paging which gives some good insights on connections between nodes (especially when users choose bigger page sizes) but it could be even better if we used a "smarter" approach for choosing pages. For example, we could take the first X nodes and visualize their relations on a depth-first basis - this might (probably) provide more valuable insights for users. Since this wasn't part of the task and I was limited in time, I've decided not to implement this approach.

## Technologies Used

* Backend: .Net Core + Entity Framework + xUnit
* Frontend: Angular + Angular Material + VisJS