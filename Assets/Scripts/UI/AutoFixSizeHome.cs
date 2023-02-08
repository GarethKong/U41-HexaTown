using System;
using Utilities;

namespace UI
{
    using UnityEngine;
    using System.Collections;

    public class AutoFixSizeHome : MonoBehaviour
    {
        
        public static AutoFixSizeHome Instance;

        private void Awake()
        {
            Instance = this;
            StartZoom();
        }

        public void StartZoom()
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr == null)
                return;

            // Set filterMode
            sr.sprite.texture.filterMode = FilterMode.Point;

            // Get stuff
            double width = sr.sprite.bounds.size.x; 
            Debug.Log ("width: " + width);
            double worldScreenHeight = Camera.main.orthographicSize * 2.0;
            double worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            // Resize
            transform.localScale = new Vector2 (1, 1) * (float)(worldScreenWidth / width);
        }
    }
}