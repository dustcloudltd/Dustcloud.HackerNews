# Dustcloud.HackerNews

## The solution

#### This solution consists of three proper projects and two test projects. It was written in .NET 6 (with nullables disabled in all projects).  
The _.Common_ project keeps the model and the extensions (helpers).  
The _.Repository_ project deals with the Hacker News API (makes calls, returns data in the HackerNews model format.  
The _Dustcloud.HackerNews_ project is the main WEB API application, where the controller lives (and where the middleware lives, although I've not implemented anytyhing in the middleware. Normally, I'd use it to check authorization, maybe a specific request token.

## Running the app
To run the app in development mode (e.g. debug with Visual Studio), the app will show the user the swagger UI, with the available `GET` method. 

## Caching
The first time the app is run and the GET method invoked (as well as any subsequent times it gets invoked, but the system sees a difference between the cache and the _.topstories.json_ call,
the cache will be built. As this requires 500 subsequent calls to the HackerNews API, it might be lengthy (the first time round). 

The good news is, as the cached 500 top stories might not really change that much, the subsequent calls to our exposed `GET` method will use the cache to return the data. The cache's `SlidingExpiration` value is set to 5 minutes, while it's absolute expiration is set to an hour.

##### __NB__: At one point of the development I played around with the _updates.json_ call of the HackerNews API. Possibly, if I had designed the cache to store each item separately, as oppposed to the bulk, we could update the updated stories in the cache, too.

## Assumptions
Due to my usage of the in-memory cache (`IMemoryCache`), it is assumed that if this API is to be run on a server farm (multiple servers), we should ensure sessions are sticky.

## Logging
The logging is minimal in this app, maybe a few more .Debugs or .Infos could be added (around the `HackerNewsService` and the `HackerNewsController`).

## Testing
I have created a couple of unit-test projects, to cover the main app. They check the data is wholesome and the mappings are correct.