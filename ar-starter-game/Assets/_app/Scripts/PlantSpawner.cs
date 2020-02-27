using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Random = UnityEngine.Random;

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
    }

    GameObject CreatePlant(Pose pose)
    {
        var localPose = transform.InverseTransformPose(pose);

        var instantiateParams = new object[]
        {
            localPose.position, 
            localPose.rotation
        };
        
        var plantGo = PhotonNetwork.InstantiateSceneObject("Plant", Vector3.zero, Quaternion.identity, data: instantiateParams);
        var plant = plantGo.GetComponent<Plant>();
        plant.BeginLookingAwesome();
        return plantGo;

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

[Serializable]
public struct PlantInstantiationData
{
    public Pose pose;
    public float initialRotation;
    public float initialScale;
}