using System;
using System.Collections.Generic;
using System.Text;
using WebApplication1.Controllers;
using Xunit;
using System.Web.Mvc;

namespace WebApplication1.Tests
{

    public class HomeControllerTests
    {
        [Fact]
        public void HomeViewResultNotNull()
        {
            HomeController controller = new HomeController();

            ActionResult result = controller.Index() as ActionResult;

            Assert.NotNull(result);
        }
        [Fact]
        public void AccountsViewResultNotNull()
        {
            AccountsController index = new AccountsController();

            Assert.NotNull(index.Index());
        }
        [Fact]
        public void CategoriesViewResultNotNull()
        {
            
        }
        [Fact]
        public void DebtsViewResultNotNull()
        {
            
        }
        [Fact]
        public void OperationsViewResultNotNull()
        {
           
        }
        [Fact]
        public void ReportsViewResultNotNull()
        {
            //ReportsController controller = new ReportsController();

            //Assert.NotNull(controller.Index());
        }
        [Fact]
        public void UsersViewResultNotNull()
        {
            //UsersController controller = new UsersController();

            //Assert.NotNull(controller.Index());
        }

    }
}
