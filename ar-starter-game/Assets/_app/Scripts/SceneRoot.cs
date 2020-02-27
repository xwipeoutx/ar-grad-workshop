using UnityEngine;

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