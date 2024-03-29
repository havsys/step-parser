﻿using System.Collections.Generic;
using StepParser.Tokens;

namespace StepParser.Syntax
{
    internal abstract class StepSyntax
    {
        public abstract StepSyntaxType SyntaxType { get; }
        public int Line { get; }
        public int Column { get; }

        protected StepSyntax(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public abstract IEnumerable<StepToken> GetTokens();
    }
}
