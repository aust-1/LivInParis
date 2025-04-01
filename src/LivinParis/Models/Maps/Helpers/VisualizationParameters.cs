using System.Globalization;

namespace LivinParis.Models.Maps.Helpers;

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
    public const double X_MIN = 22570461929;

    /// <summary>
    /// Maximum longitude value within the map bounds.
    /// </summary>
    public const double X_MAX = 24405400954;

    /// <summary>
    /// Minimum latitude value within the map bounds.
    /// </summary>
    public const double Y_MIN = 488191065956;

    /// <summary>
    /// Maximum latitude value within the map bounds.
    /// </summary>
    public const double Y_MAX = 488978026914;

    #endregion Constants

    #region Fields

    private readonly double? _x;
    private readonly double? _y;
    private readonly string _color;
    private string _label;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="VisualizationParameters"/> struct.
    /// </summary>
    /// <param name="longitude">The longitude of the node in degree.</param>
    /// <param name="latitude">The latitude of the node in degree.</param>
    /// <param name="color">The fill color to use for visualization.</param>
    /// <param name="label">The name of the node.</param>
    public VisualizationParameters(double longitude, double latitude, string color, string label)
    {
        _x = (longitude * 10E9 - X_MIN) / (X_MAX - X_MIN) * 25;
        _y = (latitude * 10E9 - Y_MIN) / (Y_MAX - Y_MIN) * 10.7218953475;
        _color = color;
        _label = label;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="VisualizationParameters"/> struct.
    /// </summary>
    public VisualizationParameters()
    {
        _x = null;
        _y = null;
        _color = "#000000";
        _label = "";
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
    /// Gets the label of the node.
    /// </summary>
    public string Label
    {
        get { return _label; }
    }

    #endregion Properties

    #region Methods

    public override string ToString()
    {
        return $"pos=\"{_x:F4},{_y:F4}!\", style=filled, fillcolor=\"{_color}\"";
    }

    #endregion Methods
}
