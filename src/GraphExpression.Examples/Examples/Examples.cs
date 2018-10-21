using GraphExpression.Examples.Models;
using SysCommand.ConsoleApp;
using SysCommand.Mapping;
using System;

namespace GraphExpression.Examples
{
    public partial class Examples : Command
    {
        public Examples()
        {
            this.OnlyMethodsWithAttribute = true;
        }
    }
}
