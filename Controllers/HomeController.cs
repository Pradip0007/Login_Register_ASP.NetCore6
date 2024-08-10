using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LoginFormASPCore6.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LoginFormASPCore6.Controllers;

public class HomeController : Controller
{
    private readonly Login_dbContext context;

    public HomeController( Login_dbContext context)
    {
        this.context = context;
    }

    public IActionResult DropDown()
    {
        EmployeeModel empModel = new EmployeeModel();
        empModel.Employeelist = new List<SelectListItem>();

        var data = context.UserTables.ToList();

        empModel.Employeelist.Add(new SelectListItem
        {
            Text = "Select name",
            Value = ""
        });
        foreach (var i in data)
        {
            empModel.Employeelist.Add(new SelectListItem
            {
                Text =  i.Name,
                Value =  i.Id.ToString()
            });
        }
        return View(empModel);
    }

    [HttpPost]
    public IActionResult DropDown(EmployeeModel emp)
    {
        var employee = context.UserTables.Where(x => x.Id == emp.Id).FirstOrDefault();
        if(emp != null)
        {
            ViewBag.selectedValue = employee.Name;
        }
        return View();
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login()
    {

        if (HttpContext.Session.GetString("UserSession") != null)
        {
            return RedirectToAction("Dashboard");
        }
        return View();
    }

    [HttpPost]
    public IActionResult Login(UserTable user)
    {
        var myUser = context.UserTables.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();
        if (myUser != null)
        {
            HttpContext.Session.SetString("UserSession", myUser.Email);
            return RedirectToAction("Dashboard");
        }
        else
        {
            ViewBag.Message = "Login Failed.";
        }
        return View();

    }
    public IActionResult Dashboard()
    {

        if(HttpContext.Session.GetString("UserSession") != null)
        {
            ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
        }
        else
        {
            return RedirectToAction("Login");
        }
        return View();
    }

    public IActionResult Logout()
    {
        if (HttpContext.Session.GetString("UserSession") != null)
        {
            HttpContext.Session.Remove("UserSession");
            return RedirectToAction("Login");
        }
        return View();
    }

    public IActionResult Register()
    {
        List<SelectListItem> Gender = new()
        {
            new SelectListItem {Value ="Male",Text = "Male"},
            new SelectListItem {Value ="Female",Text = "Female"}
        };
        ViewBag.Gender = Gender;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserTable user)
    {
        if (ModelState.IsValid)
        {
            await context.UserTables.AddAsync(user);
            await context.SaveChangesAsync();
            TempData["Success"] = "Registered Succesfully...";
            return RedirectToAction("Login");
        }
        return View();
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
