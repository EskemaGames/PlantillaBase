using System.Collections.Generic;

namespace eskemagames
{
    namespace eskemagames.data
    {
        [System.Serializable]
        public class AttackAttr : BaseAttribute
        {

            #region constructor

            public AttackAttr()
            {
            }

            public override BaseAttribute Clone()
            {
                return new AttackAttr(this);
            }

            public AttackAttr(AttackAttr attribute) : base(attribute)
            {
            }

            public AttackAttr(float value, float minvalue, float maxvalue, List<LevelsAttribute> updatelevels, GameEnums.Modifier modifier)
                : base(value, minvalue, maxvalue, updatelevels, modifier)
            {
            }

            #endregion

        }

    }
}