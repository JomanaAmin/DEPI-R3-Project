using Bookify.BusinessLayer.DTOs.BaseUserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.Contracts
{
    public interface IJwtService
    {
        Task<LoginResponseDTO> Authenticate(LoginRequestDTO loginRequestDTO);

    }
}
