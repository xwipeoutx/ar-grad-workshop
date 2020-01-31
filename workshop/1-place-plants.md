# Plant placement

By the end of this part, you will have an understanding how your phone can interact with the real world to place objects on the ground.

## 1. Create a "prefab" for your plant.

First thing to do is to create your plant model - this is what will get placed on the ground.

<details>
    <summary>Download a 3d model and drag it into your assets folder to import it</summary>

![Import plant](img/import-assets.gif)

</details>

<details>
    <summary>In your scene, right click an empty area, and choose "Create Empty".  Rename it and ensure it is not transformed in any way.</summary>

![Create Empty](img/plant-base-object.gif)

</details>

<details>
    <summary>Drag the model under your plant game object, and resize to a reasonable size for your scene.</summary>

![Add plant to scene](img/add-plant-to-scene.gif)

</details>

<details>
    <summary>Create a prefab out of the plant by dragging the root object into the Prefabs folder.</summary>

![Create plant prefab](img/plant-prefab.gif)

</details>

<details>
    <summary>Delete the plant from the scene</summary>
    <p>You can work this one out yourself :)</p>
</details>

This prefab will be one of the plants created when.

**Extension work**: Make a few more plant varieties!

## 2. Write a script for plant placement

At this point, you will have a plant prefab available - now we need to place the plant in the world, in the right spot.


<details>
<summary>Create the script file</summary>

1. Right-click the "Scripts" folder and choose "Create - C# Script"
2. Call it "PlantSpawner"
3. Open script in some codes

</details>

<details>
<summary>Add a button</summary>
<p>
</details>


TO DO

* Write a script that runs on tap
* Do a raycast
* Instantiate the plant at the right spot
