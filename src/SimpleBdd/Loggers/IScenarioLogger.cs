using JetBrains.Annotations;

namespace SimpleBdd.Loggers
{
    public interface IScenarioLogger
    {
        void Log([NotNull] string message);
    }
}