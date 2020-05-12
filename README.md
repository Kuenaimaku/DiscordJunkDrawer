# Discord Junk Drawer
An assortment of discord commands with questionable value.

## Modules

### Role

* `iam <role>` - add role to yourself.

### Demo

* `delete` - demonstrate a message that will delete itself after 10 seconds.
* `next` - demonstrate a message that takes the next message to come in from the current context
* `pagination` - demonstrate a paginated controller with custom embed fields per page
* `reaction` - demonstrate a message that will react to users reacting to the created message
* `embedreaction <bool:expiresafteruse> <bool:singleuseperuser> <bool:sourceuser>` - demonstrate an embed that has optional parameters.
  * `<bool:expiresafteruse>` - sets whether the resulting message can only be reacted to once
  * `<bool:singleuseperuser>` - sets whether the resulting message can only be reacted to once per user
  * `<bool:sourceuser>` - sets whether reactions should be restricted to whomever executed the command

### Settings
* `cleanup` - remove all managed roles from the guild and leave the guild.


## Running this Application

* refer to [this dotnet article on how to update your local database](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli#update-the-database)

* run the application