﻿using StepParser.Items;
using StepParser.Syntax;
using StepParser.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace StepParser
{
    public class StepReader
    {
        private StepLexer _lexer;
        private StepFile _file;

        public StepReader(Stream stream)
        {
            _file = new StepFile();
            var tokenizer = new StepTokenizer(stream);
            _lexer = new StepLexer(tokenizer.GetTokens());
            if (tokenizer.IsFailed)
                throw new Exception($"Unexpected parse character");
        }

        public StepFile ReadFile()
        {
            var fileSyntax = _lexer.LexFileSyntax();
            foreach (var headerMacro in fileSyntax.Header.Macros)
            {
                ApplyHeaderMacro(headerMacro);
            }

            var itemMap = new Dictionary<int, List<StepRepresentationItem>>();
            var binder = new StepBinder(itemMap);
            StepRepresentationItem.UnsupportedItemTypes.Clear();
            foreach (var itemInstance in fileSyntax.Data.ItemInstances)
            {
                if (itemMap.ContainsKey(itemInstance.Id))
                {
                    throw new StepReadException("Duplicate item instance", itemInstance.Line, itemInstance.Column);
                }
                List<StepRepresentationItem> items = new List<StepRepresentationItem>();
                if (itemInstance.SimpleItemInstance is StepSimpleItemSyntax)
                {
                    var item = StepRepresentationItem.FromTypedParameterToItem(binder, itemInstance.SimpleItemInstance, itemInstance.Id);
                    if (item != null)
                        items.Add(item);

                }
                else if (itemInstance.SimpleItemInstance is StepComplexItemSyntax)
                    items = StepRepresentationItem.FromTypedParameterToItems(binder, itemInstance.SimpleItemInstance, itemInstance.Id);
                else
                    LogWriter.Instance.WriteErrorLog("Unsupported step item syntax " + itemInstance.SimpleItemInstance.GetType().Name);
                if (items.Count > 0)
                {
                    itemMap.Add(itemInstance.Id, items);
                    foreach(var item in items)
                        _file.Items.Add(item);
                }
            }

            binder.BindRemainingValues();

            return _file;
        }

        private void ApplyHeaderMacro(StepHeaderMacroSyntax macro)
        {
            switch (macro.Name)
            {
                case StepFile.FileDescriptionText:
                    ApplyFileDescription(macro.Values);
                    break;
                case StepFile.FileNameText:
                    ApplyFileName(macro.Values);
                    break;
                case StepFile.FileSchemaText:
                    ApplyFileSchema(macro.Values);
                    break;
                default:
                    Debug.WriteLine($"Unsupported header macro '{macro.Name}' at {macro.Line}, {macro.Column}");
                    break;
            }
        }

        private void ApplyFileDescription(StepSyntaxList valueList)
        {
            valueList.AssertListCount(2);
            _file.Description = valueList.Values[0].GetConcatenatedStringValue();
            _file.ImplementationLevel = valueList.Values[1].GetStringValue(); // TODO: handle appropriate values
        }

        private void ApplyFileName(StepSyntaxList valueList)
        {
            valueList.AssertListCount(7);
            _file.Name = valueList.Values[0].GetStringValue();
            _file.Timestamp = valueList.Values[1].GetStringValue();
            _file.Author = valueList.Values[2].GetConcatenatedStringValue();
            _file.Organization = valueList.Values[3].GetConcatenatedStringValue();
            _file.PreprocessorVersion = valueList.Values[4].GetStringValue();
            _file.OriginatingSystem = valueList.Values[5].GetStringValue();
            _file.Authorization = valueList.Values[6].GetStringValue();
        }

        private void ApplyFileSchema(StepSyntaxList valueList)
        {
            valueList.AssertListCount(1);
            foreach (var schemaName in valueList.Values[0].GetValueList().Values.Select(v => v.GetStringValue()))
            {
                StepSchemaTypes schemaType;
                if (StepSchemaTypeExtensions.TryGetSchemaTypeFromName(schemaName, out schemaType))
                {
                    _file.Schemas.Add(schemaType);
                }
                else
                {
                    _file.UnsupportedSchemas.Add(schemaName);
                }
            }
        }
    }
}
