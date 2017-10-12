using System.Diagnostics;

namespace SimpleBdd.Loggers
{
    public class ScenarioLogger : IScenarioLogger
    {
        public void Log(string message)
        {
            Debug.WriteLine(message);
        }
    }
}