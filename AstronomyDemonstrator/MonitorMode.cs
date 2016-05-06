using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AstronomyDemonstrator
{
    public class MonitorMode
    {
        public void Extend()
        {
            Process pro = new Process();
            pro.StartInfo.FileName = "cmd.exe";
            pro.StartInfo.Arguments = "/c" + "displayswitch/extend";
            pro.StartInfo.UseShellExecute = false;
            pro.StartInfo.CreateNoWindow = true;
            pro.Start();
        }
        public void Clone()
        {
            Process pro = new Process();
            pro.StartInfo.FileName = "cmd.exe";
            pro.StartInfo.Arguments = "/c" + "displayswitch/clone";
            pro.StartInfo.UseShellExecute = false;
            pro.StartInfo.CreateNoWindow = true;
            pro.Start();
        }
        public void Internal()
        {
            Process pro = new Process();
            pro.StartInfo.FileName = "cmd.exe";
            pro.StartInfo.Arguments = "/c" + "displayswitch/internal";
            pro.StartInfo.UseShellExecute = false;
            pro.StartInfo.CreateNoWindow = true;
            pro.Start();
        }
        public void External()
        {
            Process pro = new Process();
            pro.StartInfo.FileName = "cmd.exe";
            pro.StartInfo.Arguments = "/c" + "displayswitch/external";
            pro.StartInfo.UseShellExecute = false;
            pro.StartInfo.CreateNoWindow = true;
            pro.Start();
        }
    }
}
