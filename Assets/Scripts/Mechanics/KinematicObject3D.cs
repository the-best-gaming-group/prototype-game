using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// Implements game physics for some in game entity.
    /// </summary>
    public class KinematicObject3D : MonoBehaviour
    {
        /// <summary>
        /// The minimum normal (dot product) considered suitable for the entity sit on.
        /// </summary>
        public float minGroundNormalY = .65f;

        /// <summary>
        /// A custom gravity coefficient applied to this entity.
        /// </summary>
        public float gravityModifier = 0f;

        /// <summary>
        /// The current velocity of the entity.
        /// </summary>
        public Vector3 velocity;

        /// <summary>
        /// Is the entity currently sitting on a surface?
        /// </summary>
        /// <value></value>
        public bool IsGrounded { get; private set; }

        protected Vector3 targetVelocity;
        protected Vector3 groundNormal = new Vector3(0, -1, 0);
        protected Rigidbody body;
        protected RaycastHit[] hitBuffer = new RaycastHit[16];

        protected const float minMoveDistance = 0.001f;
        protected const float shellRadius = 0.01f;


        /// <summary>
        /// Bounce the object's vertical velocity.
        /// </summary>
        /// <param name="value"></param>
        public void Bounce(float value)
        {
            velocity.y = value;
        }

        /// <summary>
        /// Bounce the objects velocity in a direction.
        /// </summary>
        /// <param name="dir"></param>
        public void Bounce(Vector3 dir)
        {
            velocity.y = dir.y;
            velocity.x = dir.x;
        }

        /// <summary>
        /// Teleport to some position.
        /// </summary>
        /// <param name="position"></param>
        public void Teleport(Vector3 position)
        {
            body.position = position;
            velocity *= 0;
            body.velocity *= 0;
        }

        protected virtual void OnEnable()
        {
            body = GetComponent<Rigidbody>();
            body.isKinematic = true;
        }

        protected virtual void OnDisable()
        {
            body.isKinematic = false;
        }

        protected virtual void Start()
        {
        }

        protected virtual void Update()
        {
            targetVelocity = Vector3.zero;
            ComputeVelocity();
        }

        protected virtual void ComputeVelocity()
        {

        }

        protected virtual void FixedUpdate()
        {
            //if already falling, fall faster than the jump speed, otherwise use normal gravity.
            if (velocity.y < 0)
                velocity += gravityModifier * Physics.gravity * Time.deltaTime;
            else
                velocity += Physics.gravity * Time.deltaTime;

            velocity.x = targetVelocity.x;
            velocity.y = 0;

            IsGrounded = false;

            var deltaPosition = velocity * Time.deltaTime;

            Debug.Log(groundNormal);
            var moveAlongGround = new Vector3(groundNormal.y, -groundNormal.x);

            var move = moveAlongGround * deltaPosition.x;

            PerformMovement(move, false);

            move = Vector3.up * deltaPosition.y;

            PerformMovement(move, true);

        }

        void PerformMovement(Vector3 move, bool yMovement)
        {
            var distance = move.magnitude;

            if (distance > minMoveDistance)
            {
                //check if we hit anything in current direction of travel
            }
            body.position = body.position + move.normalized * distance;
        }

    }
}