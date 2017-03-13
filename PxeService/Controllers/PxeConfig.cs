using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using PxeService.Model;

namespace PxeService.Controllers
{

    [Route("api/[controller]")]
    public class PxeConfig : Controller 
    {
        OSMap OSMap = new OSMap {
            { OSType.Windows, "pxeboot.0" },
            { OSType.Linux, "pxeboot.1" },
            { OSType.ESXi, "pxeboot.2" }
        };

        string template = @" DEFAULT menu.c32
 PROMPT 0

 MENU TITLE PXE Special Boot Menu
 MENU INCLUDE pxelinux.cfg/graphics.conf
 MENU AUTOBOOT Starting Local System in # seconds

 LABEL AutoMenu
   MENU LABEL {0}
   KERNEL {1}";

        [HttpPost]
        public void Post([FromBody]PxeClient data)
        {
            string content;
            string mac = data.mac;
            OSType os = data.os;

            content = string.Format(template, os, OSMap[os]);

			Console.Write("Writing content as {0}\n\n",mac);
			Console.Write(content);

            try 
            {
                System.IO.File.WriteAllText(mac, content);
            }
            catch 
            {
                throw new Exception();
            }

        }
    }
}
