using System;
using JetBrains.Annotations;

namespace SimpleBdd.Attributes
{
    /// <summary>
    /// The purpose of Then steps is to observe outcomes. The observations should be related to the business
    /// value/benefit in your feature description. The observations should also be on some kind of output –
    /// that is something that comes out of the system (report, user interface, message) and not something that
    /// is deeply buried inside it (that has no business value).
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class ThenAttribute : RegExtPatternAttribute
    {
        /// <param name="regExPattern">RegEx pattern to run 'Then' test method within a scenario</param>
        public ThenAttribute([NotNull] string regExPattern) : base(regExPattern)
        {
        }
    }
}