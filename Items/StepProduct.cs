﻿using StepParser.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace StepParser.Items
{
    public class StepProduct: StepComponentAssemble
    {
        public override StepItemType ItemType => StepItemType.Product;
        private StepProduct()
            : base(string.Empty, 0)
        {
        }

        internal static StepProduct CreateFromSyntaxList(StepBinder binder, StepSyntaxList syntaxList, int id)
        {
            var product = new StepProduct();
            product.SyntaxList = syntaxList;
            syntaxList.AssertListCount(4);
            product.Id = id;
            product.Name = syntaxList.Values[0].GetStringValue();
            product.Description = syntaxList.Values[1].GetStringValue();

            return product;
        }

        internal override void WriteXML(XmlWriter writer)
        {
            writer.WriteStartElement("ID");
            writer.WriteString('#' + Id.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("Name");
            writer.WriteString(Name);
            writer.WriteEndElement();

            writer.WriteStartElement("Description");
            writer.WriteString(Description);
            writer.WriteEndElement();
        }
    }
}
