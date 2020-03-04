﻿// Copyright (c) IxMilia.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace StepParser.Items
{
    public abstract class StepPoint : StepGeometricRepresentationItem
    {
        protected StepPoint(string name)
            : base(name, 0)
        {
        }
    }
}
