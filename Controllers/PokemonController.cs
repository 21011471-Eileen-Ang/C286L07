﻿using Microsoft.AspNetCore.Mvc;

namespace Lesson07.Controllers;

public class PokemonController : Controller
{
    private readonly AppDbContext _dbCtx = null!;
    private readonly List<Pokedex> data = null!;

    public PokemonController(AppDbContext dbContext)
    {
        _dbCtx = dbContext;
        DbSet<Pokedex> dbs = _dbCtx.Pokedex;
        data = dbs.ToList();
    }

    public IActionResult Index()
    {
        return View();
    }

#region Demonstration
    public IActionResult AllRecordsProperties()
    {
        ViewData["Title"] = "All Records with All Properties";
        ViewData["Query"] = "No Query";
        return View("ResultsModel", data);
    }

    public IActionResult SortingLevels()
    {
        var result = data
            .OrderBy(s => s.Type1)
            .ThenBy(s => s.Type2)
            .ThenByDescending(s => s.PokeName)
            .ToList();

        ViewData["Title"] = "Sorting: Type1, Type2, Name(desc)";
        ViewData["Query"] = @"
var result = data
    .OrderBy(s => s.Type1)
    .ThenBy(s => s.Type2)
    .ThenByDescending(s => s.Name)
    .ToList();
";

        return View("ResultsModel", result);
    }

    public IActionResult Projection()
    {
        var model = data
            .Select(
                a => new
                {
                    a.PokeName,
                    a.Attack,
                    a.Defence,
                    a.Stamina
                })
            .ToExpandoList();

        ViewData["Title"] = "Projection: Name, Attack, Defence, Stamina";
        ViewData["Query"] = @"
var model = data
    .Select(
        a => new 
            {
                Name = a.PokeName,
                Attack = a.Attack,
                Defence = a.Defence,
                Stamina = a.Stamina
            })
    .ToExpandoList();
";
        return View("ResultsDynamic", model);
    }

    public IActionResult ProjectionSorting()
    {
        var result = data
            .Select(
                a => new
                    {
                        a.PokeName,
                        a.Attack,
                        a.Defence,
                        a.Stamina
                    })
            .OrderBy(s => s.PokeName)
            .ToExpandoList();

        ViewData["Title"] = "Projection with Sorting by Name";
        ViewData["Query"] = @"
var result = data
    .Select(
        a => new
            {
                a.PokeName,
                a.Attack,
                a.Defence,
                a.Stamina
            })
    .OrderBy(s => s.PokeName)
    .ToExpandoList();
";

        return View("ResultsDynamic", result);
    }

    public IActionResult Filter1()
    {
        var result = data
            .Where(x => x.Defence == x.Attack)
            .OrderByDescending(s => s.Stamina)
            .ToList();

        ViewData["Title"] = "Filter and Sort Descending";
        ViewData["Query"] = @"
var result = data
    .Where(x => x.Defence == x.Attack)
    .OrderByDescending(s => s.Stamina)
    .ToList();
";

        return View("ResultsModel", result);
    }


    public IActionResult Filter2()
    {
        var result = data
            .Where(x => x.Type1 == "Ice" || x.Type2 == "Ice")
            .OrderBy(s => s.PokeName)
            .ToList();
        ViewData["Title"] = "Filter and Sort Ascending";
        ViewData["Query"] = @"
var result = data
    .Where(x => x.Type1 == ""Ice"" || x.Type2 == ""Ice"")
    .OrderBy(s => s.PokeName)
    .ToList();
";

        return View("ResultsModel", result);
    }

    public IActionResult FilterProjectionSort()
    {
        var result =
           data.Where(x => x.Defence == x.Attack)
               .Select(
                   a => new
                   {
                       a.PokeName,
                       AttackDefence = a.Attack,
                       a.Type1,
                       a.Type2
                   }
                )
               .OrderBy(s => s.PokeName)
               .ToExpandoList();
        ViewData["Title"] = "Filter, Projection and Sort";
        ViewData["Query"] = @"
var result =
   data.Where(x => x.Defence == x.Attack)
       .Select(
           a => new
              {
                  Name = a.Name,
                  AttackDefence = a.Attack,
                  Type1 = a.Type1,
                  Type2 = a.Type2
              }
        )
       .OrderBy(s => s.Name)
       .ToExpandoList();
";
        return View("ResultsDynamic", result);
    }

    public IActionResult Grouping()
    {
        var result =
           data.GroupBy(g => g.Type1)
               .OrderByDescending(s => s.Key)
               .Select(
                   p => new
                   {
                       PokeType = p.Key,
                       PokemonCount = p.Count(),
                       MaxAttack = p.Max(a => a.Attack),
                       MinAttack = p.Min(a => a.Attack)
                   }
                )
               .ToExpandoList();
        ViewData["Title"] = "Grouping, Projection, Aggregates and Sort";
        ViewData["Query"] = @"
var result =
   data.GroupBy(g => g.Type1)
       .OrderByDescending(s => s.Key)
       .Select(
           p => new
              {
                  PokeType = p.Key,
                  PokemonCount = p.Count(),
                  MaxAttack = p.Max(a => a.Attack),
                  MinAttack = p.Min(a => a.Attack)
              }
        )
       .ToExpandoList();
";
        return View("ResultsDynamic", result);
    }

    public IActionResult GroupingFilter()
    {
        var result = data
            .GroupBy(g => new { g.Type1, g.Type2 })
            .OrderByDescending(s => s.Key.Type1)
            .ThenByDescending(s => s.Key.Type2)
            .Select(
                p => new
                {
                    Name = $"{p.Key.Type1}-{p.Key.Type2}",
                    PokemonCount = p.Count(),
                    MaxAttack = p.Max(a => a.Attack),
                    MinAttack = p.Min(a => a.Attack)
                }
             )
            .Where(m => m.PokemonCount > 7)
            .ToExpandoList();

        ViewData["Title"] = "Multi-columns Grouping, Projection, Aggregates, Filter and Sort";
        ViewData["Query"] = @"
var result = data
    .GroupBy(g => new { g.Type1, g.Type2 })
    .OrderByDescending(s => s.Key.Type1)
    .ThenByDescending(s => s.Key.Type2)
    .Select(
        p => new
        {
            Name = $""{ p.Key.Type1}-{ p.Key.Type2}""
            PokemonCount = p.Count(),
            MaxAttack = p.Max(a => a.Attack),
            MinAttack = p.Min(a => a.Attack)
        }
        )
    .Where(m => m.PokemonCount > 7)
    .ToExpandoList();
";
        return View("ResultsDynamic", result);
    }

    public IActionResult Union()
    {
        var seq1 = data
            .Where(p => p.Attack > p.Defence)
            .Select(p => new
                {
                    p.PokeName,
                    Role = "Attacker"
                })
            .ToList();

        var seq2 = data
            .Where(p => p.Defence > p.Attack)
            .Select(p => new
                {
                    p.PokeName,
                    Role = "Defender"
                })
            .ToList();

        var result = seq1
            .Union(seq2)
            .Select(p => new
                {
                    p.PokeName,
                    p.Role
                })
            .ToExpandoList();

        ViewData["Title"] = "Union";
        ViewData["Query"] = @"
var seq1 = data
    .Where(p => p.Attack > p.Defence)
    .Select(p => new
        {
            p.PokeName,
            Role = ""Attacker""
        })
    .ToList();

var seq2 = data
    .Where(p => p.Defence > p.Attack)
    .Select(p => new
        {
            p.PokeName,
            Role = ""Defender""
        })
    .ToList();

var result = seq1
    .Union(seq2)
    .Select(p => new
        {
            p.PokeName,
            p.Role
        })
    .ToExpandoList();
";
        return View("ResultsDynamic", result);
    }

    public IActionResult Join()
    {
        var rtable = data
            .GroupBy(p => p.Type1)
            .Select(g => new
                {
                    PokeType = g.Key,
                    AverageAttack = Math.Round(g.Average(p => p.Attack))
                })
            .ToList();

        var result = data
            .Join(rtable, 
                p => p.Type1, 
                r => r.PokeType,
                (p, r) => new
                {
                    p.PokeName,
                    p.Type1,
                    p.Attack,
                    AverageAttackByType1 = r.AverageAttack,
                    Difference = p.Attack - r.AverageAttack
                })
            .ToExpandoList();

        ViewData["Title"] = "Join";
        ViewData["Query"] = @"
var rtable = data
    .GroupBy(p => p.Type1)
    .Select(g => new
        {
            PokeType = g.Key,
            AverageAttack = Math.Round(g.Average(p => p.Attack))
        })
    .ToList();

var result = data
    .Join(rtable, 
        p => p.Type1, 
        r => r.PokeType,
        (p, r) => new
        {
            p.PokeName,
            p.Type1,
            p.Attack,
            AverageAttackByType1 = r.AverageAttack,
            Difference = p.Attack - r.AverageAttack
        })
    .ToExpandoList();
";
        return View("ResultsDynamic", result);
    }
#endregion

#region Exercises

    // TODO L07 TASK 2A: LINQ Ex01
    // For only Pokemon with BOTH Type1 and Type2, sorted Type1, Type2, Name(Desc) 
    public IActionResult Ex01()
    {
        // Assuming your dataset is named pokemonList (replace this with your actual dataset)
        List<Pokedex> pokemonList = _dbCtx.Pokedex.ToList();

        var result = pokemonList
            .Where(p => !string.IsNullOrEmpty(p.Type1) && !string.IsNullOrEmpty(p.Type2))
            .OrderBy(p => p.Type1)
            .ThenBy(p => p.Type2)
            .ThenByDescending(p => p.PokeName)
            .ToList();

        ViewData["Title"] = "Ex01: Pokemon with BOTH Type1 and Type2, sorted Type1, Type2, Name(Desc)";
        ViewData["Query"] = "To be released"; 

        return View("ResultsModel", result);
    }

    // TODO L07 TASK 2B: LINQ Ex02
    // Pokemon with Stamina in between Attack and Defence OR
    // in between Defence and Attack, sorted by Id  
    public IActionResult Ex02()
    {
        List<Pokedex> pokemonList = _dbCtx.Pokedex.ToList(); 

        var result = pokemonList
            .Where(p =>
                (p.Stamina > p.Attack && p.Stamina < p.Defence) ||
                (p.Stamina > p.Defence && p.Stamina < p.Attack)
            )
            .OrderBy(p => p.Id)
            .ToList();

        ViewData["Title"] = "Ex02: Pokemon with Stamina in between Attack and Defence OR in between Defence and Attack, sorted by Id";
        ViewData["Query"] = "To be released"; // Update this with the actual query details

        return View("ResultsModel", result);
    }

    // TODO L07 TASK 2C: LINQ Ex03
    // List of Electric Pokemon with Name, 'Electric', TotalStat sorted by TotalStat (desc)   
    public IActionResult Ex03()
    {
        List<Pokedex> pokemonList = _dbCtx.Pokedex.ToList();

        var result = pokemonList
            .Where(p => p.Type1 == "Electric" || p.Type2 == "Electric")
           
        .Select(p => new
        {
            Name = p.PokeName,
            Type = "Electric",
            TotalStat = p.Attack + p.Defence + p.Stamina
        })
        .OrderByDescending(s => s.TotalStat)
        .ToExpandoList();

        ViewData["Title"] = "Ex03: List of Electric Pokemon with Name, 'Electric', TotalStat sorted by TotalStat (desc)";
        ViewData["Query"] = "To be released"; // Update this with the actual query details

        return View("ResultsDynamic", result);
    }

    // TODO L07 TASK 2D: LINQ Ex04
    // For only Pokemon with Type2, group by Type2, display Type2, Count and Avg Stamina    
    public IActionResult Ex04()
    {
        List<Pokedex> pokemonList = _dbCtx.Pokedex.ToList();

        var result = pokemonList
        .Where(p => p.Type2 != "")
        .GroupBy(p => p.Type2)
        .OrderBy(s => s.Key)
        .Select(g => new
        {
            Type2 = g.Key,
            Count = g.Count(),
            AvgStamina = (g.Average(p => p.Stamina)).ToString("0.00")
        })
        .ToExpandoList();



        ViewData["Title"] = "Ex04: Group Pokemon with Type2 by Type2, display Type2, Count and AvgStamina ";
        ViewData["Query"] = @"To be released";
        return View("ResultsDynamic", result);
    }

    // TODO L07 TASK 2E: LINQ Ex05
    // Display strong pokes followed by weak pokes
    public IActionResult Ex05()
    {
        List<Pokedex> pokemonList = _dbCtx.Pokedex.ToList();

        var strongPokes = data
            .Where(p => p.Stamina > 150 && p.Defence > 150 && p.Attack > 150)
            .ToList();

        var weakPokes = data
            .Where(p => p.Stamina < 100 && p.Defence < 100 && p.Attack < 100)
            .ToList();

        var result = strongPokes
            .Union(weakPokes) .ToList();

        ViewData["Title"] = "EX05: Display strong pokes followed by weak pokes";
        ViewData["Query"] = "To be released"; // Update this with the actual query details

        return View("ResultsModel", result);
    }

    // TODO L07 TASK 2G: LINQ Ex06
    // Display number of strong pokemons grouped and ordered by Type1 and Position
    public IActionResult Ex06()
    {
        List<Pokedex> pokemonList = _dbCtx.Pokedex.ToList();


        var strongPokes = data
            .Where(p => p.Stamina > 150 && p.Defence > 150 && p.Attack > 150)
            .Select(p => new
            {
                p.PokeName,
                Position = p.Attack > 180 ? "Front Line" : "Rear Defense",
                p.Type1
            })
            .ToList();

        List<dynamic> result = strongPokes
            .GroupBy(sp => new { sp.Type1, sp.Position })
            .OrderBy(g => g.Key.Type1)
            .ThenBy(g => g.Key.Position)
            .Select(g => new
            {
                g.Key.Type1,
                g.Key.Position,
                Strength = g.Count()
            })
            .ToExpandoList();

        ViewData["Title"] = "EX06: Display number of strong pokemons grouped and ordered by Type1 and Position";
        ViewData["Query"] = "To be released"; // Update this with the actual query details

        return View("ResultsDynamic", result);
    }

    // TODO L07 TASK 2F: LINQ Ex07
    // Display Type2 above average defense pokemons with corresponding actions
    // (above average: "Go Fight!" else "Stay Back")
    public IActionResult Ex07()
    {
        List<Pokedex> pokemonList = _dbCtx.Pokedex.ToList();
        var rtable = data
            .Where(p => p.Type2 != "")
            .GroupBy(p => p.Type2)
            .Select(g => new
            {
                PokeType = g.Key,
                AverageDefence = Math.Round(g.Average(p => p.Defence))
            })
            .ToList();

        List<dynamic>result = data
            .Join(rtable,
                p => p.Type2,
                r => r.PokeType,
               
                (p,r) => new
                {
                    p.PokeName,
                    p.Type2,
                    AverageDefenceByType2 = r.AverageDefence,
                    Action = (p.Defence - r.AverageDefence) > 0 ? "Go Fight!" : "Stay Back"
                })
            .ToExpandoList();

        ViewData["Title"] = "EX07: Display Type2 above average defense pokemons with corresponding actions (above average: Go Fight! else Stay Back)";
        ViewData["Query"] = "To be released"; // Update this with the actual query details

        return View("ResultsDynamic", result);
    }

#endregion

}
