using UnityEngine;
using System.Collections;

namespace AudioVisualizer
{
    /// <summary>
    /// Disables an object after it's been active for a set amount of time.
    /// </summary>
    public class DisableOnTime : MonoBehaviour
    {
        [Tooltip("Disable this object after it's awake x seconds")]
        public float disableTime;

        private float disableTimer = 0;
        // Use this for initialization
        void OnEnable()
        {
            disableTimer = 0;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            
            disableTimer += Time.fixedDeltaTime;

            if (disableTimer > disableTime)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
