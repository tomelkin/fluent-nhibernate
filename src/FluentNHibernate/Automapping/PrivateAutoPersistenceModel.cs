using FluentNHibernate.Conventions;

namespace FluentNHibernate.Automapping
{
    public class PrivateAutoPersistenceModel : AutoPersistenceModel
    {
        public PrivateAutoPersistenceModel()
        {
            // setup defaults for private automapping -
            // id = private field called "id"
            // members = private fields
            Expressions.FindIdentity = findIdentity;
            Expressions.FindMembers = findMembers;

            autoMapper = new AutoMapper(Expressions, new DefaultConventionFinder(), inlineOverrides);
        }

        static bool findIdentity(Member member)
        {
            return member.IsField && member.IsPrivate && member.Name == "id";
        }

        static bool findMembers(Member member)
        {
            return member.IsField && member.IsPrivate;
        }
    }
}