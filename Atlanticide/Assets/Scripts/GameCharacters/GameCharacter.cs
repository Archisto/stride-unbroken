﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Atlanticide
{
    public abstract class GameCharacter : MonoBehaviour
    {
        [SerializeField]
        protected float _speed;

        [SerializeField]
        protected float _turningSpeed;

        [SerializeField]
        protected int _maxHitpoints = 3;

        [SerializeField]
        protected GameObject _characterBody;

        protected int _hitpoints;
        protected Vector3 _characterSize;
        protected GroundCollider _groundCollider;

        public bool IsInvulnerable { get; set; }

        public bool IsImmobile { get; set; }

        public bool IsDead { get; protected set; }

        public Vector3 RespawnPosition { get; set; }

        public Vector3 Size { get { return _characterSize; } }

        /// <summary>
        /// Testing; only needed if player count can be changed from 2.
        /// Returns whether the player character object is active.
        /// </summary>
        public bool Exists
        {
            get { return gameObject.activeSelf; }
        }

        /// <summary>
        /// Initializes the object.
        /// </summary>
        protected virtual void Start()
        {
            ResetBaseValues();
            RespawnPosition = transform.position;
            _characterSize = _characterBody.GetComponent<Collider>().bounds.size;
            _groundCollider = _characterBody.GetComponent<GroundCollider>();
        }

        /// <summary>
        /// Updates the object once per frame.
        /// </summary>
        protected virtual void Update()
        {
            if (IsDead)
            {
                return;
            }
        }

        protected virtual void LookTowards(Vector3 direction, bool inputDirection, bool modifyOnlyY)
        {
            if (inputDirection)
            {
                direction = new Vector3(direction.x, 0, direction.y);
            }

            Vector3 lookDir = Quaternion.LookRotation(direction, Vector3.up).eulerAngles;
            lookDir = new Vector3(transform.rotation.x, lookDir.y, transform.rotation.z);
            transform.rotation = Quaternion.Euler(lookDir);
        }

        protected virtual void RotateTowards(Vector3 direction, bool inputDirection)
        {
            if (inputDirection)
            {
                direction = new Vector3(direction.x, 0, direction.y);
            }
            transform.rotation = Utils.RotateTowards(transform.rotation, direction, _turningSpeed);
        }

        /// <summary>
        /// Makes the character take damage.
        /// </summary>
        /// <param name="damage">The damage amount</param>
        /// <returns>Does the character die.</returns>
        public virtual bool TakeDamage(int damage)
        {
            if (!IsInvulnerable)
            {
                _hitpoints -= damage;

                if (_hitpoints <= 0)
                {
                    _hitpoints = 0;
                    Kill();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Kills the character.
        /// </summary>
        public virtual void Kill()
        {
            IsDead = true;
            CancelActions();
            Debug.Log(name + " died");

            SFXPlayer.Instance.Play(Sound.Player_Hurt_2);
        }

        /// <summary>
        /// Respawns the character.
        /// </summary>
        public virtual void Respawn()
        {
            ResetBaseValues();
            ResetPosition();
            gameObject.SetActive(true);
            Debug.Log(name + " respawned");
        }

        /// <summary>
        /// Resets the character's base values.
        /// </summary>
        protected virtual void ResetBaseValues()
        {
            IsDead = false;
            IsImmobile = false;
            _hitpoints = _maxHitpoints;
            ResetGroundCollider();
        }

        public void ResetPosition()
        {
            transform.position = RespawnPosition;
            ResetGroundCollider();
        }

        private void ResetGroundCollider()
        {
            if (_groundCollider != null)
            {
                _groundCollider.ResetGroundCollider();
            }
        }

        /// <summary>
        /// Cancels the character's current actions.
        /// </summary>
        public virtual void CancelActions()
        {
        }

        /// <summary>
        /// Draws gizmos.
        /// </summary>
        protected virtual void OnDrawGizmos()
        {
        }
    }
}
