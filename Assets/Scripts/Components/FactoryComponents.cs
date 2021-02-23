using System;
using System.Collections.Generic;
using eskemagames.eskemagames.game;
using UnityEngine;


namespace eskemagames
{
    
    [System.Serializable]
    public class FactoryComponents : MonoBehaviour
    {
        //store a list of components that can be used within the game
        private Dictionary<string, Type> componentsFactory = new Dictionary<string, Type>();

        private static FactoryComponents instance = null;

        public static FactoryComponents Instance
        {
            get { return instance; }
        }
        
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        private void OnDestroy()
        {
            componentsFactory.Clear();
        }


        public void Init(List<string> componentNames)
        {
            for (var i = 0; i < componentNames.Count; ++i)
            {
                //components are the attribute name + "Component" added
                Type myType = Type.GetType("eskemagames.eskemagames.game." + componentNames[i]);

                if (myType != null)
                {
                    if (!componentsFactory.ContainsKey(componentNames[i]))
                    {
                        componentsFactory.Add(componentNames[i], myType);
                    }
                }
            }
        }


        public BaseComponent GetCustomComponent(string name)
        {
            Type compType;

            if (componentsFactory.TryGetValue(name, out compType))
            {
                return (BaseComponent) Activator.CreateInstance(compType);
            }

            return null;
        }

    }
}