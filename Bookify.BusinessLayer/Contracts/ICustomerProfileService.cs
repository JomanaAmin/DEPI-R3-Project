using Bookify.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.Contracts
{
    public interface ICustomerProfileService
    {
        Task<string> CreateCustomerProfileAsync(BaseUser baseUser, string firstName, string lastName);

    }
}
