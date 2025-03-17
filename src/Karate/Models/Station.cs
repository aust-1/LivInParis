using System.Diagnostics.CodeAnalysis;

namespace Karate.Models;

/// <summary>
/// Represents a station on Paris intra-muros metro network.
/// </summary>
public struct Station
{
    #region Fields

    /// <summary>
    /// The name of the station.
    /// </summary>
    private string _name;
    /// <summary>
    /// The line the station is on.
    /// </summary>
    private string _line;
    /// <summary>
    /// The longitude of the station.
    /// </summary>
    private double _longitude;
    /// <summary>
    /// The latitude of the station.
    /// </summary>
    private double _latitude;

    #endregion Fields

    #region Constructors

    public Station(string name, string line, double longitude, double latitude)
    {
        _name = name;
        _line = line;
        _longitude = longitude;
        _latitude = latitude;
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
