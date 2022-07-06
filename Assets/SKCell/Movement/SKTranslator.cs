using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SKCell
{
    /// <summary>
    /// Translates this object along a path
    /// </summary>
    public class SKTranslator : MonoBehaviour
    {
        #region Fields
        public TranslateMode translateMode;
        public float speed = 1.0f;

        public List<SKTranslatorWaypoint> waypoints;
        #endregion

        void Start()
        {

        }

        void Update()
        {

        }

        public SKTranslatorWaypoint GetLastWayPoint()
        {
            return waypoints.Count > 0 ? waypoints[0] : null;
        }
        public void AddWaypoint(SKTranslatorWaypoint waypoint)
        {
            waypoints.Add(waypoint);
        }
    }

    public enum TranslateMode
    {
        OneTime,
        PingPong,
        Repeat
    }
}