using System;
using Catarinum.Coap;
using Catarinum.Coap.Util;
using Moq;

namespace Catarinum.Tests.Coap {
    public class Examples {
        public static Mock<IResource> ResourceMock = new Mock<IResource>();

        // example 1 (figure 11)
        public static Request Basic_get_request_causing_a_piggy_backed_response(int id = 0x7d34) {
            var request = new Request(CodeRegistry.Get, true) { Id = id, Uri = new Uri("coap://server/temperature") };
            ResourceMock.Setup(r => r.CanGet(It.IsAny<Uri>())).Returns(true);
            ResourceMock.Setup(r => r.Get(It.IsAny<Uri>())).Returns(ByteConverter.GetBytes("22.3 C"));
            return request;
        }

        // example 2 (figure 12)
        public static Request Basic_get_request_causing_a_piggy_backed_response_with_token() {
            var request = Basic_get_request_causing_a_piggy_backed_response(0x7d35);
            request.Token = ByteConverter.GetBytes(0x20);
            return request;
        }

        // example 5 (figure 15)
        public static Request Basic_get_request_causing_a_separate_response() {
            var request = new Request(CodeRegistry.Get, true) { Id = 0x7d38, Uri = new Uri("coap://server/temperature"), Token = ByteConverter.GetBytes(0x53) };
            ResourceMock.Setup(r => r.Get(It.IsAny<Uri>())).Returns(ByteConverter.GetBytes("22.3 C"));
            return request;
        }

        // example 7 (figure 17)
        public static Request Basic_get_request_where_the_request_and_response_are_non_confirmable() {
            var request = new Request(CodeRegistry.Get, false) { Id = 0x7d40, Uri = new Uri("coap://server/temperature"), Token = ByteConverter.GetBytes(0x75) };
            ResourceMock.Setup(r => r.Get(It.IsAny<Uri>())).Returns(ByteConverter.GetBytes("22.3 C"));
            return request;
        }
    }
}