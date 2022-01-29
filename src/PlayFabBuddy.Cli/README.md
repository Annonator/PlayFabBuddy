# PlayFabBuddy.Cli

## Setup
1. Copy `settings.json` to `local.settings.json`
2. Fill out the required settings to connect to your PlayFab Title:
   1. `titleId`: Log in to PlayFab. You should see the titleId on your the front page after logging in, or in the URL of the browser when in teh Game Manager.
   2. `devSecret`:
      1. Got to the Game Manager of your Title
      2. Click on the cogwheel on top of the left menu (next to your Title's name). Click it
      3. Select "Title Settings"
      4. Select "Secret Keys" tab (have no one watch over your shoulder, these secret keys will be visible!)
      5. Click "New secret key"
      6. Give it a potential name (and optional: an expiry date)
      7. Click "Save secret key"
      8. Copy they key to your `local.settings.json`

## Commands

### Players
Commands related to players.

#### Create
    dotnet run -- players create <number of players>

#### Delete
**WARNING:** This will delete ALL Users from the Title!

    dotnet run -- players delete