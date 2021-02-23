using UnityEngine;
using System.Collections.Generic;
using eskemagames.eskemagames.data;


namespace eskemagames
{
    namespace eskemagames.game
    {
        [System.Serializable]
        public class MovementRigidbodyComponent : MovementBaseComponent
        {
            private Vector3 targetPosition = Vector3.zero;
            private Vector3 movementDelta = Vector3.zero;
            
            #region constructor

            public MovementRigidbodyComponent() { }

            public MovementRigidbodyComponent(MovementRigidbodyComponent data)
            {
            }

            public MovementRigidbodyComponent Clone()
            {
                return new MovementRigidbodyComponent(this);
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
            

            public override void IUpdateData(EntityData data)
            {
            }

            public override void IRefreshData(params object[] args)
            {
                for (int i = 0; i < args.Length; ++i)
                {
                    if (args[i] is Vector3)
                    {
                        movementDelta = (Vector3)args[i];
                    }
                }
            } 
            
            public override void IUpdate(float time = 0f)
            {
                if (!canExecuteComponent) return;
                
                if (movementDelta.magnitude < 0.01f)
                {
                    RotateToDirectionFromPosition(targetPosition, rotateSpeed);
                    return;
                }

                RotateToDirectionFromPosition(targetPosition, rotateSpeed);
                MoveRigidbody(time);
            }

            
            public void SetData(Vector3 movementDelta, Vector3 targetPosition)
            {
                this.movementDelta = movementDelta;
                this.targetPosition = targetPosition;
            }
            
            public void SetData(Vector3 movementDelta)
            {
                this.movementDelta = movementDelta;
                this.targetPosition = tr.position + tr.forward;
            }
            #endregion


  


        }

    }
}