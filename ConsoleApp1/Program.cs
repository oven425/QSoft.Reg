﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using QSoft.Reg.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<CSS> ll = new List<CSS>() { new CSS(), new CSS() };
            var vvv = ll.Select(x => new { x.A, x.B });
            RegistryKey reg = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey uninstall = reg.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall");
            //foreach (var oo in uninstall.GetSubKeyNames())
            //{
            //    RegistryKey subkey = uninstall.OpenSubKey(oo);
            //    string displayname = subkey.GetValue("DisplayName") as string;
            //    System.Diagnostics.Trace.WriteLine(displayname);
            //    //subkey.Dispose();
            //}

            uninstall.Select(x => x.GetValue<string>("DisplayName"));
            var vv = uninstall.Where(x => x.GetValue<string>("DisplayName") == "Intel(R) Processor Graphics" || x.GetValue<string>("DisplayName") == "");
            foreach (var oo in vv)
            {

            }


            //var vv = from regt in uninstall where reg.GetValue<string>("DisplayName") == "Intel(R) Processor Graphics" select regt;
            //foreach (var oo in vv)
            //{

            //}

        }
    }

    public class CSS
    {
        public string A { set; get; } = "A";
        public int B { set; get; } = 10;
        public string C { set; get; } = "C";
    }


    //public class CAA
    //{
    //    public string Current
    //    {
    //        get { return _index >= 0 ? Items[_index] : null; }
    //    }

    //    public bool MoveNext()
    //    {
    //        if (_index < Items.Count - 1)
    //        {
    //            _index++;
    //            return true;
    //        }
    //        return false;
    //    }
    //    public CAA GetEnumerator()
    //    {
    //        return this;
    //    }
    //}

    public class CustomEnumerable
    {
        // A custom enumerator which has a Current property and a MoveNext() method, but does NOT implement IEnumerator.
        public class CustomEnumerator
        {
            private readonly CustomEnumerable _enumerable;
            private int _index = -1;

            public CustomEnumerator(CustomEnumerable enumerable)
            {
                _enumerable = enumerable;
            }

            private IList<string> Items
            {
                get { return _enumerable._Items; }
            }

            public string Current
            {
                get { return _index >= 0 ? Items[_index] : null; }
            }

            public bool MoveNext()
            {
                if (_index < Items.Count - 1)
                {
                    _index++;
                    return true;
                }
                return false;
            }
        }

        private IList<string> _Items;

        public CustomEnumerable(params string[] items)
        {
            _Items = new List<string>(items);
        }

        public CustomEnumerator GetEnumerator()
        {
            return new CustomEnumerator(this);
        }
    }
}
