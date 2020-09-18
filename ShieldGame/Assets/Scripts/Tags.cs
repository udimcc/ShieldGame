using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Tags
{
    private Tags(string value) { Value = value; }
    public string Value { get; set; }

    public static Tags PatrolArea { get { return new Tags("PatrolArea"); }}
}
