﻿using StepParser.Syntax;
using System.Xml;

namespace StepParser.Items
{
    public class StepPlane : StepElementarySurface
    {
        public override StepItemType ItemType => StepItemType.Plane;

        private StepPlane()
            : base()
        {
        }

        public StepPlane(string name, StepAxis2Placement3D position)
            : base(name, position)
        {
        }

        internal static StepPlane CreateFromSyntaxList(StepBinder binder, StepSyntaxList syntaxList, int id)
        {
            var plane = new StepPlane();
            plane.SyntaxList = syntaxList;
            plane.Id = id;
            syntaxList.AssertListCount(2);
            plane.Name = syntaxList.Values[0].GetStringValue();
            binder.BindValue(syntaxList.Values[1], v => plane.Position = v.AsType<StepAxis2Placement3D>());
            return plane;
        }

        internal override void WriteXML(XmlWriter writer)
        {
            writer.WriteStartElement("Type");
            writer.WriteString(ItemType.GetItemTypeElementString());
            writer.WriteEndElement();

            base.WriteXML(writer);
        }
    }
}
