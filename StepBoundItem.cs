﻿using StepParser.Items;
using StepParser.Syntax;
using StepParser.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace StepParser
{
    internal class StepBoundItem
    {
        public StepSyntax CreatingSyntax { get; }
        public StepRepresentationItem Item { get; }
        public bool IsAuto { get; private set; }

        public StepBoundItem(StepRepresentationItem item)
        {
            Item = item;
        }

        public StepBoundItem(StepRepresentationItem item, StepSyntax creatingSyntax)
        {
            CreatingSyntax = creatingSyntax;
            Item = item;
        }

        public TItemType AsType<TItemType>() where TItemType : StepRepresentationItem
        {
            TItemType result = null;
            if (IsAuto)
            {
                // do nothing; null is expected
            }
            else
            {
                result = Item as TItemType;
                if (result == null)
                {
                    LogWriter.Instance.WriteErrorLog("Unexpected type " + " item id = " + Item.Id + " ItemStyle " + Item.ItemType.ToString());
                    throw new StepReadException("Unexpected type", CreatingSyntax.Line, CreatingSyntax.Column);
                }
            }

            return result;
        }

        public static StepBoundItem AutoItem(StepSyntax creatingSyntax)
        {
            var boundItem = new StepBoundItem(null, creatingSyntax);
            boundItem.IsAuto = true;
            return boundItem;
        }
    }
}
