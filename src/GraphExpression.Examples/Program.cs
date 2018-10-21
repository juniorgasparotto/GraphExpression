using SysCommand.ConsoleApp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression.Examples
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var first = true;
            Strings.HelpFooterDesc = null;
            App.RunApplication(() =>
            {
                var cmds = new List<Type>() { typeof(Examples) };
                var app = new App(cmds);
                
                if (first)
                {
                    app.Console.Success("****\r\nEnter with the example name, use 'help' to show all. Eg: example1.\r\n****\r\n", forceWrite: true);
                    first = false;
                }
                return app;
            });
        }
    }
}
