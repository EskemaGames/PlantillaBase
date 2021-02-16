using System;
using System.Collections.Generic;
using UnityEngine;


namespace eskemagames
{
    namespace eskemagames.data
    {
        [System.Serializable]
        public class AttributesAndModifiersController
        {
            [SerializeField] private List<BaseAttribute> attributes = new List<BaseAttribute>(30);
            [SerializeField] private List<Modifier> modifiers = new List<Modifier>(200);

            private List<Modifier>
                modifiersTmp =
                    new List<Modifier>(
                        200); 
            //for the operations within the functions to avoid creating a list everytime



            #region constructor and destroy

            public AttributesAndModifiersController()
            {
            }

            public AttributesAndModifiersController(AttributesAndModifiersController data)
            {
                var attr = new List<BaseAttribute>();
                for (var i = 0; i < data.attributes.Count; ++i)
                {
                    attr.Add( data.attributes[i].Clone());
                }

                attributes = new List<BaseAttribute>(attr);

                var modif = new List<Modifier>();
                for (var i = 0; i < data.modifiers.Count; ++i)
                {
                    modif.Add(new Modifier(data.modifiers[i]));
                }

                modifiers = new List<Modifier>(modif);

                modifiersTmp.Clear();
            }

            public AttributesAndModifiersController Clone()
            {
                return new AttributesAndModifiersController(this);
            }

            public void Destroy()
            {
                attributes.Clear();

                var total = modifiers.Count - 1;
                for (var i = total; i > -1; --i)
                {
                    modifiers[i].Destroy();
                }

                modifiers.Clear();
                modifiersTmp.Clear();
            }

            #endregion



            #region attributes

            public void AddAttributes(List<BaseAttribute> attributes)
            {
                Destroy(); //clear any previous
                this.attributes = new List<BaseAttribute>(attributes);
            }

            public void AddAttribute(BaseAttribute attr)
            {
                if (!attributes.Contains(attr))
                {
                    attributes.Add(attr);
                }
            }

            public List<BaseAttribute> GetAllAttributesByType<T>()
            {
                var tmp = new List<BaseAttribute>();

                for (var i = 0; i < attributes.Count; ++i)
                {
                    if (attributes[i] is T)
                    {
                        tmp.Add(attributes[i]);
                    }
                }

                return tmp;
            }

            public List<BaseAttribute> GetAllAttributesButOne<T>()
            {
                var tmp = new List<BaseAttribute>();

                for (var i = 0; i < attributes.Count; ++i)
                {
                    if (attributes[i] is T)
                    {
                    }
                    else
                    {
                        tmp.Add(attributes[i]);
                    }
                }

                return tmp;
            }


            public BaseAttribute GetAttribute<T>()
            {
                for (var i = 0; i < attributes.Count; ++i)
                {
                    if (attributes[i] is T)
                    {
                        return attributes[i];
                    }
                }

                return null;
            }

            public BaseAttribute GetAttribute(string name)
            {
                for (var i = 0; i < attributes.Count; i++)
                {
                    if (attributes[i].Name == name)
                    {
                        return attributes[i];
                    }
                }

                return null;
            }

            public void RemoveAttribute<T>()
            {
                for (var i = 0; i < attributes.Count; ++i)
                {
                    if (attributes[i] is T)
                    {
                        attributes.RemoveAt(i);
                        break;
                    }
                }
            }

            public float GetAttributeBaseValue<T>()
            {
                var attr = GetAttribute<T>();
                return attr == null ? 0f : attr.Value;
            }

            public float GetAttributeBaseValue(string name)
            {
                var attr = GetAttribute(name);
                return attr == null ? 0f : attr.Value;
            }

            public float GetAttributeBaseMaxValue<T>()
            {
                var attr = GetAttribute<T>();
                return attr == null ? 0f : attr.MaxValue;
            }

            public float GetAttributeBaseMaxValue(string name)
            {
                var attr = GetAttribute(name);
                return attr == null ? 0f : attr.MaxValue;
            }

            public float GetAttributeValue<T>()
            {
                var modifiersReady = GetActiveModifiers<T>(modifiers);

                return GetModifiersValues<T>(modifiersReady);
            }

            public float GetAttributeValue(string name)
            {
                var modifiersReady = GetModifiers(name);

                return GetModifiersValues(modifiersReady, name);
            }

            public float GetAttributeMaxValue(string name)
            {
                var modifiersReady = GetModifiers(name);

                return GetModifiersMaxValues(modifiersReady, name);
            }



            #endregion



            private float GetModifiersMaxValues(List<Modifier> modifiersReady, string name)
            {
                var finalValue = 0f;
                var multValue = 0f;
                var foundValue = false;

                for (var i = 0; i < modifiersReady.Count; ++i)
                {
                    var attributeBaseValue = GetAttributeBaseMaxValue(name);

                    if (modifiersReady[i].ModifierType == GameEnums.Modifier.Percentage)
                    {
                        SetValueForPercent(modifiersReady[i].Attribute.MaxValue, attributeBaseValue, finalValue, multValue, out multValue, out foundValue, out finalValue);
                    }
                    else if (modifiersReady[i].ModifierType == GameEnums.Modifier.Addition)
                    {
                        SetValueForAddition(modifiersReady[i], attributeBaseValue, finalValue, out foundValue, out finalValue);
                    }
                }


                //no modifiers, return default value
                if (modifiersReady.Count == 0)
                {
                    return finalValue = GetAttributeBaseMaxValue(name);
                }

                //we had modifiers, but none of them apply to the attribute
                //we are looking, so just return the default value
                if (finalValue == 0f && !foundValue)
                {
                    return finalValue = GetAttributeBaseMaxValue(name);
                }

                return finalValue + (finalValue * multValue);
            }

            private float GetModifiersMaxValues<T>(List<Modifier> modifiersReady)
            {
                var finalValue = 0f;
                var multValue = 0f;
                var foundValue = false;

                for (var i = 0; i < modifiersReady.Count; i++)
                {
                    var attributeBaseValue = GetAttributeBaseMaxValue<T>();

                    if (modifiersReady[i].ModifierType == GameEnums.Modifier.Percentage)
                    {
                        SetValueForPercent(modifiersReady[i].Attribute.MaxValue, attributeBaseValue, finalValue, multValue, out multValue, out foundValue, out finalValue);
                    }
                    else if (modifiersReady[i].ModifierType == GameEnums.Modifier.Addition)
                    {
                        SetValueForAddition(modifiersReady[i], attributeBaseValue, finalValue, out foundValue, out finalValue);
                    }
                }


                //no modifiers, return default value
                if (modifiersReady.Count == 0)
                {
                    return finalValue = GetAttributeBaseMaxValue<T>();
                }

                //we had modifiers, but none of them apply to the attribute
                //we are looking, so just return the default value
                if (finalValue == 0f && !foundValue)
                {
                    return finalValue = GetAttributeBaseMaxValue<T>();
                }

                return finalValue + (finalValue * multValue);
            }

            private float GetModifiersValues(List<Modifier> modifiersReady, string name)
            {
                var finalValue = 0f;
                var multValue = 0f;
                var foundValue = false;

                for (var i = 0; i < modifiersReady.Count; i++)
                {
                    var attributeBaseValue = GetAttributeBaseValue(name);

                    if (modifiersReady[i].ModifierType == GameEnums.Modifier.Percentage)
                    {
                        var valueIncrease = modifiersReady[i].Attribute.UpdateLevels.Count > 0 ? modifiersReady[i].Attribute.UpdateLevels[0].IncreaseStep : 0f;
                        SetValueForPercent(valueIncrease, attributeBaseValue, finalValue, multValue, out multValue, out foundValue, out finalValue);
                    }
                    else if (modifiersReady[i].ModifierType == GameEnums.Modifier.Addition)
                    {
                        SetValueForAddition(modifiersReady[i], attributeBaseValue, finalValue, out foundValue, out finalValue);
                    }
                }


                //no modifiers, return default value
                if (modifiersReady.Count == 0)
                {
                    return finalValue = GetAttributeBaseValue(name);
                }

                //we had modifiers, but none of them apply to the attribute
                //we are looking, so just return the default value
                if (finalValue == 0f && !foundValue)
                {
                    return finalValue = GetAttributeBaseValue(name);
                }

                return finalValue + (finalValue * multValue);
            }

            private float GetModifiersValues<T>(List<Modifier> modifiersReady)
            {
                var finalValue = 0f;
                var multValue = 0f;
                var foundValue = false;

                for (var i = 0; i < modifiersReady.Count; i++)
                {
                    var attributeBaseValue = GetAttributeBaseValue<T>();

                    if (modifiersReady[i].ModifierType == GameEnums.Modifier.Percentage)
                    {
                        SetValueForPercent(modifiersReady[i].Attribute.Value, attributeBaseValue, finalValue, multValue, out multValue, out foundValue, out finalValue);
                    }
                    else if (modifiersReady[i].ModifierType == GameEnums.Modifier.Addition)
                    {
                        SetValueForAddition(modifiersReady[i], attributeBaseValue, finalValue, out foundValue, out finalValue);
                    }
                }

                //no modifiers, return default value
                if (modifiersReady.Count == 0)
                {
                    return finalValue = GetAttributeBaseValue<T>();
                }

                //we had modifiers, but none of them apply to the attribute
                //we are looking, so just return the default value
                if (finalValue == 0f && !foundValue)
                {
                    return finalValue = GetAttributeBaseValue<T>();
                }

                return finalValue + (finalValue * multValue);
            }

            private void SetValueForPercent(float modifierValue, float attributeValue, float finalValue, float multValue, out float multvalue, out bool foundvalue, out float finalvalue)
            {
                var tmpmultiplierValue = modifierValue;
                tmpmultiplierValue += multValue;

                //only update the value if it didn't exist
                if (finalValue == 0)
                {
                    finalvalue = attributeValue;
                }
                else
                {
                    finalvalue = finalValue;
                }

                foundvalue = true;
                multvalue = tmpmultiplierValue;
            }

            private void SetValueForAddition(Modifier modifier, float attributeBaseValue, float finalValue, out bool foundvalue, out float finalvalue)
            {
                var additionValue = attributeBaseValue;

                var tmpValue = finalValue;

                foundvalue = true;

                if (finalValue == 0)
                {
                    tmpValue = additionValue + modifier.Attribute.Value;
                }
                else
                {
                    tmpValue += modifier.Attribute.Value;
                }

                finalvalue = tmpValue;
            }


            #region modifiers

            public List<Modifier> GetActiveModifiers<T>(List<Modifier> allModifiers)
            {
                modifiersTmp.Clear();

                for (var i = 0; i < allModifiers.Count; i++)
                {
                    if (allModifiers[i].CheckIfCanApplyModifiers() && allModifiers[i].Attribute is T)
                    {
                        modifiersTmp.Add(allModifiers[i]);
                    }
                }

                return modifiersTmp;
            }

            private List<Modifier> GetModifiers(string name)
            {
                modifiersTmp.Clear();

                for (var i = 0; i < modifiers.Count; ++i)
                {
                    if (!modifiers[i].CheckIfCanApplyModifiers()) continue;
                    
                    if (modifiers[i].Attribute.Name.Equals(name))
                    {
                        modifiersTmp.Add(modifiers[i]);
                    }
                }

                return modifiersTmp;
            }


            private void RemoveModifiersOnComplete()
            {
                var total = modifiers.Count - 1;

                for (var i = total; i > -1; --i)
                {
                    if (!modifiers[i].IsModifierEnded()) continue;

                    modifiers[i].Finish();
                    modifiers.RemoveAt(i);
                }
            }

            public bool CheckForDuplicateModifier(uint id)
            {
                for (var i = 0; i < modifiers.Count; ++i)
                {
                    if (modifiers[i].Id == id)
                    {
                        return true;
                    }
                }

                return false;
            }

            public void AddModifier(Modifier modifier, bool overwrite = false)
            {
                if (overwrite)
                {
                    var canAdd = true;
                    var posId = 0;
                    for (var i = 0; i < modifiers.Count; ++i)
                    {
                        if (modifiers[i].Id == modifier.Id)
                        {
                            canAdd = false;
                            posId = i;
                            break;
                        }
                    }

                    if (canAdd)
                    {
                        modifiers.Add(modifier);
                    }
                    else
                    {
                        modifiers[posId].Overwrite(modifier.TimeActive);
                    }
                }
                else
                {
                    modifiers.Add(modifier);
                }
            }

            public void AddModifier(Modifier modifier)
            {
                modifiers.Add(modifier);
            }

            public void AddModifiers(List<Modifier> modifiersList)
            {
                var total = modifiers.Count - 1;
                for (var i = total; i > -1; --i)
                {
                    modifiers[i].Destroy();
                }

                modifiers.Clear();
                modifiersTmp.Clear();

                this.modifiers = new List<Modifier>(modifiersList);
            }

            public void ExecuteModifier(Modifier modifier)
            {
                //modifier.Execute(RemoveModifiersOnComplete);
            }

            public void StopVisualModifiers()
            {
                for (var i = 0; i < modifiers.Count; ++i)
                {
                    //modifiers[i].StopVisuals();
                }
            }

            public Modifier GetModifier<T>()
            {
                for (var i = 0; i < modifiers.Count; ++i)
                {
                    if (modifiers[i].Attribute is T)
                    {
                        return modifiers[i];
                    }
                }

                return null;
            }


            public Modifier GetActiveModifier<T>()
            {
                for (var i = 0; i < modifiers.Count; ++i)
                {
                    if (!(modifiers[i].Attribute is T)) continue;

                    if (!modifiers[i].IsModifierEnded()
                        && modifiers[i].TimeActive != -1)
                    {
                        return modifiers[i];
                    }
                }

                return null;
            }

            public Modifier GetModifier<T>(uint id)
            {
                for (var i = 0; i < modifiers.Count; ++i)
                {
                    if (!(modifiers[i].Attribute is T)) continue;

                    if (id == modifiers[i].Id)
                    {
                        return modifiers[i];
                    }
                }

                return null;
            }

            public List<Modifier> GetCopyOfAllModifiers()
            {
                return new List<Modifier>(modifiers);
            }

            #endregion


            #region remove modifiers

            public void RemoveModifier(Modifier aModifier)
            {
                for (var i = modifiers.Count - 1; i > -1; --i)
                {
                    if (modifiers[i] == aModifier)
                    {
                        modifiers.RemoveAt(i);
                        break;
                    }
                }
            }

            public void RemoveModifier(uint id)
            {
                for (var i = modifiers.Count - 1; i > -1; --i)
                {
                    if (modifiers[i].Id == id)
                    {
                        modifiers.RemoveAt(i);
                        break;
                    }
                }
            }


            public void RemoveAllModifiersWithType<T>()
            {
                for (var i = modifiers.Count - 1; i > -1; --i)
                {
                    if (modifiers[i].Attribute is T)
                    {
                        modifiers.RemoveAt(i);
                    }
                }
            }

            public void RemoveAllModifiers()
            {
                modifiers.Clear();
                modifiersTmp.Clear();
            }

            public void RemoveAllActiveModifiers()
            {
                for (var i = modifiers.Count - 1; i > -1; --i)
                {
                    if (modifiers[i].TimeActive >= 0)
                    {
                        modifiers[i].Destroy();
                        modifiers.RemoveAt(i);
                    }
                }

                modifiersTmp.Clear();
            }

            #endregion



            #region create modifier

            public static Modifier CreateModifier(
                uint id,
                int timeactive,
                GameEnums.Modifier formula,
                float modifierValue,
                float modifierMinValue,
                float modifierMaxValue,
                string attributeName)
            {
                var argTypes = new object[]
                {
                    modifierValue,
                    modifierMinValue,
                    modifierMaxValue,
                    null,
                    formula
                };

                var attr = CreateInstance<BaseAttribute>("eskemagames.eskemagames.data.", attributeName, argTypes);

                var modifier = new Modifier(
                    id,
                    timeactive,
                    formula,
                    attr);

                return modifier;
            }


            /// <summary>
            /// create a modifier with a default value min of 1 and 100 for max
            /// </summary>
            /// <param name="id"></param>
            /// <param name="turns"></param>
            /// <param name="aFormula"></param>
            /// <param name="modifierValue"></param>
            /// <param name="attributeName"></param>
            /// <returns></returns>
            public static Modifier CreateModifier(
                uint id,
                int timeactive,
                GameEnums.Modifier aFormula,
                float modifierValue,
                string attributeName)
            {
                //set to a minimum of 1 and maximum of 100
                return CreateModifier(id, timeactive, aFormula, modifierValue, 0f, 100f, attributeName);
            }
            
            #endregion


            private static I CreateInstance<I>(string namespaceName, string className, object[] someParams) where I : class
            {
                var typeClass = System.Type.GetType(namespaceName + className);
                return Activator.CreateInstance(typeClass, someParams) as I;
            }

        }

    }
}