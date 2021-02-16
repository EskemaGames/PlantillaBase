using System.Collections.Generic;

namespace eskemagames
{
    namespace eskemagames.data
    {
        [System.Serializable]
        public class HealthAttr : BaseAttribute
        {
            #region constructor

            public HealthAttr()
            {
            }

            public override BaseAttribute Clone()
            {
                return new HealthAttr(this);
            }

            public HealthAttr(HealthAttr attr) : base(attr)
            {
            }

            public HealthAttr(float value, float minvalue, float maxvalue,  List<LevelsAttribute> updatelevels, GameEnums.Modifier modifier)
                : base(value, minvalue, maxvalue, updatelevels, modifier)
            {
            }

            #endregion

        }
        
    }
}