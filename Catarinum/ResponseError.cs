using System;

namespace Catarinum {
    public class ResponseError : Exception {
        public ResponseCode Code { get; set; }

        public ResponseError(ResponseCode code) {
            Code = code;
        }
    }
}