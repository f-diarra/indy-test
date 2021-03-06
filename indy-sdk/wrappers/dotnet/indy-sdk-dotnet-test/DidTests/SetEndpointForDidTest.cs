using Hyperledger.Indy.DidApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Hyperledger.Indy.Test.DidTests
{
    [TestClass]
    public class SetEndpointForDidTest : IndyIntegrationTestWithPoolAndSingleWallet
    {
        [TestMethod]
        public async Task TestSetEndpointForDidWorks()
        {
            await Did.SetEndpointForDidAsync(wallet, DID, ENDPOINT, VERKEY);
        }

        [TestMethod]
        public async Task TestSetEndpointForDidWorksForReplace()
        {
            await Did.SetEndpointForDidAsync(wallet, DID, ENDPOINT, VERKEY);
            var receivedEndpoint = await Did.GetEndpointForDidAsync(wallet, pool, DID);
            Assert.AreEqual(ENDPOINT, receivedEndpoint.Address);
            Assert.AreEqual(VERKEY, receivedEndpoint.TransportKey);

            var newEndpoint = "10.10.10.1:9710";
            await Did.SetEndpointForDidAsync(wallet, DID, newEndpoint, VERKEY_MY2);
            var updatedEndpoint = await Did.GetEndpointForDidAsync(wallet, pool, DID);

            Assert.AreEqual(newEndpoint, updatedEndpoint.Address);
            Assert.AreEqual(VERKEY_MY2, updatedEndpoint.TransportKey);
        }

        [TestMethod]
        public async Task TestSetEndpointForDidWorksForInvalidDid()
        {
            var ex = await Assert.ThrowsExceptionAsync<InvalidStructureException>(() =>
               Did.SetEndpointForDidAsync(wallet, "invalid_base58string", ENDPOINT, VERKEY)
           );
        }

        [TestMethod]
        public async Task TestSetEndpointForDidWorksForInvalidTransportKey()
        {
            var ex = await Assert.ThrowsExceptionAsync<InvalidStructureException>(() =>
               Did.SetEndpointForDidAsync(wallet, DID, ENDPOINT, INVALID_VERKEY)
           );
        }
    }
}