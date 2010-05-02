### Fluent NHibernate in a Nutshell

Fluent NHibernate offers an alternative to NHibernate's standard XML mapping files. Rather than writing XML documents (.hbm.xml files), Fluent NHibernate lets you write mappings in strongly typed C# code. This allows for easy refactoring, improved readability and more concise code.

Fluent NHibernate also has several other tools, including:

* Auto mappings - where mappings are inferred from the design of your entities
* Persistence specification testing - round-trip testing for your entities, without ever having to write a line of CRUD
* Full application configuration with our Fluent configuration API
* Database configuration - fluently configure your database in code
* Fluent NHibernate is external to the NHibernate Core, but is fully compatible with NHibernate version 2.1, and is experimentally compatible with NHibernate trunk.

### Background

NHibernate is an Object Relational Mapping framework, which (as ORM states) maps between relational data and objects. It defines it's mappings in an XML format called HBM, each class has a corresponding HBM XML file that maps it to a particular structure in the database. It's these mapping files that Fluent NHibernate provides a replacement for.

**Why replace HBM.XML?** While the separation of code and XML is nice, it can lead to several undesirable situations.

Due to XML not being evaluated by the compiler, you can rename properties in your classes that aren't updated in your mappings; in this situation, you wouldn't find out about the breakage until the mappings are parsed at runtime.
XML is verbose; NHibernate has gradually reduced the mandatory XML elements, but you still can't escape the verbosity of XML.
Repetitive mappings - NHibernate HBM mappings can become quite verbose if you find yourself specifying the same rules over again. For example if you need to ensure all string properties mustn't be nullable and should have a length of 1000, and all ints must have a default value of -1.
How does Fluent NHibernate counter these issues? It does so by moving your mappings into actual code, so they're compiled along with the rest of your application; rename refactorings will alter your mappings just like they should, and the compiler will fail on any typos. As for the repetition, Fluent NHibernate has a conventional configuration system, where you can specify patterns for overriding naming conventions and many other things; you set how things should be named once, then Fluent NHibernate does the rest.

### Simple example

Here's a simple example so you know what you're getting into.

*Traditional HBM XML mapping*

    <?xml version="1.0" encoding="utf-8" ?>  
    <hibernate-mapping xmlns="urn:nhibernate-mapping-2.2"  
      namespace="QuickStart" assembly="QuickStart">  
     
      <class name="Cat" table="Cat">  
        <id name="Id">  
          <generator class="identity" />  
        </id>  
     
        <property name="Name">  
          <column name="Name" length="16" not-null="true" />  
        </property>  
        <property name="Sex" />  
        <many-to-one name="Mate" />  
        <bag name="Kittens">  
          <key column="mother_id" />  
            <one-to-many class="Cat" />  
          </bag>  
      </class>  
    </hibernate-mapping>


*Fluent NHibernate equivalent*

    public class CatMap : ClassMap<Cat>
    {
      public CatMap()
      {
        Id(x => x.Id);
        Map(x => x.Name)
          .Length(16)
          .Not.Nullable();
        Map(x => x.Sex);
        References(x => x.Mate);
        HasMany(x => x.Kittens);
      }
    }

