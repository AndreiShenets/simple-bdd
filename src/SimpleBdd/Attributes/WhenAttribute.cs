using System;
using JetBrains.Annotations;

namespace SimpleBdd.Attributes
{
    /// <summary>
    /// The purpose of When steps is to describe the key action the user performs
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class WhenAttribute : RegExtPatternAttribute
    {
        /// <param name="regExPattern">RegEx pattern to run 'When' test method within a scenario</param>
        public WhenAttribute([NotNull] string regExPattern) : base(regExPattern)
        {
        }
    }
}