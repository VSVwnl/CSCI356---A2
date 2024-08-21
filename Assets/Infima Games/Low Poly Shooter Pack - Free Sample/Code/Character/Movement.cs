﻿// Copyright 2021, Infima Games. All Rights Reserved.

using System.Linq;
using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class Movement : MovementBehaviour
    {
        #region FIELDS SERIALIZED

        [Header("Audio Clips")]

        [Tooltip("The audio clip that is played while walking.")]
        [SerializeField]
        private AudioClip audioClipWalking;

        [Tooltip("The audio clip that is played while running.")]
        [SerializeField]
        private AudioClip audioClipRunning;

        [Header("Speeds")]

        [SerializeField]
        private float speedWalking = 5.0f;

        [Tooltip("How fast the player moves while running."), SerializeField]
        private float speedRunning = 9.0f;

        [Header("Jump")]

        [Tooltip("The force applied when jumping.")]
        [SerializeField]
        private float jumpForce = 7.0f;

        [Tooltip("The time to wait before allowing another jump.")]
        [SerializeField]
        private float jumpCooldown = 0.25f;

        #endregion

        #region PROPERTIES

        private Vector3 Velocity
        {
            get => rigidBody.velocity;
            set => rigidBody.velocity = value;
        }

        #endregion

        #region FIELDS

        private Rigidbody rigidBody;
        private CapsuleCollider capsule;
        private AudioSource audioSource;
        private bool grounded;
        private float lastJumpTime;

        private CharacterBehaviour playerCharacter;
        private WeaponBehaviour equippedWeapon;
        private readonly RaycastHit[] groundHits = new RaycastHit[8];

        #endregion

        #region UNITY FUNCTIONS

        protected override void Awake()
        {
            playerCharacter = ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();
        }

        protected override void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            capsule = GetComponent<CapsuleCollider>();

            audioSource = GetComponent<AudioSource>();
            audioSource.clip = audioClipWalking;
            audioSource.loop = true;
        }

        private void OnCollisionStay()
        {
            Bounds bounds = capsule.bounds;
            Vector3 extents = bounds.extents;
            float radius = extents.x - 0.01f;

            Physics.SphereCastNonAlloc(bounds.center, radius, Vector3.down,
                groundHits, extents.y - radius * 0.5f, ~0, QueryTriggerInteraction.Ignore);

            if (!groundHits.Any(hit => hit.collider != null && hit.collider != capsule))
                return;

            for (var i = 0; i < groundHits.Length; i++)
                groundHits[i] = new RaycastHit();

            grounded = true;
        }

        protected override void FixedUpdate()
        {
            MoveCharacter();
            grounded = false;
        }

        protected override void Update()
        {
            equippedWeapon = playerCharacter.GetInventory().GetEquipped();

            PlayFootstepSounds();

            // Handle jump input
            if (Input.GetKeyDown(KeyCode.Space) && grounded && Time.time - lastJumpTime > jumpCooldown)
            {
                Jump();
            }
        }

        #endregion

        #region METHODS

        private void MoveCharacter()
        {
            Vector2 frameInput = playerCharacter.GetInputMovement();
            var movement = new Vector3(frameInput.x, 0.0f, frameInput.y);

            if (playerCharacter.IsRunning())
                movement *= speedRunning;
            else
                movement *= speedWalking;

            movement = transform.TransformDirection(movement);

            Velocity = new Vector3(movement.x, Velocity.y, movement.z);
        }

        private void Jump()
        {
            Velocity = new Vector3(Velocity.x, jumpForce, Velocity.z);
            lastJumpTime = Time.time;
        }

        private void PlayFootstepSounds()
        {
            if (grounded && rigidBody.velocity.sqrMagnitude > 0.1f)
            {
                audioSource.clip = playerCharacter.IsRunning() ? audioClipRunning : audioClipWalking;
                if (!audioSource.isPlaying)
                    audioSource.Play();
            }
            else if (audioSource.isPlaying)
                audioSource.Pause();
        }

        #endregion
    }
}
