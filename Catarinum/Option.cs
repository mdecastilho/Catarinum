using System.Collections.Generic;
using System.Linq;

namespace Catarinum {
    public class Option {
        private int _delta;
        private int _lenght;

        public OptionType Type { get; set; }
        public byte[] Value { get; set; }
    }

    public static class OptionExtensions {
        public static bool MatchToken(this IEnumerable<Option> options, byte[] token) {
            return options.FirstOrDefault(o => o.Value.SequenceEqual(token)) != null;
        }
    }
}