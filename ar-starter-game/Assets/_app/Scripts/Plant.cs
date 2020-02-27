using Photon.Pun;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

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