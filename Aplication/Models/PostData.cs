﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Aplication.Models
{
    public class PostData
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string UseerId { get; set; }
    }
}