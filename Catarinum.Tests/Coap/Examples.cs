﻿using System;
using Catarinum.Coap;
using Catarinum.Coap.Helpers;
using Moq;

namespace Catarinum.Tests.Coap {
    public class Examples {
        public static Mock<IResource> ResourceMock = new Mock<IResource>();

        public static Request Basic_get_request_causing_a_piggy_backed_response(int id = 0x7d34) {
            var request = new Request(id, CodeRegistry.Get, true);
            request.AddUri(new Uri("coap://server/temperature"));
            ResourceMock.Setup(r => r.CanGet(It.IsAny<byte[]>())).Returns(true);
            ResourceMock.Setup(r => r.Get(It.IsAny<byte[]>())).Returns(Converter.GetBytes("22.3 C"));
            return request;
        }

        public static Request Basic_get_request_causing_a_piggy_backed_response_with_token() {
            var request = Basic_get_request_causing_a_piggy_backed_response(0x7d35);
            request.AddToken(Converter.GetBytes(0x20));
            return request;
        }

        public static Request Basic_get_request_causing_a_separate_response() {
            var request = new Request(0x7d38, CodeRegistry.Get, true);
            request.AddUri(new Uri("coap://server/temperature"));
            request.AddToken(Converter.GetBytes(0x53));
            ResourceMock.Setup(r => r.Get(It.IsAny<byte[]>())).Returns(Converter.GetBytes("22.3 C"));
            return request;
        }

        public static Request Basic_get_request_where_the_request_and_response_are_non_confirmable() {
            var request = new Request(0x7d40, CodeRegistry.Get, false);
            request.AddUri(new Uri("coap://server/temperature"));
            request.AddToken(Converter.GetBytes(0x75));
            ResourceMock.Setup(r => r.Get(It.IsAny<byte[]>())).Returns(Converter.GetBytes("22.3 C"));
            return request;
        }
    }
}