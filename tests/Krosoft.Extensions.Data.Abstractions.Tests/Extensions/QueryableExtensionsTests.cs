using System.Linq.Expressions;
using Krosoft.Extensions.Core.Models.Exceptions;
using Krosoft.Extensions.Data.Abstractions.Extensions;
using Krosoft.Extensions.Data.Abstractions.Helpers;
using Krosoft.Extensions.Samples.Library.Models.Entities;

namespace Krosoft.Extensions.Data.Abstractions.Tests.Extensions;

[TestClass]
public class QueryableExtensionsTests
{
    [TestMethod]
    public void Filter_EmptyItems_ReturnsOriginalQuery()
    {
        var data = GetQueryable();

        var func = GetFunc();

        var query = data.Filter(new List<int>(), func);

        Check.That(query).IsEqualTo(data);
    }

    [TestMethod]
    public void Filter_Item_NoItem()
    {
        var data = GetQueryable();
        int? requestId = null;
        var query = data.Filter(requestId, id => c => c.Id == id);

        Check.That(query.Expression.ToString()).IsEqualTo(data.Expression.ToString());
    }

    [TestMethod]
    public void Filter_Item_NoMatch()
    {
        var data = GetQueryable();
        var requestId = 42;

        var query = data.Filter<SampleEntity, int>(requestId, id => c => c.Id <= id);

        Check.That(query).HasSize(4);
        Check.That(query.Select(x => x.Id)).ContainsExactly(1, 2, 3, 4);
        Check.That(query.Select(x => x.Name)).ContainsExactly("Item1", "Item2", "Item3", "Item4");
    }

    [TestMethod]
    public void Filter_Item_Ok()
    {
        var data = GetQueryable();
        var requestId = 2;

        var query = data.Filter<SampleEntity, int>(requestId, id => c => c.Id <= id);

        Check.That(query).HasSize(2);
        Check.That(query.Select(x => x.Id)).ContainsExactly(1, 2);
        Check.That(query.Select(x => x.Name)).ContainsExactly("Item1", "Item2");
    }

    [TestMethod]
    public void Filter_Mandatory()
    {
        var data = GetQueryable();
        var func = GetFunc();
        var query = data.Filter(new List<int> { 2, 3 }, func, true);

        Check.That(query).HasSize(2);
        Check.That(query.Select(x => x.Id)).ContainsExactly(2, 3);
        Check.That(query.Select(x => x.Name)).ContainsExactly("Item2", "Item3");
    }

    [TestMethod]
    public void Filter_Mandatory_ItemsEmpty()
    {
        var data = GetQueryable();
        var func = GetFunc();
        var query = data.Filter(new List<int>(), func, true);

        Check.That(query).HasSize(0);
    }

    [TestMethod]
    public void Filter_NoFunc()
    {
        var data = GetQueryable();

        Check.ThatCode(() => data.Filter<SampleEntity, int>(null!, null!, true))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'items' n'est pas renseignée.");
    }

    [TestMethod]
    public void Filter_NoItems()
    {
        var data = GetQueryable();

        Check.ThatCode(() => data.Filter(new List<int>(), null!, true))
             .Throws<KrosoftTechnicalException>()
             .WithMessage("La variable 'func' n'est pas renseignée.");
    }

    [TestMethod]
    public void Filter_NoMandatory()
    {
        var data = GetQueryable();
        var func = GetFunc();
        var query = data.Filter(new List<int> { 2, 3 }, func);

        Check.That(query).HasSize(2);
        Check.That(query.Select(x => x.Id)).ContainsExactly(2, 3);
        Check.That(query.Select(x => x.Name)).ContainsExactly("Item2", "Item3");
    }

    [TestMethod]
    public void Filter_NoMandatory_ItemsEmpty()
    {
        var data = GetQueryable();
        var func = GetFunc();
        var query = data.Filter(new List<int>(), func);

        Check.That(query).HasSize(4);
        Check.That(query.Select(x => x.Id)).ContainsExactly(1, 2, 3, 4);
        Check.That(query.Select(x => x.Name)).ContainsExactly("Item1", "Item2", "Item3", "Item4");
    }

    [TestMethod]
    public void Filter_Predciates_And_Ok()
    {
        var data = GetQueryable();

        var query = data.Filter(PredicateHelper.Or<SampleEntity, int>(new List<int> { 2 }, id => c => c.Id == id))
                        .Filter(PredicateHelper.Or<SampleEntity, int>(new List<int> { 3 }, id => c => c.Id == id))
            ;

        //On veut id 2 et 3 => impossible.
        Check.That(query).IsEmpty();
    }

    [TestMethod]
    public void Filter_Predciates_Or_Ok()
    {
        var data = GetQueryable();

        var query = data.Filter(PredicateHelper.Or<SampleEntity, int>(new List<int> { 2 }, id => c => c.Id == id),
                                PredicateHelper.Or<SampleEntity, int>(new List<int> { 3 }, id => c => c.Id == id))
            ;

        Check.That(query).HasSize(2);
        Check.That(query.Select(x => x.Id)).ContainsExactly(2, 3);
        Check.That(query.Select(x => x.Name)).ContainsExactly("Item2", "Item3");
    }

    [TestMethod]
    public void Filter_WithDuplicateItems_FiltersQueryOnce()
    {
        var data = GetQueryable();

        var items = new List<int>
        {
            1,
            2,
            3,
            1,
            2
        };
        var func = GetFunc();

        var query = data.Filter(items, func);

        Check.That(query).HasSize(3);
        Check.That(query.Select(x => x.Id)).ContainsExactly(1, 2, 3);
        Check.That(query.Select(x => x.Name)).ContainsExactly("Item1", "Item2", "Item3");
    }

    [TestMethod]
    public void Filter_WithItemsAndFunc_FiltersQuery()
    {
        var data = GetQueryable();
        var items = new List<int>
        {
            1,
            2
        };
        var func = GetFunc();

        var query = data.Filter(items, func);

        Check.That(query).HasSize(2);
        Check.That(query.Select(x => x.Id)).ContainsExactly(1, 2);
        Check.That(query.Select(x => x.Name)).ContainsExactly("Item1", "Item2");
    }

    private static Func<int, Expression<Func<SampleEntity, bool>>> GetFunc() => id => entity => entity.Id == id;

    private static IQueryable<SampleEntity> GetQueryable()
    {
        var data = new List<SampleEntity>
        {
            new SampleEntity { Id = 1, Name = "Item1", Description = "Developer" },
            new SampleEntity { Id = 2, Name = "Item2", Description = "PO" },
            new SampleEntity { Id = 3, Name = "Item3", Description = "Designer" },
            new SampleEntity { Id = 4, Name = "Item4", Description = "Manager" }
        }.AsQueryable();
        return data;
    }

    [TestMethod]
    public void Search_WithEmptyTerm_ReturnsOriginalQuery()
    {
      
        var query = new List<SampleEntity>().AsQueryable();
        var searchTerm = string.Empty;
        var selectors = new Expression<Func<SampleEntity, string?>>[] { p => p.Name, p => p.Description };

    
        var result = query.Search(searchTerm, selectors);

        
        Check.That(result).IsEqualTo(query);
    }

    [TestMethod]
    public void Search_WithNullTerm_ReturnsOriginalQuery()
    {
      
        var query = new List<SampleEntity>().AsQueryable();
        string? searchTerm = null;
        var selectors = new Expression<Func<SampleEntity, string?>>[] { p => p.Name, p => p.Description };

    
        var result = query.Search(searchTerm, selectors);

        
        Check.That(result).IsEqualTo(query);
    }

    [TestMethod]
    public void Search_WithValidTerm_FiltersQuery()
    {
      
        var persons = new List<SampleEntity>
        {
            new SampleEntity { Id = 1, Name = "John", Description = "Developer" },
            new SampleEntity { Id = 2, Name = "Jane", Description = "Designer" },
            new SampleEntity { Id = 3, Name = "Doe", Description = "Manager" }
        };

        var query = persons.AsQueryable();
        var searchTerm = "john";
        var selectors = new Expression<Func<SampleEntity, string?>>[] { p => p.Name, p => p.Description };

    
        var result = query.Search(searchTerm, selectors);

        
        Check.That(result).ContainsExactly(persons[0]);
    }

    [TestMethod]
    public void SearchAll_WithEmptyTerm_ReturnsOriginalQuery()
    {
      
        var query = new List<SampleEntity>().AsQueryable();
        var searchTerm = string.Empty;
        var selector = new Expression<Func<SampleEntity, string?>>[] { p => p.Name };

    
        var result = query.SearchAll(searchTerm, selector[0]);

        
        Check.That(result).IsEqualTo(query);
    }

    [TestMethod]
    public void SearchAll_WithNullTerm_ReturnsOriginalQuery()
    {
      
        var query = new List<SampleEntity>().AsQueryable();
        string? searchTerm = null;
        var selector = new Expression<Func<SampleEntity, string?>>[] { p => p.Name };

    
        var result = query.SearchAll(searchTerm, selector[0]);

        
        Check.That(result).IsEqualTo(query);
    }

    [TestMethod]
    public void SearchAll_WithValidTerm_FiltersQuery()
    {
      
        var persons = new List<SampleEntity>
        {
            new SampleEntity { Id = 1, Name = "John Doe" },
            new SampleEntity { Id = 2, Name = "Jane Doe" },
            new SampleEntity { Id = 3, Name = "Doe John" }
        };

        var query = persons.AsQueryable();
        var searchTerm = "john";
        var selector = new Expression<Func<SampleEntity, string?>>[] { p => p.Name };

    
        var result = query.SearchAll(searchTerm, selector[0]);

        
        Check.That(result).ContainsExactly(persons[0], persons[2]);
    }
}