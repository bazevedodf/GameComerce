using static GameCommerce.Persistencia.GeralPersist;

namespace GameCommerce.Persistencia
{
    public class GeralPersist
    {
        public class GeralPersist : IGeralPersist
        {
            private readonly RoletaContext _context;

            public GeralPersist(RoletaContext context)
            {
                _context = context;
            }

            public void Add<T>(T entity) where T : class
            {
                _context.Add(entity);
            }
            public void Update<T>(T entity) where T : class
            {
                _context.Update(entity);
            }

            public void Delete<T>(T entity) where T : class
            {
                _context.Remove(entity);
            }

            public void DeleteRange<T>(T[] entityArray) where T : class
            {
                _context.RemoveRange(entityArray);
            }

            public async Task<bool> SaveChangeAsync()
            {
                return (await _context.SaveChangesAsync()) > 0;
            }

        }
    }
}
