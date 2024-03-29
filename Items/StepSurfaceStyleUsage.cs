﻿using StepParser.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace StepParser.Items
{
    public class StepSurfaceStyleUsage: StepRepresentationItem
    {
        public override StepItemType ItemType => StepItemType.SurfaceStyleUsage;
        public string Side { get; set; }
        public StepSurfaceSideStyle SurfaceSideStyle { get; set; }

        private StepSurfaceStyleUsage()
            : base(string.Empty, 0)
        {
        }

        internal static StepSurfaceStyleUsage CreateFromSyntaxList(StepBinder binder, StepSyntaxList syntaxList, int id)
        {
            var surfaceStyleUsage = new StepSurfaceStyleUsage();
            surfaceStyleUsage.SyntaxList = syntaxList;
            syntaxList.AssertListCount(2);
            surfaceStyleUsage.Id = id;
            surfaceStyleUsage.Side = syntaxList.Values[0].GetEnumerationValue();

            //binder.BindValue(syntaxList.Values[1], v => surfaceStyleUsage.SurfaceSideStyle = v.AsType<StepSurfaceSideStyle>());
            surfaceStyleUsage.BindSyntaxList(binder, syntaxList, 1);
            return surfaceStyleUsage;
        }

        internal override void WriteXML(XmlWriter writer)
        {
            writer.WriteStartElement("SurfaceStyleUsage");
            writer.WriteAttributeString("type", ItemType.GetItemTypeString());
            writer.WriteAttributeString("side", Side);
            //writer.WriteStartElement("Styles");
            //SurfaceSideStyle.WriteXML(writer);
            base.WriteXML(writer);
            writer.WriteEndElement();
        }
    }
}
