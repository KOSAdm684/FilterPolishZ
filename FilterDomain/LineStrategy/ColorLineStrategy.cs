﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterDomain.LineStrategy
{
    public class ColorLineStrategy : ILineStrategy
    {
        public bool CanHaveOperator => false;
        public bool CanHaveComment => false;
        public bool CanHaveMultiple => false;
    }
}
