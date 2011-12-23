using System;
using System.Linq;
using Catarinum.Coap.Helpers;
using NUnit.Framework;

namespace Catarinum.Tests.Coap.Helpers {
    [TestFixture]
    public class CoapUriParserTests {
        [Test]
        public void Should_parse_uri() {
            var parser = new CoapUriParser(new Uri("coap://[2001:db8::2:1]"));
            var remoteAddress = parser.GetRemoteAddress();
            var port = parser.GetPort();
            var uriPath = parser.GetUriPath();
            var uriQuery = parser.GetUriQuery();
            Assert.AreEqual("[2001:0DB8:0000:0000:0000:0000:0002:0001]", remoteAddress);
            Assert.AreEqual(5683, port);
            Assert.AreEqual(0, uriPath.Count());
            Assert.AreEqual(0, uriQuery.Count());
        }

        [Test]
        public void Should_parse_uri_with_path() {
            var parser = new CoapUriParser(new Uri("coap://example.net/.well-known/core"));
            var uriPaths = parser.GetUriPath();
            Assert.AreEqual(2, uriPaths.Count());
        }

        [Test]
        public void Should_parse_uri_with_special_characters() {
            var parser = new CoapUriParser(new Uri("coap://xn--18j4d.example/%E3%81%93%E3%82%93%E3%81%AB%E3%81%A1%E3%81%AF"));
            var uriPaths = parser.GetUriPath();
            Assert.AreEqual("%E3%81%93%E3%82%93%E3%81%AB%E3%81%A1%E3%81%AF", ByteConverter.GetString(uriPaths.ElementAt(0).Value));
        }

        [Test]
        public void Should_parse_uri_with_query() {
            var parser = new CoapUriParser(new Uri("coap://198.51.100.1:61616//%2F//?%2F%2F&?%26"));
            var remoteAddress = parser.GetRemoteAddress();
            var port = parser.GetPort();
            var uriPath = parser.GetUriPath();
            var uriQuery = parser.GetUriQuery();
            Assert.AreEqual("198.51.100.1", remoteAddress);
            Assert.AreEqual(61616, port);
            Assert.AreEqual(4, uriPath.Count());
            Assert.AreEqual(2, uriQuery.Count());
        }
    }
}