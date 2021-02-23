using System.Collections.Generic;
using UnityEngine;


namespace eskemagames
{
    
    #region attributes base

    [System.Serializable]
    public class ParseAttributesData
    {
        public List<AttributesJsonData> Attributes = new List<AttributesJsonData>(20);
    }

    [System.Serializable]
    public class LevelsAttributeJsonData
    {
        [SerializeField] public float IncreaseStep = 0f;
        [SerializeField] public float Value = 0f;
    }
    
    [System.Serializable]
    public class AttributesJsonData
    {
        public uint Id = 0;
        public float Value = 0f;
        public float MinValue = 0f;
        public float MaxValue = 0f;
        public List<LevelsAttributeJsonData> UpdateLevels = new List<LevelsAttributeJsonData>(15);
        public string AttributeName = System.String.Empty;
        public string FormulaType = System.String.Empty;
    }

    #endregion


    #region character

    [System.Serializable]
    public class CharacterParseJsonData
    {
        public string ClassName = System.String.Empty;
        public string PortraitName = System.String.Empty;
        public string PrefabName = System.String.Empty;
        public string NameId = System.String.Empty;
        public string Behaviour = System.String.Empty;
        public string GroupType = System.String.Empty;
        [System.NonSerialized] public GameEnums.GroupTypes Group = GameEnums.GroupTypes.Max;
        public string Weapon = System.String.Empty;
        public List<AttributesJsonData> Attributes = new List<AttributesJsonData>();
        public List<ComponentJsonData> Components = new List<ComponentJsonData>(); //NEW LINE
    }

    [System.Serializable]
    public class PARSERootCharactersJsonData
    {
        public List<CharacterParseJsonData> Characters = new List<CharacterParseJsonData>();
    }

    #endregion
    
    
    #region components

    [System.Serializable]
    public class ComponentJsonData
    {
        public string ClassName = System.String.Empty;
    }
    
    [System.Serializable]
    public class PARSERootComponentsJsonData
    {
        public List<ComponentJsonData> Components = new List<ComponentJsonData>();
    }

    #endregion

}
