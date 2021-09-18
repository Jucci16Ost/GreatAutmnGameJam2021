using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Assets.Scripts.DayNightCycle
{
    public class DayNightCycle : MonoBehaviour
    {
        /// <summary>
        /// Last time this was updated.
        /// </summary>
        private float _lastUpdateTime;

        /// <summary>
        /// Global lighting component 
        /// </summary>
        private Light2D _globalLight;

        /// <summary>
        /// Length of 
        /// </summary>
        private const float HourLengthInSeconds = 20f;

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
        /// Called after start and whenever the object is enabled.
        /// </summary>
        private void Awake()
        {
            _globalLight ??= GetComponent<Light2D>() ?? GetComponentInChildren<Light2D>();
            _globalLight.intensity = DayNightCycleContext.LightIntensity;
        }

        /// <summary>
        /// Called once per frame.
        /// </summary>
        private void Update()
        {
            UpdateTime();
            UpdateLighting();
        }

        /// <summary>
        /// Update Scene Lighting
        /// </summary>
        private void UpdateLighting()
        {
            var currentLightLevel = _globalLight.intensity;
            var distanceFromHighNoon = Mathf.Abs(12 - DayNightCycleContext.TimeOfDay);
            var lightLevel = Mathf.Clamp(1 - (distanceFromHighNoon / 12f), .1f, 1f);
            var intensity = Mathf.Lerp(currentLightLevel, lightLevel, 1 / HourLengthInSeconds * Time.deltaTime);
            _globalLight.intensity = intensity;
            DayNightCycleContext.LightIntensity = intensity;
        }

        /// <summary>
        /// Update Scene Time
        /// </summary>
        private void UpdateTime()
        {
            if (Time.timeSinceLevelLoad - _lastUpdateTime < HourLengthInSeconds) return;

            _lastUpdateTime = Time.timeSinceLevelLoad;
            DayNightCycleContext.TimeOfDay += 1;
        }
    }
}
