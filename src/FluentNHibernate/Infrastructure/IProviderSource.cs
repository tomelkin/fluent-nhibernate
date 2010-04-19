namespace FluentNHibernate.Infrastructure
{
    public interface IProviderSource
    {
        CompilationResult Compile(IMappingCompiler mappingCompiler);
    }
}