# PlayFabBuddy
A collection of useful stuff to make PlayFab development easier.

This project is very much work in process in experimentation phase. At the begining there will be a bunch of experiments to evaluate what the best way of moving forward would be.

Stuff will be ugly!!!!!

## But why do you define your own Entities?
Long story short, I want the models to be minimal in nature and easy to understand. By defining my own "TitlePlayerEntity" I'm able to map my mental modell better then using "PlayerProfileModel" from the PlayFab SDK. In addition it makes it easier to follow what parts of the models are relevant for what context. As PlayFab uses a PlayerProfileModel in the Client as well Admin Namespace which might be confusing anyway. Remember, I do not want to replicate the PlayFab SDK, I want to make to provide some tools to help working with PlayFab that will utilize the SDK!

It's ok  to disagree with me here and I might change this in the future based on the feeling when I'm actually using it :).