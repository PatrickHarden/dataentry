# Setup

## .NET 5
- In a web browser, visit https://dotnet.microsoft.com/en-us/download/dotnet/5.0
- Download and install the latest SDK (most CBRE workstations are Windows x64)

## PostgreSQL


#### Required Software

    Dotnet Core 5
    Postgres 11+
    Visual Studio Code
        [C# Extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)
    Node 12

#### Initial Setup

    Open terminal (ctrl + `)

        cd DataEntry\ClientApp

        npm install

    Open root folder

    Hit F5 *or* run `dotnet run` in the terminal

### Sites

    US PROD: https://dataentry-us.cbrelistings.com/

    SG PROD: https://dataentry.cbrelistings.com/ 

    IN PROD: https://dataentry-in.cbrelistings.com/
    
    Global PROD (All other countries): https://manage.cbrelistings.com/

    DEV: https://dev-dataentry.cbrelistings.com/

    UAT: https://uat-dataentry.cbrelistings.com/

## Database Setup

1. [Download PostgresSQL](https://www.postgresql.org/download/)
2. Install with password=password
3. Run `dotnet ef database update -c DataEntryContext`
4. Run `dotnet ef database update -c UserContext`

OR
1. [Install Docker CE](https://docs.docker.com/install/)
2. Run `docker run --name postgres -e POSTGRES_PASSWORD=password -d -p 5432:5432 postgres`
3. Run `dotnet ef database update -c DataEntryContext`
4. Run `dotnet ef database update -c UserContext`

DBContext Update
1. Run `dotnet ef migrations add <MigrationName> -c DataEntryContext`
2. Run `dotnet ef database update -c DataEntryContext`


# GraphQL Authentication

	Install Chrome ModHeader extension

	Open Chrome Developer tools and retrieve access token from Application/Local Storage

	Open Chrome ModHeader and add new Request Header 
	[Name = Authorization , Value = bearer [+token]]

# Front-end Resources

  Chrome ModHeader:
  https://chrome.google.com/webstore/detail/modheader/idgpnmonknjnojddfkpgkljpfnnfcklj

  Invision link:
  https://cbre.invisionapp.com/share/TESQIRJBK5W#/screens/371201449

  React + Typescript cheatsheet:
  https://github.com/typescript-cheatsheets/react-typescript-cheatsheet

  React + Redux + Typescript Guide:
  https://github.com/piotrwitek/react-redux-typescript-guide

  React Hooks:
  https://reactjs.org/docs/hooks-overview.html

  Redux Hooks: 
  https://react-redux.js.org/api/hooks

# Testing

  In client app:
  npm run test

  npm run storybook

  To update the storybook:
  npm run build-storybook

  it should be visitable via /.storybook-static/index.html
  

# Todo's

    - Update graphql api to send only updates to the listing instead of the whole listing
    - Figure out a way to construct the preview quicker
    - Load test - determine the limits of our application before we find out through other means
    - Double check to make sure we aren't keeping images in blob that are no longer attached to the listing
    - Double check to make sure values cleared from dropdowns aren't sending unexpected values to the backend/storeApi
    - Containerize files which are becoming too large, separating state/rerenders/redux from dom construction
    - Memoize what we can. This will improve performance and also act as a failsafe against errant rerenders (such as double api call, bubbling)
    - Move logic to redux selectors where we can, such as spaces suffix
    - Refactor daisy-chain if-else statements into loops (could possibly destructure type interfaces into string arrays)
    - Typify things down, add interfaces where need be
    - Comment abstract functions, not only for what they do, but most importantly, for how they fit into the rest of the file's logic
    - Group similar logic together with spacing (such as validation, saving, ect), so we get a better intuitive visual understanding
    - Refactor some component state into one state object to minimize rerenders
    - Pass large values (such as config) down into components via props instead of repulling from store (improves performance, reduces potential rerenders)
    - Pass values in from props instead of pulling from redux store for components (keep the views/components well defined)
    - Reduce massaging logic in glQueries - move to components or refactor components to bubble values properly
    - Move node-modules to RAM to improve build times/pipeline
    - Investigate redux-toolkit to potentially reduce bloat/clean code
