using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisBall.Entites;
using TennisBall.Logic;

namespace TennisBall.Tests
{
    [TestFixture]
    public class ChangingStatesTests
    {
        string name1;
        string name2;
        PlayerNumber server;

        [OneTimeSetUp]
        public void Init()
        {
            name1 = "p1";
            name2 = "p2";
            server = PlayerNumber.One;
        }

        [Test]
        public void StatesTest()
        {
            Match match = new Match(name1,name2,server);
            match.AddPoint(PlayerNumber.One);
            match.AddPoint(PlayerNumber.One);

            match.GoBack();
            Assert.AreEqual(match.PlayerOne.GamePoints, 1);

            match.GoBack();
            Assert.AreEqual(match.PlayerOne.GamePoints, 0);

            match.GoForward();
            match.GoForward();
            Assert.AreEqual(match.PlayerOne.GamePoints, 2);
        }

        [Test]
        public void BackTest()
        {
            Match match = new Match(name1, name2,server);
            Assert.IsFalse(match.CanGoBack);
            Assert.IsFalse(match.CanGoForward);

            match.AddPoint(PlayerNumber.One);
            Assert.IsTrue(match.CanGoBack);

            match.GoBack();
            Assert.IsTrue(match.CanGoForward);
            Assert.IsFalse(match.CanGoBack);

            match.GoForward();
            Assert.AreEqual(match.PlayerOne.GamePoints, 1);

        }

        [Test]
        public void StatesCountTest()
        {
            Match match = new Match(name1, name2, server);
            match.AddPoint(PlayerNumber.One);
            match.AddPoint(PlayerNumber.One);
            match.AddPoint(PlayerNumber.One);
            match.AddPoint(PlayerNumber.One);
            match.AddPoint(PlayerNumber.One);
            match.AddPoint(PlayerNumber.One);
            match.AddPoint(PlayerNumber.One);
            match.AddPoint(PlayerNumber.One);
            Assert.AreEqual(match.PlayerOne.Games, 2);
            match.GoBack();
            match.GoBack();
            match.GoBack();
            match.GoBack();
            match.GoBack();
            Assert.AreEqual(match.PlayerOne.GamePoints, 3);
        }

        
    }
}
