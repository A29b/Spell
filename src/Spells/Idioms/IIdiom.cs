namespace Spells.Idioms
{
    /// <summary>
    /// Providing another phrases.
    /// </summary>
    public interface IIdiom<TIn, TOut>
    {
        /// <summary>
        /// Convert to another phrases.
        /// </summary>
        TOut Call(TIn phrase);
    }
}
