﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using QSoft.Registry;
using QSoft.Registry.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static T GetValue<T>(string name)
        {
            T t = default(T);
            
            return t;
        }
        static void Main(string[] args)
        {
            RegistryKey reg_32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
            RegistryKey reg_64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey uninstall = reg_64.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
            //foreach (var oo in uninstall.GetSubKeyNames())
            //{
            //    RegistryKey subkey = uninstall.OpenSubKey(oo);
            //    object obj = subkey.GetValue("DisplayName");
            //    string displayname = subkey.GetValue("DisplayName") as string;
            //    System.Diagnostics.Trace.WriteLine(displayname);
            //    subkey.Dispose();
            //}

            List<AppData> apps = new List<AppData>();
            apps.Add(new AppData() { Name = "WinFlash" });
            apps.Add(new AppData() { Name = "Dropbox 25 GB" });
            apps.Add(new AppData() { Name = "AnyDes", IsOfficial = true });
            //var apptest = apps.Join(apps, func=> { return true; }, (x, y) => new { x, y });
            //apptest.ToList();
            foreach (var app in apps)
            {
                var reg1 = uninstall.FirstOrDefault(x => x.GetValue<string>("DisplayName") == app.Name);
                if (reg1 != null)
                {
                    //app.Ver = reg1.GetValue<string>("DisplayVersion");
                    //app.Uninstallstring = reg1.GetValue<string>("UninstallString");
                }
            }
            Func<AppData, RegistryKey, AppData> f = ((a, b) =>
            {
                a.Name = b.GetValue<string>("DisplayName");
                a.Uninstallstring = b.GetValue<string>("UninstallString");
                a.Ver = b.GetValue<string>("DisplayVersion");
                return a;
            });
            //var jj = uninstall.Where(x => x.GetValue<string>("DisplayName") != "").Join(apps, x => x.GetValue<string>("DisplayName"), y => y.Name, (x, y) => new { x, y }).Select(x => f(x.y, x.x));
            //var jj = uninstall.Join(apps, x => x.GetValue<string>("DisplayName"), y => y.Name, (x, y) => new { x, y });
            //var jj = uninstall.Join(apps, x => x.GetValue<string>("DisplayName"), app => app.Name, (x, app) => new { x, app })
            //    .Select(x =>
            //    {
            //        x.app.Uninstallstring = x.x.GetValue<string>("UninstallString");
            //        x.app.Ver = x.x.GetValue<string>("DisplayVersion");
            //        return x.app;
            //    });
            var existapps = uninstall.Join(apps, (reg, app) =>
            {
                bool hr = false;
                //object obj = reg.GetValue("DisplayName");
                //string str = obj as string;
                string dispay = reg.GetValue<string>("DisplayName");
                if (app.IsOfficial == true)
                {
                    hr = string.IsNullOrEmpty(dispay) == false && app.Name.Contains(dispay);
                }
                else
                {
                    hr = app.Name == dispay;
                }

                return hr;
            }, (x, app) => new { x, app })
                .Select(x =>
                {
                    x.app.Uninstallstring = x.x.GetValue<string>("UninstallString");
                    x.app.Ver = x.x.GetValue<string>("DisplayVersion");
                    return x.app;
                });
            int runcount = 50;
            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
            for(int i=0; i< runcount; i++)
            {
                foreach (var oo in uninstall.GetSubKeyNames())
                {
                    RegistryKey subkey = uninstall.OpenSubKey(oo);
                    object obj = subkey.GetValue("DisplayName");
                    //object obj1 = subkey.GetValue("UninstallString");
                    //object obj2 = subkey.GetValue("DisplayVersion");
                    //string displayname = subkey.GetValue("DisplayName") as string;
                    //System.Diagnostics.Trace.WriteLine(displayname);
                    subkey.Dispose();
                }


                //existapps.ToList();
            }
            sw.Stop();
            System.Diagnostics.Trace.WriteLine($"{sw.ElapsedMilliseconds/ runcount}");
            //foreach (var oo in existapps)
            //{

            //}

            //var nonexist = apps.Except(existapps);
            var jjj = uninstall.Select(x => new { DisplayName = x.GetValue<string>("DisplayName"), DisplayVersion = x.GetValue<string>("DisplayVersion") });
            foreach (var oo in jjj)
            {
                
            }


            var first = uninstall.FirstOrDefault(x => x.GetValue<string>("DisplayName") == "Intel(R) Processor Graphics" || x.GetValue<string>("DisplayName") == "");
            var last = uninstall.LastOrDefault(x => x.GetValue<string>("DisplayName") == "Intel(R) Processor Graphics");
            var count = uninstall.Count();
            var count_1 = uninstall.Count(x => string.IsNullOrEmpty(x.GetValue<string>("DisplayName")) == false);
            var select = uninstall.Select(x => x.GetValue<string>("DisplayName"));
            var where = uninstall.Where(x => x.GetValue<string>("DisplayName") == "Intel(R) Processor Graphics");
            var list = uninstall.ToList();
            var dic = uninstall.ToDictionary(x => x.Name);
            var groups = uninstall.GroupBy(x => x.GetValue<string>("DisplayName"));
            foreach (var item in groups)
            {
                System.Diagnostics.Trace.WriteLine($"DisplayName:{item.Key} count:{item.Count()}");
                foreach (var oo in item)
                {
                    //System.Diagnostics.Trace.WriteLine($"DisplayName:{oo.GetValue<string>("DisplayName")}");
                }
            }
            var lookups = uninstall.ToLookup(x => x.GetValue<string>("DisplayName"));
            foreach (var item in lookups)
            {
                System.Diagnostics.Trace.WriteLine($"DisplayName:{item.Key} count:{item.Count()}");
                foreach (var oo in item)
                {
                    //System.Diagnostics.Trace.WriteLine($"DisplayName:{oo.GetValue<string>("DisplayName")}");
                }
            }
            var takes = uninstall.Take(3);



        }

    }

    public class CEqualityComparer<T> : IEqualityComparer<T>
    {
        public bool Equals(T x, T y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(T obj)
        {
            if(obj != null)
            {
                return ((object)obj).GetHashCode();
            }
            return 1;
        }
    }
    public class AppData
    {
        public string Name { set; get; }
        public string Ver { set; get; }
        public string Uninstallstring { set; get; }
        public bool IsOfficial { set; get; }
    }
}
