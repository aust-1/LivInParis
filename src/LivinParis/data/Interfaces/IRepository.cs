namespace LivInParisRoussilleTeynier.Data.Interfaces;

/// <summary>
/// Defines basic CRUD operations for an entity.
/// </summary>
/// <typeparam name="T">Entity type.</typeparam>
public interface IRepository<T>
    where T : class
{
    /// <summary>
    /// Adds a new entity.
    /// </summary>
    /// <param name="entity">Entity to add.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    Task AddAsync(T entity);

    /// <summary>
    /// Retrieves entities that match a given predicate.
    /// </summary>
    /// <param name="predicate">Predicate to filter entities.</param>
    /// <returns>Filtered entities.</returns>
    Task<IEnumerable<T>> ReadAsync(Func<T, bool>? predicate = null);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">Entity to update.</param>
    void Update(T entity);

    /// <summary>
    /// Deletes an entity.
    /// </summary>
    /// <param name="entity">Entity to delete.</param>
    void Delete(T entity);

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    Task SaveChangesAsync();

    /// <summary>
    /// Retrieves all entities.
    /// </summary>
    /// <returns>All entities.</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Finds an entity by its primary key.
    /// </summary>
    /// <param name="id">Primary key of the entity.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    Task<T?> GetByIdAsync(int id);
}
