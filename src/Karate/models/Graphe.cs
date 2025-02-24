using System.Drawing;

namespace Karate.models;

public class Graphe
{
    #region Fields

    /// <summary>
    /// Liste des noeuds du graphe.
    /// </summary>
    private SortedSet<Noeud> noeuds;

    /// <summary>
    /// Liste des liens du graphe.
    /// </summary>
    private List<Lien> liens;
    
    /// <summary>
    /// Indique si le graphe est orienté (pour la matrice d'adjacence).
    /// </summary>
    private readonly bool estOriente;

    /// <summary>
    /// Liste d'adjacence : dictionnaire associant chaque noeud à la liste de ses voisins.
    /// Clé : Id du noeud
    /// Valeur : Liste des noeuds adjacents
    /// </summary>
    private SortedDictionary<int, SortedSet<int>> listeAdjacence;

    /// <summary>
    /// Matrice d'adjacence : tableau 2D où la case [i, j] indique la présence (ou le poids) d'un lien entre i et j.
    /// </summary>
    private double[,]? matriceAdjacence;
    
    #endregion Fields
    
    #region Constructors

    /// <summary>
    /// Constructeur du graphe.
    /// </summary>
    /// <param name="estOriente">Spécifie si le graphe est orienté.</param>
    public Graphe(bool estOriente = false)
    {
        this.noeuds = new SortedSet<Noeud>();
        this.liens = new List<Lien>();
        this.estOriente = estOriente;
        this.listeAdjacence = new SortedDictionary<int, SortedSet<int>>();
        this.matriceAdjacence = null;
    }
    
    #endregion Constructors

    #region Properties

    public double[,]? MatriceAdjacence
    {
        get
        {
            ConstruireMatriceAdjacence();
            return matriceAdjacence;
        }
    }
    
    public int Ordre
    {
        get { return noeuds.Count; }
    }
    
    public int Taille
    {
        get { return liens.Count; }
    }
    
    public bool EstOriente
    {
        get { return estOriente; }
    }
    
    public bool EstPondere
    {
        get { return liens.Any(lien => lien.Poids != 1.0); }
    }
    
    #endregion Properties
    
    #region Methods

    /// <summary>
    /// Ajoute un noeud au graphe.
    /// </summary>
    /// <param name="noeud">Noeud à ajouter.</param>
    public void AjouterNoeud(Noeud noeud)
    {
        noeuds.Add(noeud);
        if (!listeAdjacence.ContainsKey(noeud.Id))
        {
            listeAdjacence[noeud.Id] = new SortedSet<int>();
        }
    }

    /// <summary>
    /// Ajoute un lien entre deux noeuds dans le graphe.
    /// </summary>
    /// <param name="lien">Lien à ajouter.</param>
    public void AjouterLien(Lien lien)
    {
        liens.Add(lien);
        
        int idSource = lien.Source.Id;
        int idCible = lien.Cible.Id;
        
        if (!listeAdjacence.ContainsKey(idSource))
        {
            listeAdjacence[idSource] = new SortedSet<int>();
        }
        if (!listeAdjacence.ContainsKey(idCible))
        {
            listeAdjacence[idCible] = new SortedSet<int>();
        }

        listeAdjacence[idSource].Add(idCible);
        if (!estOriente)
        {
            listeAdjacence[idCible].Add(idSource);
        }
    }

    /// <summary>
    /// Construit la matrice d'adjacence du graphe.
    /// </summary>
    public void ConstruireMatriceAdjacence()
    {
        int n = noeuds.Count;
        matriceAdjacence = new double[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                matriceAdjacence[i, j] = 0.0; 
            }
        }
        
        foreach(Lien lien in liens)
        {
            int i = lien.Source.Id;
            int j = lien.Cible.Id;
            double p = lien.Poids;
            
            matriceAdjacence[i, j] = p;
            if (!estOriente)
            {
                matriceAdjacence[j, i] = p;
            }
        }
    }
    
    #endregion Methods
    
    #region Parcours

    /// <summary>
    /// Effectue un parcours en largeur (BFS) à partir du noeud de départ.
    /// Retourne la liste des Id de noeuds dans l'ordre de visite.
    /// </summary>
    public List<int> BFS(int idDepart)
    {
        var resultat = new List<int>();
        var file = new Queue<int>();
        var visites = new HashSet<int>();

        file.Enqueue(idDepart);
        visites.Add(idDepart);

        while (file.Count > 0)
        {
            int courant = file.Dequeue();
            resultat.Add(courant);

            foreach(int voisin in listeAdjacence[courant])
            {
                if (!visites.Contains(voisin))
                {
                    visites.Add(voisin);
                    file.Enqueue(voisin);
                }
            }
        }

        return resultat;
    }
    
    /// <summary>
    /// Effectue un parcours en profondeur (DFS) récursif à partir du noeud de départ.
    /// Retourne la liste des Id de noeuds dans l'ordre de visite.
    /// </summary>
    public List<int> DFSRecursif(int idDepart)
    {
        var visites = new HashSet<int>();
        var resultat = new List<int>();
        DFSUtil(idDepart, visites, resultat);
        return resultat;
    }
    
    public List<int> DFSRecursif(string nomDepart)
    {
        int idDepart = Noeud.ObtientIdDepuisNom(nomDepart);
        if (idDepart == -1)
        {
            throw new ArgumentException($"Noeud {nomDepart} introuvable.");
        }
        return DFSRecursif(idDepart);
    }

    private void DFSUtil(int noeud, HashSet<int> visites, List<int> resultat)
    {
        visites.Add(noeud);
        resultat.Add(noeud);

        foreach(int voisin in listeAdjacence[noeud])
        {
            if (!visites.Contains(voisin))
            {
                DFSUtil(voisin, visites, resultat);
            }
        }
    }
    
    /// <summary>
    /// Effectue un parcours en profondeur (DFS) itératif à partir du noeud de départ.
    /// </summary>
    public List<int> DFSIteratif(int idDepart)
    {
        var resultat = new List<int>();
        var pile = new Stack<int>();
        var visites = new HashSet<int>();

        pile.Push(idDepart);

        while (pile.Count > 0)
        {
            int courant = pile.Pop();
            if (!visites.Contains(courant))
            {
                visites.Add(courant);
                resultat.Add(courant);

                foreach(int voisin in listeAdjacence[courant])
                {
                    if (!visites.Contains(voisin))
                    {
                        pile.Push(voisin);
                    }
                }
            }
        }

        return resultat;
    }
    
    public List<int> DFSIteratif(string nomDepart)
    {
        int idDepart = Noeud.ObtientIdDepuisNom(nomDepart);
        if (idDepart == -1)
        {
            throw new ArgumentException($"Noeud {nomDepart} introuvable.");
        }
        return DFSIteratif(idDepart);
    }
    
    #endregion Parcours
    
    #region Propriétés et méthodes pour les algorithmes de graphe
    
    public bool EstConnexe()
    {
        if (!estOriente)
        {
            int n = noeuds.Count;
            return BFS(0).Count == n;
        }
        
        throw new InvalidOperationException("Algorithme non implémenté pour les graphes orientés.");
    }
    
    /// <summary>
    /// Détecte la présence de cycle simple ou de circuit dans un graphe.
    /// </summary>
    /// <returns> <c>true</c> si un cycle existe, <c>false</c> sinon.</returns>
    public bool DetecterCircuitEtCycleSimple()
    {
        var visites = new HashSet<int>();

        foreach (int noeudId in noeuds.Select(noeud => noeud.Id))
        {
            if (!visites.Contains(noeudId) && DFSDetectCycle(noeudId, -1, visites))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// DFS pour la détection de cycle dans un graphe non orienté.
    /// parent = -1 pour indiquer qu'il n'y a pas de parent pour la racine.
    /// </summary>
    private bool DFSDetectCycle(int courant, int parent, HashSet<int> visites)
    {
        visites.Add(courant);
    
        foreach(int voisin in listeAdjacence[courant])
        {
            if (!visites.Contains(voisin))
            {
                if (DFSDetectCycle(voisin, courant, visites))
                {
                    return true;
                }
            }
            else if (voisin != parent)
            {
                return true;
            }
        }

        return false;
    }
    
    #endregion Propriétés et méthodes pour les algorithmes de graphe

    public void DessinerGraphe(string nomFichier = "graphe.png")
    {
        const int largeur = 800;
        const int hauteur = 600;
        using (Bitmap bitmap = new Bitmap(largeur, hauteur))
        using (Graphics g = Graphics.FromImage(bitmap))
        {
            g.Clear(Color.White);

            // TODO : Placer les noeuds dans un cercle ou un layout spécifique
            // Ex: calculer la position (x, y) de chaque noeud
            int n = noeuds.Count;
            double angleStep = 2 * Math.PI / n;
            int rayon = 200;
            Point center = new Point(largeur / 2, hauteur / 2);

            Dictionary<int, Point> positions = new Dictionary<int, Point>();

            int i = 0;
            foreach (int noeudId in noeuds.Select(noeud => noeud.Id))
            {
                double angle = i * angleStep;
                int x = center.X + (int)(rayon * Math.Cos(angle));
                int y = center.Y + (int)(rayon * Math.Sin(angle));
                positions[noeudId] = new Point(x, y);
                i++;
            }

            // Dessiner les liens
            Pen penLien = new Pen(Color.Gray, 2);
            foreach(var kvp in listeAdjacence)
            {
                int idSource = kvp.Key;
                foreach(int idCible in kvp.Value)
                {
                    // Pour éviter de dessiner deux fois la même arête en non-orienté,
                    // on peut dessiner uniquement si idSource < idCible
                    if (!estOriente && idSource < idCible)
                    {
                        g.DrawLine(penLien, positions[idSource], positions[idCible]);
                    }
                    else if (estOriente)
                    {
                        g.DrawLine(penLien, positions[idSource], positions[idCible]);

                        const int arrowSize = 5;
                        double angle = Math.Atan2(positions[idCible].Y - positions[idSource].Y, positions[idCible].X - positions[idSource].X);
                        PointF arrowPoint1 = new PointF(
                            (float)(positions[idCible].X - arrowSize * Math.Cos(angle - Math.PI / 6)),
                            (float)(positions[idCible].Y - arrowSize * Math.Sin(angle - Math.PI / 6))
                        );
                        PointF arrowPoint2 = new PointF(
                            (float)(positions[idCible].X - arrowSize * Math.Cos(angle + Math.PI / 6)),
                            (float)(positions[idCible].Y - arrowSize * Math.Sin(angle + Math.PI / 6))
                        );
                        g.DrawLine(penLien, positions[idCible], arrowPoint1);
                        g.DrawLine(penLien, positions[idCible], arrowPoint2);
                    }
                }
            }

            // Dessiner les noeuds
            Brush brushNoeud = Brushes.LightBlue;
            foreach(int noeudId in noeuds.Select(noeud => noeud.Id))
            {
                Point p = positions[noeudId];
                int size = 20;
                g.FillEllipse(brushNoeud, p.X - size/2, p.Y - size/2, size, size);
                g.DrawEllipse(Pens.Black, p.X - size/2, p.Y - size/2, size, size);

                // Dessiner l'id du noeud
                string text = noeudId.ToString();
                var textSize = g.MeasureString(text, SystemFonts.DefaultFont);
                g.DrawString(text, SystemFonts.DefaultFont, Brushes.Black,
                             p.X - textSize.Width/2, p.Y - textSize.Height/2);
            }

            // Sauvegarder l'image
            bitmap.Save(nomFichier);
        }
    }
    
            /// <summary>
        /// Lit un fichier .mtx et construit un graphe (non orienté ou orienté selon paramètre).
        /// Format attendu :
        ///    % ... (commentaires)
        ///    34 34 78  (n, n, nb_arêtes)
        ///    2 1
        ///    3 1
        ///    ...
        /// </summary>
    public static Graphe LireFichierMtx(string filePath, bool estOriente)
    {
        Graphe graphe = new Graphe(estOriente);
        bool premiereLigneTrouvee = false;
        int nbNoeuds = 0, nbLiensAttendus = 0, nbLiensLus = 0;
        
        var dicNoeuds = new Dictionary<int, Noeud>();

        foreach(string line in File.ReadLines(filePath))
        {
            if (line.StartsWith('%') || string.IsNullOrWhiteSpace(line))
                continue;
            
            if (!premiereLigneTrouvee)
            {
                (nbNoeuds, nbLiensAttendus) = ParseFirstLine(line);
                premiereLigneTrouvee = true;
                InitializeNodes(nbNoeuds, dicNoeuds, graphe);
            }
            else
            {
                if (TryParseLink(line, dicNoeuds, out Lien lien))
                {
                    graphe.AjouterLien(lien);
                    nbLiensLus++;
                }
            }
        }

        Console.WriteLine($"Fichier .mtx lu. {nbNoeuds} noeuds, {nbLiensLus} liens (attendus: {nbLiensAttendus}).");
        return graphe;
    }

    private static (int, int) ParseFirstLine(string line)
    {
        string[] parts = line.Split(new char[]{' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 3)
        {
            throw new Exception("Format .mtx invalide : première ligne incomplète.");
        }
        
        int nbNoeuds = int.Parse(parts[0]);
        int nbLiensAttendus = int.Parse(parts[2]);
        return (nbNoeuds, nbLiensAttendus);
    }

    private static void InitializeNodes(int nbNoeuds, Dictionary<int, Noeud> dicNoeuds, Graphe graphe)
    {
        for (int i = 1; i <= nbNoeuds; i++)
        {
            Noeud noeud = new Noeud($"Node{i}");
            dicNoeuds[i] = noeud;
            graphe.AjouterNoeud(noeud);
        }
    }

    private static bool TryParseLink(string line, Dictionary<int, Noeud> dicNoeuds, out Lien lien)
    {
        lien = null;
        string[] parts = line.Split(new char[]{' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2) 
            return false;

        int sourceId = int.Parse(parts[0]);
        int cibleId = int.Parse(parts[1]);

        if (!dicNoeuds.ContainsKey(sourceId) || !dicNoeuds.ContainsKey(cibleId))
        {
            Console.WriteLine($"Avertissement : Id {sourceId} ou {cibleId} hors [1..{dicNoeuds.Count}]");
            return false;
        }

        lien = new Lien(dicNoeuds[sourceId], dicNoeuds[cibleId]);
        return true;
    }
}
