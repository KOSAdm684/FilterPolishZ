﻿using FilterDomain.LineStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterCore.Line
{
    public interface IFilterLine
    {
        ILineValueCore Value { get; set; }
        IFilterLine Clone();
        string Serialize();

        string Ident { get; set; }
        string Comment { get; set; }

        bool identCommented { get; set; }
    }

    public static class EFilterLine
    {
        public static IFilterLine Define(this IFilterLine me, string ident)
        {
            me.Ident = ident;
            return me;
        }

        public static IFilterLine AddCore(this IFilterLine me, ILineValueCore LineValueCore)
        {
            me.Value = LineValueCore;
            return me;
        }
    }
}
