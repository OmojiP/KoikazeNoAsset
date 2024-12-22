using System;
using UnityEngine;

namespace OmojiP.Utils
{
    [RequireComponent(typeof(Camera))]
    public class CameraScaler : MonoBehaviour
    {
        private float baseOrthographicSize = 5f;
        
        private float targetAspect = 16f / 9f; 
        
        [SerializeField] float targetAspectRatioX = 16f;
        [SerializeField] float targetAspectRatioY = 9f;

        [SerializeField] bool isScalingOnUpdate = false;
        
        private Camera cam;

        void Start()
        {
            targetAspect = targetAspectRatioX / targetAspectRatioY;
            
            cam = GetComponent<Camera>();
            AdjustCameraSize();
        }

        void AdjustCameraSize()
        {
            float windowAspect = (float)Screen.width / Screen.height;
            float scaleHeight = targetAspect / windowAspect;

            if (scaleHeight < 1.0f)
            {
                cam.orthographicSize = baseOrthographicSize;
            }
            else
            {
                cam.orthographicSize = baseOrthographicSize * scaleHeight;
            }
        }

        void Update()
        {
            if (isScalingOnUpdate)
            {
                AdjustCameraSize();
            }
        }
    }
}