using PublicApiGenerator;

namespace Tests.ApiTests;

/// <summary>
/// See more info about API approval tests here <see href="https://github.com/JakeGinnivan/ApiApprover"/>.
/// </summary>
public class ApiApprovalTests
{
    [Theory]
    [InlineData(typeof(PostGrid))]
    public void PublicApi(Type type)
    {
        string publicApi = type.Assembly.GeneratePublicApi(new ApiGeneratorOptions {
            IncludeAssemblyAttributes = false,
            UseDenyNamespacePrefixesForExtensionMethods = false,
        }) + Environment.NewLine;

        publicApi.ShouldMatchApproved(options => {
            options.NoDiff();
            options.WithFilenameGenerator((testMethodInfo, discriminator, fileType, fileExtension) => $"{type.Assembly.GetName().Name}.{fileType}.{fileExtension}");
        });
    }
}
