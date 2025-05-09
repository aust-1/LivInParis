# Rapport d'optimisation de la classe `Graph<T>`

## Initialisation

Pour initialiser le graphe du métro parisien nous pouvions utiliser le fichier `MetroParis.xlsx` fourni dans le projet en le convertisant en une structure de données adaptée. Nous avions le choix entre deux structures de données :

1) Une matrice d'adjacence
2) Une liste d'adjacence

Pour chosir entre ces deux structures, nous avons testé les deux en chargeant le graphe du métro parisien et en mesurant le temps d'exécution moyen sur 20 itérations. Nous avons obtenu ces résultats :

|       Méthode       | Temps (ms) |
|---------------------|------------|
| Matrice d'adjacence | 11         |
| Liste d'adjacence   | 23         |

Nous avons donc choisi d'utiliser la matrice d'adjacence pour sa rapidité d'initialisation.

## Pathfinding

Pour le pathfinding, nous avons implémenté l'algorithme de Dijkstra, de Bellman-Ford et de Roy-Floyd-Warshall. Pour les comparer nous avons mesuré le temps d'exécution moyen en prenant comme point de départ chacune des 330 stations du métro parisien. Nous avons ensuite calculé le temps d'exécution moyen pour chaque algorithme. Voici les résultats obtenus :

|     Algorithme     | Temps (ms) |
|--------------------|------------|
| Dijkstra           | 5          |
| Bellman-Ford       | 4          |
| Roy-Floyd-Warshall | 198        |

Pour une meilleure comparaison, nous avons également mesuré les temps total d'exécution de Dijkstra et de Bellman-Ford sur l'ensemble du graphe. Voici les résultats obtenus :

|     Algorithme     | Temps (ms) |
|--------------------|------------|
| Dijkstra           | 1542       |
| Bellman-Ford       | 1429       |

Donc nous pouvons dans un but d'optimisation soit calculer la `pathMatrix` avec l'algorithme de Roy-Floyd-Warshall dès l'initialisation du graphe, soit la calculer à la demande avec l'algorithme de Bellman-Ford. Nous avons choisi de la calculer à la demande avec l'algorithme de Bellman-Ford car cela permet de ne pas surcharger la mémoire avec une matrice de 330x330 et de ne pas avoir à recalculer la matrice à chaque fois que le graphe est modifié si nous décidons d'intégrer les perturbations du réseau en temps réel plus tard.
