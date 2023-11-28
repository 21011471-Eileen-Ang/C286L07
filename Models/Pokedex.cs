using System;
using System.Collections.Generic;

namespace Lesson07.Models;

public partial class Pokedex
{
    public int Id { get; set; }

    public string PokeName { get; set; } = null!;

    public string Type1 { get; set; } = null!;

    public string Type2 { get; set; } = null!;

    public int Attack { get; set; }

    public int Defence { get; set; }

    public int Stamina { get; set; }
}
