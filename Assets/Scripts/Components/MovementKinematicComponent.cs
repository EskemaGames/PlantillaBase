using UnityEngine;
using System.Collections.Generic;
using eskemagames.eskemagames.data;


namespace eskemagames
{
    namespace eskemagames.game
    {
        [System.Serializable]
        public class MovementKinematicComponent : MovementBaseComponent
        {
            
            private Vector3 targetPosition = Vector3.zero;
            private Vector3 movementDelta = Vector3.zero;
            
            
            #region constructor
            
            public MovementKinematicComponent() { }

            public MovementKinematicComponent(MovementKinematicComponent data) { }

            public MovementKinematicComponent Clone()
            {
                return new MovementKinematicComponent(this);
            }

            #endregion


            #region component

            public override void InitComponent(Transform transform, EntityData data, List<BaseComponent> components,
                params object[] args)
            {
                var mov = data.GetAttributesController.GetAttributeValue<MovementSpeedAttr>();
                var rot = data.GetAttributesController.GetAttributeValue<RotationAttr>();

                ISetData(
                    data.CharacterId,
                    transform.GetComponent<Transform>(),
                    transform.GetComponent<Rigidbody>(),
                    mov,
                    rot,
                    mov);
            }

            public override void IReset()
            {
            }

            /// <summary>
            /// param 0 uint Id
            /// param 1 int health
            /// </summary>
            /// <param name="args"></param>
            public override void ISetData(params object[] args)
            {
                Transform tr = null;
                Rigidbody rb = null;

                for (var i = 0; i < args.Length; ++i)
                {
                    if (args[i] is uint)
                    {
                        entityId = (uint) args[i];
                    }
                    else if (args[i] is Transform)
                    {
                        tr = args[i] as Transform;
                    }
                    else if (args[i] is Rigidbody)
                    {
                        rb = args[i] as Rigidbody;
                    }
                    else if (args[i] is float)
                    {
                        if (moveSpeed == 0f)
                        {
                            moveSpeed = (float) args[i];
                        }
                        else if (rotateSpeed == 0f)
                        {
                            rotateSpeed = (float) args[i];
                        }
                        else if (maxSpeed == 0f)
                        {
                            maxSpeed = (float) args[i];
                        }
                    }
                }
                
                SetInitialParameters(tr, rb);
            }
            

            public override void IUpdate(float time = 0f)
            {
                if (!canExecuteComponent) return;

                RotateToDirectionFromPosition(targetPosition, rotateSpeed);

                if (movementDelta.magnitude < 0.01f)
                {
                    return;
                }
                
                MoveKinematic(movementDelta, time);
            }

            
            public void SetData(Vector3 moveAxisDirection, Vector3 targetPosition)
            {
                this.movementDelta = moveAxisDirection;
                this.targetPosition = targetPosition;
            }

            public override void IStop()
            {
                movementDelta = Vector3.zero;
                base.IStop();
            }
            #endregion



        }

    }
}