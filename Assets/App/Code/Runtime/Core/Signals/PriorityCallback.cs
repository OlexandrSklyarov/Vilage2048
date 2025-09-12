namespace Assets.App.Code.Runtime.Core.Signals
{
    public class PriorityCallback
    {
        public int Priority {get;}
        public object Callback {get;}
        
        public PriorityCallback(int priority, object callback)
        {
            Priority = priority;
            Callback = callback;
        }
    }
}

