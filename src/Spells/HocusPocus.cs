using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Spells
{
    /// <summary>
    /// HocusPocus is unknown phrase that may be spell.
    /// </summary>
    public class HocusPocus
    {
        /// <summary>
        /// Randomly generate HocusPocus as Spell.
        /// </summary>
        /// <param name="salt">External factor for complexity to result.</param>
        /// <param name="seedByteLength">This is used internal for length of random values that for complexity to result. Load of Memory and CPU scale with its length.</param>
        public static Spell Generate(string salt, int seedByteLength = 1024)
            => Generate(Encoding.Unicode.GetBytes(salt), seedByteLength);

        public static Spell Generate(IEnumerable<byte> salt, int seedByteLength = 1024)
        {
            var ticks = DateTime.UtcNow.Ticks;
            var spice = Encoding.UTF32.GetBytes(ticks.ToString());

            // 混ぜる
            var seasoning = Shuffle(salt, spice);
            var seed = GenerateRandomBytes(seedByteLength);

            // Spellと同じフローで結果を生成します。
            var spell = Spell.Create(seasoning, seed);
            return spell;
        }

        /// <summary>
        /// 二つのバイト配列を結合して、順序をランダムに並び替えます。
        /// </summary>
        private static IEnumerable<byte> Shuffle(IEnumerable<byte> one, IEnumerable<byte> two)
        {
            var random = new Random();
            var result = one
                .Concat(two)
                .OrderBy(x => random.Next());

            return result;
        }

        /// <summary>
        /// 指定した長さのランダムバイト配列を生成します。
        /// </summary>
        private static byte[] GenerateRandomBytes(int length)
        {
            var result = new byte[length];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(result);

            return result;
        }
    }
}
