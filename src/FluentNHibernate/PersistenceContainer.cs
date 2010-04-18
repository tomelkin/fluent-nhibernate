using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using FluentNHibernate.Conventions;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using NHibernate.Cfg;

namespace FluentNHibernate
{
    //public class PersistenceContainer
    //{
    //    IEnumerable<HibernateMapping> compiledMappings;
    //    readonly List<IMappingModelVisitor> visitors = new List<IMappingModelVisitor>();
    //    readonly ValidationVisitor validationVisitor;
    //    readonly List<IProviderSource> sources = new List<IProviderSource>();

    //    public PersistenceContainer()
    //    {
    //        Conventions = new ConventionsCollection();

    //        //visitors.Add(new SeparateSubclassVisitor(subclassProviders));
    //        //visitors.Add(new ComponentReferenceResolutionVisitor(componentProviders));
    //        visitors.Add(new ComponentColumnPrefixVisitor());
    //        visitors.Add(new BiDirectionalManyToManyPairingVisitor(BiDirectionalManyToManyPairer));
    //        visitors.Add(new ManyToManyTableNameVisitor());
    //        visitors.Add(new ConventionVisitor(new ConventionFinder(Conventions)));
    //        visitors.Add((validationVisitor = new ValidationVisitor()));
    //    }

    //    public ConventionsCollection Conventions { get; set; }
    //    public bool MergeMappings { get; set; }
    //    public PairBiDirectionalManyToManySidesDelegate BiDirectionalManyToManyPairer { get; set; }

    //    public bool ValidationEnabled
    //    {
    //        get { return validationVisitor.Enabled; }
    //        set { validationVisitor.Enabled = value; }
    //    }

    //    //public IEnumerable<IMappingProvider> Classes
    //    //{
    //    //    get { return classProviders; }
    //    //}

    //    public void Add(IProviderSource source)
    //    {
    //        sources.Add(source);
    //    }

    //    private void ApplyVisitors(IEnumerable<HibernateMapping> mappings)
    //    {
    //        foreach (var visitor in visitors)
    //            visitor.Visit(mappings);
    //    }

    //    public bool ContainsMapping(Type type)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Configure(Configuration cfg)
    //    {
    //        EnsureMappingsBuilt();

    //        foreach (var mapping in compiledMappings.Where(m => m.Classes.Count() == 0))
    //        {
    //            var serializer = new MappingXmlSerializer();
    //            XmlDocument document = serializer.Serialize(mapping);
    //            cfg.AddDocument(document);
    //        }

    //        foreach (var mapping in compiledMappings.Where(m => m.Classes.Count() > 0))
    //        {
    //            var serializer = new MappingXmlSerializer();
    //            XmlDocument document = serializer.Serialize(mapping);

    //            if (cfg.GetClassMapping(mapping.Classes.First().Type) == null)
    //                cfg.AddDocument(document);
    //        }
    //    }

    //    public void WriteMappingsTo(string folder)
    //    {
    //        WriteMappingsTo(mapping => new XmlTextWriter(Path.Combine(folder, DetermineMappingFileName(mapping)), Encoding.Default), true);
    //    }

    //    public void WriteMappingsTo(TextWriter writer)
    //    {
    //        WriteMappingsTo(_ => new XmlTextWriter(writer), false);
    //    }

    //    void WriteMappingsTo(Func<HibernateMapping, XmlTextWriter> writerBuilder, bool shouldDispose)
    //    {
    //        EnsureMappingsBuilt();

    //        foreach (HibernateMapping mapping in compiledMappings)
    //        {
    //            var serializer = new MappingXmlSerializer();
    //            var document = serializer.Serialize(mapping);

    //            XmlTextWriter xmlWriter = null;

    //            try
    //            {
    //                xmlWriter = writerBuilder(mapping);
    //                xmlWriter.Formatting = Formatting.Indented;
    //                document.WriteTo(xmlWriter);
    //            }
    //            finally
    //            {
    //                if (shouldDispose && xmlWriter != null)
    //                    xmlWriter.Close();
    //            }
    //        }
    //    }

    //    string DetermineMappingFileName(HibernateMapping mapping)
    //    {
    //        if (MergeMappings)
    //            return GetMappingFileName();

    //        if (mapping.Classes.Count() > 0)
    //            return mapping.Classes.First().Type.FullName + ".hbm.xml";

    //        return "filter-def." + mapping.Filters.First().Name + ".hbm.xml";
    //    }

    //    void EnsureMappingsBuilt()
    //    {
    //        if (compiledMappings != null) return;

    //        compiledMappings = BuildMappings();
    //    }

    //    protected virtual string GetMappingFileName()
    //    {
    //        return "FluentMappings.hbm.xml";
    //    }
    //}
}