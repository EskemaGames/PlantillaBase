using UnityEngine;
using System.Collections.Generic;
using eskemagames.eskemagames.data;

namespace eskemagames
{
    namespace eskemagames.game
    {
        //
        //any component will inherit from the base
        //
        
        public abstract class BaseComponent
        {
            protected bool canExecuteComponent = false;
            protected uint entityId = 0;

            public bool ICanExecuteComponent
            {
                get { return canExecuteComponent; }
            }
            
            //what entity is tied to this component?
            public virtual uint GetId
            {
                get { return entityId; }
            } 

            //some components can handle data, so they could be "busy" if they implement an internal flag
            public virtual bool IIsBusy()
            {
                return false;
            } 
            
            public virtual bool ICanExecute()
            {
                return false;
            }

            public abstract void InitComponent(Transform transform, EntityData data, List<BaseComponent> components, params object[] args);

            //can receive delta time or other value you want to pass as a float
            public virtual void IUpdate(float value = 0f) { } 

            public virtual void IGamePaused(bool ispaused) { }

            public virtual void IReset()
            {
                canExecuteComponent = false;
            }

            public virtual void IResetForPool()
            {
                canExecuteComponent = false;
            }
            
            //set data to be used by this component
            public virtual void ISetData(params object[] args) { } 

            //you can use it to update vars or assign something to those vars
            public virtual void IRefreshData(params object[] args) { } 

            //you can use it to update vars or assign something to those vars
            public virtual void IUpdateData(EntityData data) {} 

            
            public virtual void IExecute() {}
            
            public virtual void IExecuteWithResponse(System.Action onExecuted) {}

            public virtual void IStart()
            {
                canExecuteComponent = true;
            }

            public virtual void IStop()
            {
                canExecuteComponent = false;
            }

            public virtual void IDestroy() { }
        }

    }
}