using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Smapple.DbContext;
using Smapple.Models;
using Smapple.Services;

namespace Smapple.Controllers;

[Route("api/host")]
public class HostController : Controller
{
    private readonly SmappleDbContext _db;
    private readonly MailService mail;
    
    public HostController(SmappleDbContext db, MailService mail)
    {
        _db = db;
        this.mail = mail;
    }
}