using System;
using System.Collections.Generic;

namespace Lesson07.Models;

public partial class Package
{
    public int Id { get; set; }

    public string PkgName { get; set; } = null!;

    public virtual ICollection<Client> Client { get; set; } = new List<Client>();
}
