﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.DynamicModules.Builder.Model;
using Telerik.Sitefinity.Taxonomies;
using Telerik.Sitefinity.Taxonomies.Model;

namespace DynamicContent.FieldsGenerator
{
    /// <summary>
    /// This class represents field generation strategy for multiple choice dynamic fields.
    /// </summary>
    public class MultipleChoiceFieldGenerationStrategy : FieldGenerationStrategy
    {
        /// <inheritdoc/>
        public override bool GetFieldCondition(DynamicModuleField field)
        {
            var condition = base.GetFieldCondition(field)
                && (field.FieldType == FieldType.MultipleChoice || field.FieldType == FieldType.Choices);

            return condition;
        }

        /// <inheritdoc/>
        public override string GetFieldMarkup(DynamicModuleField field)
        {
            var markup = string.Empty;

            if (field.FieldType == FieldType.MultipleChoice)
            {
                markup = string.Format(MultipleChoiceFieldGenerationStrategy.fieldMarkupMultipleChoiceTempalte, field.Name, field.Title);
            }
            else
            {
                markup = string.Format(MultipleChoiceFieldGenerationStrategy.fieldMarkupSingleChoiceTempalte, field.Name, field.Title);
            }

            return markup;
        }

        private const string fieldMarkupMultipleChoiceTempalte = @"@Html.Sitefinity().ChoiceField((IEnumerable)Model.Item.{0}, ""{0}"", ""{1}"", ""sfitemChoices"")";
        private const string fieldMarkupSingleChoiceTempalte = @"@Html.Sitefinity().ChoiceField((string)Model.Item.{0}, ""{0}"", ""{1}"", ""sfitemChoices"")";
    }
}
