using System;

namespace Catarinum.Coap.Layers {
    public static class BlockHelper {
        public static BlockOption GetBlockOption(this Message message, OptionNumber optionNumber) {
            return message.GetFirstOption(optionNumber) as BlockOption;
        }

        public static bool IsBlockwise(this Message message) {
            var block1 = message.GetBlockOption(OptionNumber.Block1);
            var block2 = message.GetBlockOption(OptionNumber.Block2);
            return block1 != null || block2 != null;
        }

        public static bool IsBlockwiseRequest(this Message message) {
            var block2 = message.GetBlockOption(OptionNumber.Block2);
            return message is Request && block2 != null;
        }

        public static bool IsBlockwiseAcknowledgement(this Message message) {
            var block1 = message.GetBlockOption(OptionNumber.Block1);
            return message is Response && block1 != null;
        }

        public static bool IsBlockwiseResponse(this Message message) {
            var block2 = message.GetBlockOption(OptionNumber.Block2);
            return message is Response && block2 != null;
        }


        public static Message GetBlock(this Message message, BlockInfo blockInfo) {
            return message.GetBlock(blockInfo.Num, blockInfo.Szx);
        }

        public static Message GetBlock(this Message message, int num, int szx) {
            var blockSize = BlockOption.DecodeSzx(szx);
            var payloadOffset = num * blockSize;
            var payloadLeft = message.Payload.Length - payloadOffset;

            if (payloadLeft > 0) {
                var block = message is Request
                    ? (Message) new Request(message.Code, message.Type == MessageType.Confirmable) { Id = message.Id }
                    : new Response(message.Type, message.Code) { Id = message.Id };

                foreach (var option in message.Options) {
                    block.AddOption(option);
                }

                var m = blockSize < payloadLeft ? 1 : 0;

                if (m == 0) {
                    blockSize = payloadLeft;
                }

                var payload = new byte[blockSize];
                Buffer.BlockCopy(message.Payload, payloadOffset, payload, 0, blockSize);
                block.Payload = payload;
                var optionNumber = message is Request ? OptionNumber.Block1 : OptionNumber.Block2;
                block.AddOption(new BlockOption(optionNumber, num, m, szx));
                return block;
            }

            return null;
        }
    }
}