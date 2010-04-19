namespace FluentNHibernate.Infrastructure
{
    public interface IPersistenceInstructionGatherer
    {
        IPersistenceInstructions GetInstructions();
    }
}