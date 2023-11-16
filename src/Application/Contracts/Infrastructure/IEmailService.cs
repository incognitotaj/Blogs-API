using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Contracts.Infrastructure;

public interface IEmailService
{
    Task<bool> SendEmailAsync(Email email);
}
