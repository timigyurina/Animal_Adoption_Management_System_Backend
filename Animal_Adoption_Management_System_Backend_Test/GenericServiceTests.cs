using Animal_Adoption_Management_System_Backend.Configurations;
using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Models.Pagination;
using Animal_Adoption_Management_System_Backend.Services.Implementations;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Animal_Adoption_Management_System_Backend_Test
{
    public abstract class GenericServiceTests<T, TResult> where T : class where TResult : class
    {
        protected DbContextOptions<AnimalAdoptionContext> _dbContextOptions;
        protected AnimalAdoptionContext _context;
        protected readonly IMapper _mapper;
        private IGenericService<T> _genericService;

        protected static IEnumerable<TestCaseData> QueryParameterCases()
        {
            yield return new TestCaseData(1, 10);
            yield return new TestCaseData(1, 1);
            yield return new TestCaseData(2, 1);
            yield return new TestCaseData(1, 3);
            yield return new TestCaseData(2, 3);
        }

        public GenericServiceTests()
        {
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperConfiguration>()).CreateMapper();
            _dbContextOptions = new DbContextOptionsBuilder<AnimalAdoptionContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [SetUp]
        public virtual async Task SetupAsync()
        {
            _context = new AnimalAdoptionContext(_dbContextOptions);
            _genericService = new GenericService<T>(_context, _mapper);

            await _context.Set<T>().AddRangeAsync(CreateSampleValues());
            await _context.SaveChangesAsync();
        }

        [TearDown]
        public void TearDown()
        {
            _context.ChangeTracker.Clear();
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


        [Test]
        public async Task AddAsync_SearchingForAddedEntity_ReturnsTrue()
        {
            T entityToAdd = CreateSampleValue(6);
            await _genericService.AddAsync(entityToAdd);

            bool dbHasAddedValue = await _context.Set<T>()
                .FirstOrDefaultAsync(e => e.Equals(entityToAdd)) != null;

            Assert.That(dbHasAddedValue, Is.True);
        }

        [Test]
        public async Task AddAsync_SearchingForNotAddedEntity_ReturnsFalse()
        {
            T entityToAdd = CreateSampleValue(6);
            await _genericService.AddAsync(entityToAdd);

            T valueToSeek = CreateSampleValue(55);
            bool dbDoesNotHaveNotAddedValue = await _context.Set<T>()
                .FirstOrDefaultAsync(e => e.Equals(valueToSeek)) == null;

            Assert.That(dbDoesNotHaveNotAddedValue, Is.True);
        }

        [Test]
        public async Task GetAsync_SearchingForExistingEntity_ReturnsCorrectEntity()
        {
            T entityToFind = await _context.Set<T>().FindAsync(1) ?? throw new Exception("Not found");

            T requiredEntity = await _genericService.GetAsync(1);

            Assert.That(requiredEntity, Is.EqualTo(entityToFind));
        }

        [Test]
        public void GetAsync_SearchingForNonExistentEntity_ThrowsNotFoundException()
        {
            int nonExistentId = 100;
            string exceptionMessage = GetExceptionMessageForEntity(nonExistentId);

            Assert.ThrowsAsync(Is.TypeOf<NotFoundException>()
               .And.Message.EqualTo(exceptionMessage),
               async () => await _genericService.GetAsync(nonExistentId));
        }

        [Test]
        public async Task Exists_SearchingForExistingEntity_ReturnsTrue()
        {
            bool entityExists = await _genericService.Exists(1);

            Assert.That(entityExists, Is.True);
        }

        [Test]
        public async Task Exists_SearchingForNonExistentEntity_ReturnsFalse()
        {
            bool entityExists = await _genericService.Exists(11);

            Assert.That(entityExists, Is.False);
        }

        [Test]
        public async Task UpdateAsync_UpdatedEntity__IsCorrectlyUpdated()
        {
            T entityToModify = await _context.Set<T>().FindAsync(1) ?? throw new Exception("Not found");
            T modifiedEntity = ModifyEntity(entityToModify);

            await _genericService.UpdateAsync(modifiedEntity);

            T updatedEntity = await _context.Set<T>()
                .FirstAsync(e => e.Equals(modifiedEntity));

            Assert.That(modifiedEntity, Is.EqualTo(updatedEntity));
        }

        [Test]
        public async Task DeleteAsync_EntityDeleted_IsNotInDb()
        {
            await _genericService.DeleteAsync(2);
            T? deletedEntity = await _context.Set<T>().FindAsync(2);

            Assert.That(deletedEntity, Is.Null);
        }

        // GetAllAsync tests
        [Test]
        public async Task GetAllAsync_GettingAllEntitiesFromDb_ReturnsAllEntities()
        {
            IEnumerable<T> entitiesInDb = await _genericService.GetAllAsync();

            Assert.That(entitiesInDb, Is.EquivalentTo(_context.Set<T>()));
        }

        [Test]
        public async Task GetAllAsync_GettingAllEntitiesFromDbWithFilters_ReturnsFilteredEntities()
        {
            Expression<Func<T, bool>> filter = GetFilterForEntity();
            IEnumerable<T> filteredEntitiesInDb = await _genericService.GetAllAsync(filter);

            Assert.That(filteredEntitiesInDb, Is.EquivalentTo(_context.Set<T>().Where(filter)));
        }

        [Test]
        public async Task GetAllAsync_GettingAllEntitiesFromDbWithIncludeProperties_ReturnsEntities()
        {
            string includeProperty = GetIncludePropertyForEntity();

            if (includeProperty == string.Empty)
            {
                IEnumerable<T> entitiesInDb = await _genericService.GetAllAsync();
                Assert.That(entitiesInDb, Is.EquivalentTo(_context.Set<T>()));
            }
            else
            {
                IEnumerable<T> entitiesInDb = await _genericService.GetAllAsync(null, null, includeProperty);
                T[] entitiesInDbArray = entitiesInDb.ToArray();
                bool hasProp = true;

                for (int i = 0; i < entitiesInDbArray.Length; i++)
                {
                    hasProp = typeof(T).GetProperty($"{includeProperty}") != null;
                    if (!hasProp)
                        break;
                }

                Assert.That(hasProp, Is.True);
            }
        }

        [Test]
        public async Task GetAllAsync_GettingAllEntitiesFromDbOrderedByIdDesc_ReturnsOrderedEntities()
        {
            Func<IQueryable<T>, IOrderedQueryable<T>> orderByFunc = GetOrderByDescIdForEntity();
            IEnumerable<T> filteredEntitiesInDb = await _genericService.GetAllAsync(null, orderByFunc);

            bool orderingWorked = TestIfOrderingWorked(orderByFunc, filteredEntitiesInDb);
            Assert.That(orderingWorked, Is.True);
        }

        // GetAllAsync tests with paged results
        [TestCaseSource(nameof(QueryParameterCases))]
        public async Task GetAllAsyncPaged_InDestinationType_ReturnsPagedResultWithCorrectProperties(int pageNumber, int pageSize)
        {
            QueryParameters queryParameters = new()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            IQueryable<T> query = _context.Set<T>()
                    .Skip(queryParameters.StartIndex)
                    .Take(queryParameters.PageSize);

            IEnumerable<TResult> mappedResult = query.Select(q => _mapper.Map<TResult>(q));

            PagedResult<TResult> expectedResult = new()
            {
                TotalCount = await _context.Set<T>().CountAsync(),
                CurrentPage = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize,
                Items = mappedResult.ToList(),
            };

            PagedResult<TResult> actualResult = await _genericService.GetAllAsync<TResult>(queryParameters);

            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.InstanceOf<PagedResult<TResult>>());
                Assert.That(actualResult.CurrentPage, Is.EqualTo(expectedResult.CurrentPage));
                Assert.That(actualResult.PageSize, Is.EqualTo(expectedResult.PageSize));
                Assert.That(actualResult.TotalCount, Is.EqualTo(expectedResult.TotalCount));
            });
        }

        [TestCaseSource(nameof(QueryParameterCases))]
        public async Task GetAllAsyncPaged_InDestinationType_ReturnsCorrectItems(int pageNumber, int pageSize)
        {
            QueryParameters queryParameters = new()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            PagedResult<TResult> expectedResult = new()
            {
                TotalCount = await _context.Set<T>().CountAsync(),
                CurrentPage = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize,
                Items = await _context.Set<T>()
                    .Skip(queryParameters.StartIndex)
                    .Take(queryParameters.PageSize)
                    .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                    .ToListAsync(),
            };
            IEqualityComparer<TResult> equalityComparer = GetEqualityComparerForDTO();

            PagedResult<TResult> actualResult = await _genericService.GetAllAsync<TResult>(queryParameters);

            //CompareCountAndPropsOfActualResultItemsWithExpected(actualResult.Items, expectedResult.Items);
            Assert.That(actualResult, Is.Not.Null);
            Assert.That(actualResult.Items, Has.Count.EqualTo(expectedResult.Items.Count));
            for (int i = 0; i < actualResult.Items.Count; i++)
                Assert.That(actualResult.Items[i], Is.EqualTo(expectedResult.Items[i]).Using(equalityComparer));
        }

        //GetPagedAndFiltered tests
        [TestCaseSource(nameof(QueryParameterCases))]
        public async Task GetPagedAndFiltered_InDestinationType_ReturnsPagedResultWithCorrectProperties(int pageNumber, int pageSize)
        {
            QueryParameters queryParameters = new()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            IEnumerable<Expression<Func<T, bool>>> filters = GetMultipleFiltersForEntity();

            PagedResult<TResult> actualResult = await _genericService.GetPagedAndFiltered<TResult>(queryParameters, filters);

            Assert.Multiple(() =>
            {
                Assert.That(actualResult, Is.InstanceOf<PagedResult<TResult>>());
                Assert.That(actualResult.CurrentPage, Is.EqualTo(queryParameters.PageNumber));
                Assert.That(actualResult.PageSize, Is.EqualTo(queryParameters.PageSize));
            });
        }

        [TestCaseSource(nameof(QueryParameterCases))]
        public async Task GetPagedAndFiltered_InDestinationType_ReturnsCorrectItems(int pageNumber, int pageSize)
        {
            QueryParameters queryParameters = new()
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            IEnumerable<Expression<Func<T, bool>>> filters = GetMultipleFiltersForEntity();
            PagedResult<TResult> actualResult = await _genericService.GetPagedAndFiltered<TResult>(queryParameters, filters);

            TestIfFiltersWorked(actualResult.Items);
        }

        protected abstract void TestIfFiltersWorked(List<TResult> actualResultItems);
        protected abstract bool TestIfOrderingWorked(Func<IQueryable<T>, IOrderedQueryable<T>> orderByFunc, IEnumerable<T> filteredEntitiesInDb);

        protected abstract Expression<Func<T, bool>> GetFilterForEntity();
        protected abstract IEnumerable<Expression<Func<T, bool>>> GetMultipleFiltersForEntity();
        protected abstract Func<IQueryable<T>, IOrderedQueryable<T>> GetOrderByDescIdForEntity();
        protected abstract string GetIncludePropertyForEntity();
        protected abstract string GetExceptionMessageForEntity(int id);
        protected abstract IEqualityComparer<TResult> GetEqualityComparerForDTO();

        protected abstract T ModifyEntity(T entityToUpdate);

        protected abstract T CreateSampleValue(int id);
        protected abstract IEnumerable<T> CreateSampleValues();

    }
}


