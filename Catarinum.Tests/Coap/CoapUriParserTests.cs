using System;
using System.Linq;
using Catarinum.Coap;
using NUnit.Framework;

namespace Catarinum.Tests.Coap {
    [TestFixture]
    public class CoapUriParserTests {
        private CoapUriParser _parser;

        [SetUp]
        public void SetUp() {
            _parser = new CoapUriParser();
        }

        [Test]
        public void Should_parse_uri() {
            var uri = new Uri("coap://[2001:db8::2:1]");
            var destinationAddress = _parser.GetDestinationAddress(uri);
            var destinationPort = _parser.GetDestinationPort(uri);
            var uriPath = _parser.GetUriPath(uri);
            var uriQuery = _parser.GetUriQuery(uri);
            Assert.AreEqual("[2001:0DB8:0000:0000:0000:0000:0002:0001]", destinationAddress);
            Assert.AreEqual(5683, destinationPort);
            Assert.AreEqual(0, uriPath.Count());
            Assert.AreEqual(0, uriQuery.Count());
        }

        [Test]
        public void Should_parse_uri_with_path() {
            var uri = new Uri("coap://[2001:db8::2:1]/.well-known/core");
            var uriPaths = _parser.GetUriPath(uri);
            Assert.AreEqual(2, uriPaths.Count());
        }

        [Test]
        public void Should_parse_uri_with_special_characters() {
            var uri = new Uri("coap://[2001:db8::2:1]/%E3%81%93%E3%82%93%E3%81%AB%E3%81%A1%E3%81%AF");
            var uriPaths = _parser.GetUriPath(uri);
            Assert.AreEqual("%E3%81%93%E3%82%93%E3%81%AB%E3%81%A1%E3%81%AF", Util.GetString(uriPaths.ElementAt(0).Value));
        }

        [Test]
        public void Should_parse_uri_with_query() {
            var uri = new Uri("coap://198.51.100.1:61616//%2F//?%2F%2F&?%26");
            var destinationAddress = _parser.GetDestinationAddress(uri);
            var destinationPort = _parser.GetDestinationPort(uri);
            var uriPath = _parser.GetUriPath(uri);
            var uriQuery = _parser.GetUriQuery(uri);
            Assert.AreEqual("198.51.100.1", destinationAddress);
            Assert.AreEqual(61616, destinationPort);
            Assert.AreEqual(4, uriPath.Count());
            Assert.AreEqual(2, uriQuery.Count());
        }
    }
}