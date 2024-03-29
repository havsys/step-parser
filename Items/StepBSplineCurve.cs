﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using StepParser.Syntax;

namespace StepParser.Items
{
    public enum StepBSplineCurveForm
    {
        Polyline,
        CircularArc,
        EllipticalArc,
        ParabolicArc,
        HyperbolicArc,
        Unspecified
    };

    public abstract class StepBSplineCurve : StepBoundedCurve
    {
        public int Degree { get; set; }

        public List<StepCartesianPoint> ControlPointsList { get; } = new List<StepCartesianPoint>();

        public StepBSplineCurveForm CurveForm { get; set; } = StepBSplineCurveForm.Unspecified;

        public bool ClosedCurve { get; set; }

        public bool SelfIntersect { get; set; }

        public StepBSplineCurve(string name, IEnumerable<StepCartesianPoint> controlPoints) : base(name)
        {
            ControlPointsList.AddRange(controlPoints);
        }

        public StepBSplineCurve(string name, params StepCartesianPoint[] controlPoints)
            : this(name, (IEnumerable<StepCartesianPoint>)controlPoints)
        {
        }

        private const string POLYLINE_FORM = "POLYLINE_FORM";
        private const string CIRCULAR_ARC = "CIRCULAR_ARC";
        private const string ELLIPTIC_ARC = "ELLIPTIC_ARC";
        private const string PARABOLIC_ARC = "PARABOLIC_ARC";
        private const string HYPERBOLIC_ARC = "HYPERBOLIC_ARC";
        private const string UNSPECIFIED = "UNSPECIFIED";

        protected static StepBSplineCurveForm ParseCurveForm(string enumerationValue)
        {
            switch (enumerationValue.ToUpperInvariant())
            {
                case POLYLINE_FORM:
                    return StepBSplineCurveForm.Polyline;
                case CIRCULAR_ARC:
                    return StepBSplineCurveForm.CircularArc;
                case ELLIPTIC_ARC:
                    return StepBSplineCurveForm.EllipticalArc;
                case PARABOLIC_ARC:
                    return StepBSplineCurveForm.ParabolicArc;
                case HYPERBOLIC_ARC:
                    return StepBSplineCurveForm.HyperbolicArc;
                default:
                    return StepBSplineCurveForm.Unspecified;
            }
        }

        protected static string GetCurveFormString(StepBSplineCurveForm form)
        {
            switch (form)
            {
                case StepBSplineCurveForm.Polyline:
                    return POLYLINE_FORM;
                case StepBSplineCurveForm.CircularArc:
                    return CIRCULAR_ARC;
                case StepBSplineCurveForm.EllipticalArc:
                    return ELLIPTIC_ARC;
                case StepBSplineCurveForm.ParabolicArc:
                    return PARABOLIC_ARC;
                case StepBSplineCurveForm.HyperbolicArc:
                    return HYPERBOLIC_ARC;
                case StepBSplineCurveForm.Unspecified:
                    return UNSPECIFIED;
            }

            throw new NotImplementedException();
        }

        internal override void WriteXML(XmlWriter writer)
        {
            writer.WriteStartElement(ItemType.GetItemTypeElementString());

            writer.WriteStartElement("Type");
            writer.WriteString(ItemType.GetItemTypeElementString());
            writer.WriteEndElement();

            writer.WriteStartElement("Degree");
            writer.WriteString(Degree.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("ControlPointsList");
            base.WriteXML(writer);
            writer.WriteEndElement();

            writer.WriteEndElement();

            //_vector.WriteXML(writer);
        }
    }
}
