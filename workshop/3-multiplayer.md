# Multiplayer

By the end of this part, you will be growing plants with friends

Apologies, I had no time to gif this one up.

## High level approach

We need 2 things for multiplayer: A shared idea of space, and a shared idea of game state.

We will agree on state using _code_.

We will agree on space using an image target as an anchor.

## 1. Add a hose toggle button

This will be much easier if you have a button that turns the hose on and off.

<details>
<summary>Add a toggle method to the `Hose` class</summary>

```cs
public void ToggleHose()
{
    enabled = !enabled;
}
```
</details>

<details>
<summary>Add a button to your UI and make it call that method</summary>

> You can do this! I believe in you!

</details>

Your hose should now turn on and off whenever you toggle that button.  I recommend starting it disabled.

## 2. Join a multiplayer scene

The lobby is what we'll use to ensure a player can join a room.

Usually you want to be able to create, list, and join rooms. We won't do such nonsense, and hardcode it.

<details>
    <summary>Find PUN2 Free in the Unity asset store and import it</summary>

Feel free to use my AppId when setting it up: `982f95c3-580d-48ef-9216-b75e1e62caa2`

</details>

<details>
    <summary>Create a `Lobby.cs`</summary>

This handles your connection logic and allows you to join a room.

```cs
public class Lobby : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks
{
    [SerializeField] string roomName = "Default Room";
    [SerializeField] Text topText;
    [SerializeField] GameObject[] onJoinActivate = new GameObject[0];

    void Awake()
    {
        StopGame();
        topText.text = $"Look at the image target to join a room";
        PhotonNetwork.AddCallbackTarget(this);
    }

    void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.J) && !PhotonNetwork.IsConnected)
        {
            Join();
        }
        #endif
    }
    
    public void Join()
    {
        topText.text = $"Connecting to Photon...";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnConnected()
    {
        topText.text = $"Connecting to Master...";
    }

    public void OnConnectedToMaster()
    {
        topText.text = $"Connected! Joining room {roomName}...";
        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions(), TypedLobby.Default);
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        topText.text = $"Disconnected: {cause}";
        StopGame();
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
    }

    public void OnCreatedRoom()
    {
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
    }

    public void OnJoinedRoom()
    {
        StartCoroutine(StartGame());
    }

    private void StopGame()
    {
        foreach (var go in onJoinActivate)
        {
            go.SetActive(false);
        }
    }

    private IEnumerator StartGame()
    {
        topText.text = $"Joined room: {roomName}";
        foreach (var go in onJoinActivate)
        {
            go.SetActive(true);
        }
        yield return new WaitForSeconds(3);
        
        topText.text = $"You're in! Enjoy :)";
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        topText.text = $"Failed to join room {roomName}: {message}";
        StopGame();
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
    }

    public void OnLeftRoom()
    {
        topText.text = $"Left room: {roomName}";
        StopGame();
    }
}
```

</details>

<details>
    <summary>Configure it</summary>

Add an empty game object to your scene, I called mine `comms`.  Add this lobby object to it.

Choose your own room name - you don't want to be sending network messages to other players.

Use the top status text for the text - this will just let you know when you're connected and stuff

</details>

<details>
    <summary>Do a quick test</summary>

The `Lobby` component has some hacky code to join when you press `J` in the editor.  Hit play and you should connect to your room!

</details>

<details>
    <summary>Improve the experience a bit</summary>

We want to ensure the players can only create plants and toggle their hose when they're connected to the room - we do this vie that "On Join Activate" game object.

Add the `Place Plant` button, the `Toggle Hose` button and the `Plants` object (theh one with the plant spawner) to this array.

Test it again using `J` - those elements should start disabled, and enable when you connect.

</details>

## 3. Use the image marker

In order for players to agree on space, a shared physical image marker works well.

We will use this as our "scene root", and position all plants relative to this.

Note this undoes some of the anchoring work we did for our plants - they no longer anchor to the world, but will anchor to this image marker and offset themselves.

<details>
    <summary>Add an empty game object to the scene, call it "Scene Root"</summary>

> I wonder if I should hide messages in these ones that have no further details.

</details>

<details>
    <summary>Put all "world-space" objects under it</summary>

This is just the cube and the "plants" spawner.

</details>

<details>
    <summary>Create a SceneRoot component</summary>

This component has a public follow method, and causes the scene root to always match that target.

It will also connect to the multiplayer when `Follow` is called.

```cs
public class SceneRoot : MonoBehaviour
{
    [SerializeField] Lobby lobby;
    private Transform followedTransform;

    public void Follow(Transform target)
    {
        followedTransform = target;
        lobby.Join();
    }

    void Update()
    {
        if (followedTransform != null && followedTransform.hasChanged)
        {
            transform.position = followedTransform.position;
            transform.rotation = followedTransform.rotation;
            
            followedTransform.hasChanged = false;
        }
    }
}
```
</details>

<details>
    <summary>Create a SceneRootTrackedImage component</summary>

This looks for the scene root component, and calls its `Follow` method.

See where we're heading?

```cs
public class SceneRootTrackedImage : MonoBehaviour
{
    private SceneRoot sceneRoot;
    
    void Awake()
    {
        var sceneRoot = FindObjectOfType<SceneRoot>();
        sceneRoot.Follow(this.transform);
    }
}
```

</details>

<details>
    <summary>Wire it up</summary>

Add the `SceneRootTrackedImage` component to the tracked image prefab.

Add the `SceneRoot` component to your scene root, and wire it up (it needs a reference to the lobby component)

</details>

<details>
    <summary>Test it out</summary>

Hit play. Simply dragging on the `Tracked Image` prefab while in play mode will connect you to photon in the editor.

Deploy to your device.  Point at the tracked image.

Same thing.

Whoa.

> If you're super keen now, you can implement all sorts of things by exploring Pun callbacks - notification when players join and leave the room for example.

</details>

## 4. Sync plant creation and growth

Time to get more complicated.

<details>
    <summary>Get an overview of the approach</summary>

Basically, our job here is to make it when one player creates a plant, all players (present and future) will get it.

To do this, we need to instantiate it using Photon, not using Unity's in built method.

Next, we need to ensure that all players agree on the position - so we need to send over scene-root relative coordinates and initial sizes.

We also need to ensure all players honour those coordinates and initial sizes, setting up their plants appropriately.

</details>

<details>
    <summary>Update `PlantSpawner` to be multiplayer friendly</summary>

Just one method change:

```cs
void CreatePlant(Pose pose)
{
    var localPose = transform.InverseTransformPose(pose);

    var instantiateParams = new object[]
    {
        localPose.position, 
        localPose.rotation,
        Random.Range(0f, 360f), // y-rotation
        Random.Range(0.8f, 1.1f) // initial scale
    };
    
    PhotonNetwork.Instantiate("Plant", Vector3.zero, Quaternion.identity, data: instantiateParams);
}
```

Honestly, it's actually a bit simpler, because we're not dealing with anchors.
</details>

<details>
    <summary>Update the Plant script to be multiplayer friendly</summary>

This one is much more complicated:

```cs
public class Plant : MonoBehaviour, IPunInstantiateMagicCallback, IPunObservable
{
    [SerializeField] PhotonView photonView;
    [SerializeField] LayerMask growthSource;
    [SerializeField] float growthRate = 0.01f;
    [SerializeField] float maxScale = 2f;

    private Vector3 initialScale;
    private float scaleMultiplier = 1;

    private void OnCollisionEnter(Collision other)
    {
        if ((other.gameObject.layer & growthSource) == growthSource)
            return;

        photonView.RPC(nameof(GrowABit), RpcTarget.All);
    }

    [PunRPC]
    private void GrowABit()
    {
        scaleMultiplier += growthRate;

        if (scaleMultiplier > maxScale)
        {
            scaleMultiplier = maxScale;
        }
        UpdateScale();
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        var instantiationData = info.photonView.InstantiationData;
        var localPose = new Pose
        {
            position = (Vector3) instantiationData[0],
            rotation = (Quaternion) instantiationData[1]
        };

        var sceneRoot = FindObjectOfType<SceneRoot>();
        var worldPose = sceneRoot.transform.TransformPose(localPose);
        transform.parent = sceneRoot.transform;
        transform.position = worldPose.position;
        transform.rotation = worldPose.rotation;
        
        transform.Rotate(Vector3.up, (float)instantiationData[2]);
        initialScale = transform.localScale = Vector3.one * (float)instantiationData[3];
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(scaleMultiplier);
        }
        else if (stream.IsReading)
        {
            scaleMultiplier = (float) stream.ReceiveNext();
            UpdateScale();
        }
    }

    void UpdateScale()
    {
        transform.localScale = initialScale * scaleMultiplier;
    }
}
```

So this is heavy.  The run down:

* On instantiation, `OnPhotonInstantiate` is called.  Here we transform from local coordinates back to world, so it spawns in the right spot.
* Whenever there is a collision, Photon sends an `RPC` (Remote Procedure Call) to all players, telling the plant to grow
* All clients increase their plant size
* As required, Photon will sync the objects using the `OnPhotonSerializeView`.  This describes how the _owner_ of the plant (the creator in our case) informs the other players of the _state_ of the plant.
* Clients receiving this state will update their scale and view accordingly

This might seem a bit weird, but conceptually, the _owner_ of the object has authority here, though in this case it's overkill.

</details>

<details>
    <summary>Update the Plants prefab</summary>

We need to do a few things to the plants prefab now

* Add a `PhotonView` component - this is how photon-aware components talk to each other
* Add the `Plants` component as an observer of the photon view component
* Leave the `Observe` option as `Unreliable On Change` - we don't necessarily care if updates are sometimes missed, and we want to reduce traffic by not sending unchanged info over
* Create a `Resources` folder under `_app`, and move the prefab into this folder.  (Resources is a special folder name in Unity, that basically means it's serialized separately from the app. Photon needs this for some reason, I don't know why)

</details>

<details>
    <summary>Test it!</summary>

You're actually done.  Deploy to your phone, run in the editor, run on your phone and enjoy shared plant placement and growth!

</details>

## 5. Ship it

Check it in, and you're done

Or [back to top](README.md)