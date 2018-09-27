﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlanticide
{
    public class Door : MonoBehaviour
    {
        [SerializeField]
        private int keyCode;

        public bool unlocked;
        public bool open;

        private PlayerProximitySwitch _proxSwitch;

        /// <summary>
        /// Initializes the object.
        /// </summary>
        private void Start()
        {
            _proxSwitch = GetComponent<PlayerProximitySwitch>();
            if (_proxSwitch == null)
            {
                Debug.LogError(Utils.GetComponentMissingString("ProximitySwitch"));
            }
        }

        /// <summary>
        /// Updates the object once per frame.
        /// </summary>
        private void Update()
        {
            if (open)
            {
                return;
            }

            if (_proxSwitch != null && _proxSwitch.Activated)
            {
                foreach (int ownedKeyCode in World.Instance.keyCodes)
                {
                    if (keyCode == ownedKeyCode)
                    {
                        Unlock();
                        Open();
                    }
                }
            }
        }

        public void Open()
        {
            if (unlocked && !open)
            {
                open = true;
            }
        }

        public void Close()
        {
            if (open)
            {
                open = false;
            }
        }

        public void Unlock()
        {
            if (!unlocked)
            {
                unlocked = true;
            }
        }

        public void Lock()
        {
            if (unlocked)
            {
                Close();
                unlocked = false;
            }
        }

        /// <summary>
        /// Draws gizmos.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (unlocked)
            {
                Gizmos.color = (open ? Color.green : Color.yellow);
                Gizmos.DrawSphere(transform.position, 0.5f);
            }
        }
    }
}