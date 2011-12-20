using System;

namespace Catarinum.Coap {
    public class ResponseError : Exception {
        public CodeRegistry Code { get; set; }

        public ResponseError(CodeRegistry code) {
            Code = code;
        }
    }
}