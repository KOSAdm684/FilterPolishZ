﻿using FilterCore.Constants;
using FilterCore.Entry;
using FilterCore.FilterComponents.Tier;
using FilterCore.Line;
using FilterCore.Line.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterCore
{
    public class Filter
    {
        public bool Debug_StoreLines { get; set; } = false;

        public List<IFilterEntry> FilterEntries { get; set; } = new List<IFilterEntry>();
        public List<IFilterLine> FilterLines { get; set; }

        public Filter(List<string> input)
        {
            List<IFilterLine> lineList = new List<IFilterLine>();

            foreach (var rawLine in input)
            {
                lineList.Add(LineParser.GenerateFilterLine(LineParser.TokenizeFilterLineString(rawLine)));
            }

            if (Debug_StoreLines)
            {
                this.FilterLines = lineList; //TODO: REMOVE
            }

            this.GenerateFilterEntries(lineList);
        }

        private void GenerateFilterEntries(List<IFilterLine> lineList)
        {
            FilterEntries.Clear();

            FilterEntry lastDataEntry = new FilterEntry();
            FilterEntry lastCommentEntry = new FilterEntry();
            FilterConstants.FilterEntryType entryType = FilterConstants.FilterEntryType.Unknown;

            foreach (var line in lineList)
            {
                if (!string.IsNullOrEmpty(line.Ident))
                {
                    entryType = FilterConstants.FilterEntryType.Content;
                    if (line.Ident == "Show" || line.Ident == "Hide")
                    {
                        lastDataEntry = FilterEntry.CreateDataEntry(line);
                        FilterEntries.Add(lastDataEntry);
                    }
                    else
                    {
                        lastDataEntry.Content.Add(line);

                    }
                }

                else if (line.Comment != string.Empty)
                {
                    if (entryType != FilterConstants.FilterEntryType.Comment)
                    {
                        lastCommentEntry = FilterEntry.CreateCommentEntry(line);
                        entryType = FilterConstants.FilterEntryType.Comment;
                        FilterEntries.Add(lastCommentEntry);
                    }
                    else
                    {
                        lastCommentEntry.Content.AddComment(line);
                    }
                    entryType = FilterConstants.FilterEntryType.Comment;
                }

                else if (line.Comment == string.Empty)
                {
                    if (entryType == FilterConstants.FilterEntryType.Filler)
                    {
                        continue;
                    }
                    else
                    {
                        FilterEntries.Add(FilterEntry.CreateFillerEntry());
                        entryType = FilterConstants.FilterEntryType.Filler;
                    }
                }
            }
        }

        public Dictionary<string, TierGroup> ExtractTiers(HashSet<string> addressedTiers)
        {
            Dictionary<string, TierGroup> result = new Dictionary<string, TierGroup>();

            foreach (var entry in this.FilterEntries)
            {
                if (entry.Header.TierTag != null)
                {
                    foreach (var tierTag in entry.Header.TierTag)
                    {
                        if (addressedTiers.Contains(tierTag.Category))
                        {
                            if (!result.ContainsKey(tierTag.Category))
                            {
                                result.Add(tierTag.Category, new TierGroup(tierTag.Category));
                            }

                            result[tierTag.Category].FilterEntries[tierTag.Category] = new SingleTier() { Entry = entry, TierName = tierTag.SubCategory };
                        }
                    }
                }
            }

            return result;
        }

        public void ExecuteCommandTags()
        {
            foreach (var entry in this.FilterEntries)
            {
                if (entry.Header.Type != FilterConstants.FilterEntryType.Content)
                {
                    continue;
                }

                if (entry.Header.IsFrozen)
                {
                    continue;
                }

                foreach (var command in entry.Header.GenerationTags)
                {
                    
                }
            }
        }

        public List<string> Serialize()
        {
            var results = new List<string>();
            this.FilterEntries.ForEach(x => results.AddRange(x.Serialize()));
            return results;
        }
    }
}