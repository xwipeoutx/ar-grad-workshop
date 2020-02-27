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
        CreatePlant(pose);
    }

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
        
        var plantGo = PhotonNetwork.Instantiate("Plant", Vector3.zero, Quaternion.identity, data: instantiateParams);
        var plant = plantGo.GetComponent<Plant>();
    }
}

[Serializable]
public struct PlantInstantiationData
{
    public Pose pose;
    public float initialRotation;
    public float initialScale;
}