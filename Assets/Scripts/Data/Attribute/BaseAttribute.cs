using System.Collections.Generic;

namespace eskemagames
{
    namespace eskemagames.data
    {
        [System.Serializable]
        public abstract class BaseAttribute
        {
            [System.Serializable]
            public class LevelsAttribute
            {
                public float IncreaseStep { set; get; }
                public float Value { set; get; }

                public LevelsAttribute(float increase, float val)
                {
                    IncreaseStep = increase;
                    Value = val;
                }
            }

            public float Value { get; private set; }
            public float MinValue { get; private set; }
            public float MaxValue { get; private set; }
            public GameEnums.Modifier Modifier { get; private set; }
            public List<LevelsAttribute> UpdateLevels { private set; get; }

            public string Name
            {
                get { return GetType().Name; }
            }



            #region constructor

            public BaseAttribute()
            {
                UpdateLevels = new List<LevelsAttribute>();
            }

            public BaseAttribute(
                float value,
                float minvalue,
                float maxvalue,
                List<LevelsAttribute> updateLevels,
                GameEnums.Modifier modifier)
            {
                Value = value;
                MinValue = minvalue;
                MaxValue = maxvalue;
                Modifier = modifier;

                var someData = new List<LevelsAttribute>();
                for (var i = 0; i < updateLevels.Count; ++i)
                {
                    someData.Add(updateLevels[i]);
                }

                UpdateLevels = new List<LevelsAttribute>(someData);
            }

            public BaseAttribute(BaseAttribute attr)
            {
                Value = attr.Value;
                MinValue = attr.MinValue;
                MaxValue = attr.MaxValue;
                Modifier = attr.Modifier;
                UpdateLevels = attr.UpdateLevels;
            }

            public virtual BaseAttribute Clone()
            {
                return null;
            }

            #endregion

        }

    }
}