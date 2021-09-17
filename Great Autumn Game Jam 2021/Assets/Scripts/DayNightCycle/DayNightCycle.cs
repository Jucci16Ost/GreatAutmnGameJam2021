using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.DayNightCycle
{
    public class DayNightCycle : MonoBehaviour
    {
        /// <summary>
        /// Last time this was updated.
        /// </summary>
        private float _lastUpdateTime;

        /// <summary>
        /// Called on object start
        /// </summary>
        private void Start()
        {
            // Cuz fuck em.
            DayNightCycleContext.TimeOfDay += 1;
            _lastUpdateTime = Time.timeSinceLevelLoad;
        }

        /// <summary>
        /// Called once per frame.
        /// </summary>
        private void Update()
        {
            if (Time.timeSinceLevelLoad - _lastUpdateTime < .5) return;

            _lastUpdateTime = Time.timeSinceLevelLoad;
            DayNightCycleContext.TimeOfDay += 1;
        }
    }
}
