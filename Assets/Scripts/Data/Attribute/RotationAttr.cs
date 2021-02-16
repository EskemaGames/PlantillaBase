using System.Collections.Generic;

namespace eskemagames
{
    namespace eskemagames.data
    {
        [System.Serializable]
        public class RotationAttr : BaseAttribute
        {

            #region constructor

            public RotationAttr()
            {
            }

            public override BaseAttribute Clone()
            {
                return new RotationAttr(this);
            }

            public RotationAttr(RotationAttr attribute) : base(attribute)
            {
            }

            public RotationAttr(float value, float minvalue, float maxvalue,  List<LevelsAttribute> updatelevels, GameEnums.Modifier modifier)
                : base(value, minvalue, maxvalue, updatelevels, modifier)
            {
            }

            #endregion

        }

    }
}