using System;
using JetBrains.Annotations;

namespace SimpleBdd.Attributes
{
    /// <summary>
    /// The purpose of givens is to put the system in a known state before the user (or external system)
    /// starts interacting with the system (in the When steps). Avoid talking about user interaction in givens.
    /// If you were creating use cases, givens would be your preconditions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed class GivenAttribute : RegExtPatternAttribute
    {
        /// <param name="regExPattern">RegEx pattern to run 'Given' test method within a scenario</param>
        public GivenAttribute([NotNull] string regExPattern) : base(regExPattern)
        {
        }
    }
}