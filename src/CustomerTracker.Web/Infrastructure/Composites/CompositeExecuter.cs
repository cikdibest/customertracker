namespace CustomerTracker.Web.Infrastructure.Composites
{
    public class CompositeExecuter : IExecute
    {
        private IExecute[] _executers;

        public CompositeExecuter(params IExecute[] executers)
        {
            _executers = executers; 
        }

        public void Execute()
        {
            foreach (var executer in _executers)
            {
                executer.Execute();
            }
        }
    }
}