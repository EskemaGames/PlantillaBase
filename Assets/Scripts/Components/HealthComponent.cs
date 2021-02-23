using UnityEngine;
using System.Collections.Generic;
using eskemagames.eskemagames.data;


namespace eskemagames
{
    namespace eskemagames.game
    {
        [System.Serializable]
        public class HealthComponent : BaseComponent
        {
            private int initialMaxHealth;
            private int health = 0;

            public int GetInitialHealth
            {
                get { return initialMaxHealth; }
            }

            public int GetHealth
            {
                get { return health; }
            }


            #region constructor

            public HealthComponent() { }

            public HealthComponent(HealthComponent data)
            {
                initialMaxHealth = data.initialMaxHealth;
                health = data.health;
            }

            public HealthComponent(int value)
            {
                initialMaxHealth = value;
                health = value;
            }

            public HealthComponent Clone()
            {
                return new HealthComponent(this);
            }

            #endregion


            #region component

            public override void InitComponent(Transform transform, EntityData data, List<BaseComponent> components,
                params object[] args)
            {
                var value = data.GetAttributesController.GetAttributeValue<HealthAttr>();
                ISetData(data.CharacterId, (int) value);
            }

            public override void IReset()
            {
                ResetHealth();
            }

            /// <summary>
            /// param 0 uint Id
            /// param 1 int health
            /// </summary>
            /// <param name="args"></param>
            public override void ISetData(params object[] args)
            {
                entityId = (uint) args[0];
                health = (int) args[1];
                initialMaxHealth = health;
            }
            

            public override void IUpdateData(EntityData data)
            {
                //some modifier was added to this entity
                //update our health accordingly
                var m = data.GetAttributesController.GetActiveModifier<HealthAttr>();

                if (m != null)
                {
                    SetData((int) m.Attribute.Value);
                }
            }

            #endregion


            public void SetData(int value)
            {
                if (Mathf.Sign(value) == 1)
                {
                    IncreaseHealth(value);
                }
                else
                {
                    RemoveHealth(value);
                }
            }



            private void ResetHealth()
            {
                health = initialMaxHealth;
            }


            /// <summary>
            /// positive value passed In
            /// </summary>
            /// <param name="value"></param>
            private void IncreaseHealth(int value)
            {
                health += value;
                health = Mathf.Clamp(health, 0, initialMaxHealth);
            }

            
            /// <summary>
            /// negative value passed In
            /// </summary>
            /// <param name="value"></param>
            private void RemoveHealth(int value)
            {
                health += value;
                health = Mathf.Max(0, health);
            }


        }

    }
}