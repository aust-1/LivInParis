namespace Karate.models;

/// <summary>
/// Représente un noeud (sommet) dans le graphe.
/// </summary>
public class Noeud : IComparable<Noeud>
{
    #region Fields

    private static SortedDictionary<string, int> existingNodes = new ();

    /// <summary>
    /// Identifiant unique du noeud.
    /// </summary>
    private readonly int id;

    /// <summary>
    /// Nom du noeud.
    /// </summary>
    private readonly string nom;
    
    #endregion Fields

    #region Constructors
    
    /// <summary>
    /// Constructeur principal.
    /// </summary>
    /// <param name="nom">Nom du noeud.</param>
    public Noeud(string nom = "")
    {
        int CompteurId = existingNodes.Count;
        if (!existingNodes.TryAdd(nom, CompteurId))
        {
            throw new ArgumentException($"Un noeud avec le nom {nom} existe déjà.");
        }
        
        this.nom = nom;
        this.id = CompteurId;
    }
    
    #endregion Constructors

    #region Properties

    /// <summary>
    /// Obtient l'identifiant du noeud.
    /// </summary>
    public int Id
    {
        get { return id; }
    }

    #endregion Properties
    
    #region Methods
    
    /// <summary>
    /// Retourne une chaîne qui représente le noeud.
    /// </summary>
    /// <returns>Une chaîne de caractères qui représente le noeud.</returns>
    public override string ToString()
    {
        return $"Noeud: Id={id}, Nom={nom}";
    }

    public static int ObtientIdDepuisNom(string nomAChercher)
    {
        if (existingNodes.TryGetValue(nomAChercher, out int id))
        {
            return id;
        }

        return -1;
    }
    
    public static string ObtientNomDepuisId(int idAChercher)
    {
        return existingNodes.FirstOrDefault(x => x.Value == idAChercher).Key;
    }
    
    #endregion Methods
    
    # region IComparable<Noeud> implementation
    
    public int CompareTo(object? other)
    {
        if (other is Noeud noeud)
        {
            return id.CompareTo(noeud.Id);
        }

        return 1;
    }
    public int CompareTo(Noeud? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }
        if (other is null)
        {
            return 1;
        }

        return id.CompareTo(other.Id);
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is Noeud other && other.Id==this.id;
    }
    public override int GetHashCode()
    {
        return id;
    }
    public static int Compare(Noeud left, Noeud right)
    {
        return left.CompareTo(right);
    }
    public static bool operator == (Noeud left, Noeud right)
    {
        if (object.ReferenceEquals(left, null))
        {
            return object.ReferenceEquals(right, null);
        }
        return left.Equals(right);
    }
    public static bool operator > (Noeud left, Noeud right)
    {
        return Compare(left, right) > 0;
    }
    public static bool operator < (Noeud left, Noeud right)
    {
        return Compare(left, right) < 0;
    }
    public static bool operator != (Noeud left, Noeud right)
    {
        return !(left == right);
    }
    public static bool operator >= (Noeud left, Noeud right)
    {
        return Compare(left, right) >= 0;
    }
    public static bool operator <= (Noeud left, Noeud right)
    {
        return Compare(left, right) <= 0;
    }

    #endregion IComparable<Noeud> implementation
}
