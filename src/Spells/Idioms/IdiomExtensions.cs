using System.Collections.Generic;

namespace Spells.Idioms
{
    /// <summary>
    /// Helping for Idiom and Spell.
    /// </summary>
    public static class IdiomExtensions
    {
        /// <summary>
        /// Fluent calling.
        /// </summary>
        public static TOut With<TOut>(this Spell spell, IIdiom<IEnumerable<byte>, TOut> idiom)
            => spell.Value.With(idiom);

        /// <summary>
        /// Fluent calling.
        /// </summary>
        public static TOut With<TIn, TOut>(this TIn phrase, IIdiom<TIn, TOut> idiom)
            => idiom.Call(phrase);
    }
}
