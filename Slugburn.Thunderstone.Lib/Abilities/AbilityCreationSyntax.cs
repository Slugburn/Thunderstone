namespace Slugburn.Thunderstone.Lib.Abilities
{
    public interface IAbilityCard
    {
        Card Card { get; }
    }

    public interface ICreateAbilitySyntax : IAbilityCard {}

    public interface IAbilitySelectCardsSyntax : IAbilityCard {}

    public interface IAbilityCardsSelectedContext : IAbilityCard { }

    public interface IAbilityDescriptionCompleteSyntax : IAbilityCard {}

    public interface IAbilityActionSyntax : IAbilityCard {}

    public interface IAbilityCardsSelectedSyntax : IAbilityDefinedSyntax, IAbilityDescriptionCompleteSyntax, IAbilitySelectCardsSyntax { }

    public interface IAbilityDefinedSyntax
    {
        Card On(params Phase[] phases);
    }
}
