using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace PxeService.Controllers
{

    public enum OSType {
        Windows,
        Linux,
        ESXi
    }

    [Route("api/[controller]")]
    public class PxeConfig : Controller 
    {
        public Dictionary<OSType, String> OSMap = new Dictionary<OSType, String>
        {
            { OSType.Windows, "pxeboot.0" },
            { OSType.Linux, "pxeboot.1"},
            { OSType.ESXi, "pxeboot.2"}
        };
        string template = @"
 DEFAULT menu.c32
 PROMPT 0
 
 MENU TITLE PXE Special Boot Menu
 MENU INCLUDE pxelinux.cfg/graphics.conf
 MENU AUTOBOOT Starting Local System in # seconds

 LABEL AutoMenu
   MENU LABEL {0}
   KERNEL {1}";

        [HttpGet]
        public string Get(string mac, OSType os)
        {
            string content;

            content = string.Format(template, os, OSMap[os]);
            System.IO.File.WriteAllText(mac, content);
            return "Done.";
        }
    }
}