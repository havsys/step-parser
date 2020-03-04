﻿// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace StepParser.Tokens
{
    internal class StepAsteriskToken : StepToken
    {
        public override StepTokenKind Kind => StepTokenKind.Asterisk;

        public StepAsteriskToken(int line, int column)
            : base(line, column)
        {
        }

        public override string ToString()
        {
            return "*";
        }

        public static StepAsteriskToken Instance { get; } = new StepAsteriskToken(-1, -1);
    }
}
