# CardsForClimate

This is a card game intended to make players more aware of the very serious issue of climate change and what they can do to help prevent it from ending society as we know it.

Hope you enjoy!

---

## Resources

- [List of Cards](https://docs.google.com/spreadsheets/d/1jZSGPYr4IXfvTKF-ufFWVQosDYh2DKbHl6gTqsAfanA/edit?ts=5e7e1747#gid=173452736)

## Contributing

Check out the [CONTRIBUTING](CONTRIBUTING.md) guide to learn more about how we work together on this project.

## Getting Started

To start working on this project, you'll need to do the following:

1. Clone this repository [[help](https://help.github.com/en/github/creating-cloning-and-archiving-repositories/cloning-a-repository)]
1. Download and install Unity Hub [[help](https://docs.unity3d.com/Manual/GettingStartedInstallingUnity.html)]
1. Open the project in Unity (all files are in the `CardsForClimateUnityProj` folder) [[help](https://docs.unity3d.com/Manual/GettingStartedOpeningProjects.html)]

With that, you're good to start developing!

To get yourself oriented, a good place to start (as of March 2020) is the Game Manager file (`CardsForClimateUnityProj/Assets/Scripts/GameManager.cs`).

Happy buidling!

## Game Mechanics Questions

_Pulled from meeting notes_

> What are the max values for CO2 and Money?

> Maximum card numbers (action + events)

> Do you place cards on bottom of deck?

> What happens to discarded cards?

> What happens if the player runs out of action cards?

> What happens if the player runs out of event cards?

> Why do we have smile and frown in the card list?

> Will need to create separate list for cards

> What does paying money do in the singleplayer version for hope cards?

> You have to play a card with any kind of hope value when a negative hope card is played?

> The only other option if you have to hope based cards is to skip?

> Momentum on hope cards? How does it work? If a player already met the hope requirement then they shouldn’t matter if the card has hope after it?

> When hope reaches negative 3 should the player lose at the start of the turn or end of it?
- Can lose before turn starts
- Money loses at start
- Co2 above 30 start
- Run out of event cards check at end of turn

> If an event gives momentum, then can the player still play three cards?

Does not matter, can only play two cards

> Can the player skip their turn?

Want that feature, only during hope breaker
Bad hope only applies to current turn

> What if they run out of action cards?

No concrete answer given

> If hope is below is still negative after first hope card is played does momentum matter?

Break the initial hope problem

_Need to remove commas from description with another denote character_

