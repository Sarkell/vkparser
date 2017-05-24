using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace VKParserUI
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() : base()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains("Newtonsoft.Json"))
            {
                return Assembly.Load(VKParserUI.Properties.Resources.Newtonsoft_Json);
            }
            if (args.Name.Contains("xNet"))
            {
                return Assembly.Load(VKParserUI.Properties.Resources.xNet);
            }
            return null;
        }
    }
}