namespace FluentNHibernate.Automapping
{
    public class PrivateAutoPersistenceModel : AutoPersistenceModel
    {
        public PrivateAutoPersistenceModel()
        {
            Setup(s =>
            {
                s.FindIdentity = findIdentity;
                s.FindMembers = findMembers;
            });
        }

        static bool findMembers(Member member)
        {
            return member.IsField && member.IsPrivate;
        }

        static bool findIdentity(Member member)
        {
            return member.IsField && member.IsPrivate && member.Name == "id";
        }
    }
}