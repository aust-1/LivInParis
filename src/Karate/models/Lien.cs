namespace Karate.models;

/// <summary>
/// Représente un lien (arête ou arc) entre deux noeuds dans le graphe.
/// </summary>
public class Lien
{
    #region Fields

    /// <summary>
    /// Noeud source.
    /// </summary>
    private readonly Noeud source;

    /// <summary>
    /// Noeud cible (dans un graphe non orienté, c'est simplement l'autre extrémité).
    /// </summary>
    private readonly Noeud cible;

    /// <summary>
    /// Poids éventuel (si le graphe est pondéré).
    /// </summary>
    private readonly double poids;
    
    #endregion Fields

    #region Constructors
    
    /// <summary>
    /// Constructeur par défaut.
    /// </summary>
    /// <param name="source">Le noeud source.</param>
    /// <param name="cible">Le noeud cible.</param>
    /// <param name="poids">Le poids du lien.</param>
    public Lien(Noeud source, Noeud cible, double poids = 1.0)
    {
        this.source = source;
        this.cible = cible;
        this.poids = poids;
    }
    
    #endregion Constructors
    
    #region Properties
    
    public Noeud Source
    {
        get { return source; }
    }
    
    public Noeud Cible
    {
        get { return cible; }
    }
    
    public double Poids
    {
        get { return poids; }
    }
    
    #endregion Properties

    #region Methods
    
    /// <summary>
    /// Retourne une représentation textuelle du lien.
    /// </summary>
    /// <returns>Une chaîne de caractères représentant le lien.</returns>
    public override string ToString()
    {
        return $"Lien: {source.Id} --({poids})--> {cible.Id}";
    }
    
    #endregion Methods
}
