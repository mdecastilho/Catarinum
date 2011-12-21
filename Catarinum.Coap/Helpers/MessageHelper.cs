namespace Catarinum.Coap.Helpers {
    public class MessageHelper {
        private const int Version = 1;
        private const int VersionBits = 2;
        private const int TypeBits = 2;
        private const int OptionCountBits = 4;
        private const int CodeBits = 8;
        private const int IdBits = 16;
        private const int OptionDeltaBits = 4;
        private const int OptionLengthBits = 4;

        public byte[] GetBytes(Message message) {
            var helper = new DatagramHelper();
            helper.AddBits(Version, VersionBits);
            helper.AddBits((int) message.Type, TypeBits);
            helper.AddBits(message.OptionCount, OptionCountBits);
            helper.AddBits((int) message.Code, CodeBits);
            helper.AddBits(message.Id, IdBits);
            helper.AddBytes(GetOptions(message));
            helper.AddBytes(message.Payload);
            return helper.GetBytes();
        }

        private static byte[] GetOptions(Message message) {
            var helper = new DatagramHelper();
            var lastOptionNumber = 0;

            foreach (var option in message.Options) {
                var delta = option.Number - lastOptionNumber;
                var lenght = option.Value.Length;
                helper.AddBits(delta, OptionDeltaBits);
                helper.AddBits(lenght, OptionLengthBits);
                helper.AddBytes(option.Value);
                lastOptionNumber = option.Number;
            }

            return helper.GetBytes();
        }
    }
}