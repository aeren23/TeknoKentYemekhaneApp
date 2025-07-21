using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YemekhaneApp.Application.Services.Token
{
    public interface ITokenService
    {
        string GenerateToken(string username);
    }
}
