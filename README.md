# EventStore

EventStore is designed to be an example of Event Sourcing design pattern.
## Design

![alt text](https://github.com/1bcrona/EventStore.PoC/blob/master/eventstore_diagram.png?raw=true)

A User, can consume this API by two types of operations
#### Command

Operation that writes on the database.

#### Query

Operation that reads from database.

## How it works

These two operations are handled separately on the API. **Command** operations generates operation-related event and writes to **Event Store** database.
After this process, there is a **Background Service** that listens to the events in the "Event Store". This service creates **Projections** over events and writes these projections to the **Event Store** database.
"Query" operations are processed by querying through these prepared projections.


## Prerequities
   - ##### Docker(https://www.docker.com/get-started)
   - ##### [.NET 5](https://dotnet.microsoft.com/download/dotnet/5.0) installed 
   - ##### Postgres DB

## Libraries that used

   - [Marten](https://github.com/JasperFx/marten) - Event Store
   - [MediatR](https://github.com/jbogard/MediatR) - For Processing Commands, Queries, Events
   - [TestEnvironment.Docker](https://github.com/Deffiss/testenvironment-docker)

## Solution Structure

- ### EventStore.API
  
  It is the application that contains the API.
 
- ### EventStore.Domain

  Library that contains the entity and value objects used in the application. It does not contain any Business Logic.
 
- ### EventStore.Store
 
  It is the library that contains the Store types used in the application.
 
    * Event Store
      - It includes Event Store features. It has a Marten implementation.
    * Document Store
      - It contains Document Store features. MongoDb implementation was made for later use.
 
 - ### EventStore.Listener
      It is the application that contains the "Background Service" that listens events which coming to the "Event Store" database, then generates projections from them and writes  them to the database.
 
 ## Installation
 
To run the application, please do the followings
 * Clone the project
 * Open the terminal at project directory
 * Execute following command
 <pre><code>docker-compose up</code></pre>
 
After the command is executed, an application with three containers on docker will be launched.
These;
 * Postgres DB[^1]
 * EventStore.API[^2]
 * EventStore.StreamListener
 
 [^1]: By default, Postgres access port is set to **5432** and port forwarding is enabled.</sup>
 [^2]: By default, EventStore.API is listening  **http:5000** and port forwarding is enabled.</sup>
 
 ## Tests
 
The application includes integration tests on its own.
These tests are located under **EventStore.API.Test.Integration**.
 
To run these tests;
 * Build the solution.
 * Open the terminal at **EventStore.API.Test.Integration/bin/Debug/5.0** directory
 * Execute following command
 <pre><code>dotnet test EventStore.API.Test.Integration.dll</code></pre>
 * Or use Visual Studio's **Test Explorer** pane
 
 Before the tests run, it creates a test host by referencing from **API** and a background service host by referencing from **StreamListener**
 At the same time, a **Postgres** container is launched to be closed after the tests are finished.
