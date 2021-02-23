using System.Collections.Generic;

namespace eskemagames
{
    namespace eskemagames.data
    {
        [System.Serializable]
        public class EntityData
        {
            //assign a number for enemies or players
            public uint CharacterId { get; private set; }
            public string Id { get; private set; }
            public string PortraitName { get; private set; }
            public string ClassName { get; private set; }
            public string Weapon { get; private set; }
            public string Behaviour { get; private set; }
            public string PrefabName { get; private set; }
            public GameEnums.GroupTypes GroupType { get; private set; }
            private AttributesAndModifiersController attributesAndModifiersController = new AttributesAndModifiersController();
    
            public AttributesAndModifiersController GetAttributesController
            {
                get { return attributesAndModifiersController; }
            }


            
            #region constructor

            public EntityData()
            {
            }

            public EntityData(
                uint characterid,
                string id,
                string portraitname,
                string classname,
                string behaviour,
                string weapon,
                string prefabName,
                GameEnums.GroupTypes group,
                List<BaseAttribute> attributes)
            {
                CharacterId = characterid;
                Id = id;
                PortraitName = portraitname;
                ClassName = classname;
                Behaviour = behaviour;
                Weapon = weapon;
                PrefabName = prefabName;
                GroupType = group;
                attributesAndModifiersController.AddAttributes(attributes);
            }

            public EntityData(EntityData data)
            {
                CharacterId = data.CharacterId;
                Id = data.Id;
                PortraitName = data.PortraitName;
                ClassName = data.ClassName;
                PortraitName = data.PortraitName;
                Behaviour = data.Behaviour;
                Weapon = data.Weapon;
                PrefabName = data.PrefabName;
                GroupType = data.GroupType;
                attributesAndModifiersController = data.attributesAndModifiersController;
            }

            public EntityData Clone()
            {
                return new EntityData(this);
            }

            #endregion



            public void Destroy()
            {
                attributesAndModifiersController.Destroy();
            }
            

        }

    }
}