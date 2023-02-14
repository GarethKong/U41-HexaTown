using System;
using Utilities;

namespace UI
{
    using UnityEngine;
    using System.Collections;

    public class AutoFixSize : MonoBehaviour
    {
        
        public static AutoFixSize Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void StartZoom(float endSize)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr == null)
                return;

            // Set filterMode
            sr.sprite.texture.filterMode = FilterMode.Point;

            // Get stuff
            double width = sr.sprite.bounds.size.x;
            Debug.Log ("width: " + width);
            double worldScreenHeight = endSize * 2.0;
            double worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            // Resize
            transform.localScale = new Vector2 (1f, 1f) * (float)(worldScreenWidth / width);
        }
    }
}