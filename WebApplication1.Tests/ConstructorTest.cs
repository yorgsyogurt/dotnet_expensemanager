using System;
using WebApplication1.Models;
using Xunit;

namespace WebApplication1.Tests
{
    public class ConstructorTest
    {

        [Fact]
        public void AccountModelTest()
        {
            Account acc = new Account(1, "2", "3", "4", 5, 6);
            Assert.Equal(1, acc.Id);
            Assert.Equal("2", acc.type);
            Assert.Equal("3", acc.name);
            Assert.Equal("4", acc.currency);
            Assert.Equal(5, acc.balance);
            Assert.Equal(6, acc.UserId);
        }

        [Fact]
        public void CategoryModelTest()
        {

        }
        [Fact]
        public void DebtModelTest()
        {

        }
        [Fact]
        public void OperationModelTest()
        {

        }
        [Fact]
        public void ReportModelTest()
        {

        }
        [Fact]
        public void UserModelTest()
        {

        }

    }
}
