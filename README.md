# What is the tile system
The tile system is a framework that allows the developer to specify interactions between objects and manage the state of the tile level. It is comprised of "Tiles", "Entities" and "Solvers" which interact together using "TileChange" events.

# Why we developed a tile system
The tile system was built to allow a quick development of simple tile based games, it was designed from the ground up with testability in mind. In the vast majority of Unity projects the logic is embedded directly in Unity "GameObjects" this is great for prototyping and smaller systems, but can lead to unmanageble code if written incorrectly, and makes changing game systems very difficult.

In tile based games the interaction logic has to be correct every time, we found it hard to test the component based architecture that is used in Unity, so we split the logic from the game object representation and use events to have the game objects update themselves, calling back into the logic when they have finished.

# Getting started
We provide a dll include which is compiled to .Net 3.5 Unity Full base classes. You can include this into your Unity project and start using it right away. If you prefer to clone this repo and have a look through it you will need .Net 4.5 for the unit test project.

# Nuget Packages
The tile system does not use any external packages.

The packages used for the Unit tests:
[NUnit 3](https://www.nuget.org/packages/NUnit/)

If you are using Visual Studio it is recommended that you install NUnit 3 test adapter
[NUnit 3 Test Adapter](https://visualstudiogallery.msdn.microsoft.com/0da0f6bd-9bb6-4ae3-87a8-537788622f2d)
