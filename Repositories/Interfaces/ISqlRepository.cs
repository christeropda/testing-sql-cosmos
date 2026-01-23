// ============================================================================
// IMPORTANT: Why do we have separate interfaces for Cosmos/SQL? 
// Isn't the point of using repositories to abstract database differences?
// ============================================================================
// In NORMAL production code, you would use ONE interface (IPlayedGameService) and
// swap between implementations (SQL vs Cosmos) through dependency injection configuration.
// The whole point of interfaces is to abstract away the implementation choice.
//
// However, in THIS educational example, we're running BOTH databases simultaneously
// in the same API to compare them side-by-side. This requires us to distinguish 
// between the two services in our dependency injection.
//
// Solution: We create ICosmosPlayedGameService and ISqlPlayedGameService that both
// inherit from IPlayedGameService. This gives us:
// - Distinct types for DI registration (so we can inject both)
// - Shared contract from the base interface (avoiding true duplication)
// - Clear indication in each controller which database it's using
//
// DO NOT use this pattern in real applications unless you genuinely need multiple
// databases running simultaneously. For normal "choose one database" scenarios,
// use a single interface and configure which implementation to use via DI.
// This would mean removing the cosmos and sql interfacing and dependency injecting
// with IPlayedGameService instead.
// ============================================================================

namespace cosmosdb_test.Repositories.Interfaces;

public interface ISqlRepository : IRepository
{
}

