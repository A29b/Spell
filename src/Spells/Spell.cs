using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Spells
{
    /// <summary>
    /// A Spell. This meaning of phrase is known me.
    /// </summary>
    public class Spell
    {
        public Spell(byte[] phrase)
        {
            Value = phrase;
        }

        public readonly byte[] Value;

        /// <summary>
        /// Build spell by salt and seed.
        /// </summary>
        /// <param name="salt">A key to unlock seed.</param>
        /// <param name="seed">A secret phrase to cause spell.</param>
        public static Spell Create(string salt, string seed)
            => Create(Encoding.Unicode.GetBytes(salt), Encoding.Unicode.GetBytes(seed));

        public static Spell Create(IEnumerable<byte> salt, IEnumerable<byte> seed)
        {
            // seedの複雑性を高める。
            // 魔女が料理をするような工程。
            var material = Materialize(seed);
            var raw = Sprinkle(salt, material);
            var food = Cook(raw.ToArray());

            var spell = new Spell(food);
            return spell;
        }

        /// <summary>
        /// バイト配列に対して独自の変換処理を行い増長します。
        /// </summary>
        private static byte[] Materialize(IEnumerable<byte> seed)
        {
            byte[] result;
            using (var sha = SHA512.Create())
            {
                // Hashを使うのは再現性が必要なため。
                result = seed.SelectMany(x =>
                {
                    // byte毎にbyteとそのHashを結合する。
                    // 元のbyteは残して、複雑性が失われないようにする。
                    var _ = new[] { x };
                    return _.Concat(sha.ComputeHash(_));
                })
                .ToArray();
            }

            return result;
        }

        /// <summary>
        /// バイト配列にバイト配列を振りかけるような変換処理を行います。
        /// </summary>
        private static IEnumerable<byte> Sprinkle(IEnumerable<byte> seasoning, IEnumerable<byte> material)
        {
            var result = material.SelectMany(x => new[] { x }.Concat(seasoning));
            return result;
        }

        /// <summary>
        /// バイト配列に混ぜ合わせるような変換処理を行います。
        /// 結果を固定長（128bytes）にして扱いやすいサイズにします。
        /// </summary>
        private static byte[] Cook(byte[] raw)
        {
            using (var sha = SHA512.Create())
            {
                var hash = sha.ComputeHash(raw);
                var reversedHash = sha.ComputeHash(hash.Reverse().ToArray());

                // かき混ぜる: 元のbyte配列とその並び順を逆転させたバイト配列を1つずつ取り出して結合する。
                var result = hash
                    .SelectMany((x, index) => new[] { x, reversedHash[index] })
                    .ToArray();

                return result;
            }
        }
    }
}
