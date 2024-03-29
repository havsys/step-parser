﻿using System;
using System.Collections.Generic;
using System.Linq;
using StepParser.Syntax;

namespace StepParser.Items
{
    public enum StepKnotType
    {
        UniformKnots,
        QuasiUniformKnots,
        PiecewiseBezierKnots,
        Unspecified
    };

    public class StepBSplineCurveWithKnots : StepBSplineCurve
    {
        public override StepItemType ItemType => StepItemType.BSplineCurveWithKnots;
        public List<int> KnotMultiplicities { get; } = new List<int>();

        public List<double> Knots { get; } = new List<double>();

        public StepKnotType KnotSpec { get; set; } = StepKnotType.Unspecified;

        public StepBSplineCurveWithKnots(string name, IEnumerable<StepCartesianPoint> controlPoints)
            : base(name, controlPoints)
        {
        }

        public StepBSplineCurveWithKnots(string name, params StepCartesianPoint[] controlPoints)
            : base(name, controlPoints)
        {
        }

        private const string UNIFORM_KNOTS = "UNIFORM_KNOTS";
        private const string QUASI_UNIFORM_KNOTS = "QUASI_UNIFORM_KNOTS";
        private const string PIECEWISE_BEZIER_KNOTS = "PIECEWISE_BEZIER_KNOTS";
        private const string UNSPECIFIED = "UNSPECIFIED";

        private static StepKnotType ParseKnotSpec(string enumerationValue)
        {
            switch (enumerationValue.ToUpperInvariant())
            {
                case UNIFORM_KNOTS:
                    return StepKnotType.UniformKnots;
                case QUASI_UNIFORM_KNOTS:
                    return StepKnotType.QuasiUniformKnots;
                case PIECEWISE_BEZIER_KNOTS:
                    return StepKnotType.PiecewiseBezierKnots;
                default:
                    return StepKnotType.Unspecified;
            }
        }

        private static string GetKnotSpec(StepKnotType spec)
        {
            switch (spec)
            {
                case StepKnotType.UniformKnots:
                    return UNIFORM_KNOTS;
                case StepKnotType.QuasiUniformKnots:
                    return QUASI_UNIFORM_KNOTS;
                case StepKnotType.PiecewiseBezierKnots:
                    return PIECEWISE_BEZIER_KNOTS;
                case StepKnotType.Unspecified:
                    return UNSPECIFIED;
            }

            throw new NotImplementedException();
        }

        internal static StepBSplineCurveWithKnots CreateFromSyntaxList(StepBinder binder, StepSyntaxList syntaxList, int id)
        {
            syntaxList.AssertListCount(9);
            var controlPointsList = syntaxList.Values[2].GetValueList();

            var spline = new StepBSplineCurveWithKnots(string.Empty, new StepCartesianPoint[controlPointsList.Values.Count]);
            spline.SyntaxList = syntaxList;
            spline.Id = id;
            spline.Name = syntaxList.Values[0].GetStringValue();
            spline.Degree = syntaxList.Values[1].GetIntegerValue();
            
            spline.BindSyntaxList(binder, syntaxList, 2, 3);

            spline.CurveForm = ParseCurveForm(syntaxList.Values[3].GetEnumerationValue());
            spline.ClosedCurve = syntaxList.Values[4].GetBooleanValue();
            spline.SelfIntersect = syntaxList.Values[5].GetBooleanValue();

            var knotMultiplicitiesList = syntaxList.Values[6].GetValueList();
            spline.KnotMultiplicities.Clear();
            for (int i = 0; i < knotMultiplicitiesList.Values.Count; i++)
            {
                spline.KnotMultiplicities.Add(knotMultiplicitiesList.Values[i].GetIntegerValue());
            }

            var knotslist = syntaxList.Values[7].GetValueList();
            spline.Knots.Clear();
            for (int i = 0; i < knotslist.Values.Count; i++)
            {
                spline.Knots.Add(knotslist.Values[i].GetRealVavlue());
            }

            spline.KnotSpec = ParseKnotSpec(syntaxList.Values[8].GetEnumerationValue());

            return spline;
        }
    }
}
