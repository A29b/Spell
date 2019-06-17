using System;
using System.Linq;

namespace Spells.Idioms
{
    /// <summary>
    /// Abbreviate phrase.
    /// </summary>
    public class Abbreviate : IIdiom<string, string>
    {
        public Abbreviate(int length)
        {
            Length = length;
        }

        public readonly int Length;

        public string Call(string phrase)
        {
            var result = new String(phrase.Take(Length).ToArray());
            return result;
        }
    }
}
