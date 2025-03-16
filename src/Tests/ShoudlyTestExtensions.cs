using System.Net.Http;
using Shouldly;

namespace Tests;

/// <summary>
/// Extension methods for testing.
/// </summary>
public static class ShouldlyTestExtensions
{
    /// <summary>
    /// Asserts that a MultipartFormDataContent contains a part with the specified name and content type.
    /// </summary>
    /// <param name="content">The MultipartFormDataContent to check.</param>
    /// <param name="name">The name of the part to look for.</param>
    /// <param name="contentType">The content type to check for.</param>
    /// <param name="message">Optional message to display if the assertion fails.</param>
    public static void ShouldContainPart(this MultipartFormDataContent content, string name, string contentType, string? message = null)
    {
        foreach (var part in content) {
            if (part.Headers.ContentDisposition != null &&
                part.Headers.ContentDisposition.Name != null &&
                part.Headers.ContentDisposition.Name.Trim('"') == name &&
                part.Headers.ContentType != null &&
                part.Headers.ContentType.MediaType == contentType) {
                return; // Found the part with the correct content type
            }
        }

        throw new ShouldAssertException(message ?? $"MultipartFormDataContent should contain a part named '{name}' with content type '{contentType}'");
    }

    /// <summary>
    /// Asserts that a MultipartFormDataContent contains a part with the specified name and value.
    /// </summary>
    /// <param name="content">The MultipartFormDataContent to check.</param>
    /// <param name="name">The name of the part to look for.</param>
    /// <param name="value">The expected value of the part.</param>
    /// <param name="message">Optional message to display if the assertion fails.</param>
    public static void ShouldContainPartWithValue(this MultipartFormDataContent content, string name, string value, string? message = null)
    {
        foreach (var part in content) {
            if (part.Headers.ContentDisposition != null &&
                part.Headers.ContentDisposition.Name != null &&
                part.Headers.ContentDisposition.Name.Trim('"') == name) {
                // Found the part, now check its value
                if (part is StringContent stringContent) {
                    // Use ReadAsStringAsync().Result to get the value synchronously
                    var partContent = stringContent.ReadAsStringAsync().Result;
                    if (partContent == value) {
                        return; // Found the part with the correct value
                    }

                    throw new ShouldAssertException(message ?? $"Part '{name}' has value '{partContent}' but expected '{value}'");
                }

                throw new ShouldAssertException(message ?? $"Part '{name}' is not StringContent");
            }
        }

        throw new ShouldAssertException(message ?? $"MultipartFormDataContent should contain a part named '{name}'");
    }
}
