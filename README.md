# PlayFabBuddy
[![.NET](https://github.com/Annonator/PlayFabBuddy/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Annonator/PlayFabBuddy/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/Annonator/PlayFabBuddy/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/Annonator/PlayFabBuddy/actions/workflows/codeql-analysis.yml)

A collection of useful stuff to make PlayFab development easier by providing a CLI tool to manage and test you PlayFab titles during development.

The CLI is based on a Library that can be used to build out your own tooling and testings workflows against PlayFab.

Many of the features requested are based on real world uses cases of game developers working with PlayFab. This includes Indie titles as well as AAA titles. But don't get me wrong, this project is not a statement of PlayFab being bad for any of these users. This project is supposed to close a gap towards a developer focused audience that is used to CLI based tools and processes. Ultimately this project will enable to get up and running with PlayFab faster and test your games integrations faster and more reliable, even in a CI/CD environment.

# Usage
## Cli
You may use the PlayFabBuddy.Cli app to execute commands against the PlayFab API.

Please see the [PlayFabBuddy.Cli Readme](src/PlayFabBuddy.Cli/README.md)

## Contribute
We are very much interested in working on this in collaboration with a community. So, if you are using PlayFab and have interest in collaborating please let us know by:
* Creating a feature request
* Contribute code through a Pull Request
* Participate by providing feedback about the current experience through github discussions

Even though this project is opinionated, we are inviting everyone at every skill level to reach out and collaborate.

## Disclaimer
This is not an official supported Microsoft tool! This is a project mainly driven by the Game Developer Experience team within XBox, but not an official supported Microsoft tool!

In addition, this project is very much work in progress, expect significant and breaking changes any time
## FAQ
Some questions that get asked some time...
### But why do you define your own Entities?
Application Boundaries are important. That's why each layer in the architecture implements its own Entities, where it makes sense or is necessary. As different layers do have different concerns, they have different requirements on the Entities. The PlayFab SDK and PlayFab in general is not called directly as it is considered an infrastructure concern and an external dependency. 

The only project with a direct PlayFab dependency should be PlayFabBuddy.Infrastructure, there might be stages in the development where this is not completely reflected yet.

### Why there are Adapters to the entities in the Application / Library layer?
The reason here is that the entity itself should have as little concerns as possible. One reason is that it makes integration for external data stores easier and more accessible. For Example, JSON Serialization with circular dependency gets easier if used on very simple object implementation. Yes, there are configuration that would enable default constructors passed into the serializer, but this would increase complexity by understanding the Serialization dependency. 

And yes, I'm aware that talking to entites through Adapters to provide functionality increases Complexity as well and requires domain knowledge, but at least for me this is a logical trade off.

### Why .NET 6?
In short, its to most recent iteration of dotnet. Yes, I'm aware that this dependency will prevent folks from implement and using the library in Unity directly.
