namespace _4RTools.Model
{
    public interface IAction
    {
        void Start();
        void Stop();
        string GetConfiguration();

        string GetActionName();
    }
}
