# Drawboard technical test project

Author: Alister Hatt (alister.hatt@gmail.com)

## Running the application

- Open `Drawboard.sln` in Visual Studio 2019
- Set the `Drawboard` projects as the startup application
- Run the project (e.g. Run with debug etc...)

## Architecture

The application is a simple UWP application.

The root view is `App`. 

`App` creates two main child instances of interest:

- `ProjectClient`: this is a static instance of the Drawboard API client, `IProjectClient`.
- `ProjectListView`: this view loads and displays the list of projects for the authenticated user. This view is bound to the view model `ProjectListViewModel`.

## Testing

Unit tests are contained in the `Test` project and should work using Visual Studios Test Explorer.

## TODO

- Unit test client authentication

## Possible improvements

- Add an authentication screen where user can enter credentials
- Add a decorator to the `IProjectClient` instance that caches the project list (expires after some duration).
- Persist the users 'access token' between instances.
- Cache project logos

