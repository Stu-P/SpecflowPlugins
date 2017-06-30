using System.CodeDom;
using TechTalk.SpecFlow.Generator;
using TechTalk.SpecFlow.Generator.Configuration;
using TechTalk.SpecFlow.Generator.Plugins;
using TechTalk.SpecFlow.Generator.UnitTestProvider;
using TechTalk.SpecFlow.UnitTestProvider;
using TechTalk.SpecFlow.Utils;
using System.Linq;
using TechTalk.SpecFlow.Parser.SyntaxElements;
using System;

namespace MTMGeneratorProvider.Generator.SpecFlowPlugin
{


    /// <summary>
    /// The CodedUI generator.
    /// </summary>
    public class MTMGeneratorProvider : MsTest2010GeneratorProvider
    {

        private const string DATASOURCE_ATTR = "Microsoft.VisualStudio.TestTools.UnitTesting.DataSourceAttribute";


        /// <summary>
        /// Initializes a new instance of the <see cref="CodedUiGeneratorProvider"/> class.
        /// </summary>
        /// <param name="codeDomHelper">
        /// The code dom helper.
        /// </param>
        public MTMGeneratorProvider(CodeDomHelper codeDomHelper)
            : base(codeDomHelper)
        {
        }

        /// <summary>
        /// The set test class.
        /// </summary>
        /// <param name="generationContext">
        /// The generation context.
        /// </param>
        /// <param name="featureTitle">
        /// The feature title.
        /// </param>
        /// <param name="featureDescription">
        /// The feature description.
        /// </param>
        public override void SetTestClass(TestClassGenerationContext generationContext, string featureTitle, string featureDescription)
        {
            base.SetTestClass(generationContext, featureTitle, featureDescription);

        }

        public override void SetTestClassInitializeMethod(TestClassGenerationContext generationContext)
        {
            var field = new CodeMemberField()
            {
                Name = "testContext",
                Type = new CodeTypeReference("Microsoft.VisualStudio.TestTools.UnitTesting.TestContext"),
                Attributes = MemberAttributes.Private
            };
            generationContext.TestClass.Members.Add(field);

            var codeMemberProperty = new CodeMemberProperty();
            codeMemberProperty.Name = "TestContext";
            codeMemberProperty.Type = new CodeTypeReference("Microsoft.VisualStudio.TestTools.UnitTesting.TestContext");
            codeMemberProperty.Attributes = MemberAttributes.Public;
            codeMemberProperty.HasGet = true;
            codeMemberProperty.HasSet = true;
            codeMemberProperty.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(), "testContext")));
            codeMemberProperty.SetStatements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(), "testContext"),
                    new CodePropertySetValueReferenceExpression()));

            generationContext.TestClass.Members.Add(codeMemberProperty);



            base.SetTestClassInitializeMethod(generationContext);


        }

        public override void FinalizeTestClass(TestClassGenerationContext generationContext)
        {
            base.FinalizeTestClass(generationContext);
        }

        public override void SetTestInitializeMethod(TestClassGenerationContext generationContext)

        {

            base.SetTestInitializeMethod(generationContext);



            generationContext.TestClassInitializeMethod.Statements.Add(new CodeSnippetStatement("TechTalk.SpecFlow.FeatureContext.Current[\"TestLibraryAssembly\"] = System.Reflection.Assembly.GetExecutingAssembly();"));

            generationContext.TestClassInitializeMethod.Statements.Add(new CodeSnippetStatement("TechTalk.SpecFlow.FeatureContext.Current[\"TestContext\"] = TestContext"));



        }


        public override void SetTestMethod(TestClassGenerationContext generationContext, CodeMemberMethod testMethod, string scenarioTitle)
        {
   
            //       [TestMethod, DataSource("Microsoft.VisualStudio.TestTools.DataSource.TestCase", "http://tfs-win2012:8080/tfs/StuCollection;SpecflowDemo, "297860", DataAccessMethod.Sequential)]


            foreach (var scenario in generationContext.Feature.Scenarios)
            {
                if (scenario.Title == scenarioTitle)
                {
                    if (scenario.Tags != null)
                    {
                        Tag WorkItemId = scenario.Tags.FirstOrDefault(x => x.Name.StartsWith("workitem"));
                        if (WorkItemId != null)
                        {
                            scenario.Tags.Remove(WorkItemId);

                            var WorkItemIdText = WorkItemId.Name.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries)[1];

                            CodeTypeReferenceExpression dataAccessMethodCodeTypeRefExpr = new CodeTypeReferenceExpression("Microsoft.VisualStudio.TestTools.UnitTesting.DataAccessMethod");

                            var datasourceArgs = new CodeAttributeArgument[] {
                            new CodeAttributeArgument(new CodePrimitiveExpression("Microsoft.VisualStudio.TestTools.DataSource.TestCase")),
                            new CodeAttributeArgument(new CodePrimitiveExpression(string.Format("http://tfs-win2012:8080/tfs/StuCollection;{0}", "SpecflowDemo"))),
                            new CodeAttributeArgument(new CodePrimitiveExpression(WorkItemIdText)),
                            new CodeAttributeArgument(new CodeFieldReferenceExpression(dataAccessMethodCodeTypeRefExpr, "Sequential"))
                        };

                            CodeDomHelper.AddAttribute(testMethod, DATASOURCE_ATTR, datasourceArgs);

                        }
                    }
                }
            }

            base.SetTestMethod(generationContext, testMethod, scenarioTitle);

        }
    }
}