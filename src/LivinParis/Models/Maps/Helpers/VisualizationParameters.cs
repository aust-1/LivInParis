namespace LivInParisRoussilleTeynier.Models.Maps.Helpers;

/// <summary>
/// Defines the visual parameters for positioning and styling nodes on a map.
/// </summary>
public class VisualizationParameters
{
    #region Constants

    /// <summary>
    /// The minimum longitude value within the map bounds (scaled by 1e-9).
    /// </summary>
    public const double X_MIN = 22570461929;

    /// <summary>
    /// The maximum longitude value within the map bounds (scaled by 1e-9).
    /// </summary>
    public const double X_MAX = 24405400954;

    /// <summary>
    /// The minimum latitude value within the map bounds (scaled by 1e-9).
    /// </summary>
    public const double Y_MIN = 488191065956;

    /// <summary>
    /// The maximum latitude value within the map bounds (scaled by 1e-9).
    /// </summary>
    public const double Y_MAX = 488978026914;

    #endregion Constants

    #region Fields

    private readonly double? _x;
    private readonly double? _y;
    private string _color;
    private readonly string _label;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="VisualizationParameters"/> struct
    /// with the specified longitude, latitude, color, and label.
    /// </summary>
    /// <param name="longitude">The longitude of the node in degrees.</param>
    /// <param name="latitude">The latitude of the node in degrees.</param>
    /// <param name="color">The fill color to use for visualization (e.g., "#000000").</param>
    /// <param name="label">A label or name for the node.</param>
    /// <remarks>
    /// The <paramref name="longitude"/> and <paramref name="latitude"/> values
    /// are scaled by 1e9 to map them into a [X_MIN, X_MAX] or [Y_MIN, Y_MAX] range, respectively.
    /// </remarks>
    public VisualizationParameters(
        double? longitude = null,
        double? latitude = null,
        string color = "#000000",
        string label = ""
    )
    {
        _x = ((longitude * 10e9) - X_MIN) / (X_MAX - X_MIN) * 25;
        _y = ((latitude * 10e9) - Y_MIN) / (Y_MAX - Y_MIN) * 10.7218953475;
        _color = color;
        _label = label;
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Gets the fill color of the node.
    /// </summary>
    public string Color
    {
        get { return _color; }
        set { _color = value; }
    }

    /// <summary>
    /// Gets the label or name of the node.
    /// </summary>
    public string Label
    {
        get { return _label; }
    }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Returns a string that represents this <see cref="VisualizationParameters"/>,
    /// including position (x,y) and color information for DOT/GraphViz usage.
    /// </summary>
    /// <returns>A string describing node position and style.</returns>
    public override string ToString()
    {
        var position =
            _x.HasValue && _y.HasValue ? $"pos=\"{_x.Value:F4},{_y.Value:F4}!\", " : string.Empty;

        return position + $"style=filled, fillcolor=\"{_color}\"";
    }

    #endregion Methods
}
