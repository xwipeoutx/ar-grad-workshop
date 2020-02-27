using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Plant : MonoBehaviour, IPunInstantiateMagicCallback
{
    [SerializeField] LayerMask growthSource;
    [SerializeField] float growthRate = 0.01f;
    [SerializeField] float maxScale = 2f;

    private Vector3 initialScale;
    private float scaleMultiplier;

    void Start()
    {
        initialScale = transform.localScale;
        scaleMultiplier = 1;
    }

    private void OnCollisionEnter(Collision other)
    {
        if ((other.gameObject.layer & growthSource) == growthSource)
            return;

        scaleMultiplier += growthRate;

        if (scaleMultiplier > maxScale)
        {
            enabled = false;
            scaleMultiplier = maxScale;
        }

        transform.localScale = initialScale * scaleMultiplier;
    }

    public void BeginLookingAwesome()
    {
        transform.Rotate(Vector3.up, Random.Range(0, 360));
        transform.localScale = Vector3.one * Random.Range(0.8f, 1.1f);
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
        transform.position = worldPose.position;
        transform.rotation = worldPose.rotation;
    }
}
