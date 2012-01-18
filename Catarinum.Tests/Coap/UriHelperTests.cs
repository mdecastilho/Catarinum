using System;
using System.Linq;
using Catarinum.Coap;
using Catarinum.Coap.Util;
using NUnit.Framework;

namespace Catarinum.Tests.Coap {
    [TestFixture]
    public class UriHelperTests {
        [Test]
        public void Should_get_uri() {
            var request = new Request(CodeRegistry.Get, true) { RemoteAddress = "server", Port = 8080 };
            request.AddOption(new Option(OptionNumber.UriPath, ByteConverter.GetBytes("temperature")));
            Assert.AreEqual("coap://server:8080/temperature", request.Uri.ToString());
        }

        [Test]
        public void Should_set_uri() {
            var request = new Request(CodeRegistry.Get, true) { Uri = new Uri("coap://[2001:db8::2:1]") };
            Assert.AreEqual("[2001:0DB8:0000:0000:0000:0000:0002:0001]", request.RemoteAddress);
            Assert.AreEqual(5683, request.Port);
            Assert.AreEqual(0, GetCount(request, OptionNumber.UriPath));
            Assert.AreEqual(0, GetCount(request, OptionNumber.UriQuery));
        }

        [Test]
        public void Should_set_uri_with_path() {
            var request = new Request(CodeRegistry.Get, true) { Uri = new Uri("coap://example.net/.well-known/core") };
            Assert.AreEqual(2, GetCount(request, OptionNumber.UriPath));
        }

        [Test]
        public void Should_set_uri_with_special_characters() {
            var request = new Request(CodeRegistry.Get, true) { Uri = new Uri("coap://xn--18j4d.example/%E3%81%93%E3%82%93%E3%81%AB%E3%81%A1%E3%81%AF") };
            var uriPath = ByteConverter.GetString(request.GetFirstOption(OptionNumber.UriPath).Value);
            Assert.AreEqual("%E3%81%93%E3%82%93%E3%81%AB%E3%81%A1%E3%81%AF", uriPath);
        }

        [Test]
        public void Should_set_uri_with_query() {
            var request = new Request(CodeRegistry.Get, true) { Uri = new Uri("coap://198.51.100.1:61616//%2F//?%2F%2F&?%26") };
            Assert.AreEqual("198.51.100.1", request.RemoteAddress);
            Assert.AreEqual(61616, request.Port);
            Assert.AreEqual(4, GetCount(request, OptionNumber.UriPath));
            Assert.AreEqual(2, GetCount(request, OptionNumber.UriQuery));
        }

        private static int GetCount(Request request, OptionNumber number) {
            return request.Options.Where(o => o.Number == number).Count();
        }
    }
}