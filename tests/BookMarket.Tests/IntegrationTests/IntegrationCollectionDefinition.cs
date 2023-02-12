using Xunit;

namespace BookMarket.Tests.IntegrationTests;

[CollectionDefinition(DefinitionName)]
public class IntegrationCollectionDefinition : ICollectionFixture<IntegrationTestsFixture>
{
    public const string DefinitionName = "Controller Collection Fixture";
}