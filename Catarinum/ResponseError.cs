using System;

namespace Catarinum {
    public class ResponseError : Exception {
        public CodeRegistry Code { get; set; }

        public ResponseError(CodeRegistry code) {
            Code = code;
        }
    }
}