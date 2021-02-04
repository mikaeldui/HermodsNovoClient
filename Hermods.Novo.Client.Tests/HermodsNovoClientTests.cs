using Liber.Onlinebok;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hermods.Novo.Client.Tests
{
    [TestClass]
    public class HermodsNovoClientTests
    {
        private static readonly string USERNAME = Environment.GetEnvironmentVariable("HERMODS_NOVO_USERNAME");
        private static readonly string PASSWORD = Environment.GetEnvironmentVariable("HERMODS_NOVO_PASSWORD");

        [TestMethod]
        public async Task GetEbooksAsync()
        {
            LiberOnlinebokClient liberClient;
            using (var hermodsClient = new HermodsNovoClient())
            {
                var loginSuccess = await hermodsClient.TryAuthenticateAsync(USERNAME, PASSWORD);

                Assert.IsTrue(loginSuccess, "Authentication failed.");

                var ebooks = await hermodsClient.GetEbooksAsync();

                Assert.IsTrue(ebooks.Length > 0, "No e-books retrieved.");

                liberClient = await hermodsClient.GetLiberOnlinebokClientAsync(ebooks[0]);
            }

            using (liberClient)
            {
                var document = await liberClient.GetDocumentAsync();

                Assert.IsNotNull(document, "Document is null");

                Assert.IsTrue(document.Content.ContentItems.Length > 0, "No content items");

                var assetLocation = await liberClient.GetAssetsLocationAsync();

                Assert.IsNotNull(assetLocation, "Asserts location is null");
            }
        }
    }
}
