using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace rmxwebapi.ViewModels
{
    public class PhotoInfoViewModel
    {
        public byte[] Photo { get; set; }
        public bool IsSquarePhoto { get; set; }
    }
}