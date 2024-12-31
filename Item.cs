using System;
using System.Collections.Generic;

namespace TodoApi;

public partial class Item
{
    public int Iditem { get; set; }

    public string? Taskname { get; set; }

    public bool? Iscomplate { get; set; }
}
