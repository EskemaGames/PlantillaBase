

using System.Collections.Generic;

namespace eskemagames
{
    namespace eskemagames.data
    {
        [System.Serializable]
        public class DefenseAttr : BaseAttribute
        {
            #region constructor

            public DefenseAttr()
            {
            }

            public override BaseAttribute Clone()
            {
                return new DefenseAttr(this);
            }

            public DefenseAttr(DefenseAttr attribute) : base(attribute)
            {
            }


            public DefenseAttr(float value, float minvalue, float maxvalue, List<LevelsAttribute> updatelevels, GameEnums.Modifier modifier)
                : base(value, minvalue, maxvalue, updatelevels, modifier)
            {
            }

            #endregion

        }

    }
}