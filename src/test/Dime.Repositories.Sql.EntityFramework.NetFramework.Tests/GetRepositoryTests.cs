using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Effort.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.Sql.EntityFramework.NetFramework.Tests
{
    [TestClass]
    public partial class RepositoryTests
    {
        [TestMethod]
        public void Repository_FindAll_Contains_ShouldFindMatches()
        {
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();

            using (BloggingContext context = new BloggingContext(connection))
            {
                context.Blogs.Add(new Blog { Url = "http://sample.com/cats" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/catfish" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/dogs" });
                context.SaveChanges();
            }

            IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(connection), new RepositoryConfiguration());
            IEnumerable<Blog> result = repo.FindAll(x => x.Url.Contains("cat"));
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task Repository_FindAllAsync_Contains_ShouldFindMatches()
        {
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();

            using (BloggingContext context = new BloggingContext(connection))
            {
                context.Blogs.Add(new Blog { Url = "http://sample.com/cats" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/catfish" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/dogs" });
                await context.SaveChangesAsync();
            }

            IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(connection), new RepositoryConfiguration());
            IEnumerable<Blog> result = await repo.FindAllAsync(x => x.Url.Contains("cat"));
            Assert.AreEqual(2, result.Count());
        }
    }
}