using EntityFrameworkMock;
using Gourmet.Core.Domain.Entities;
using Gourmet.Core.ServiceContracts;
using Gourmet.Core.DataBase.GourmetDbcontext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gourmet.Test
{
    public class SignUpTest
    {
        private readonly IUsersService _personService;

        //constructor
        public SignUpTest()
        {


        }
        #region
        [Fact]
        public void Null_request()
        {

        }
        #endregion
    }
}
