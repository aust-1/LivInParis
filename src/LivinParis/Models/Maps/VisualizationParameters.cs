namespace LivinParis.Models.Maps;

//HACK: refactor

/// <summary>
/// Defines the visual parameters for positioning and styling nodes on a map.
/// </summary>
public struct VisualizationParameters
{
    #region Constants

    /// <summary>
    /// Minimum longitude value within the map bounds.
    /// </summary>
    public const double X_MIN = 2257046193;

    /// <summary>
    /// Maximum longitude value within the map bounds.
    /// </summary>
    public const double X_MAX = 2440540095;

    /// <summary>
    /// Minimum latitude value within the map bounds.
    /// </summary>
    public const double Y_MIN = 48819106600;

    /// <summary>
    /// Maximum latitude value within the map bounds.
    /// </summary>
    public const double Y_MAX = 48897802690;

    #endregion Constants

    #region Fields

    private readonly double? _x;
    private readonly double? _y;
    private readonly string _color;
    private string _cluster;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="VisualizationParameters"/> struct.
    /// </summary>
    /// <param name="longitude">The longitude of the node in radians.</param>
    /// <param name="latitude">The latitude of the node in radians.</param>
    /// <param name="color">The fill color to use for visualization.</param>
    /// <param name="cluster">The cluster identifier the node belongs to (optional).</param>
    public VisualizationParameters(
        double longitude,
        double latitude,
        string color,
        string cluster = ""
    )
    {
        _x = (longitude * 10E9 - X_MIN) / (X_MAX - X_MIN) * 20.985097455 * 10E6; //FIXME: c'est le zbeul
        _y = (latitude * 10E9 - Y_MIN) / (Y_MAX - Y_MIN) * 9 * 10E6;
        _color = color;
        _cluster = cluster;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VisualizationParameters"/> struct.
    /// </summary>
    public VisualizationParameters()
    {
        _x = null;
        _y = null;
        _color = "#000000";
        _cluster = "";
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Gets the color of the node.
    /// </summary>
    public string Color
    {
        get { return _color; }
    }

    /// <summary>
    /// Gets the clusters the node belongs to.
    /// </summary>
    public string Cluster
    {
        get { return _cluster; }
        set { _cluster = value; }
    }

    #endregion Properties

    #region Methods

    public override string ToString()
    {
        return $"[pos=\"{_y},{_x}!\", style=filled, fillcolor=\"{_color}\"]";
        //BUG: neato: y, x; sfdp: x, y voir des moins ??
    }

    #endregion Methods
}
