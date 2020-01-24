using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Augo.Scripts
{
    [RequireComponent(typeof(ARPointCloud))]
    [RequireComponent(typeof(ParticleSystem))]
    public class CustomPointCloudVisualiser : MonoBehaviour
    {
        ARPointCloud pointCloud;
        ParticleSystem arParticleSystem;

        [Range(0, 200), SerializeField] private int particlesPerSecond = 20;

        void Awake()
        {
            pointCloud = GetComponent<ARPointCloud>();
            arParticleSystem = GetComponent<ParticleSystem>();
        }

        void Update()
        {
            if (!isActiveAndEnabled)
                return;
            
            #if !UNITY_EDITOR
            if (pointCloud.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.None)
                return;
            #endif

            var particlesPerFrame = particlesPerSecond * Time.deltaTime;

            var remainder = particlesPerFrame;
            
            while (remainder > 1)
            {
                var pos = RandomParticlePosition();
                arParticleSystem.Emit(new ParticleSystem.EmitParams()
                {
                    position = pos
                }, 1);
                remainder -= 1;
            }
            
            if (Random.value < particlesPerFrame)
            {
                var pos = RandomParticlePosition();

                arParticleSystem.Emit(new ParticleSystem.EmitParams()
                {
                    position = pos
                }, 1);
            }
        }

        private Vector3 RandomParticlePosition()
        {
#if UNITY_EDITOR
            var randomInCircle = Random.insideUnitCircle;
            return 3*new Vector3(randomInCircle.x, 0, randomInCircle.y);
#else
            var randomPosition = Random.Range(0, pointCloud.positions.Length - 1);
            return pointCloud.positions[randomPosition];
#endif
        }
    }
}