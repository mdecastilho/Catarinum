using System;
using Catarinum.Coap;
using Catarinum.Util;
using Moq;

namespace Catarinum.Tests.Coap {
    public class Examples {
        public static Mock<IResource> ResourceMock = new Mock<IResource>();

        public static Request Basic_get_request_causing_a_piggy_backed_response(int id = 0x7d34) {
            var request = new Request(id, CodeRegistry.Get, true) { Uri = new Uri("coap://server/temperature") };
            ResourceMock.Setup(r => r.CanGet(It.IsAny<byte[]>())).Returns(true);
            ResourceMock.Setup(r => r.Get(It.IsAny<byte[]>())).Returns(ByteConverter.GetBytes("22.3 C"));
            return request;
        }

        public static Request Basic_get_request_causing_a_piggy_backed_response_with_token() {
            var request = Basic_get_request_causing_a_piggy_backed_response(0x7d35);
            request.Token = ByteConverter.GetBytes(0x20);
            return request;
        }

        public static Request Basic_get_request_causing_a_separate_response() {
            var request = new Request(0x7d38, CodeRegistry.Get, true) {
                Uri = new Uri("coap://server/temperature"),
                Token = ByteConverter.GetBytes(0x53)
            };
            ResourceMock.Setup(r => r.Get(It.IsAny<byte[]>())).Returns(ByteConverter.GetBytes("22.3 C"));
            return request;
        }

        public static Request Basic_get_request_where_the_request_and_response_are_non_confirmable() {
            var request = new Request(0x7d40, CodeRegistry.Get, false) {
                Uri = new Uri("coap://server/temperature"),
                Token = ByteConverter.GetBytes(0x75)
            };
            ResourceMock.Setup(r => r.Get(It.IsAny<byte[]>())).Returns(ByteConverter.GetBytes("22.3 C"));
            return request;
        }
    }
}