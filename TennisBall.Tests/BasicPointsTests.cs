using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TennisBall.Logic;

namespace TennisBall.Tests
{
    [TestFixture]
    public class BasicPointsTests
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
        public void SimpleWonGameTest()
        {
            Match match = new Match(name1, name2,server);
            match.AddPoint(PlayerNumber.One);
            match.AddPoint(PlayerNumber.One);
            Assert.AreEqual(match.PlayerOne.DisplayPoints, "30");

            match.AddPoint(PlayerNumber.One);
            match.AddPoint(PlayerNumber.One);
            Assert.AreEqual(match.PlayerOne.Games, 1);
        }

        [Test]
        public void WonGameByAdventageTest()
        {
            Match match = CreateDraw();

            Assert.AreEqual(match.PlayerOne.DisplayPoints,"40");
            Assert.AreEqual(match.PlayerTwo.DisplayPoints, "40");

            match.AddPoint(PlayerNumber.One);
            Assert.AreEqual(match.PlayerOne.DisplayPoints, "AD");
            Assert.AreEqual(match.PlayerOne.HasAdventage, true);
            match.AddPoint(PlayerNumber.One);
            Assert.AreEqual(match.PlayerOne.GamePoints, 0);
            Assert.AreEqual(match.PlayerOne.Games, 1);
        }

        [Test]
        public void DrawFewTimesTest()
        {
            Match match = CreateDraw();
            match.AddPoint(PlayerNumber.One);
            Assert.AreEqual(match.PlayerOne.DisplayPoints, "AD");
            match.AddPoint(PlayerNumber.Two);
            Assert.AreNotEqual(match.PlayerOne.DisplayPoints, "AD");
            match.AddPoint(PlayerNumber.Two);
            Assert.AreEqual(match.PlayerTwo.DisplayPoints, "AD");
            match.AddPoint(PlayerNumber.One);
            Assert.AreEqual(match.PlayerOne.DisplayPoints, "40");
            match.AddPoint(PlayerNumber.One);
            match.AddPoint(PlayerNumber.One);
            Assert.AreEqual(match.PlayerOne.GamePoints, 0);
            Assert.AreEqual(match.PlayerOne.Games, 1);
            Assert.AreEqual(match.PlayerOne.TotalPoints, 7);
        }

        public Match CreateDraw()
        {
            Match match = new Match(name1, name2,server);
            match.AddPoint(PlayerNumber.One);
            match.AddPoint(PlayerNumber.One);
            match.AddPoint(PlayerNumber.One);

            match.AddPoint(PlayerNumber.Two);
            match.AddPoint(PlayerNumber.Two);
            match.AddPoint(PlayerNumber.Two);
            return match;
        }

        [Test]
        public void CheckServersTest()
        {
            Match match = new Match(name1, name2, server);
            Assert.IsTrue(match.PlayerOne.IsServer);
            match.AddPoint(PlayerNumber.One);
            match.AddPoint(PlayerNumber.One);
            match.AddPoint(PlayerNumber.One);
            match.AddPoint(PlayerNumber.One);
            Assert.IsTrue(match.PlayerTwo.IsServer);
            Assert.IsFalse(match.PlayerOne.IsServer);
            match.GoBack();
            Assert.IsTrue(match.PlayerOne.IsServer);
            Assert.AreEqual(match.PlayerOne.Games, 0);
        }
    }
}
