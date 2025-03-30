# Rapport d'optimisation de la classe `Graph<T>`

## Initialisation

Pour initialiser le graphe du métro parisien nous pouvions utiliser le fichier `MatroParis.xlsx` fourni dans le projet en le convertisant en une structure de données adaptée. Nous avions le choix entre deux structures de données :

1) Une matrice d'adjacence
2) Une liste d'adjacence

Pour chosir entre ces deux structures, nous avons testé les deux en chargeant le graphe du métro parisien et en mesurant le temps d'exécution moyen sur 20 itérations. Nous avons obtenu ces résultats :

|       Méthode       | Temps (ms) |
|---------------------|------------|
| Matrice d'adjacence | 193        |
| Liste d'adjacence   | 199        |

Nous avons donc choisi d'utiliser la matrice d'adjacence pour sa rapidité d'initialisation.

## Pathfinding

Pour le pathfinding, nous avons implémenté l'algorithme de Dijkstra, de Bellman-Ford et de Roy-Floyd-Warshall. Pour les comparer nous avons mesuré le temps d'exécution moyen en prenant comme point de départ chacune des 330 stations du métro parisien. Nous avons ensuite calculé le temps d'exécution moyen pour chaque algorithme. Voici les résultats obtenus :

|     Algorithme     | Temps (ms) |
|--------------------|------------|
| Dijkstra           | 8          |
| Bellman-Ford       | 5          |
| Roy-Floyd-Warshall |         |
