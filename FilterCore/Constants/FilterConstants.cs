﻿using FilterCore.Commands.EntryCommands;
using FilterDomain.LineStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterCore.Constants
{
    public class FilterConstants
    {
        static FilterConstants()
        {
            short i = 0;
            foreach (var keys in LineTypes)
            {
                LineTypesSort.Add(keys.Key, i);
                i++;
            }

            LineTypesSort.Add("", i);
            i++;
            LineTypesSort.Add("comment", i);
        }

        /// <summary>
        /// Lists all known FilterLine identifiers, their treatment strategy and the sort order.
        /// </summary>
        public static Dictionary<string, ILineStrategy> LineTypes = new Dictionary<string, ILineStrategy>(){
            { "Show",          new VariableLineStrategy(0,0) },
            { "Hide",          new VariableLineStrategy(0,0) },

            { "ShapedMap",     new BoolLineStrategy() },
            { "ElderItem",     new BoolLineStrategy() },
            { "ShaperItem",    new BoolLineStrategy() },
            { "Corrupted",     new BoolLineStrategy() },
            { "Identified",    new BoolLineStrategy() },
            { "ElderMap",      new BoolLineStrategy() },
            { "LinkedSockets", new NumericLineStrategy() },
            { "Sockets",       new NumericLineStrategy() },
            { "Quality",       new NumericLineStrategy() },
            { "MapTier",       new NumericLineStrategy() },
            { "Width",         new NumericLineStrategy() },
            { "Height",        new NumericLineStrategy() },
            { "StackSize",     new NumericLineStrategy() },
            { "GemLevel",      new NumericLineStrategy() },
            { "ItemLevel",     new NumericLineStrategy() },
            { "DropLevel",     new NumericLineStrategy() },
            { "Rarity",        new NumericLineStrategy() },
            { "SocketGroup",   new EnumLineStrategy() },
            { "Class",         new EnumLineStrategy() },
            { "BaseType",      new EnumLineStrategy() },
            { "Prophecy",      new EnumLineStrategy() },
            { "HasExplicitMod",new EnumLineStrategy() },

            { "SetFontSize",             new VariableLineStrategy(1,1) },
            { "SetTextColor",            new ColorLineStrategy() },
            { "SetBorderColor",          new ColorLineStrategy() },
            { "SetBackgroundColor",      new ColorLineStrategy() },
            { "DisableDropSound",        new VariableLineStrategy(0,1) },
            { "PlayAlertSound",          new VariableLineStrategy(1,2) },
            { "PlayAlertSoundPositional",new VariableLineStrategy(1,2) },
            { "CustomAlertSound",        new VariableLineStrategy(1,2) },
            { "PlayEffect",              new VariableLineStrategy(2,3) },
            { "MinimapIcon",             new VariableLineStrategy(3,3) }};

        public static Dictionary<string, IEntryCommand> EntryCommand = new Dictionary<string, IEntryCommand>()
        {
            { "D",      new DisableEntryCommand() },
            { "H",      new HideEntryCommand() },
            { "RF",     null },
            { "HS",     null },
            { "REMS",   null },
            { "UP",     null },
            { "RVR",    null }
        };

        public static char[] WhiteLineChars = new char[] { ' ', '\t' };

        /// <summary>
        /// A quick access to the sort order of the filterline idents
        /// </summary>
        public static Dictionary<string, int> LineTypesSort = new Dictionary<string, int>();

        public enum FilterEntryType
        {
            Content,
            Filler,
            Comment,
            Unknown
        }

        public enum FilterLineIdentPhase
        {
            None,
            IdentScan,
            CommentScan,
            ValueScan,
            ValueCommentScan,
            ValueTagScan
        }

        public static HashSet<string> FilterOperators = new HashSet<string>()
        {
            "=", ">=", "<=", ">", "<"
        };
    }
}