using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using eskemagames;
using eskemagames.eskemagames.data;
using UnityEngine;




public class DataDDBB : MonoBehaviour
{
    [SerializeField] TextAsset[] charactersTextList = null;

    
    private static DataDDBB instance = null;
    private List<EntityData> charactersData = new List<EntityData>();

    public static DataDDBB Instance
    {
        get { return instance; }
    }
    




    
    #region init and destroy

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Init()
    {
        uint characterIdCounterTmp = 1;

        //create the characters for the data parsed
        charactersData = new List<EntityData>(50);

        ParseCharactersData(charactersTextList, ref characterIdCounterTmp);
    }
    
    public void OnDestroy()
    {
        charactersData.Clear();
    }

    #endregion



    
    

    #region parse data from json

    private void ParseCharactersData(TextAsset[] files, ref uint characterIdCounter)
    {
        for (var i = 0; i < files.Length; ++i)
        {
            var charsData = files[i];
            PARSERootCharactersJsonData allData = null;

            try
            {
                allData = JsonUtility.FromJson<PARSERootCharactersJsonData>(charsData.text);
            }
            catch (Exception)
            {
                Debug.LogError("error parsing characters json number in list= " + charsData.name);
                return;
            }


            for (var cnt = 0; cnt < allData.Characters.Count; ++cnt)
            {
                //parse the character attributes
                var attributesParsed = ParseAttributes(allData.Characters[cnt].Attributes);

                var groupType = (GameEnums.GroupTypes) System.Enum.Parse(typeof(GameEnums.GroupTypes),
                    allData.Characters[cnt].GroupType);
                
                var character = new EntityData(
                    characterIdCounter,
                    allData.Characters[cnt].NameId,
                    allData.Characters[cnt].PortraitName,
                    allData.Characters[cnt].ClassName,
                    allData.Characters[cnt].Behaviour,
                    allData.Characters[cnt].Weapon,
                    groupType,
                    attributesParsed);

                charactersData.Add(character);
                
                characterIdCounter++;
            }
        }
    }

    #endregion


    #region internal parse of elements

    private List<BaseAttribute> ParseAttributes(List<AttributesJsonData> attributes)
    {
        //parse the character attributes
        var attributesParsed = new List<BaseAttribute>(10);

        for (var counter = 0; counter < attributes.Count; ++counter)
        {
            var formula = (GameEnums.Modifier) System.Enum.Parse(typeof(GameEnums.Modifier),
                attributes[counter].FormulaType);

            var typeClass = System.Type.GetType("eskemagames.eskemagames.data." + attributes[counter].AttributeName);

            var levels = new List<BaseAttribute.LevelsAttribute>(15);

            for (var i = 0; i < attributes[counter].UpdateLevels.Count; ++i)
            {
                levels.Add(new BaseAttribute.LevelsAttribute(attributes[counter].UpdateLevels[i].IncreaseStep,
                    attributes[counter].UpdateLevels[i].Value));
            }
            
            object[] argTypes = new object[]
            {
                attributes[counter].Value,
                attributes[counter].MinValue,
                attributes[counter].MaxValue,
                levels,
                formula
            };

            var attr = Activator.CreateInstance(typeClass, argTypes) as BaseAttribute;

            attributesParsed.Add(attr);
        }

        return attributesParsed;
    }

    #endregion
    

    #region getters
    
    public ReadOnlyCollection<EntityData> GetCharactersData
    {
        get { return charactersData.AsReadOnly(); }
    }

    public EntityData GetCharacterData(string name)
    {
        //look in both players and npcs, enemies, etc
        for (var i = 0; i < charactersData.Count; ++i)
        {
            if (charactersData[i].ClassName.Equals(name))
            {
                return charactersData[i];
            }
        }

        return null;
    }

    #endregion

    

}

