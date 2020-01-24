using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Augo.Scripts
{
    public class EditorCamera : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float sensitivity = 0.1f;
        private Vector3 currentPosition;

        void Start()
        {
            if (!Application.isEditor)
            {
                Destroy(this);
                return;
            }

            Camera.main.clearFlags = CameraClearFlags.Skybox;
            Destroy(GetComponent<ARCameraBackground>());
        }
        
        void Update()
        {
            if (!Input.GetMouseButton(1))
                return;

            if (Input.GetMouseButtonDown(1))
                currentPosition = Input.mousePosition;

            var delta = sensitivity * (Input.mousePosition - currentPosition);
            
            transform.Rotate(Vector3.right, -delta.y, Space.Self);
            transform.Rotate(Vector3.up, delta.x, Space.World);
            currentPosition = Input.mousePosition;

            var speed = Time.deltaTime;
            if (Input.GetKey(KeyCode.LeftShift))
                speed *= 3;
            
            if (Input.GetKey(KeyCode.W))
            {
                Camera.main.transform.Translate(Vector3.forward * speed);
            }
            
            if (Input.GetKey(KeyCode.A))
            {
                Camera.main.transform.Translate(Vector3.left * speed);
            }
            
            if (Input.GetKey(KeyCode.S))
            {
                Camera.main.transform.Translate(Vector3.back * speed);
            }
            
            if (Input.GetKey(KeyCode.D))
            {
                Camera.main.transform.Translate(Vector3.right * speed);
            }
            
            if (Input.GetKey(KeyCode.E))
            {
                Camera.main.transform.Translate(Vector3.up * speed, Space.World);
            }
            
            if (Input.GetKey(KeyCode.Q))
            {
                Camera.main.transform.Translate(Vector3.down * speed, Space.World);
            }
        }
    }
}
