using System;
using Utilities;

namespace UI
{
    using UnityEngine;
    using System.Collections;

    public class SmoothCameraSize : MonoBehaviour
    {
        public Camera interpolateCam;
        public static SmoothCameraSize Instance;
        private float startingSize;
        private float endSize;
        private float t = 0;

        private void Awake()
        {
            Instance = this;
            startingSize = interpolateCam.orthographicSize;
            StartZoom();
        }

        private bool isStartZoom = false;

        public void StartZoom()
        {
            float width = Camera.main.orthographicSize * Screen.width / Screen.height;
            float scaleValue = width / 5.5f;

            endSize = Camera.main.orthographicSize / scaleValue;
            if (endSize < 9)
            {
                endSize = 9;
            }
            isStartZoom = true;
            if (AutoFixSize.Instance!=null)
            {
                AutoFixSize.Instance.StartZoom(endSize);
            }
        }

        private void Update()
        {
            if (!isStartZoom) return;
            t += Time.deltaTime;
            interpolateCam.orthographicSize = Mathf.SmoothStep(startingSize, endSize, t);
        }
    }
}