namespace eskemagames
{
    namespace eskemagames.data
    {
        [System.Serializable]
        public class Modifier
        {
            //try to put a unique id, that way same modifier cant be added twice
            public uint Id { private set; get; } 
            //3 basic formulas "percent", "addition" and "critics"
            public GameEnums.Modifier ModifierType { private set; get; } 
            //what is the attribute for this modifier?
            public BaseAttribute Attribute { private set; get; } 
            //how many time will be active (in seconds), -1 will be active forever
            public int TimeActive { private set; get; }
            
            private uint timerId = 0;
            
            
            #region constructors
            //constructor
            public Modifier(uint id, int timeActive,
                GameEnums.Modifier formula,
                BaseAttribute attribute)
            {
                Id = id;
                ModifierType = formula;
                Attribute = attribute.Clone();
                TimeActive = timeActive;
                if (TimeActive != -1)
                {
                    // timerId = EG_Core.GetInstance().StartTimerId(timeActive, this, cacheAction =>
                    // {
                    //     (cacheAction.Context as Modifier).ConsumeTime();
                    // });
                }
            }

            public Modifier() { }

            public Modifier(Modifier modifier)
            {
                Id = modifier.Id;
                ModifierType = modifier.ModifierType;
                Attribute = modifier.Attribute.Clone();
                TimeActive = modifier.TimeActive;
                timerId = modifier.timerId;
            }

            public Modifier Clone()
            {
                return new Modifier(this);
            }
            #endregion


            public void ConsumeTime()
            {
                if (TimeActive == -1)
                {
                    return;
                }

                TimeActive = 0;
            }

            public bool CheckIfCanApplyModifiers()
            {
                //-1 is infinite
                if (TimeActive == -1)
                {
                    return true;
                }
                else if (TimeActive > 0)
                {
                    return true;
                }

                return false;
            }

            public bool IsModifierEnded()
            {
                return TimeActive == 0;
            }

            public void Overwrite(int time)
            {
                TimeActive = time;
            }
            public void Finish()
            {
            }
            
            //just clear the attribute
            public void Destroy()
            {
                // if (EG_Core.GetInstance() != null)
                // {
                //     EG_Core.GetInstance().StopTimerWithID(timerId);
                // }
                Attribute = null;
            }
        }

    }
}