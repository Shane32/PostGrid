using PublicApiGenerator;

namespace Tests.ApiTests;

/// <summary>
/// See more info about API approval tests here <see href="https://github.com/JakeGinnivan/ApiApprover"/>.
/// </summary>
public class ApiApprovalTests
{
#if NET8_0_OR_GREATER
    [Theory]
#else
    [Theory(Skip = "Only test .NET 8.0 project")]
#endif
    [InlineData(typeof(PostGrid))]
    public void PublicApi(Type type)
    {
        string publicApi = type.Assembly.GeneratePublicApi(new ApiGeneratorOptions {
            IncludeAssemblyAttributes = false,
            UseDenyNamespacePrefixesForExtensionMethods = false,
        }) + Environment.NewLine;

        publicApi.ShouldMatchApproved(options => options.NoDiff());
        //options.WithFilenameGenerator((testMethodInfo, discriminator, fileType, fileExtension) => $"{type.Assembly.GetName().Name}.ApiApprovals.{fileType}.{fileExtension}");
    }
}
