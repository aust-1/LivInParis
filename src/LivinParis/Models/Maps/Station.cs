namespace LivinParis.Models.Maps;

/// <summary>
/// Represents a station on Paris intra-muros metro network.
/// </summary>
public struct Station
{
    #region Fields

    /// <summary>
    /// The name of the station.
    /// </summary>
    private readonly string _name;

    /// <summary>
    /// The line the station is on.
    /// </summary>
    private readonly string _line;

    /// <summary>
    /// The longitude of the station.
    /// </summary>
    private readonly double _longitude;

    /// <summary>
    /// The latitude of the station.
    /// </summary>
    private readonly double _latitude;

    /// <summary>
    /// The commune the station is in.
    /// </summary>
    private readonly string _commune;

    /// <summary>
    /// The INSEE code of the commune the station is in.
    /// </summary>
    private readonly int _insee;

    #endregion Fields

    #region Constructors

    public Station(string line, string name, double longitude, double latitude, string commune, int insee)
    {
        _line = line;
        _name = name;
        _longitude = longitude;
        _latitude = latitude;
        _commune = commune;
        _insee = insee;
    }

    #endregion Constructors

    #region Properties

    public double Latitude
    {
        get { return _latitude; }
    }

    public double Longitude
    {
        get { return _longitude; }
    }

    #endregion Properties

    #region Methods

    public override string ToString()
    {
        return $"{_name} ({_line})";
    }

    #endregion Methods
}
