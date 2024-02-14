using SmartGroceries.Models;
using SmartGroceries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartGroceries.Commands
{
    public class TestCommand : CommandBase
    {
        public override void Execute(object parameter)
        {
            MessageBox.Show("Test");
        }
    }
}
