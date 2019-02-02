﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterCore.Util
{
    public static class StringWork
    {
        public static string CombinePieces(params string[] pieces)
        {
            var first = true;
            StringBuilder builder = new StringBuilder();

            foreach (var piece in pieces)
            {
                if (!string.IsNullOrEmpty(piece))
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        builder.Append(" ");
                    }

                    builder.Append(piece);
                }
            }

            return builder.ToString();
        }

        public static string GetDateString()
        {
            return DateTime.Now.ToString("yyyy-M-d");
        }
    }
}