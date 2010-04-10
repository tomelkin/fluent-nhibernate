﻿using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace FluentNHibernate.Specs.FluentInterface.Fixtures
{
    class EntityWithCollectionsParent : EntityParent
    {}

    class EntityWithCollections : EntityWithCollectionsParent
    {
        public EntityCollectionChild[] ArrayOfChildren { get; set; }
        public IList<EntityCollectionChild> BagOfChildren { get; set; }
        public ISet<EntityCollectionChild> SetOfChildren { get; set; }

        public IList<string> BagOfStrings { get; set; }
    }

    class EntityWithFieldCollectionsParent : EntityParent
    {}

    class EntityWithFieldCollections : EntityWithFieldCollectionsParent
    {
        public IList<EntityCollectionChild> BagOfChildren;
    }

    class EntityCollectionChild
    {
        public int Position { get; set; }
    }
}
