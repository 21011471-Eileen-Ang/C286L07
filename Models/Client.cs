using System;
using System.Collections.Generic;

namespace Lesson07.Models;

public partial class Client
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter Name")]
    public string CustName { get; set; } = null!;

    public string PaymentMode { get; set; } = null!;

    [Required(ErrorMessage = "Please enter Package")]
    public int PackageId { get; set; }

    [ValidateNever]
    public virtual Package Package { get; set; } = null!;
}

// 21011471 Eileen Ang
