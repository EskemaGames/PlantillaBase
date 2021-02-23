using System.Collections.Generic;
using eskemagames.eskemagames.data;
using UnityEngine;

namespace eskemagames
{
    namespace eskemagames.game
    {
        public class MovementBaseComponent : BaseComponent
        {
            
            protected float moveSpeed = 0;
            protected float rotateSpeed = 0f;
            protected float maxSpeed = 0f;
            protected Collider collider = null;
            protected Rigidbody rb = null;
            protected Transform tr = null;
            protected float sqrMaxVelocity = 0;
            protected Vector3 movement = Vector3.zero;
            protected Vector3 direction = Vector3.zero;
            protected Vector3 oldPosition = Vector3.zero;
            protected Vector3 initialPosition = Vector3.zero;
            protected Quaternion initialRotation = Quaternion.identity;
            protected bool isMoving = false;
            protected float currentSpeed = 0f;
            
            
            public Collider GetCollider
            {
                get { return collider; }
            }

            public Vector3 GetDirection
            {
                get { return direction; }
            }

            public Vector3 GetMovement
            {
                get { return movement; }
            }

            public float MoveSpeed
            {
                get { return moveSpeed; }
            }

            public float RotateSpeed
            {
                get { return rotateSpeed; }
            }

            public float MaxSpeed
            {
                get { return maxSpeed; }
            }

            public Transform GetTransform
            {
                get { return tr; }
            }

            public Rigidbody GetRigidbody
            {
                get { return rb; }
            }
            
            public Vector3 Position
            {
                get
                {
                    if (rb == null)
                    {
                        if (tr != null)
                        {
                            return tr.position;
                        }
                    }
                    else
                    {
                        return rb.position;
                    }

                    return Vector3.zero;
                }
            }
            
            
            
            #region constructor

            public MovementBaseComponent() { }

            public MovementBaseComponent(MovementBaseComponent data) { }

            public MovementBaseComponent Clone()
            {
                return new MovementBaseComponent(this);
            }

            #endregion
            
            
            
            #region component

            public override void InitComponent(Transform transform, EntityData data, List<BaseComponent> components,
                params object[] args) { }


            public override void IStop()
            {
                base.IStop();
                
                oldPosition = Position;
                
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

                currentSpeed = 0f;
                movement = Vector3.zero;
                isMoving = false;
            }
            
            #endregion
            
            
            public virtual void SetInitialParameters(Transform atransform)
            {
                tr = atransform;
                tr.parent = null;
                initialPosition = tr.position;
                initialRotation = tr.rotation;
                sqrMaxVelocity = maxSpeed * maxSpeed; //square root speed
                collider = atransform.GetComponent<Collider>();
            }

            public virtual void SetInitialParameters(Transform atransform, Rigidbody aRigidbody)
            {
                tr = atransform;
                tr.parent = null;
                rb = aRigidbody;
                collider = atransform.GetComponent<Collider>();
                initialPosition = tr.position;
                initialRotation = rb.rotation;
                sqrMaxVelocity = maxSpeed * maxSpeed; //square root speed
            }
            
            public void SetNewPosition(Vector3 newposition)
            {
                if (rb != null)
                {
                    rb.MovePosition(newposition);
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
                else
                {
                    tr.position = newposition;
                }

                oldPosition = newposition;
                Physics.SyncTransforms();
            }
            
            
            #region movement
            
            public void MoveRigidbody(float extraMovementSpeed = 1f)
            {
                oldPosition = rb.transform.position;

                if (rb.velocity.sqrMagnitude < sqrMaxVelocity)
                {
                    rb.AddForce(rb.transform.forward * (moveSpeed * extraMovementSpeed));
                    rb.AddForce(Physics.gravity);
                }
                else
                {
                    var brakeSpeed = rb.velocity.sqrMagnitude - sqrMaxVelocity; // calculate the speed decrease
                    var brakeVelocity = rb.velocity.normalized * brakeSpeed; // make the brake Vector3 value
                    rb.AddForce(new Vector3(-brakeVelocity.x, Physics.gravity.y, -brakeVelocity.z));
                }

                movement = tr.forward * (moveSpeed * extraMovementSpeed) * Time.deltaTime;
                direction = tr.forward;
                currentSpeed = (moveSpeed * extraMovementSpeed);
                isMoving = true;
            }
            
            public void MoveKinematic(Vector3 newPosition, float extraMovementSpeed = 1f)
            {
                oldPosition = Position;
                tr.position = tr.position + (newPosition * ((moveSpeed * extraMovementSpeed) * Time.deltaTime));
                movement = tr.forward * (moveSpeed * extraMovementSpeed) * Time.deltaTime;
                direction = tr.forward;
                isMoving = true;
                currentSpeed = (moveSpeed * extraMovementSpeed);
            }
            
            #endregion
            
            
            #region rotation
            
            public void RotateToDirectionFromPosition(Vector3 positionTarget, float turnSpeed)
            {
                var characterPos = tr.position;

                characterPos.y = 0;
                positionTarget.y = 0;

                var newDir = positionTarget - characterPos;
                var dirLooking = Quaternion.identity;
                if (newDir != Vector3.zero)
                {
                    dirLooking = Quaternion.LookRotation(newDir);
                }

                var slerp = Quaternion.Lerp(tr.rotation, dirLooking, turnSpeed * Time.deltaTime);
                tr.rotation = slerp;
            }
            
            public void RotateToDirection(Vector3 directionTarget, float turnSpeed)
            {
                var dirLooking = Quaternion.identity;
                if (directionTarget != Vector3.zero)
                {
                    dirLooking = Quaternion.LookRotation(directionTarget);
                }
                
                var slerp = Quaternion.Lerp(rb.rotation, dirLooking, turnSpeed * Time.deltaTime);
                rb.MoveRotation(slerp);
            }
            
            #endregion
            
        }
    }
}
