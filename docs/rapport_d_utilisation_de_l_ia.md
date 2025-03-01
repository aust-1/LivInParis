# Rapport d'utilisation de l'IA pour l'implémentation de DrawGraph
<!-- TOC -->
- [Rapport d'utilisation de l'IA pour l'implémentation de DrawGraph](#rapport-dutilisation-de-lia-pour-limplémentation-de-drawgraph)
  - [Introduction](#introduction)
  - [1. Premiers échanges](#1-premiers-échanges)
    - [Prompt](#prompt)
    - [Résultat obtenu](#résultat-obtenu)
    - [Corrections apportées](#corrections-apportées)
  - [2. Gestion des flèches pour les graphes orientés](#2-gestion-des-flèches-pour-les-graphes-orientés)
    - [Prompt ajout](#prompt-ajout)
    - [Résultat obtenu](#résultat-obtenu-1)
    - [Corrections apportées](#corrections-apportées-1)
    - [Prompt taille des noeuds](#prompt-taille-des-noeuds)
    - [Résultat obtenu](#résultat-obtenu-2)
  - [3. Problèmes lors d'appels successifs](#3-problèmes-lors-dappels-successifs)
    - [Prompt](#prompt-1)
    - [Résultat obtenu](#résultat-obtenu-3)
  - [4. Problèmes esthétiques](#4-problèmes-esthétiques)
    - [Prompt](#prompt-2)
<!-- TOC -->
## Introduction

Ce rapport détaille nos requêtes textuelles faites auprès de différents agents d'intelligence artificielle, principalement via GitHub Copilot intégré à VS code, et qui donne accès au contexte global de notre solution aux agents utilisés. Lorsqu'aucun modèle n'est précisé, il s'agit de ChatGPT o1.

## 1. Premiers échanges

### Prompt

>_"Generate a C# method to draw a graph using System.Drawing, ensuring that node positioning follows a circular layout. The implementation should adhere to best coding practices, including proper encapsulation, consistent variable naming, and clear separation of logic. The method should be structured to allow easy extension for future enhancements such as different layouts or additional visual features. Avoid redundant calculations and ensure efficient handling of node positioning."_

### Résultat obtenu

Copilot a proposé une implémentation basique utilisant `Bitmap` et `Graphics`, avec un positionnement circulaire des noeuds. Cependant, plusieurs points ont posé problème :

- Code ne respectant pas entièrement les conventions de notre projet. L'intégration de nos classes était quasiment parfaite à quelques propriétés près, mais le code était extrêmement commenté `//` et les conventions concernant la nomenclature des variables n'étaient pas toujours respectées.
- Taille des différents objets initialisée en dur, pas d'adaptation dynamique selon le graphe, son ordre, etc.
- Arêtes dessinées deux fois pour chacun des sens.
- Implémentation valable uniquement pour les graphes non-orientés.

### Corrections apportées

Nous avons renommé les variables et supprimé les commentaires manuellement. En utilisant Copilot, l'erreur n'a plus été faite lors des générations suivantes.

La gestion des doubles arêtes a été assurée par nous-mêmes :

```csharp
if (!_isDirected && source.Id < target.Id)
{
        g.DrawLine(edgePen, positions[source], positions[target]);
}
```

Pour les autres défauts, nous avons reprompté.

## 2. Gestion des flèches pour les graphes orientés

Pour pouvoir plus tard afficher des graphes orientés, nous avons cherché à afficher des flèches représentant le sens du lien.

### Prompt

>_"Add arrows to indicate edge direction in a directed graph while maintaining a clean and readable visualization. Ensure that arrow positioning remains clear regardless of node density. The solution should integrate well with the existing graph rendering logic and adhere to the project's style guidelines."_

### Résultat obtenu

Copilot a proposé une implémentation utilisant `Math.Atan2` pour calculer l'angle et dessiner deux segments simulant une flèche:

```csharp
const int arrowSize = 5;
double angle = Math.Atan2(
    positions[target].Y - positions[source].Y,
    positions[target].X - positions[source].X);
PointF arrowPoint1 = new PointF(
    (float)(positions[target].X - arrowSize * Math.Cos(angle - Math.PI / 6)),
    (float)(positions[target].Y - arrowSize * Math.Sin(angle - Math.PI / 6)));
PointF arrowPoint2 = new PointF(
    (float)(positions[target].X - arrowSize * Math.Cos(angle + Math.PI / 6)),
    (float)(positions[target].Y - arrowSize * Math.Sin(angle + Math.PI / 6)));
```

Cela a fonctionné, mais les flèches étaient en dessous des noeuds et leurs pointes étaient situées aux centres des noeuds et non à leurs bords. De plus, la taille de la flèche était une constante et n'évoluait pas en fonction de la taille du graphe.

### Corrections apportées

Nous avons ajouté un simple calcul précédant ceux de `arrowPoint1` et `arrowPoint2` soustrayant le rayon du noeud à la position cible.

```csharp
float stopX = positions[target].X - (nodeSize / 2 * (float)Math.Cos(angle));
float stopY = positions[target].Y - (nodeSize / 2 * (float)Math.Sin(angle));
```

De plus, nous avons arbitrairement fixé la taille des flèches à 40% de celle des noeuds.

```csharp
int arrowSize = (int)(nodeSize * 0.4);
```

### Prompt taille des noeuds

>_"Adjust the node size dynamically based on the graph dimensions and the number of nodes. Ensure that nodes remain distinguishable and well-spaced while preventing overlap. Implement an adaptive approach that scales proportionally with the graph size. Maintain a minimum and maximum size threshold to ensure readability in all cases."_

### Résultat obtenu

```csharp
int minDimension = Math.Min(width, height);
int nodeSize = Math.Max(20, Math.Min(40, minDimension / (2 * Math.Max(1, _nodes.Count))));
```

## 3. Problèmes lors d'appels successifs

Pendant les tests, nous avons remarqué que l'on ne pouvait pas appeler deux fois la méthode DrawGraph. Nous avons donc interrogé Claude 3.7 Sonnet Thinking via GitHub Copilot (explication du problème + accès aux logs).

### Prompt

_"Is it possible that the problem stems from poor management of graphic resources, such as an object that needs to be explicitly closed or that can only be instantiated once? After several tests, my method works correctly when called once, but generates an exception when invoked several times in the same program. Here's the error: Unhandled exception. System.ArgumentException: Parameter is not valid. at System.Drawing.Graphics.CheckErrorStatus(Status status) at System.Drawing.Graphics.FillEllipse(Brush brush, Single x, Single y, Single width, Single height) at System.Drawing.Graphics. FillEllipse(Brush brush, Int32 x, Int32 y, Int32 width, Int32 height) at Karate.Models.Graph.DrawGraph(String fileName) in \Karate\src\Karate\Models\Graph.cs:line 613 at Karate.Program.Main(String[] args) in \Karate\src\Karate\Program.cs:line 272. Could you identify a possible unreleased resource or underlying problem that would explain this error after several calls?"_

### Résultat obtenu

>The issue is likely related to how GDI+ handles graphics resources. Let's modify the DrawGraph method to ensure proper resource management and coordinate validation

```csharp
using var bitmap = new Bitmap(width, height);
using var g = Graphics.FromImage(bitmap);
g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
// ... existing code ...

    using (var edgePen = new Pen(Color.Gray, 2))
    {
        // ... existing edge drawing code ...
    }

    using (var nodeBrush = new SolidBrush(Color.LightBlue))
    using (var nodePen = new Pen(Color.Black, 1))
    {
        // ... existing node drawing code ...
    }

    using (var memory = new MemoryStream())
    {
        bitmap.Save(memory, ImageFormat.Png);
        byte[] bytes = memory.ToArray();
        File.WriteAllBytes(fileName, bytes);
    }
```

## 4. Problèmes esthétiques

Ayant déjà utilisé PlantUML et d'autres logiques de visualisation de lien entre objets, nous avons voulu chercher une autre solution intégrant un algorithme de disposition des noeuds plus intelligent. Après quelques recherches, notamment sur ces pages :

- <https://fr.wikipedia.org/wiki/Th%C3%A9orie_des_graphes>
- <https://en.wikipedia.org/wiki/Graph_(discrete_mathematics)>
- <https://en.wikipedia.org/wiki/Graph_drawing>
- <https://fr.wikipedia.org/wiki/DOT_(langage)>
- <https://en.wikipedia.org/wiki/Graphviz>

Nous avons approfondi nos recherches sur GraphViz, qui semble être une solution très utilisée. Cette option est également avantageuse car décrire notre graphe en DOT est simple contrairement aux autres librairies qui requièrent une réadaptation de nos classes dans d'autres formats, ce qui est plus lourd et complexe à implémenter.

Après avoir lu la documentation, nous avons implémenté les méthodes actuelles de notre projet pour générer un fichier. Cette méthode a l'inconvénient de nécessiter une installation de GraphViz sur la machine de l'utilisateur pour générer l'image. Nous avons donc automatisé cette installation dans notre projet (avec l'autorisation de l'utilisateur) en demandant à ChatGPT comment exécuter la commande `winget install -e --id Graphviz.Graphviz` dans le terminal PowerShell de l'utilisateur depuis C#.

### Prompt

>_"Write a C# method that runs a PowerShell command to install GraphViz using Winget. The method should execute "winget install -e --id Graphviz.Graphviz" within PowerShell from a C# application. Ensure that the process runs silently, captures both standard output and error streams, and waits for execution to complete before returning. Implement proper exception handling to manage potential errors, such as Winget not being available or user permissions preventing installation."_

### Résultats obtenus

```csharp
ProcessStartInfo psi = new ProcessStartInfo
{
    FileName = "powershell",
    Arguments =
        "-NoProfile -ExecutionPolicy Bypass -Command \"winget install -e --id Graphviz.Graphviz\"",
    UseShellExecute = false,
    RedirectStandardOutput = true,
    RedirectStandardError = true,
    CreateNoWindow = true,
};

using (Process processGraphVizInstall = new Process { StartInfo = psi })
{
    processGraphVizInstall.Start();
    string output = processGraphVizInstall.StandardOutput.ReadToEnd();
    string error = processGraphVizInstall.StandardError.ReadToEnd();
    processGraphVizInstall.WaitForExit();

    Console.WriteLine("Output:\n" + output);
    if (!string.IsNullOrEmpty(error))
    {
        Console.WriteLine("Error:\n" + error);
    }
}
```

## Conclusion

Nous avons ensuite encapsulé les différentes méthodes obtenues dans des éléments logiques (test de la présence ou non de GraphViz sur l'ordinateur de l'utilisateur, graphe orienté ou non, ...) jusqu'à obtenir le résultat final aujourd'hui implémenté.

Nous avons enfin fait le choix de garder les deux méthodes obtenues afin de ne pas obliger l'utilisateur à avoir `GraphViz` installé sur son PC.
