using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerTracker.Web.Infrastructure.Tasks
{
    public static class TaskExecutor
    {
        private static readonly ThreadLocal<List<BackgroundTask>> TasksToExecute =
            new ThreadLocal<List<BackgroundTask>>(() => new List<BackgroundTask>());
         
        public static Action<Exception> ExceptionHandler { get; set; }

        public static void ExcuteLater(BackgroundTask task)
        {
            TasksToExecute.Value.Add(task);
        }

        public static void Discard()
        {
            TasksToExecute.Value.Clear();
        }

        public static void StartExecuting()
        {
            var value = TasksToExecute.Value;

            var copy = value.ToArray();

            value.Clear();

            if (copy.Length <= 0) return;

            Task.Factory.StartNew(() =>
                                      {
                                          foreach (var backgroundTask in copy)
                                          {
                                              ExecuteTask(backgroundTask);
                                          }
                                      }, TaskCreationOptions.LongRunning)
                                  .ContinueWith(task =>
                                  {
                                      if (ExceptionHandler != null) ExceptionHandler(task.Exception);
                                  }, TaskContinuationOptions.OnlyOnFaulted);
        }

        private static void ExecuteTask(BackgroundTask task)
        {
            for (var i = 0; i < 10; i++)
            {
                switch (task.Run())
                {
                    case true:
                    case false:
                        return;
                    case null:
                        break;
                }

            }
        }
    }
}