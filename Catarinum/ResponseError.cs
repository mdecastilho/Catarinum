using System;

namespace Catarinum {
    public class ResponseError : Exception {
        public MessageCode Code { get; set; }

        public ResponseError(MessageCode code) {
            Code = code;
        }
    }
}