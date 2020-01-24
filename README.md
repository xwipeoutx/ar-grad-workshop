# AR workshop

This project is a little starter - comes with ARFoundation, ARCore, ARKit and a mostly empty first scene to verify it works.

If this works, you're on your way!

## Prerequisites

* Unity 2019.3.0f3 or higher (use Unity Hub)
* Include "Android" or "iOS" build support as desired
  * Also whatever native SDKs you need for these - eg. XCode, Android SDK, whatever
  * You may need to point your Unity install at the right folder - see `Edit` -> `Preferences` -> `External Tools`

## How to run

* Open the project up
* Choose your platform
* Plug your phone into USB
* Hit build and run (press ctrl+B)
  * I build to `/ar-starter-game/Build` - which is included in `.gitignore`.  Safe!

Once it starts up, you should see sparkles on detected world feature points, and a cube staying in the same place in the world

Having trouble? See [Unity's guide](https://learn.unity.com/tutorial/building-for-mobile) for more info (admittedly for an older version).  Docs available [for Android](https://docs.unity3d.com/Manual/android-GettingStarted.html) or [for iOS](https://docs.unity3d.com/Manual/iphone-GettingStarted.html)

## Running in the editor

The scene has an "editor camera" set up.  Press play in the editor and use RMB + WASD to move around. Like an FPS.

# What now?

Check out the workshop