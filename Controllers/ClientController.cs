using Microsoft.AspNetCore.Mvc;

// TODO L07 TASK 3A: Open Models/Client.cs and annotate the properties

namespace Lesson07.Controllers;

public class ClientController : Controller
{
    private readonly AppDbContext _dbCtx = null!;

    public ClientController(AppDbContext dbContext)
    {
        _dbCtx = dbContext;
    }

    public IActionResult Welcome()
    {
        return View();
    }

    public IActionResult Main()
    {
        // TODO L07 TASK 3B: Prepare the Model for the View using .Include 
       

        DbSet<Client> dbs = _dbCtx.Client;
        var model = dbs
            .Include(c => c.Package)
            .ToList();


        return View("ClientMain", model);
    }

    public IActionResult CreateClient()
    {
        ViewData["packages"] = new SelectList(_dbCtx.Package, "Id", "PkgName");

        return View("ClientCreate");
    }

    // TODO L07 TASK 3F: Implement the CreateClient HttpPost action
    [HttpPost]
    public IActionResult CreateClient(Client client)
    {
        if (ModelState.IsValid)
        {
            DbSet<Client> dbs = _dbCtx.Client;
            dbs.Add(client);
            if (_dbCtx.SaveChanges() == 1)
                TempData["Msg"] = "New Client Added";
            else
                TempData["Msg"] = "Failed to Add New Client!";
        }
        else
        {
            TempData["Msg"] = "Invalid Information Entered";

        }

        return RedirectToAction("Main");
    }



    public IActionResult UpdateClient(int id)
    {
        DbSet<Client> dbs = _dbCtx.Client;
        Client? tClient = dbs
            .Where(c => c.Id == id)
            .FirstOrDefault();

        if (tClient != null)
        {
            ViewData["packages"] = new SelectList(_dbCtx.Package, "Id", "PkgName");
            return View("ClientUpdate", tClient);
        }
        else
        {
            TempData["Msg"] = "Client not found!";
            return RedirectToAction("Main");
        }
    }

    // TODO L07 TASK 3G: Implement the UpdateClient HttpPost action
    [HttpPost]
    public IActionResult UpdateClient(Client newClient)
    {
        if (ModelState.IsValid) 
        {
            Client? orgClient = _dbCtx.Client
                .Where(mo => mo.Id == newClient.Id)
                .FirstOrDefault();

            if (orgClient != null)
            {
                orgClient.CustName = newClient.CustName;
                orgClient.PackageId = newClient.PackageId;
                orgClient.PaymentMode = newClient.PaymentMode;

                if (_dbCtx.SaveChanges() == 1)
                    TempData["Msg"] = "Client Updated";
                else
                    TempData["Msg"] = "Failed to Update Client";

            }
            else 
            {
                TempData["Msg"] = "Client not found";
                return RedirectToAction("Main");
            }
        }
        else
        {
            TempData["Msg"] = "Invalid Information entered";
        }
       
        return RedirectToAction("Main");
    }

    // TODO L07 TASK 3H: Implement the DeleteClient HttpPost action
    public IActionResult DeleteClient(int id)
    {

        DbSet<Client> dbs = _dbCtx.Client;
        Client? tClient = dbs
            .Where(t=> t.Id ==id)
            .FirstOrDefault();

        if (tClient != null) 
        {
            dbs.Remove(tClient);
            if (_dbCtx.SaveChanges() == 1)
                TempData["Msg"] = "Client deleted";
            else
                TempData["Msg"] = "Failed to delete client";
        }
        else 
        {
            TempData["Msg"] = "Client not found!";
        }

        return RedirectToAction("Main");
    }

    public IActionResult Report()
    {
        DbSet<Package> dbs = _dbCtx.Package;
        var model = dbs
            .Include(p => p.Client)
            .ToList();
        return new ViewAsPdf("ClientReport", model)
        {
            PageSize = Rotativa.AspNetCore.Options.Size.B5,
            PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
        };

    }
}
//21011471 Eileen Ang
