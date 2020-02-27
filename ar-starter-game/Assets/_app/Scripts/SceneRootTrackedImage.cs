using UnityEngine;

public class SceneRootTrackedImage : MonoBehaviour
{
    private SceneRoot sceneRoot;
    
    void Awake()
    {
        var sceneRoot = FindObjectOfType<SceneRoot>();
        sceneRoot.Follow(this.transform);
    }
}