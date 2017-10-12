using System;
using JetBrains.Annotations;

namespace SimpleBdd.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class RegExtPatternAttribute : Attribute
    {
        [NotNull]
        public string RegExPattern { get; }


        protected RegExtPatternAttribute([NotNull] string regExPattern)
        {
            RegExPattern = regExPattern;
        }
    }
}