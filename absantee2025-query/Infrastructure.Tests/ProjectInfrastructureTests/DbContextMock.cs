using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Tests.Helpers
{
    public static class DbContextMock
    {
        // Cria um mock de DbSet<T> com dados fornecidos
        public static DbSet<T> GetQueryableMockDbSet<T>(params T[] sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            dbSetMock.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) =>
            {
                var list = queryable.ToList();
                list.Add(s);
                queryable = list.AsQueryable();
            });

            return dbSetMock.Object;
        }

        // Cria um contexto com dados reais usando InMemoryDatabase (ideal para testes async)
        public static AbsanteeContext CreateWithData(params object[] entities)
        {
            var options = new DbContextOptionsBuilder<AbsanteeContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AbsanteeContext(options);

            foreach (var entity in entities)
            {
                context.Add(entity);
            }

            context.SaveChanges();
            return context;
        }
    }
}
