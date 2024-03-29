﻿using System;
using System.Collections.Generic;
using System.Xml;
using StepParser.Syntax;

namespace StepParser.Items
{
    public class StepVertexPoint : StepVertex
    {
        public override StepItemType ItemType => StepItemType.VertexPoint;

        private StepCartesianPoint _location;

        public StepCartesianPoint Location
        {
            get { return _location; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                _location = value;
            }
        }

        private StepVertexPoint()
            : base(string.Empty)
        {
        }

        public StepVertexPoint(string name, StepCartesianPoint location)
            : base(name)
        {
            Location = location;
        }

        internal static StepVertexPoint CreateFromSyntaxList(StepBinder binder, StepSyntaxList syntaxList)
        {
            var vertex = new StepVertexPoint();
            vertex.SyntaxList = syntaxList;
            syntaxList.AssertListCount(2);
            vertex.Name = syntaxList.Values[0].GetStringValue();
            binder.BindValue(syntaxList.Values[1], v => vertex.Location = v.AsType<StepCartesianPoint>());
            return vertex;
        }

        internal override void WriteXML(XmlWriter writer)
        {
            _location.WriteXML(writer);
        }
    }
}
