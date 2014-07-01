using System;

namespace CustomerTracker.ClientController.Core
{
    public interface IMessageNotifier
    {
        event EventHandler OnMessageFired;
        void MessageFired(string message);
    }

    public   class MessageNotifier : IMessageNotifier
    { 
        public event EventHandler OnMessageFired;
        public void MessageFired(string message)
        {
            if (OnMessageFired!=null)
            {
                OnMessageFired(message, null); 
            }
        }
    }

    public class NullMessageNotifier : IMessageNotifier
    {
        public event EventHandler OnMessageFired;
        public void MessageFired(string message)
        {
           
        }
    }
}