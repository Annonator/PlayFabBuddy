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
**WARNING:** This will delete **ALL** Master Player Accounts from the Title, which have been previously created with PlayFabBuddy on this very machine!

    dotnet run -- players delete --local <Local_Path>

#### Delete by Segment
Will delete all Master Player Accounts from a given segment, identified by segment name.
If a Master Player Account is assigned to another Title as well, players will **NOT** be deleted!

    dotnet run -- players delete --remote <PlayFab_Segment_Name>
   
As `segment name`, you may use one of the pre-defined segment names:

* `All Players` - All players
* `Lapsed Players` - Last login greater than 30 days ago
* `Payers` - Total value to date in USD greater than $0.00
* `Week Two Active Players` - First login less than 14 days ago AND First login greater than or equal to 7 days ago AND Last login less than 7 days ago

> Please note: Due to the nature of how segments work, you may need to execute this action multiple times to make sure to really delete all players in the given segment.