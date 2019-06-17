using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spells.Idioms
{
    /// <summary>
    /// Byte phrase is converted to ASCII character string its printable.
    /// </summary>
    public class Asciinize : IIdiom<IEnumerable<byte>, string>
    {
        private static readonly byte[] EnableDefaultSymbols = ConvertBytes("!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~");
        private static readonly byte[] EnableDefaultLetters = ConvertBytes("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");

        public Asciinize(string enableSymbols = null)
        {
            var symbols = enableSymbols == null
                ? EnableDefaultSymbols
                : ConvertBytes(enableSymbols);

            EnableCharacters = new HashSet<byte>(symbols.Concat(EnableDefaultLetters));
        }

        public readonly HashSet<byte> EnableCharacters;

        public string Call(IEnumerable<byte> phrase)
        {
            // 文字数が減らないように反転したものと、4bitずつ結合して1byteにしてそれをASCII文字に変換する。
            var characters = phrase
                .Zip(phrase.Reverse(), (orign, reverse) => new[]
                {
                    MergeHigh(orign, reverse),
                    MergeLow(orign, reverse)
                })
                .SelectMany(x => x)
                .Select((x, index) => TryGetChar(x, out var c) ? c : ToFallbackCharacter(index, x))
                .ToArray();

            return new string(characters);
        }

        public char ToFallbackCharacter(int index, byte value)
        {
            var result = BitConverter.ToString(new[] { value })[0];
            return index / 2 == 0
                ? result
                : Char.ToLower(result);
        }

        public bool TryGetChar(byte value, out char validChar)
        {
            validChar = default;

            if (!IsValid(value))
                return false;

            validChar = Encoding.ASCII.GetString(new[] { value })[0];
            return true;
        }

        public bool IsValid(byte value)
        {
            return EnableCharacters.Contains(value);
        }

        /// <summary>
        /// 上位4bit同士を結合します。
        /// </summary>
        private byte MergeHigh(byte highPart, byte lowPart)
        {
            var high = highPart & 0xF0;
            var low = lowPart >> 4;
            var result = (byte)(high | low);
            return result;
        }

        /// <summary>
        /// 下位4bit同士を結合します。
        /// </summary>
        private byte MergeLow(byte highPart, byte lowPart)
        {
            var high = (highPart & 0x0F) << 4;
            var low = lowPart & 0x0F;
            var result = (byte)(high | low);
            return result;
        }

        private static byte[] ConvertBytes(string characters)
        {
            return Encoding.ASCII.GetBytes(characters);
        }
    }
}
