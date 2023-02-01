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
        }

        private bool isStartZoom = false;

       public  void StartZoom(LevelGrid levelGrid)
        {
            int minX = 0;
            int minY = 0;
            int maxX = 0;
            int maxY = 0;
            var mapInfos = levelGrid.mapInfos;
            //Calculator scale size
            for (int row = 0; row < GameConfig.MAPSIZE; row++)
            {
                for (int column = 0; column <GameConfig.MAPSIZE; column++)
                {
                    if (mapInfos[column, row]!=0)
                    {
                        if (minX> row)
                        {
                            minX = row;
                        }
                        if (maxX< row)
                        {
                            maxX = row;
                        }
                        if (minY> column)
                        {
                            minY = column;
                        }
                        if (maxY< column)
                        {
                            maxY = column;
                        }
                    }
                }
            }

            int widthSize = maxX - minX;
            int heightSize = maxY - minY;
            float scaleX = 21f / widthSize;
            float scaleY = 21f / heightSize;

            Debug.Log("SIZE CAMERA" + "X: " + scaleX+ " Y: " + scaleY);
            var scale = scaleY;
            if (scaleX > scaleY)
            {
                scale = scaleX;
            }
            
            endSize = startingSize /scale;
            isStartZoom = true;
        }

        private void Update()
        {
            if (!isStartZoom) return;
            t += Time.deltaTime;
            interpolateCam.orthographicSize = Mathf.SmoothStep(startingSize, endSize, t);

        }
    }
}