using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sanasoppa.API.Interfaces;

public interface IReCaptchaService
{
    public Task<bool> ValidateReCaptchaAsync(string token);
}