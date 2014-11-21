using System;

namespace CustomerTracker.Web.Angular.Infrastructure.Tasks
{
    public abstract class BackgroundTask
    { 

        protected virtual void OnError(Exception e)
        {
        } 

        public bool? Run()
        {
        
            try
            {
                Execute();
               
                TaskExecutor.StartExecuting();

                return true;
            }
            
            catch (Exception e)
            {
                OnError(e);
                return false;
            }
            finally
            {
                TaskExecutor.Discard();
            }
        }

        protected abstract void Execute();       
    }  
}