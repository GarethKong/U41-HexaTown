using System;
using Utilities;

namespace UI
{
    using UnityEngine;
    using System.Collections;

    public class RotatePro : MonoBehaviour
    {
        public static RotatePro Instance;

        public float Speed = 1f;

        private void Awake()
        {
            Instance = this;
        }

        void Update()
        {
            transform.eulerAngles += new Vector3(0, 0, Speed * 100f * Time.deltaTime);
        }
    }
}