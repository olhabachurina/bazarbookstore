using ConsoleApp1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.ViewModels
{
    public struct ItemView : IShow<int>
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
