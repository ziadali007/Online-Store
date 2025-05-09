using Shared;
using Shared.OrdersDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IAuthService
    {
        Task<UserResultDto> LoginAsync(LoginDto loginDto);

        Task<UserResultDto> RegisterAsync(RegisterDto registerDto);

        Task<bool> CheckEmailExistsAsync(string email);

        Task<UserResultDto> GetCurrentUserAsync(string email);

        Task<AddressDto> GetCurrentUserAddressAsync(string email);

        Task<AddressDto> UpdateCurrentUserAddressAsync(AddressDto addressDto,string email);
    }
}
