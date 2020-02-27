using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlantSpawner : MonoBehaviour
{
    [SerializeField] private ARAnchorManager anchorManager;
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject plantPrefab;

    public void PlacePlant()
    {
        var ray = new Ray(camera.transform.position, camera.transform.forward);
        
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isRemoteConnected)
        {
            var pos = ray.GetPoint(2); // 2m in front
            PlacePlant(new Pose(pos, Quaternion.identity));
            return;
        }
#endif
        
        var raycastHits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(ray, raycastHits))
        {
            var pose = raycastHits[0].pose;
            PlacePlant(pose);
        }
    }
    
    void PlacePlant(Pose pose)
    {
        var plant = CreatePlant(pose);
        plant.transform.Rotate(Vector3.up, Random.Range(0, 360));
        plant.transform.localScale = Vector3.one * Random.Range(0.8f, 1.1f);
    }

    GameObject CreatePlant(Pose pose)
    {
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isRemoteConnected)
        {
            var parent = new GameObject();
            parent.transform.position = pose.position;
            parent.transform.rotation = pose.rotation;
            
            var editorPlantObject = Instantiate(plantPrefab, parent.transform);
            return editorPlantObject;
        }
#endif
        
        var anchor = anchorManager.AddAnchor(pose);
        var plantObject = Instantiate(plantPrefab, anchor.transform);
        return plantObject;
    } 
}
