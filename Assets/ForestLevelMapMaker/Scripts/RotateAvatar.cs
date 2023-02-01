using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mkey
{
    //[ExecuteInEditMode]
    public class RotateAvatar : MonoBehaviour
    {

        public RectTransform Group;
        public RectTransform maskRect;
        float groupAngleZ;
        public bool topHalf;
        public float acceler = 2.0f;

        private LevelButton activeButton;
        private Canvas mainCanvas;

        void Start()
        {
            Group = GetComponent<RectTransform>();
        }

        void Update()
        {
            activeButton = MapController.Instance.ActiveButton;
            if (!mainCanvas) mainCanvas = MapController.Instance.parentCanvas;

            if (activeButton && mainCanvas)
            {
                var position = activeButton.transform.position;
                Group.transform.position = new Vector3(position.x,position.y  + 210) ;
                //  Group.anchoredPosition = Coordinats.RectTransformToCanvasSpaceCenterCenter(activeButton.GetComponent<RectTransform>(), MapController.Instance.parentCanvas);
                topHalf = Group.anchoredPosition.y > 0;

                // groupAngleZ = (topHalf) ? Mathf.LerpAngle(groupAngleZ, 180, Time.deltaTime * 2.0f) : Mathf.LerpAngle(groupAngleZ, 0, Time.deltaTime * 2.0f);
                // maskRect.localRotation = Quaternion.Euler(-Group.localRotation.eulerAngles);
                // Group.localRotation = Quaternion.Euler(new Vector3(0, 0, groupAngleZ + 5f * Mathf.Sin(2f * Time.time)));
            }
        }
    }
}
