using System;
using UnityEngine;

namespace SKCell
{
    [Serializable]
    public class SKTranslatorWaypoint 
    {
        public Vector3 position;
        public Quaternion rotation;

        public float stayTime = 0;
    }
}
