﻿using System.Linq;
using System.Web.Mvc;
using FeatherWidgets.TestUtilities.CommonOperations;
using MbUnit.Framework;
using Telerik.Sitefinity.Frontend.Forms.Mvc.Controllers;
using Telerik.Sitefinity.Frontend.Forms.Mvc.StringResources;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Controllers;
using Telerik.Sitefinity.Frontend.TestUtilities;
using Telerik.Sitefinity.Frontend.TestUtilities.CommonOperations;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Modules.Forms;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Mvc.Proxy;
using Telerik.Sitefinity.TestIntegration.SDK.DevelopersGuide.SitefinityEssentials.Modules.Forms;
using Telerik.WebTestRunner.Server.Attributes;

namespace FeatherWidgets.TestIntegration.Forms.Fields
{
    /// <summary>
    /// This class ensures TextField functionalities work correctly.
    /// </summary>
    [TestFixture]
    [Description("This class ensures TextField functionalities work correctly.")]
    public class TextFieldTests
    {
        /// <summary>
        /// Ensures that when a text field widget is added to form the default value is presented in the page markup.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling"), Test]
        [Category(TestCategories.Forms)]
        [Author(FeatherTeams.SitefinityTeam6)]
        [Description("Ensures that when a text field widget is added to form the default value is presented in the page markup.")]
        public void TextField_EditDefaultValue_MarkupIsCorrect()
        {
            var control = new MvcWidgetProxy();
            control.ControllerName = typeof(TextFieldController).FullName;
            var controller = new TextFieldController();
            controller.MetaField.DefaultValue = "My default text";
            control.Settings = new ControllerSettings(controller);

            var formId = ServerOperationsFeather.Forms().CreateFormWithWidget(control);

            var pageManager = PageManager.GetManager();

            try
            {
                var template = pageManager.GetTemplates().FirstOrDefault(t => t.Name == "SemanticUI.default" && t.Title == "default");
                Assert.IsNotNull(template, "Template was not found");

                var pageId = FeatherServerOperations.Pages().CreatePageWithTemplate(template, "TextFieldValueTest", "text-field-value-test");
                ServerOperationsFeather.Forms().AddFormControlToPage(pageId, formId);

                var pageContent = ServerOperationsFeather.Pages().GetPageContent(pageId);

                Assert.IsTrue(pageContent.Contains("My default text"), "Form did not render the default text in the text field.");
            }
            finally
            {
                Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.Pages().DeleteAllPages();
                FormsModuleCodeSnippets.DeleteForm(formId);
            }
        }

        /// <summary>
        /// Ensures that when a text field widget is submitted with certain value then the response is correct.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object)"), Test]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling"), Category(TestCategories.Forms)]
        [Author(FeatherTeams.SitefinityTeam6)]
        [Description("Ensures that when a text field widget is submitted with certain value then the response is correct.")]
        public void TextField_SubmitValue_ResponseIsCorrect()
        {
            var control = new MvcWidgetProxy();
            control.ControllerName = typeof(TextFieldController).FullName;
            var controller = new TextFieldController();
            controller.MetaField.DefaultValue = "My default text";
            control.Settings = new ControllerSettings(controller);

            var formId = ServerOperationsFeather.Forms().CreateFormWithWidget(control);

            var formManager = FormsManager.GetManager();
            var form = formManager.GetForms().FirstOrDefault(f => f.Id == formId);

            var pageManager = PageManager.GetManager();

            try
            {
                var template = pageManager.GetTemplates().FirstOrDefault(t => t.Name == "SemanticUI.default" && t.Title == "default");
                Assert.IsNotNull(template, "Template was not found");

                var pageId = FeatherServerOperations.Pages().CreatePageWithTemplate(template, "TextFieldValueTest", "text-field-value-test");
                ServerOperationsFeather.Forms().AddFormControlToPage(pageId, formId);

                var textFieldName = ServerOperationsFeather.Forms().GetFirstFieldName(formManager, form);
                ServerOperationsFeather.Forms().SubmitField(textFieldName, "Submitted value", pageManager, pageId);
                var formEntry = formManager.GetFormEntries(form).LastOrDefault();

                Assert.IsNotNull(formEntry, "Form entry has not been submitted.");
                var submittedValue = formEntry.GetValue(textFieldName) as string;

                Assert.IsTrue(submittedValue == "Submitted value", "Form did not persisted the submitted text value correctly.");
            }
            finally
            {
                Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.Pages().DeleteAllPages();
                FormsModuleCodeSnippets.DeleteForm(formId);
            }
        }

        /// <summary>
        /// Ensures that when a text field widget with URL type is submitted with incorrect value then the validation fails.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object)"), Test]
        [Category(TestCategories.Forms)]
        [Author(FeatherTeams.SitefinityTeam6)]
        [Description("Ensures that when a text field widget with URL type is submitted with incorrect value then the validation fails.")]
        public void TextFieldUrl_SubmitIncorrectValue_ServerValidationFails()
        {
            var control = new MvcWidgetProxy();
            control.ControllerName = typeof(TextFieldController).FullName;
            var controller = new TextFieldController();
            controller.Model.InputType = Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields.TextField.TextType.Url;
            control.Settings = new ControllerSettings(controller);

            var formId = ServerOperationsFeather.Forms().CreateFormWithWidget(control);

            var formManager = FormsManager.GetManager();
            var form = formManager.GetForms().FirstOrDefault(f => f.Id == formId);

            var pageManager = PageManager.GetManager();

            try
            {
                var template = pageManager.GetTemplates().FirstOrDefault(t => t.Name == "SemanticUI.default" && t.Title == "default");
                Assert.IsNotNull(template, "Template was not found");

                var pageId = FeatherServerOperations.Pages().CreatePageWithTemplate(template, "TextFieldValidationTest", "text-field-validation-test");
                ServerOperationsFeather.Forms().AddFormControlToPage(pageId, formId);

                var textFieldName = ServerOperationsFeather.Forms().GetFirstFieldName(formManager, form);
                var result = ServerOperationsFeather.Forms().SubmitField(textFieldName, "Submitted value", pageManager, pageId);
                var contentResult = result as ContentResult;
                Assert.IsNotNull(contentResult, "Submit should return content result.");
                Assert.AreEqual(Res.Get<FormResources>().UnsuccessfullySubmittedMessage, contentResult.Content, "The Submit didn't result in error as expected!");
                
                var formEntry = formManager.GetFormEntries(form).LastOrDefault();
                Assert.IsNull(formEntry, "Form entry has been submitted even when the form is not valid.");
            }
            finally
            {
                Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.Pages().DeleteAllPages();
                FormsModuleCodeSnippets.DeleteForm(formId);
            }
        }
        
        /// <summary>
        /// Ensures that when a text field widget is added to form the input type is presented in the page markup.
        /// </summary>
        [Test]
        [Category(TestCategories.Forms)]
        [Author(FeatherTeams.SitefinityTeam6)]
        [Description("Ensures that when a text field widget is added to form the input type is presented in the page markup.")]
        public void TextField_EditInputType_MarkupIsCorrect()
        {
            var control = new MvcWidgetProxy();
            control.ControllerName = typeof(TextFieldController).FullName;
            var controller = new TextFieldController();
            controller.Model.InputType = Telerik.Sitefinity.Frontend.Forms.Mvc.Models.Fields.TextField.TextType.DateTimeLocal;
            control.Settings = new ControllerSettings(controller);

            var formId = ServerOperationsFeather.Forms().CreateFormWithWidget(control);

            var pageManager = PageManager.GetManager();

            try
            {
                var template = pageManager.GetTemplates().FirstOrDefault(t => t.Name == "SemanticUI.default" && t.Title == "default");
                Assert.IsNotNull(template, "Template was not found");

                var pageId = FeatherServerOperations.Pages().CreatePageWithTemplate(template, "TextFieldTypeTest", "text-field-type-test");
                ServerOperationsFeather.Forms().AddFormControlToPage(pageId, formId);

                var pageContent = ServerOperationsFeather.Pages().GetPageContent(pageId);

                Assert.IsTrue(pageContent.Contains("type=\"datetime-local\""), "Form did not render the input type in the text field.");
            }
            finally
            {
                Telerik.Sitefinity.TestUtilities.CommonOperations.ServerOperations.Pages().DeleteAllPages();
                FormsModuleCodeSnippets.DeleteForm(formId);
            }
        }
    }
}
