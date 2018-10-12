﻿using UnityEngine;

namespace Atlanticide
{
    public class ShieldBashSwitch : Switch
    {
        private void Update()
        {
            if (Activated && !_permanent)
            {
                Activated = false;
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (!Activated)
            {
                foreach (ContactPoint cp in collision.contacts)
                {
                    Shield shield = cp.otherCollider.gameObject.GetComponent<Shield>();
                    if (shield != null && shield.BashActive)
                    {
                        Activated = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Draws gizmos.
        /// </summary>
        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.DrawSphere(transform.position + Vector3.up * 2f, 0.5f);
        }
    }
}