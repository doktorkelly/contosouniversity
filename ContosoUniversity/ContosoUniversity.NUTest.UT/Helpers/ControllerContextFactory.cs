using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using System.Security.Principal;
using System.Collections.Specialized;

namespace ContosoUniversity.NUTest.UT.Helpers
{
    public class ControllerContextFactory
    {
        public static ControllerContext Create(Controller controller) 
        {
            var userName = "user01";
            var contextMock = new Mock<ControllerContext>();
            contextMock
                .Setup(c => c.Controller)
                .Returns(controller);
            //contextMock
            //    .Setup(c => c.HttpContext.User.Identity.Name)
            //    .Returns(userName);
            //alternative:
            contextMock
                .Setup(c => c.HttpContext.User)
                .Returns(new GenericPrincipal(new GenericIdentity(userName), null));
            return contextMock.Object;
        }
    }
}
