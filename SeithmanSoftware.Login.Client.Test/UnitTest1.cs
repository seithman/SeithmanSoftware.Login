using NUnit.Framework;
using SeithmanSoftware.Login.Client.Events;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SeithmanSoftware.Login.Client.Test
{
    public class Tests
    {
        static readonly string baseUrl = "http://localhost:5000";
        UserClient client;

        [SetUp]
        public void Setup()
        {
            client = new UserClient(baseUrl);
            client.OnError += RecieveErrorEvent; 
        }

        public void RecieveErrorEvent(object sender, UserClientErrorEventArgs e)
        {
            Debug.WriteLine($"ERROR: {e.Message}");
        }

        [Test]
        public async Task VerifyEachMethodWorks()
        {
            string name = "test";
            string email = "zoom@blah.doit";
            string password = "baba";
            string password2 = "caca";

            int userId = await client.CreateUser(name, email, password);
            Assert.AreNotEqual(userId, -1);
            bool loggedIn = await client.Login(name, password);
            Assert.AreEqual(loggedIn, true);
            await client.LogOut();
            Assert.AreEqual(client.LoggedIn, true);
            loggedIn = await client.Login(email, password);
            Assert.AreEqual(loggedIn, true);
            bool refreshed = await client.RefreshToken();
            Assert.AreEqual(refreshed, true);
            int id = await client.GetUser(name);
            Console.Write($"id = {id}");
            Assert.AreNotEqual(id, -1);
            int id2 = await client.GetUser(email);
            Assert.AreEqual(id, id2);
            int id3 = await client.GetUser(id);
            Assert.AreEqual(id, id3);
            bool changed = await client.ChangePassword(password2);
            Assert.AreEqual(changed, true);
            await client.LogOut();
            await client.Login(name, password2);
            Assert.AreEqual(client.LoggedIn, true);
            var tokenInfo = await client.GetTokenInfo(client.Token);
            Assert.NotNull(tokenInfo);
            await client.DeleteUser();
            Assert.AreEqual(client.LoggedIn, false);

        }
    }
}