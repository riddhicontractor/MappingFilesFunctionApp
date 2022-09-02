﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MappingFilesFunctionApp
{
    class Demo
    {
     
            public static void RedirectAssembly()
            {
                var list = AppDomain.CurrentDomain.GetAssemblies().OrderByDescending(a => a.FullName).Select(a => a.FullName).ToList();
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            }

            private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
            {
                var requestedAssembly = new AssemblyName(args.Name);
                Assembly assembly = null;
                AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
                try
                {
                    assembly = Assembly.Load(requestedAssembly.Name);
                }
                catch (Exception)
                {
                }
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                return assembly;
            }

        }
    
}
